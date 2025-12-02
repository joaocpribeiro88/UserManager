namespace UserManager.Application.Features.Users.CreateUser;

public class AddressDto
{
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public int? PostCode { get; set; }
}