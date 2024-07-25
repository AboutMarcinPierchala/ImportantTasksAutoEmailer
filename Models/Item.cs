using System.ComponentModel.DataAnnotations;

namespace ImpListApp.Models
{
    public class Item
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public DateOnly EndDate { get; set; }
    }
}
