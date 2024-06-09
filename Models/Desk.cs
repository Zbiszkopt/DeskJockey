using System.ComponentModel.DataAnnotations;

namespace DeskJockey.Models
{
    public class Desk
    {
        public int DeskId { get; set; }

        [Required]
        public string DeskNumber { get; set; }

        public string Status { get; set; }

    }
}