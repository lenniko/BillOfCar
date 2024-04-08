using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace BillOfCar.Helpers;

public static class JwtHelper
{
    private static string ISS = "https://www.billofcar.com";
    private static string SUB = "Authentication";
    private static string sercretKey = "A7b9RcD2EeFfG3hIiJkL4mNnO5pQqR6SsTtU7VW8XxY9Za0BbCc1DdE2F";
    public static string GenerateJwtToken(int userId)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(sercretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, SUB),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            // 添加其他必要的 Claims
        };

        var token = new JwtSecurityToken(
            issuer: ISS,
            audience: userId.ToString(),
            claims: claims,
            expires: DateTime.Now.AddHours(2),
            signingCredentials: credentials
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return tokenString;
    }

    public static int GetUserId(string token)
    {
        if (ValidJwtToken(token))
            return -1;
        return DecodeJwtToken(token);
    }
    public static int DecodeJwtToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(token);
        var userIdStr = jwtToken.Audiences.FirstOrDefault();
        return int.Parse(userIdStr);
    }

    public static bool ValidJwtToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = ISS,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(sercretKey))
        };

        try
        {
            SecurityToken validatedToken;
            var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}