namespace UserManager.Domain.ViewModels;

public class GetUserResponseViewModel
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public AddressViewModel? Address { get; set; }
    public List<EmploymentViewModel> Employments { get; set; } = [];
}