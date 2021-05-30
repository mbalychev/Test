using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTest.Entities;
using WebTest.Interfaces;
using WebTest.Models;

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
            
            if ((inn.Length != 10 && inn.Length != 12) || ratingFl <= 0)
            {
                throw new Exception(string.Format("error parse inn or rating {0};{1}", inn, rating));
            }

            Organization organization = FindOrganization(innInt);
            await CreateRating(organization.Id, ratingFl);
        }

        private async Task CreateRating(int orgId, float rating)
        {
            OrgRating orgRating = await db.OrgRatings.Where(x => x.OrganizationId == orgId).FirstOrDefaultAsync();
            if (orgRating != null)
            {
                orgRating.Rating = rating;
                await db.SaveChangesAsync();
            }
            else
            {
                try
                {
                    await db.OrgRatings.AddAsync(new OrgRating { OrganizationId = orgId, Rating = rating });
                    await db.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
        }

        private Organization FindOrganization(long inn)
        {
            Organization organization = db.Organizations.Where(x => x.Inn == inn).FirstOrDefault();
            if (organization == null)
            {
                try
                {
                    db.Organizations.Add(new Organization { Inn = inn });
                    db.SaveChanges();
                    organization = db.Organizations.Where(x => x.Inn == inn).FirstOrDefault();
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
                Organization organization = null;
                foreach (var item in orgRatings)
                {
                    organization = db.Organizations.Find(item.OrganizationId);
                    models.Add(new OrgRatingsModel(item, organization));
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
                Organization organization = await db.Organizations.FindAsync(id);
                OrgRating orgRating = await db.OrgRatings.FindAsync(id);
                OrgRatingsModel model = new OrgRatingsModel(orgRating, organization);
                return model;
            }
            catch (Exception)
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
