using System.Security.Claims;

namespace OnlineEducationPlatform.Web.Users
{
    public class Teacher: ApplicationUser
    {
        public ICollection<Class> Classes { get; set; }
    }
}
