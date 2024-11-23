namespace OnlineMoviesVN.Utility.Cookies
{
    public static class CookieMethod
    {
        /*
         * Lưu cookie
         * 
         * **/
        public static void SetCookie(this HttpResponse response, string key, string value, int expireTime)
        {
            var options = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTimeOffset.Now.AddDays(expireTime),
                Secure = true,
            };
            response.Cookies.Append(key, value, options);
        }

        /*
         * Lấy cookie
         * **/

        public static string GetCookie(this HttpRequest request, string key)
        {
            request.Cookies.TryGetValue(key, out var value);
            return value;
        }

        /*
         * Xoá cookie
         * **/

        public static void DeleteCookie(this HttpResponse response, string key)
        {
            response.Cookies.Delete(key);
        }

        /*
         * Kiểm tra cookie có tồn tại chưa
         * **/
        public static bool ContainsKey(this HttpRequest request, string key)
        {
            return request.Cookies.ContainsKey(key);
        }
    }
}
