using System.Collections.Generic;

namespace TheFinalProject.Models
{

    public enum Category
    {
        AirCompressor,
        Automotive,
        HandTool,
        PowerTool,
        Welding,
        Vacuum,
        Sporting,
        LawnMachinery,
        Outdoors
    }

    public class Tool
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Photo { get; set; }
        public string Description { get; set; }
        public bool IsAvailable { get; set; }

        public Category ToolCategory { get; set; }

        
        public virtual ApplicationUser Owner { get; set; }

        public virtual ICollection<ApplicationUser> OnPeoplesWorkBench { get; set; } = new List<ApplicationUser>();

    }
}