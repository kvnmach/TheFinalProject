using System.Collections.Generic;
using TheFinalProject.Controllers;

namespace TheFinalProject.Models
{
    public class ProfileVm
    {
        public ProfileVm()
        {
            
        }

        public ProfileVm(ApplicationUser u)
        {
            UserId = u.Id;
            Email = u.Email;
            Phone = u.Phone;
            City = u.City;
            State = u.State;
            Zip = u.Zip;
        }
       public string UserId { get; set; }
        public string Email { get; set; }
        public string Photo { get; set; }
        public string Phone { get; set; }
        public int Zip { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        public List<ToolsVm> MyTools { get; set; } = new List<ToolsVm>();
        public List<ToolsVm> Workbench { get; set; } = new List<ToolsVm>();
       public List<ProfileVm> Following { get; set; } = new List<ProfileVm>();
    }
}