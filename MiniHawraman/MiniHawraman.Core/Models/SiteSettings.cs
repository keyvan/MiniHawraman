using System.ComponentModel.DataAnnotations;

namespace MiniHawraman.Core.Models
{
    public class SiteSettings
    {
        [Required(ErrorMessage = "Please enter a title.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Please enter a description.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please enter a page size.")]
        public int PageSize { get; set; }

        [Required(ErrorMessage = "Please enter a admin page size.")]
        public int AdminPageSize { get; set; }
    }
}
