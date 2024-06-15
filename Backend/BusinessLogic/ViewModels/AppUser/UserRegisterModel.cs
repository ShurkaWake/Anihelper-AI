namespace BusinessLogic.ViewModels.AppUser
{
    public sealed record UserRegisterModel(
        string FullName,
        string Username,
        string Email,
        string Password,
        string RepeatPassword
        );
}
