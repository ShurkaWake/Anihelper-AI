namespace API.Requests.Auth
{
    public sealed record RegisterRequest(
        string FullName,
        string Username,
        string Email,
        string Password,
        string RepeatPassword
        );
}
