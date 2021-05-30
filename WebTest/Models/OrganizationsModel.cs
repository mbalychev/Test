using System;
using WebTest.Entities;

namespace WebTest.Models
{
    public class OrganizationsModel : Organization
    {
        public OrganizationsModel(int id, int inn)
        {
            Id = id;
            if (inn == 12) //inn organization
                Inn = inn;
            else
                throw new Exception("wrong inn number");
        }

        public OrganizationsModel(Organization organization)
        {
            Id = organization.Id;
            if (Inn == 12) //inn organization
                Inn = organization.Inn;
            else
                throw new Exception("wrong inn number");
        }
    }
}
