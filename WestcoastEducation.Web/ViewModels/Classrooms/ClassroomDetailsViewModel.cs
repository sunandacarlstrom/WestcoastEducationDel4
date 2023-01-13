using System.ComponentModel;

namespace WestcoastEducation.Web.ViewModels.Classrooms;

public class ClassroomDetailsViewModel
{
    [DisplayName("KursId")]
    public int ClassroomId { get; set; }

    [DisplayName("Kursnummer")]
    public string Number { get; set; } = "";

    [DisplayName("Kursnamn")]
    public string Name { get; set; } = "";

    [DisplayName("Kurstitel")]

    public string Title { get; set; } = "";

    [DisplayName("Kursinnehåll")]

    public string Content { get; set; } = "";

    [DisplayName("Startdatum")]

    public DateTime Start { get; set; }

    [DisplayName("Slutdatum")]

    public DateTime End { get; set; }

    [DisplayName("Kurslängd")]

    public TimeSpan Length { get => End - Start; }

    [DisplayName("På distans?")]

    public bool IsOnDistance { get; set; } = false;
}