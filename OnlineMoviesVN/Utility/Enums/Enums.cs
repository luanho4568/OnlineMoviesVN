using Inner.Libs.Helpful;

namespace OnlineMoviesVN.Utility.Enums
{
    public class Enums
    {
        public enum UserRole
        {
            [EnumAttr("Admin", "Admin")] Admin = 100,
            [EnumAttr("Manager", "Manager")] Manager = 50,
            [EnumAttr("Member", "Member")] Member = 1,
        }
    }
}
