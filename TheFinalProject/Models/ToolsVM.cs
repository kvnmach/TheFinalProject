using System.Collections.Generic;
using Humanizer;
using TheFinalProject.Models;

namespace TheFinalProject.Controllers
{
    public class ToolsVM
    {

        public ToolsVM()
        {

        }

        public ToolsVM(Tool t)
        {
            Title = t.Title;
            Photo = t.Photo;
            ToolId = t.Id;
            Description = t.Description;
            CategoryName = t.ToolCategory.Humanize();
            IsAvailable = t.IsAvailable;
        }
        public int ToolId { get; set; }
        public string Title { get; set; }
        public object Photo { get; set; }
        public object Description { get; set; }
        public object CategoryName { get; set; }
        public object IsAvailable { get; set; }
    }
}