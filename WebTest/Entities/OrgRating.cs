using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebTest.Entities
{
    public class OrgRating
    {
        public int Id { get; set; }
        public int OrganizationId { get; set; }
        public float Rating { get; set; }
    }
}
