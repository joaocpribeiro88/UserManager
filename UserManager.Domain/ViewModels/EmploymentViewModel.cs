namespace UserManager.Domain.ViewModels;

public class EmploymentViewModel
{
    public int Id { get; set; }
    public string Company { get; set; } = string.Empty;
    public uint MonthsOfExperience { get; set; }
    public uint Salary { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
}