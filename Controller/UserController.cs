using ExpenseTracker.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;



[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly AppDbContext _db;

    private readonly IConfiguration _configuration;
    public UserController(AppDbContext db, IConfiguration configuration)
    {
        _db = db;
        _configuration=configuration;
    }

    [HttpPost("Register")]
    public ActionResult<IEnumerable<UserRegistrationInformations>> Registration([FromBody]UserRegistrationInformationsDTO userRegistrationInformationsDTO)
    {
        var databaseOutput = _db.userRegistrationInformations.Where(u => u.emailAdress == userRegistrationInformationsDTO.emailAdress).ToList();
        if (databaseOutput.Count != 0)
        {
            return StatusCode(StatusCodes.Status409Conflict);
        }
        UserRegistrationInformations userRegistrationInformations = new UserRegistrationInformations()
        {
            emailAdress = userRegistrationInformationsDTO.emailAdress,
            telefonNumber = userRegistrationInformationsDTO.telefonNumber,
            password = userRegistrationInformationsDTO.password
        };

        _db.userRegistrationInformations.Add(userRegistrationInformations);
        _db.SaveChanges();

        return StatusCode(StatusCodes.Status201Created, userRegistrationInformations);
    }
    [HttpPost("Login")]
    public ActionResult<UserRegistrationInformations> Login([FromBody] UserRegistrationInformationsDTO userRegistrationInformationsDTO)
    {
        var authentication = _db.userRegistrationInformations.FirstOrDefault(u => u.emailAdress == userRegistrationInformationsDTO.emailAdress && u.password == userRegistrationInformationsDTO.password && u.telefonNumber == userRegistrationInformationsDTO.telefonNumber);
        userRegistrationInformationsDTO.id = authentication.id.ToString();
        UserRegistrationInformations comparedUserrRegistrationInformations = new UserRegistrationInformations()
        {
            emailAdress = userRegistrationInformationsDTO.emailAdress,
            telefonNumber = userRegistrationInformationsDTO.telefonNumber,
            password = userRegistrationInformationsDTO.password
        };

        if (authentication!= null)
        {
            new Claim(ClaimTypes.NameIdentifier, userRegistrationInformationsDTO.id);
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"] ?? ""));
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userRegistrationInformationsDTO.id),
            };
            var jwtToken = new JwtSecurityToken(
              expires: DateTime.Now.AddMinutes(15),
              claims: claims,
              signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            var response = new
            {
                Jwt = new JwtSecurityTokenHandler().WriteToken(jwtToken)
            };

            return StatusCode(StatusCodes.Status200OK, response);
        }
        else
        {
            System.Console.WriteLine("Userusername or password are incorrect");
            return StatusCode(StatusCodes.Status404NotFound);
        }
    }
}
