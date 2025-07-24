using OnlineEducationPlatform.Web.Models;
using OnlineEducationPlatform.Web.Users;
using System.ComponentModel;

namespace OnlineEducationPlatform.Web.ViewModels
{
    public class ClassSubjectsViewModel
    {
        public Class Class { get; set; }
        public List<ClassSubject> ClassSubjects { get; set; } = new List<ClassSubject>();
        public List<Subject> AllSubjects { get; set; } = new List<Subject>();
        public List<ApplicationUser> Students { get; set; } = new List<ApplicationUser>();
    }
}
