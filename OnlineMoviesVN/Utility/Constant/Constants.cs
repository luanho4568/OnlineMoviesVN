namespace OnlineMoviesVN.Utility.Constant
{
    public static class StorageConstants
    {
        public const string KeyTokenCookie = "jwt";
        public const string KeySessionUser = "user";
    }
    public static class RoleConstants
    {
        public const string Admin = "ADMIN";
        public const string User = "MEMBER";
    }
    public static class AccountTypeConstants
    {
        public const string Local = "LOCAL";
        public const string Google = "GOOGLE";
        public const string Phone = "PHONE";
    }
    public static class UserStatusConstants
    {
        public const bool Active = true;
        public const bool Inactive = false;
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
        public const string logout = "LOGOUT";
    }
}
