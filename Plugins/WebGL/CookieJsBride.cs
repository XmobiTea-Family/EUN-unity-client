namespace XmobiTea.EUN.Plugin.WebGL
{
    using System.Runtime.InteropServices;

    public class CookieJsBride
    {
#if EUN
        [DllImport("__Internal")]
        public static extern void SetCookie(string cname, string cvalue, int expiredMs = 1 * 1000 * 60 * 60 * 24);

        [DllImport("__Internal")]
        public static extern string GetCookie(string cname);

        [DllImport("__Internal")]
        public static extern void DeleteCookie(string cname);
#endif
    }
}