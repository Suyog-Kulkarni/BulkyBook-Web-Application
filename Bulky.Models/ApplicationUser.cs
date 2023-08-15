using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Models;

public class ApplicationUser : IdentityUser
{
    [Required]
    public string Name { get; set; }

    public string? StreetAddress { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? PostalCode { get; set; }

    public int? CompanyId { get; set; }
    [ForeignKey("CompanyId")]
    [ValidateNever]// this is used to prevent the company object from being validated because it is not being used in the view 
    public Company Company { get; set; }// this not going to add in the database but it is going to be used to get the company object from the database
    // this is a just a navigation property to the Company table so it will not be added to the database 

}

