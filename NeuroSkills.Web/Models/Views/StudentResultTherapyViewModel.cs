namespace NeuroSkills.Web.Models.Views;

public class StudentResultTherapyViewModel
{
    public int Id { get; set; }
    public string TherapyName { get; set; } = string.Empty;
    public bool Completed { get; set; }
    public List<StudentResultTherapyModuleViewModel> ModulesResult { get; set; } = new List<StudentResultTherapyModuleViewModel>();
}
