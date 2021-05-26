using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTest.Entities;

namespace WebTest.Models
{
    public class OrganizationsModel
    {
        private int id;
        private int inn;

        public int Id { get => id; }
        public int Inn { get => inn; }
        public OrganizationsModel(int id, int inn)
        {
            this.id = id;
            if (inn == 12) //inn organization
                this.inn = inn;
            else
                throw new Exception("wrong inn number");
        }

        public OrganizationsModel(Organization organization)
        {
            this.id = organization.Id;
            if (inn == 12) //inn organization
                this.inn = organization.Inn;
            else
                throw new Exception("wrong inn number");
        }
    }
}
