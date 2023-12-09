namespace MyCarForSale.Core.DTOs;

public class UserAccountJwt
{
    public bool AuthenticateResult { get; set; }
    public string? AuthToken { get; set; }
    public DateTime AccessTokenExpireDate { get; set; }
}