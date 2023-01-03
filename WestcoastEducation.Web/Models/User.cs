namespace WestcoastEducation.Web.Models;

public abstract class User
{
    public int UserId { get; init; }
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string CompleteName { get { return FirstName + " " + LastName; } }
    public int SocialSecurityNumber { get; set; }
    public string StreetAddress { get; set; } = "";
    public string PostalCode { get; set; } = "";
    public int Phone { get; set; }

    public Email? Email { get; set; }
}