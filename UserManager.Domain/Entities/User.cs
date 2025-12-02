namespace UserManager.Domain.Entities;
public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty; //MANDATORY 
    public string LastName { get; set; } = string.Empty; //MANDATORY 
    public string Email { get; set; } = string.Empty; //MANDATORY, UNIQUE
    public Address? Address { get; set; }
    // add, update an existing employment,
    // delete an existing employment
    public List<Employment> Employments { get; set; } = [];
}
