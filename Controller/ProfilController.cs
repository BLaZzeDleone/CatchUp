using ExpenseTracker.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;



[ApiController]
[Route("[controller]")]
public class ProfilController : ControllerBase
{
    private readonly AppDbContext _db;

    private readonly IConfiguration _configuration;
    public ProfilController(AppDbContext db, IConfiguration configuration)
    {
        _db = db;
        _configuration = configuration;
    }

    [HttpPost]
    public ActionResult<ProfilController> CreateProfil([FromBody] ProfilDTO profilDTO)
    {
        
        return StatusCode(StatusCodes.Status201Created)
    }
}