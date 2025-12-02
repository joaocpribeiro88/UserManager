namespace UserManager.Api.Payloads.Users;

public class CreateUserPayload
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public AddressPayload? Address { get; set; }
    public List<EmploymentPayload> Employments { get; set; } = [];
}
