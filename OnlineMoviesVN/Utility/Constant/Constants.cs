namespace OnlineMoviesVN.Utility.Constant
{
    public static class StorageConstants
    {
        public const string KeyTokenCookie = "jwt";
        public const string KeySessionUser = "user";
        public const string KeyRememberCookie = "remember";
    }
    public static class RoleConstants
    {
        public const string Admin = "ADMIN";
        public const string Member = "MEMBER";
    }
    public static class AccountTypeConstants
    {
        public const string Local = "LOCAL";
        public const string Google = "GOOGLE";
        public const string Facebook = "FACEBOOK";
    }
    public static class UserStatusConstants
    {
        public const bool Status = true;
        public const bool InStatus = false;
    }
    public static class ApiEndpoints
    {

    }
    public static class FilePathConstants
    {
        public const string AvatarPath = @"Uploads\users";
        public const string MoviePosterPath = @"Uploads\posters";
    }

    public static class ActiveTypeConstants
    {
        public const string Login = "LOGIN";
        public const string Logout = "LOGOUT";
        public const string Register = "REGISTER";
    }
}
