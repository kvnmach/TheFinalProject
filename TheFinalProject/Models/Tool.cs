using System.Collections.Generic;
using Humanizer;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheFinalProject.Models
{

    public enum Category
    {
        AirCompressors,
        Generators,
        Automotive,
        HandTools,
        PowerTools,
        Welding,
        Vacuum,
        Sporting,
        LawnEquipment,
        Outdoors,
        Crafting,
        ToolPackageKit,
        Pottery,
        MicroTools,
    }

    public class Tool
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Photo { get; set; }
        public string Description { get; set; }
        public bool IsAvailable { get; set; } = true;
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }


        public Category ToolCategory { get; set; }

        public string CategoryName => ToolCategory.Humanize();

        public virtual ApplicationUser Owner { get; set; }

        public virtual ICollection<ApplicationUser> OnPeoplesWorkBench { get; set; } = new List<ApplicationUser>();

    }
}