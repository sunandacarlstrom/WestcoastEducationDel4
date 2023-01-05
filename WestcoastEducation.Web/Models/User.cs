using System.ComponentModel.DataAnnotations;

namespace WestcoastEducation.Web.Models;

public class User
{
    [Key]
    public int UserId { get; set; }
    public string Email { get; set; } = ""; 
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string CompleteName { get { return FirstName + " " + LastName; } }
    public string SocialSecurityNumber { get; set; } = "";
    public string StreetAddress { get; set; } = "";
    public string PostalCode { get; set; } = "";
    public string Phone { get; set; } = ""; 
}