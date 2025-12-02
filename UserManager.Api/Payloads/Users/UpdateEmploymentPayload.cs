namespace UserManager.Api.Payloads.Users;

public class UpdateEmploymentPayload
{
    public int? Id { get; set; }
    public string Company { get; set; } = string.Empty;
    public uint MonthsOfExperience { get; set; }
    public uint Salary { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public bool IsDeleted { get; set; }
}
