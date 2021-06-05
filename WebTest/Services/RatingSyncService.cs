using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebTest.Entities;
using WebTest.Interfaces;
using WebTest.Models;

namespace WebTest.Services
{
    public class RatingSyncService : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private static ILogger<RatingSyncService> logger;
        private Timer timer;
        static CancellationTokenSource source = new CancellationTokenSource();
        static CancellationToken token = source.Token;

        public RatingSyncService(ILogger<RatingSyncService> _logger)
        {
            logger = _logger;
        }

        public async Task StartAsync(CancellationToken token)
        {
            logger.LogInformation("Rating service start");
            //HACK DoWork create async
            //TODO change timer
            //timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
            //while(!token.IsCancellationRequested)
            //{
                await Task.Delay(10000, token);
                await DoWorkAsync(null);
            //}
            //return Task.CompletedTask;
        }

        private async Task DoWorkAsync(object state)
        {
            var count = Interlocked.Increment(ref executionCount);
            try
            {
               await GetLoadAsync();
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
            }
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {

            this.Dispose();
            //timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            logger.LogInformation("Timed Hosted Service is stopping.");
            source.Dispose();
            //timer?.Dis`pose();
        }

        private async Task GetLoadAsync()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = null;
            try
            {
                logger.LogInformation("try get csv file from host: {0:t}", DateTime.Now);
                response = await client.GetAsync("http://localhost:29461/ratings");
                if (response.IsSuccessStatusCode)
                {
                    byte[] responseBytes = await response.Content.ReadAsByteArrayAsync();
                    logger.LogInformation("get {0} bytes", responseBytes.Length);
                    await ParseFile(responseBytes);
                }
                else
                {
                    throw new Exception(String.Format("connection error: {0} at {1:t}", response.StatusCode, DateTime.Now));
                }
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
            }
        }

        private static async Task ParseFile(byte[] responseBytes)
        {
            char[] splits = new char[] { ';' };
            string responseStr = Encoding.Default.GetString(responseBytes).Replace("\r\n", ";");
            string[] csv = responseStr.Split(splits);
            string inn = null, rating = null;
            try
            {
                logger.LogInformation("try parse csv file");
                using (Context db = new Context())
                using (IServices<OrgRatingsModel> ratings = new OrgRatingService(db))
                    for (int i = 0; i < csv.Length; ++i)
                    {
                        inn = csv[i];
                        rating = csv[++i];
                        await ratings.CreateAsync(inn, rating);
                    }
                logger.LogInformation("parse succesfull");
            }
            catch (Exception e)
            {
                logger.LogError(string.Format($"{inn}; {rating}; {e.Message}"));
            }
        }
    }
}
