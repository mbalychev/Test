using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly ILogger<RatingService> logger;
        private Timer timer;
        IServices<OrgRatingsModel> ratings;
        IServices<OrganizationsModel> organization;
        public RatingService(ILogger<RatingService> logger)//, IServices<OrgRatingsModel> ratings, IServices<OrganizationsModel> organization)
        {
            Context db = new Context();
            this.logger = logger;
            this.organization = new OrganizationService(db);
            this.ratings = new OrgRatingService(db);
        }

        public async Task StartAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("Rating service start");

            //timer = new Timer(DoWork, null, TimeSpan.Zero,
            //    TimeSpan.FromSeconds(5));
            try
            {
                await Rating.GetLoadAsync(ratings);
            }
            catch (Exception e)
            {
                logger.LogError("error load file");
            }
            //return Task.CompletedTask;
        }

        private async Task DoWork(object state)
        {
            var count = Interlocked.Increment(ref executionCount);
            try
            {
                await Rating.GetLoadAsync(ratings);
            }
            catch (Exception e)
            {
                logger.LogError("error load file");
            }
            logger.LogInformation("try get csv file from host: {0}, {0:t}", count, DateTime.Now);
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
    }
}
