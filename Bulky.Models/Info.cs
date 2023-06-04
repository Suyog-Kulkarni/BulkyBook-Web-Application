using System.ComponentModel.DataAnnotations;

namespace Bulky.Models
{
    public class Info
    {
        [Key] // tells the database that Id is the primary key 
        public int Id { get; set; }

        [Required] // tells the database that Name is required
        public string? Name { get; set; }
        public int DisplayOrder { get; set; }

        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
        // datetime.now is a static method that returns the current date and time
    }
}
