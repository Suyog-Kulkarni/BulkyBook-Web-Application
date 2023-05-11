using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BulkyBook_Razor.Models;
public class Category
{
    [Key] // tells the database that Id is the primary key 
    public int Id { get; set; }

    [Required] // tells the database that Name is required
    [DisplayName("Category Name")]
    [MaxLength(30)]
    public string? Name { get; set; }

    [DisplayName("Display Order")]
    [Range(0, 100, ErrorMessage = "Display Order must be between 1 and 100 only!!")]
    public int DisplayOrder { get; set; }
}

