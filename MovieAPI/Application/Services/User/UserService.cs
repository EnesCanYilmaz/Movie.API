namespace MovieAPI.Application.Services.User;

public class UserService : IUserService
{
    private readonly UserManager<AppUser> _userManager;

    public UserService(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
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

    public async Task UpdateRefreshTokenAsync(string refreshToken, AppUser user, DateTime accessTokenDate, int addOnAccessTokenDate)
    {
        user.RefreshToken = refreshToken;
        user.RefreshTokenEndDate = accessTokenDate.AddSeconds(addOnAccessTokenDate);
        await _userManager.UpdateAsync(user);
    }
}