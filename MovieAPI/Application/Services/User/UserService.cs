namespace MovieAPI.Application.Services.User;

public class UserService : IUserService
{
    private readonly UserManager<AppUser?> _userManager;
    private readonly SignInManager<AppUser> _signInManager;

    public UserService(UserManager<AppUser?> userManager, SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<LoginUserResponse> LoginAsync(SignInRequestModel requestModel)
    {
        var response = new LoginUserResponse();
        
        var user = 
            await _userManager.FindByEmailAsync(requestModel.UserNameOrEmail) ?? 
            await _userManager.FindByNameAsync(requestModel.UserNameOrEmail);

        if (user is null || !(await _signInManager.CheckPasswordSignInAsync(user, requestModel.Password, false)).Succeeded)
        {
            response.Message = user == null ? "Kullanıcı bulunamadı." : "Kullanıcı adı, email veya şifrenizi kontrol ediniz.";
            response.Succeeded = false;
        }

        response.Message = "Giriş başarılı";
        response.Succeeded = true;
        response.User = user;
        return response;
    }
    
    public async Task<CreateUserResponse> CreateAsync(CreateUserModel model)
    {
        var result = await _userManager.CreateAsync(new AppUser { UserName = model.UserName, Email = model.UserName, PhoneNumber = model.PhoneNumber }, model.Password);

        CreateUserResponse response = new();

        if (result.Succeeded)
        {
            response.Message = "Kullanıcı başarıyla oluşturulmuştur.";
            response.Succeeded = true;
        }
        else
        {
            response.Succeeded = false;
            response.Message = "Kullanıcı oluşturulurken bir hata meydana geldi.";
        }

        return response;
    }

    public async Task UpdateRefreshTokenAsync(string refreshToken, AppUser? user, DateTime accessTokenDate, int addOnAccessTokenDate)
    {
        user.RefreshToken = refreshToken;
        user.RefreshTokenEndDate = accessTokenDate.AddSeconds(addOnAccessTokenDate);
        await _userManager.UpdateAsync(user);
    }
}