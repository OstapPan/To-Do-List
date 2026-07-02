using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TO_DO_List.Models.DTOs; // Переконайся, що тут є класи RegisterDto та LoginDto

namespace TO_DO_List.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        // ВИПРАВЛЕНО: Додали SignInManager у конструктор
        public AuthController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userExists = await _userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
                return BadRequest(new { Message = "Користувач з таким Email вже існує." });

            IdentityUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Email 
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return BadRequest(new { Message = "Помилка валідації даних.", Errors = errors });
            }

            // Автоматичний вхід після реєстрації
            await _signInManager.SignInAsync(user, isPersistent: false);

            return Ok(new { Message = "Реєстрація та вхід пройшли успішно!" });
        }

        // ДОДАНО: Метод для входу в існуючий акаунт
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            // Перевіряємо логін і пароль. Встановлюємо Cookie.
            var result = await _signInManager.PasswordSignInAsync(
                model.Email, 
                model.Password, 
                isPersistent: false, // Сесія до закриття браузера
                lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return Ok(new { Message = "Вхід успішний!" });
            }

            return Unauthorized(new { Message = "Невірний email або пароль." });
        }

        // ДОДАНО: Метод для виходу з акаунта
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            // Видаляємо Cookie авторизації
            await _signInManager.SignOutAsync();
            return Ok(new { Message = "Вихід успішний!" });
        }
    }
}