namespace UserManager.Api.Payloads.Users;

public class AddressPayload
{
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public int? PostCode { get; set; }
}
