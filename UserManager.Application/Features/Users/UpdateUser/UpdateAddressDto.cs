namespace UserManager.Application.Features.Users.UpdateUser;
public class UpdateAddressDto
{
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public int? PostCode { get; set; }
}
