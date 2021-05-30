using WebTest.Entities;

namespace WebTest.Models
{
    public class OrgRatingsModel : OrgRating
    {
        public Organization Organization { get; set; }
        public OrgRatingsModel() { }

        public OrgRatingsModel(int id, int organizationId, float rating, Organization organization)
        {
            Id = id;
            OrganizationId = organizationId;
            Rating = rating;
            Organization = organization;
        }

        public OrgRatingsModel(OrgRating orgRatings, Organization organization)
        {
            Id = orgRatings.Id;
            OrganizationId = orgRatings.OrganizationId;
            Rating = orgRatings.Rating;
            Organization = organization;
        }
    }
}
