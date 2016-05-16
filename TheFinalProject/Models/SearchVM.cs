using System.Collections.Generic;
using TheFinalProject.Controllers;

namespace TheFinalProject.Models
{
    public class SearchVM
    {
        public List<ToolsVm> SearchTools { get; set; } = new List<ToolsVm>();
        public List<ToolsVm> Workbench { get; set; } = new List<ToolsVm>();
    }
}