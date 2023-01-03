namespace WestcoastEducation.Web.Models; 

public class Email
{
    public string Address { get; } = "";

    public Email(string email)
    {
        if (!Validate(email))
        {
            throw new ArgumentException("E-post är i fel format");
        }

        Address = email;
    }

    public bool Validate(string email) => true;
}