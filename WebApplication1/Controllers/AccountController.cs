using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication1.DTO;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration config;

        public AccountController(UserManager<ApplicationUser> userManager, IConfiguration config)
        {
            this.userManager = userManager;
            this.config = config;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDTO userDto)//(un,ps,email)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser appuser = new ApplicationUser()
                {
                    UserName = userDto.UserName,
                    Email = userDto.Email,
                    PasswordHash = userDto.Password
                };
                IdentityResult result =
                    await userManager.CreateAsync(appuser, userDto.Password);
                if (result.Succeeded)
                {
                    return Ok("Account Created");
                }
                return BadRequest(result.Errors);
            }
            return BadRequest(ModelState);
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO userDto)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser? userFromDb =
                     await userManager.FindByNameAsync(userDto.UserName);
                if (userFromDb != null)
                {
                    bool found = await userManager.CheckPasswordAsync(userFromDb, userDto.Password);
                    if (found)
                    {
                        List<Claim> myclaims = new List<Claim>();
                        myclaims.Add(new Claim(ClaimTypes.Name, userFromDb.UserName));
                        myclaims.Add(new Claim(ClaimTypes.NameIdentifier, userFromDb.Id));
                        myclaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

                        var roles = await userManager.GetRolesAsync(userFromDb);
                        foreach (var role in roles)
                        {
                            myclaims.Add(new Claim(ClaimTypes.Role, role));
                        }


                        var SignKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(config["JWT:SecritKey"]));

                        SigningCredentials signingCredentials =
                            new SigningCredentials(SignKey, SecurityAlgorithms.HmacSha256);

                        JwtSecurityToken mytoken = new JwtSecurityToken(
                            issuer: config["JWT:ValidIss"],
                            audience: config["JWT:ValidAud"],
                            expires: DateTime.Now.AddHours(2),
                            claims: myclaims,
                            signingCredentials: signingCredentials);
                        return Ok(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(mytoken),
                            expired = mytoken.ValidTo
                        });
                    }
                }
                return Unauthorized("Invalid account");
            }
            return BadRequest(ModelState);
        }
    }
}
