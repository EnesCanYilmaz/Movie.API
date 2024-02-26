namespace MovieAPI.Controllers;

[Route("api/v1/[controller]/[action]")]
[ApiController]
public class AuthController : BaseApiController
{
    private readonly IUserService _userService;
    private readonly ITokenHandler _tokenHandler;

    public AuthController(IUserService userService, ITokenHandler tokenHandler)
    {
        _userService = userService;
        _tokenHandler = tokenHandler;
    }

    [HttpPost]
    public async Task<IActionResult> Login(SignInRequestModel signInModel)
    {
        var loginResponse = await _userService.LoginAsync(signInModel);

        if (!loginResponse.Succeeded)
            return OK(400, loginResponse.Message, null);

        var token = _tokenHandler.CreateAccessToken(900, loginResponse.User);
        await _userService.UpdateRefreshTokenAsync(token.RefreshToken, loginResponse.User, token.Expiration, 15);
        return OK(400, loginResponse.Message, token);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateUserModel createUserModel)
    {
        var createUserResult = await _userService.CreateAsync(new CreateUserModel() { Email = createUserModel.Email, PhoneNumber = createUserModel.PhoneNumber, UserName = createUserModel.UserName, Password = createUserModel.Password });

        return !createUserResult.Succeeded ? OK(400, createUserResult.Message, null) : OK(200, createUserResult.Message, null);
    }
}