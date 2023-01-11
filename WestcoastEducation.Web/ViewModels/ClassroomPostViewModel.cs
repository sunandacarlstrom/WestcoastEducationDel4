using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WestcoastEducation.Web.ViewModels;

public class ClassroomPostViewModel
{
    [Required(ErrorMessage = "Kursnummer är obligatoriskt")]
    [DisplayName("Kursnummer")]
    public string Number { get; set; } = "";

    [Required(ErrorMessage = "Kursnamn är obligatoriskt")]
    [DisplayName("Kursnamn")]
    public string Name { get; set; } = "";

    [Required(ErrorMessage = "Kurstitel är obligatoriskt")]
    [DisplayName("Kurstitel")]
    public string Title { get; set; } = "";

    [DisplayName("Kursinnehåll")]
    public string Content { get; set; } = "";

    [DisplayName("Genomsnittligt betyg som kursen har fått av studenter")]
    public double AvgGrade { get; set; }

    [Required(ErrorMessage = "Startdatum är obligatoriskt")]
    [DisplayName("Startdatum")]
    public DateTime Start { get; set; }

    [Required(ErrorMessage = "Slutdatum är obligatoriskt")]
    [DisplayName("Slutdatum")]
    public DateTime End { get; set; }

    [DisplayName("Kurslängd")]
    public TimeSpan Length { get => End - Start; }

    [DisplayName("Kursschema")]
    public string Schedule { get; set; } = "";
    [DisplayName("Är kursen på distans?✅")]
    public bool IsOnDistance { get; init; }
    [DisplayName("LärarId")]
    public int TeacherId { get; set; }
}
