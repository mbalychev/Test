using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTest.Entities;

namespace WebTest.Models
{
    public class OrgRatingsModel : OrgRating
    {

        public OrgRatingsModel(int id, int organizationId, float rating)
        {
            Id = id;
            OrganizationId = organizationId;
            Rating = rating;
        }

        public OrgRatingsModel(OrgRating orgRatings)
        {
            Id = orgRatings.Id;
            OrganizationId = orgRatings.OrganizationId;
            Rating = orgRatings.Rating;
        }
    }
}
