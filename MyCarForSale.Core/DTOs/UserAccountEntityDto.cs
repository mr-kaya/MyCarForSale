namespace MyCarForSale.Core.DTOs;

public class UserAccountEntityDto
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public DateTime Birthday { get; set; }
    public string PhoneNumber { get; set; }
    public string Country { get; set; }
    public string Province { get; set; }
    public string District { get; set; }
    public string FullAddress { get; set; }
    public int ZipCode { get; set; }
}