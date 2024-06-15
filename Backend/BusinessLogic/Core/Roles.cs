namespace BusinessLogic.Core
{
    public static class Roles
    {
        public const string Admin = "admin";

        public const string Artist = "artist";

        public static string[] AllowedRoles => new[] { Admin, Artist };
    }
}