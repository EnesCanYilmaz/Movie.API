using MovieAPI.Application.Services.Token;
using MovieAPI.Application.Services.User;

namespace MovieAPI.Controllers;

[Route("api/v1/[controller]/[action]")]
[ApiController]
public class AuthController : BaseApiController
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IUserService _userService;
    private readonly ITokenHandler _tokenHandler;
    private readonly SignInManager<AppUser> _signInManager;

    public AuthController(UserManager<AppUser> userManager, IUserService userService, SignInManager<AppUser> signInManager, ITokenHandler tokenHandler)
    {
        _userManager = userManager;
        _userService = userService;
        _signInManager = signInManager;
        _tokenHandler = tokenHandler;
    }

    [HttpPost]
    public async Task<IActionResult> Login(SignInRequestModel signInModel)
    {
        var user = await _userManager.FindByEmailAsync(signInModel.UserNameOrEmail) ?? await _userManager.FindByNameAsync(signInModel.UserNameOrEmail);

        if (user is null)
            return OK(400, "Kullanıcı bulunamadı.", null);

        var signInResult = await _signInManager.CheckPasswordSignInAsync(user, signInModel.Password, false);

        if (!signInResult.Succeeded)
            return OK(400, "Giriş başarısız", null);

        var token = _tokenHandler.CreateAccessToken(900, user);
        await _userService.UpdateRefreshTokenAsync(token.RefreshToken, user, token.Expiration, 15);
        return OK(400, "Giriş başarılı.", token);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateUserModel createUserModel)
    {
        var createUserResult = await _userService.CreateAsync(new CreateUserModel() { Email = createUserModel.Email, PhoneNumber = createUserModel.PhoneNumber, UserName = createUserModel.UserName, Password = createUserModel.Password });

        return !createUserResult.Succeeded ? OK(400, createUserResult.Message, null) : OK(200, createUserResult.Message, null);
    }
}