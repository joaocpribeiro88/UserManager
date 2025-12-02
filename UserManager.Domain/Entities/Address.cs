namespace UserManager.Domain.Entities;
public class Address
{
    public int Id { get; set; }
    public string Street { get; set; } = string.Empty; //MANDATORY        
    public string City { get; set; } = string.Empty; //MANDATORY 
    public int? PostCode { get; set; }
}
