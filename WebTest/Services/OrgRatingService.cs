using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTest.Interfaces;
using WebTest.Models;
using WebTest.Entities;
using Microsoft.EntityFrameworkCore;

namespace WebTest.Services
{
    public class OrgRatingService : IServices<OrgRatingsModel>, IDisposable
    {
        public readonly Context db;
        public OrgRatingService(Context context)
        {
            db = context;
        }
        public async Task CreateAsync(OrgRatingsModel model)
        {
            try
            {
                OrgRating orgRating = new OrgRating { OrganizationId = model.OrganizationId, Rating = model.Rating };
                await db.OrgRatings.AddAsync(orgRating);
                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task CreateAsync(string inn, string rating)
        {
            long innInt;
            float ratingFl;
            rating = rating.Replace('.', ',');
            long.TryParse(inn, out innInt);
            float.TryParse(rating, out ratingFl);

            if ((inn.Length != 10 || inn.Length != 12) && innInt == 0 && ratingFl > 0)
            {
                 throw new Exception(string.Format("error parse inn or rating {0};{1}", inn, rating));
            }

            Organization organization = await FindOrganizationAsync(innInt);
            await CreateRating(organization.Id, ratingFl);
        }

        private async Task CreateRating(int orgId, float rating)
        {
            OrgRating orgRating = await db.OrgRatings.Where(x => x.OrganizationId == orgId).FirstOrDefaultAsync();
            if (orgRating != null)
                orgRating.Rating = rating;
            else
                await db.OrgRatings.AddAsync(new OrgRating { OrganizationId = orgId, Rating = rating });

            await db.SaveChangesAsync();
        }

        private async Task<Organization> FindOrganizationAsync(long inn)
        {
            Organization organization = await db.Organizations.Where(x => x.Inn == inn).FirstOrDefaultAsync();
            if (organization == null)
            {
                try
                {
                    await db.Organizations.AddAsync(new Organization { Inn = inn });
                    await db.SaveChangesAsync();
                    organization = await db.Organizations.Where(x => x.Inn == inn).FirstOrDefaultAsync();
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
            return organization;
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                OrgRating orgRating = await db.OrgRatings.FindAsync(id);
                db.OrgRatings.Remove(orgRating);
                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async void Dispose()
        {
            await db.DisposeAsync();
        }

        public async Task<List<OrgRatingsModel>> ReadAllAsync()
        {
            try
            {
                List<OrgRatingsModel> models = new List<OrgRatingsModel>();
                List<OrgRating> orgRatings = await db.OrgRatings.ToListAsync();
                foreach (var item in orgRatings)
                {
                    models.Add(new OrgRatingsModel(item));
                }
                return models;
            }
            catch (Exception e)
            {
                throw new Exception("error read list org ratings");
            }
        }

        public async Task<OrgRatingsModel> ReadAsync(int id)
        {
            try
            {
                OrgRating orgRating = await db.OrgRatings.FindAsync(id);
                OrgRatingsModel model = new OrgRatingsModel(orgRating);
                return model;
            }
            catch (Exception e)
            {
                throw new Exception("error read org rating");
            }
        }

        public async Task UpdateAsync(OrgRatingsModel model)
        {
            if (model != null)
            {
                OrgRating rating = await db.OrgRatings.FindAsync(model.Id);
                if (rating != null)
                {
                    rating.Rating = model.Rating;
                    await db.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("rating not found");
                }
            }

        }
    }
}
