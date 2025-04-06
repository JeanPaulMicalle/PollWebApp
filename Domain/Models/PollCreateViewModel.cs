using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class PollCreateViewModel
    {
        [Required(ErrorMessage = "The poll title is required.")]
        [StringLength(200, ErrorMessage = "The poll title must be at most 200 characters.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Option 1 is required.")]
        [StringLength(100, ErrorMessage = "Option 1 must be at most 100 characters.")]
        public string Option1Text { get; set; }

        [Required(ErrorMessage = "Option 2 is required.")]
        [StringLength(100, ErrorMessage = "Option 2 must be at most 100 characters.")]
        public string Option2Text { get; set; }

        [Required(ErrorMessage = "Option 3 is required.")]
        [StringLength(100, ErrorMessage = "Option 3 must be at most 100 characters.")]
        public string Option3Text { get; set; }
    }
}
