using CsvHelper;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebTest.Entities;
using WebTest.Interfaces;
using WebTest.Models;

namespace WebTest.Services
{
    public class RatingService : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private static ILogger<RatingService> logger;
        private Timer timer;
        //static IServices<OrgRatingsModel> ratings;
        //static IServices<OrganizationsModel> organization;

        public RatingService(ILogger<RatingService> _logger)//, IServices<OrgRatingsModel> ratings, IServices<OrganizationsModel> organization)
        {
            //Context db = new Context();
            logger = _logger;
            //organization = new OrganizationService(db);
            //ratings = new OrgRatingService(db);
        }

        public async Task StartAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("Rating service start");
            timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
            //try
            //{
            //    //await Rating.GetLoadAsync(ratings);
            //}
            //catch (Exception e)
            //{
            //    logger.LogError("error load file: " + e.Message);
            //}
            //return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            var count = Interlocked.Increment(ref executionCount);
            try
            {
                logger.LogInformation("try get csv file from host: {0}, {0:t}", count, DateTime.Now);
                GetLoadAsync();
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
            }
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("Timed Hosted Service is stopping.");

            timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            timer?.Dispose();
        }


        async Task GetLoadAsync()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = null;
            try
            {
                response = await client.GetAsync("http://localhost:29461/weatherforecast");
                if (response.IsSuccessStatusCode)
                {
                    byte[] res = await response.Content.ReadAsByteArrayAsync();
                    //WriteFile(res);
                    await ParseFile(res); 
                }
                else
                {
                    throw new Exception("connection error: " + response.StatusCode);
                }
            }
            catch (Exception e)
            {
                throw new Exception($"connecting error: {e.Message}");
            }

        }

        //private static void WriteFile(byte[] res)
        //{
        //    try
        //    {
        //        using (FileStream fstream = new FileStream($"note.csv", FileMode.OpenOrCreate))
        //        {
        //            fstream.Write(res, 0, res.Length);
        //        }
        //    }
        //    catch (Exception e)
        //    {

        //        throw new Exception(e.Message);
        //    }
        //}

        private static async Task ParseFile(byte[] bytes)
        {
            //try
            //{
            //    using (var reader = new StreamReader("note.csv"))
            //    {
            //        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            //        {
            //            List<RatingModel> models = csv.GetRecords<RatingModel>().ToList();
            //        }
            //    }
            //}
            //catch (Exception e)
            //{
            //    throw new Exception(e.Message);
            //}

            char[] splits = new char[] { ';'};
            string str = Encoding.Default.GetString(bytes).Replace("\r\n", ";");
            string[] csv = str.Split(splits);
            for (int i = 0; i < csv.Length; ++i)
            {
                string inn = csv[i];
                string rating = csv[++i];
                try
                {
                    using (Context db = new Context())
                    using (IServices<OrgRatingsModel> Services = new OrgRatingService(db) )
                    await Services.CreateAsync(inn, rating);
                }
                catch (Exception e)
                {
                    logger.LogError(string.Format($"{inn}; {rating}; {e.Message}"));
                }
            }
        }
    }
}
