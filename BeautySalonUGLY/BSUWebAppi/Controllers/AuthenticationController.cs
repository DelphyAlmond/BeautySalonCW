using BSUcontractmodels.BusinessLogicContracts;
using BSUcontractmodels.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BSUWebAppi.Controllers;

/// Контроллер для аутентификации пользователей
/// Отвечает за логин и генерацию JWT токенов
[Route("api/[controller]")]
[ApiController]
public class AuthenticationController(IAuthenticationBLC authBLC, ILogger<AuthenticationController> logger) : ControllerBase
{
    private readonly IAuthenticationBLC _authBLC = authBLC;
    private readonly ILogger<AuthenticationController> _logger = logger;

    /// Аутентифицировать пользователя и получить JWT токен
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] LoginRequestVM request, CancellationToken ct)
    {
        try
        {
            // Валидация входных данных
            if (string.IsNullOrWhiteSpace(request?.Username) || string.IsNullOrWhiteSpace(request?.Password))
            {
                _logger.LogWarning("Login attempt with empty username or password");
                return BadRequest(new { message = "Username and password are required" });
            }

            // Проверяем учетные данные
            var user = await _authBLC.AuthenticateAsync(request.Username, request.Password);
            
            if (user == null)
            {
                _logger.LogWarning($"Failed login attempt for user: {request.Username}");
                return Unauthorized(new { message = "Invalid username or password" });
            }

            // Генерируем JWT токен
            var token = GenerateJwtToken(user);

            // Формируем ответ
            var response = new LoginResponseVM
            {
                Token = token,
                ExpiresIn = 480, // 8 часов
                User = new UserInfoVM
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    FullName = user.FullName,
                    Role = user.Role
                }
            };

            _logger.LogInformation($"User '{request.Username}' logged in successfully");
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login");
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred during login" });
        }
    }

    /// Зарегистрировать нового пользователя
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register(
        [FromBody] RegisterRequestVM request,
        CancellationToken ct)
    {
        try
        {
            // Валидация входных данных
            if (string.IsNullOrWhiteSpace(request?.Username) ||
                string.IsNullOrWhiteSpace(request?.Email) ||
                string.IsNullOrWhiteSpace(request?.FullName) ||
                string.IsNullOrWhiteSpace(request?.Password))
            {
                _logger.LogWarning("Registration attempt with incomplete data");
                return BadRequest(new { message = "All fields are required" });
            }

            // Регистрируем пользователя
            var newUser = await _authBLC.RegisterUserAsync(
                request.Username,
                request.Email,
                request.FullName,
                request.Password,
                "User" // По умолчанию роль User
            );

            _logger.LogInformation($"User '{request.Username}' registered successfully");
            
            return CreatedAtAction(nameof(Register), new
            {
                id = newUser.Id,
                message = "User registered successfully"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during registration");
            
            if (ex.Message.Contains("already exists"))
                return Conflict(new { message = ex.Message });

            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred during registration" });
        }
    }

    /// Проверить, активен ли текущий пользователь (требует аутентификации)
    [HttpGet("verify")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> VerifyToken(CancellationToken ct)
    {
        try
        {
            // Получаем ID пользователя из claim'ов токена
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (string.IsNullOrWhiteSpace(userIdClaim))
            {
                _logger.LogWarning("Token verification failed: No user ID in token");
                return Unauthorized(new { message = "Invalid token" });
            }

            // Проверяем, активен ли пользователь
            var isActive = await _authBLC.IsUserActiveAsync(userIdClaim);
            
            if (!isActive)
            {
                _logger.LogWarning($"Token verification failed: User {userIdClaim} is inactive");
                return Unauthorized(new { message = "User is inactive" });
            }

            return Ok(new { message = "Token is valid", userId = userIdClaim });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error verifying token");
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred during token verification" });
        }
    }

    /// Генерировать JWT токен для пользователя
    private static string GenerateJwtToken(BSUcontractmodels.DataModels.UserDM user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = AuthOptions.GetSymmetricSecurityKey();

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.GivenName, user.FullName),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(8),
            Issuer = AuthOptions.ISSUER,
            Audience = AuthOptions.AUDIENCE,
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}

/// ViewModel для запроса регистрации
public class RegisterRequestVM
{
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string FullName { get; set; }
    public required string Password { get; set; }
}
