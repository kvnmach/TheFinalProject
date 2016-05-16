using System.Collections.Generic;
using TheFinalProject.Controllers;

namespace TheFinalProject.Models
{
    public class ProfileVM
    {
        public string Email { get; set; }
        public string Photo { get; set; }
        public string Phone { get; set; }
        public int Zip { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        public List<ToolsVm> MyTools { get; set; } = new List<ToolsVm>();
        public List<ToolsVm> Workbench { get; set; } = new List<ToolsVm>();
    }
}