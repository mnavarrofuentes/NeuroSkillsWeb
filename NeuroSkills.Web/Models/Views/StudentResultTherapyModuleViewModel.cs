namespace NeuroSkills.Web.Models.Views;

public class StudentResultTherapyModuleViewModel
{
    public int Id { get; set; }
    public string ModuleName { get; set; } = string.Empty;
    public string ModuleDescription { get; set; } = string.Empty;
    public decimal Score { get; set; }
    public bool Completed { get; set; }
}
