namespace UserManager.Api.Payloads.Users;

public class UpdateUserPayload
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public UpdateAddressPayload? Address { get; set; }
    public List<UpdateEmploymentPayload> Employments { get; set; } = [];
}
