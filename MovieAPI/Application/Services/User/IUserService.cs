
namespace MovieAPI.Application.Services.User;

public interface IUserService
{
    Task<LoginUserResponse> LoginAsync(SignInRequestModel requestModel);
    Task<CreateUserResponse> CreateAsync(CreateUserModel model);
    Task UpdateRefreshTokenAsync(string refreshToken, AppUser? user, DateTime accessTokenDate, int addOnAccessTokenDate);
}