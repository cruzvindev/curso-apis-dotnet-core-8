using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    
    private readonly ITokenService _tokenService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthController> _logger;
    
    public AuthController(ITokenService tokenService, UserManager<ApplicationUser> userManager, 
        RoleManager<IdentityRole> roleManager, IConfiguration configuration, ILogger<AuthController> logger)
    {
        _tokenService = tokenService;
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _logger = logger;
    }
    
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        //Busca um usuário
        var user = await _userManager.FindByNameAsync(model.UserName!);
        
                                                    //Verifica se a senha passada bate com a do usuário registrado
        if (user is not null && await _userManager.CheckPasswordAsync(user, model.Password!)) //! => Operador de supressão de nulidade
        {
            var userRoles = await _userManager.GetRolesAsync(user); //Obtém as roles do user

            //Incluindo claims que vão fazer parte do token
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim("id", user.UserName!),
                //Json Web Tokens Id(JTI), fornece um identificador único para o token
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) 
            };

            //Insere as claims do user na lista contendo as novas claims
            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var token = _tokenService.GenerateAccessToken(authClaims, _configuration);
            var refreshToken = _tokenService.GenerateRefreshToken();

            //Operador discard -> O valor da variável pode ser descartado, afinal será a variável do out a receber um valor e assim não há alocação de memória desnecessária
            //Indica que essa variável nunca é usada no código
            _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInMinutes"],
                out int refreshTokenValidityInMinutes);

            user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(refreshTokenValidityInMinutes);
            user.RefreshToken = refreshToken;
            await _userManager.UpdateAsync(user);

            return Ok(new
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken,
                Expiration = token.ValidTo
            });
        }

        return Unauthorized();
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        var userExists = await _userManager.FindByNameAsync(model.Username!);

        if (userExists != null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new Response { Status = "Error", Message = "User already exists!" });
        }

        ApplicationUser user = new()
        {
            Email = model.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = model.Username
        };

        var resut = await _userManager.CreateAsync(user, model.Password!);

        if (!resut.Succeeded)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new Response { Status = "Error", Message = "User creation failed." });
        }

        return Ok(new Response { Status = "Sucess", Message = "User created sucessfully!" });
    }

    [HttpPost]
    [Route("refresh-token")]
    public async Task<IActionResult> RefreshToken(TokenModel tokenModel)
    {
        if (tokenModel is null)
        {
            return BadRequest("Invalid client request");
        }

        string? acessToken = tokenModel.AcessToken ?? throw new ArgumentNullException(nameof(tokenModel));
        string? refreshToken = tokenModel.RefreshTokem ?? throw new ArgumentException((nameof(tokenModel)));

        var principal = _tokenService.GetPrincipalFromExpiredToken(acessToken!, _configuration);

        if (principal == null)
        {
            return BadRequest("Invalid acess token/refresh token");
        }

        string userName = principal.Identity.Name;
        var user = await _userManager.FindByNameAsync(userName!);

        if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
        {
            return BadRequest("Invalid acess token/refresh token");
        }

        var newAcessToken = _tokenService.GenerateAccessToken(principal.Claims.ToList(), _configuration);
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        await _userManager.UpdateAsync(user);

        return new ObjectResult(new
        {
            acessToken = new JwtSecurityTokenHandler().WriteToken(newAcessToken),
            refreshToken = newRefreshToken
        });
    }

    [Authorize(Policy = "ExclusiveOnly")]
    [HttpPost]
    [Route("revoke/{userName}")]
    public async Task<IActionResult> Revoke(string username)
    {
        var user = await _userManager.FindByNameAsync(username);

        if (user == null) return BadRequest("Invalid user name");

        user.RefreshToken = null;

        await _userManager.UpdateAsync(user);
        return NoContent();
    }

    [HttpPost]
    [Route("CreateRole")]
    [Authorize(Policy = "SuperAdminOnly")]
    public async Task<IActionResult> CreateRole(string roleName)
    {
        var roleExist = await _roleManager.RoleExistsAsync(roleName);
        if (!roleExist)
        {
            var roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));

            if (roleResult.Succeeded)
            {
                _logger.LogInformation(1, "Roles Added");
                return StatusCode(StatusCodes.Status200OK,
                    new Response { Status = "Sucess", Message = $"Role {roleName} added sucessfully" });
            }
            else
            {
                _logger.LogInformation(2, "Error");
                return StatusCode(StatusCodes.Status400BadRequest,
                    new Response { Status = "Error", Message = $"Issue adding the new {roleName} role" });
            }
        }

        return StatusCode(StatusCodes.Status400BadRequest,
            new Response { Status = "Error", Message = "Role already exist" });
    }

    [HttpPost]
    [Route("AddUserToRole")]
    [Authorize(Policy = "SuperAdminOnly")]
    public async Task<IActionResult> AddUserToRole(string email, string roleName)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is not null)
        {
            var result = await _userManager.AddToRoleAsync(user, roleName);

            if (result.Succeeded)
            {
                _logger.LogInformation(1, $"User {user.Email} added to the {roleName} role");
                return StatusCode(StatusCodes.Status200OK,
                    new Response { Status = "Success", Message = $"User {user.Email} added to the {roleName} role" });
            }
            else
            {
                _logger.LogInformation(1, $"Error: Unable to add user {user.Email} to the {roleName} role");

                return StatusCode(StatusCodes.Status400BadRequest,
                    new Response
                    {
                        Status = "Error", Message = $"Error: Unable to add user {user.Email} to the {roleName} role"
                    });
            }
        }
        return BadRequest(new { error = "Unable to find user" });
    }
}