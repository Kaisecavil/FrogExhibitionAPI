using FrogExhibitionDAL.Enums;
using Microsoft.AspNetCore.Identity;

namespace FrogExhibitionDAL.Models
{
    public class ApplicationUser : IdentityUser
    {
        public UserKnowledgeLevel KnowledgeLevel { get; set; }
        public virtual ICollection<Vote> Votes { get; set; } = new List<Vote>();
    }
}
