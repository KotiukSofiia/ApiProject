using ApiProject.Models;
using ApiProject.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[Route("api/Auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private List<Person> people = new()
    {
        new Person { Login = "admin", Password = "admin123", Role = "Admin" },
        new Person { Login = "user", Password = "user123", Role = "User" }
    };

    [HttpPost("login")]
    public IActionResult Login([FromBody] Person person)
    {
        var identity = GetIdentity(person.Login, person.Password);
        if (identity == null) return Unauthorized(new { message = "Невірний логін або пароль" });

        var now = DateTime.UtcNow;
        var jwt = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            notBefore: now,
            claims: identity.Claims,
            expires: now.AddMinutes(AuthOptions.LIFETIME),
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
        );

        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

        return Ok(new { token = encodedJwt });
    }

    private ClaimsIdentity GetIdentity(string login, string password)
    {
        var person = people.FirstOrDefault(x => x.Login == login && x.Password == password);
        if (person == null) return null;

        var claims = new List<Claim>
        {
            new Claim(ClaimsIdentity.DefaultNameClaimType, person.Login),
            new Claim(ClaimsIdentity.DefaultRoleClaimType, person.Role)
        };

        return new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
    }
}
