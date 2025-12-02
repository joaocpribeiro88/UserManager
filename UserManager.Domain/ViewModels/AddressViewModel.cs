namespace UserManager.Domain.ViewModels;

public class AddressViewModel
{
    public int Id { get; set; }
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public int? PostCode { get; set; }
}