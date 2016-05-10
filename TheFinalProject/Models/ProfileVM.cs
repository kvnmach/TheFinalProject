using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

        public List<ToolsVM> MyTools { get; set; } = new List<ToolsVM>();
        public List<ToolsVM> Workbench { get; set; } = new List<ToolsVM>();
    }
}