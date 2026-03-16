using AP_FINAL.DBContext;
using DataPOO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RepositoryPOO;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace AP_FINAL.Controllers
{
    [ApiController]
    [Route("Auth")]
    public class AuthController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration; // ← ajouté

        public AuthController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ITokenService tokenService,
            IConfiguration configuration) // ← ajouté
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _configuration = configuration; // ← ajouté
        }

        // POST: api/auth/register
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return BadRequest(new { errors = result.Errors.Select(e => e.Description) });

            await _userManager.AddToRoleAsync(user, "User");

            // ← ajouté : créer l'utilisateur métier

            try
            {
                MysqlRepository repo = new MysqlRepository(_configuration.GetConnectionString("DefaultConnection")!);
                var utilisateur = new Utilisateurs()
                {
                    Nom = model.Nom,
                    Prenom = model.Prenom,
                    Email = model.Email,
                    Ville = model.Ville,
                    Rue = model.Rue,
                    NumeroDeTelephone = model.NumeroDeTelephone,
                    CodePostal = model.CodePostal,
                    MotDePasse = "",
                    AspNetUserId = user.Id
                };
                repo.SaveObject(utilisateur);

                 // debug 
                //int saveResult = repo.SaveObject(utilisateur);
                //return StatusCode(500, new { debug = saveResult, aspNetId = user.Id, nom = model.Nom });

                var roles = await _userManager.GetRolesAsync(user);
                var token = _tokenService.GenerateAccessToken(user, roles);
                var refreshToken = _tokenService.GenerateRefreshToken();

                return Ok(new
                {
                    message = "Inscription réussie",
                    accessToken = token,
                    refreshToken = refreshToken,
                    expiresIn = 3600, // secondes (1 heure)
                    user = new
                    {
                        id = user.Id,
                        email = user.Email,
                        username = user.UserName,
                        roles = roles
                    }
                });

            }
            catch (Exception ex)
            {
                // En cas d'erreur lors de la création de l'utilisateur métier, supprimer l'utilisateur Identity créé
                return StatusCode(500, new { erreur = ex.Message });
            }

        }

        // POST: api/auth/login
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Vérifier que l'utilisateur existe
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return Unauthorized(new { message = "Email ou mot de passe incorrect" });

            // Vérifier le mot de passe
            var isValidPassword = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!isValidPassword)
                return Unauthorized(new { message = "Email ou mot de passe incorrect" });

            // Générer les tokens
            var roles = await _userManager.GetRolesAsync(user);
            var token = _tokenService.GenerateAccessToken(user, roles);
            var refreshToken = _tokenService.GenerateRefreshToken();

            return Ok(new
            {
                message = "Connexion réussie",
                accessToken = token,
                refreshToken = refreshToken,
                expiresIn = 3600, // secondes (1 heure)
                user = new
                {
                    id = user.Id,
                    email = user.Email,
                    username = user.UserName,
                    roles = roles
                }
            });
        }

        // GET: api/auth/me
        [HttpGet("Me")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            // Récupérer l'utilisateur depuis le token JWT
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId!);

            if (user == null)
                return NotFound(new { message = "Utilisateur non trouvé" });

            var roles = await _userManager.GetRolesAsync(user);

            return Ok(new
            {
                id = user.Id,
                email = user.Email,
                username = user.UserName,
                roles = roles,
                emailConfirmed = user.EmailConfirmed
            });
        }

        [HttpPut("ChangePwd")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId!);

            if (user == null)
                return NotFound(new { message = "Utilisateur non trouvé" });

            var result = await _userManager.ChangePasswordAsync(
                user,
                model.CurrentPassword,
                model.NewPassword
            );

            if (!result.Succeeded)
                return BadRequest(new { errors = result.Errors.Select(e => e.Description) });

            return Ok(new { message = "Mot de passe modifié avec succès" });
        }
    }

    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(8)]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string Nom { get; set; } = string.Empty;        // ← ajouté

        [Required]
        public string Prenom { get; set; } = string.Empty;     // ← ajouté

        public string Ville { get; set; } = string.Empty;      // ← ajouté
        public string Rue { get; set; } = string.Empty;        // ← ajouté
        public string NumeroDeTelephone { get; set; } = string.Empty; // ← ajouté
        public string CodePostal { get; set; } = string.Empty; // ← ajouté
    }

    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }

    public class ChangePasswordDto
    {
        [Required]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required]
        [MinLength(8)]
        public string NewPassword { get; set; } = string.Empty;
    }
}