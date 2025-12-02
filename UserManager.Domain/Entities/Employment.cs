namespace UserManager.Domain.Entities;

public class Employment
{
    public int Id { get; set; }
    public string Company { get; set; } = string.Empty; //MANDATORY
    public uint MonthsOfExperience { get; set; } //MANDATORY 
    public uint Salary { get; set; } //MANDATORY
    public DateTime StartDate { get; set; } //MANDATORY
    public DateTime? EndDate { get; set; }
}
