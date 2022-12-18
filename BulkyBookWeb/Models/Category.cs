
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BulkyBookWeb.Models
{
    public class Category
    {
        [Key] // tells the database that Id is the primary key 
        public int Id { get; set; }

        [Required] // tells the database that Name is required
        public string? Name { get; set; }
        [DisplayName("Display Order")]
        [Range(0,100,ErrorMessage="Display Order must be between 1 and 100 only!!")]
        public int DisplayOrder { get; set; }

        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
        // datetime.now is a static method that returns the current date and time
        public static Category fun()
        {
            Category c = new();
            return c;
        }

    } 
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
