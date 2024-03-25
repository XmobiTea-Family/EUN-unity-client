namespace XmobiTea.EUN.Plugin.WebGL
{
#if EUN_USING_ONLINE
    using System.Runtime.InteropServices;
#endif

    public class CookieJsBride
    {
#if EUN_USING_ONLINE
        [DllImport("__Internal")]
        public static extern void setCookie(string cname, string cvalue, int expiredMs = 1 * 1000 * 60 * 60 * 24);

        [DllImport("__Internal")]
        public static extern string getCookie(string cname);

        [DllImport("__Internal")]
        public static extern void deleteCookie(string cname);
#endif

    }
}
