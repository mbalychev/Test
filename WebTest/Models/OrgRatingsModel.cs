using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTest.Entities;

namespace WebTest.Models
{
    public class OrgRatingsModel
    {
        private int id;
        private int organizationId;
        private float rating;

        public int Id { get => id; }
        public int OrganizationId { get => organizationId; }
        public float Rating 
        { 
            get => rating;
            set
            {
                if (value > 0)
                    rating = value;
                else
                    throw new Exception("raiting number error (less then one)");
            }
        }

        public OrgRatingsModel(int id, int organizationId, float rating)
        {
            this.id = id;
            this.organizationId = organizationId;
            this.rating = rating;
        }

        public OrgRatingsModel(OrgRating orgRatings)
        {
            this.id = orgRatings.Id;
            this.organizationId = orgRatings.OrganizationId;
            this.rating = orgRatings.Rating;
        }
    }
}
