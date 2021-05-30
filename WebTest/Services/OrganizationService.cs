using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebTest.Entities;
using WebTest.Interfaces;
using WebTest.Models;

namespace WebTest.Services
{
    public class OrganizationService : IServices<OrganizationsModel>, IDisposable
    {
        private readonly Context db;
        public OrganizationService(Context context)
        {
            db = context;
        }

        public async Task CreateAsync(OrganizationsModel model)
        {
            try
            {
                Organization organization = new Organization { Inn = model.Inn };
                await db.Organizations.AddAsync(organization);
                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                Organization organization = await db.Organizations.FindAsync(id);
                if (organization != null)
                {
                    db.Remove(organization);
                    await db.SaveChangesAsync();
                }
                else
                    throw new Exception("organization not found");
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

        public async Task<List<OrganizationsModel>> ReadAllAsync()
        {
            List<OrganizationsModel> models = new List<OrganizationsModel>();
            List<Organization> organizations = await db.Organizations.ToListAsync();
            foreach (var item in organizations)
            {
                models.Add(new OrganizationsModel(item));
            }
            return models;
        }

        public async Task<OrganizationsModel> ReadAsync(int id)
        {
            Organization organization = await db.Organizations.FindAsync(id);
            if (organization != null)
            {
                OrganizationsModel model = new OrganizationsModel(organization);
                return model;
            }
            else
            {
                throw new Exception("organization not found");
            }
        }

        public async Task UpdateAsync(OrganizationsModel model)
        {
            //TODO при изменении полей таблице, на на данный момент нет полей для изменения (инн присваиваетсься только однажды)
            throw new NotImplementedException("no field to changes");
        }

        Task IServices<OrganizationsModel>.CreateAsync(string inn, string rating)
        {
            throw new NotImplementedException();
        }
    }
}
