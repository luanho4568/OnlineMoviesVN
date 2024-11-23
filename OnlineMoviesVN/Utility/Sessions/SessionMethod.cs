using System.Text.Json;

namespace OnlineMoviesVN.Utility.Sessions
{
    public static class SessionMethod
    {
        /*
         * Lưu data vào session
         **/
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        /*
         * Lấy data từ session
         * **/
        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }

        /*
         * Xoá data khỏi session
         * **/
        public static void RemoveSession(this ISession session, string key)
        {
            session.Remove(key);
        }
    }
}
