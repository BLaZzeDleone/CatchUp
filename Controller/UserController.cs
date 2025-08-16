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
            System.Console.WriteLine("userName already exists");
            return StatusCode(StatusCodes.Status409Conflict);
        }
            UserRegistrationInformations authentication = new UserRegistrationInformations()
        {
            username = userWorkingDTO.username,
            password = userWorkingDTO.password
        };

        _db.user.Add(authentication);
        _db.SaveChanges();

        return StatusCode(StatusCodes.Status201Created, authentication);
    }
    [HttpPost("Login")]
    public ActionResult<UserRegistrationInformations> Login([FromBody] UserWorkingDTO userWorkingDTO)
    {
        var authentication = _db.user.FirstOrDefault(u => u.username == userWorkingDTO.username && u.password == userWorkingDTO.password);
        userWorkingDTO.id = authentication.id.ToString();
        User comparedAuthentication = new User()
        {
            username = userWorkingDTO.username,
            password = userWorkingDTO.password,
        };

        if (authentication!= null)
        {
            new Claim(ClaimTypes.NameIdentifier, userWorkingDTO.id);
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"] ?? ""));
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userWorkingDTO.id),
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
