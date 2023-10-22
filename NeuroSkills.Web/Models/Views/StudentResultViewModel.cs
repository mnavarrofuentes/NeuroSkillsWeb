namespace NeuroSkills.Web.Models.Views;

public class StudentResultViewModel
{
    public string Code { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public List<StudentResultTherapyViewModel> Therapies { get; set; } = new List<StudentResultTherapyViewModel>();
}
