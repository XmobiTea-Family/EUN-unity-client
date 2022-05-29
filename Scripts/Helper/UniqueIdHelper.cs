namespace XmobiTea.EUN.Helper
{
    using System.Text;
    using UnityEngine;

    public static class UniqueIdHelper
    {
        private const string UniqueId = "UniqueId";

        /// <summary>
        /// Get an id for this device
        /// </summary>
        /// <returns></returns>
        public static string GetId()
        {
            var deviceId = string.Empty;

#if UNITY_EDITOR
            deviceId = "_Editor_" + SystemInfo.deviceUniqueIdentifier;
#else
#if UNITY_IOS
            deviceId = PlayerPrefs.GetString(UniqueId);
            if (string.IsNullOrEmpty(deviceId))
            {
                deviceId = System.Guid.NewGuid().ToString();
                PlayerPrefs.SetString(UniqueId, deviceId);
            }
#elif UNITY_WEBGL
            deviceId = EUN.Plugin.WebGL.CookieJsBride.GetCookie(UniqueId);
            Debug.LogError(deviceId);
            if (string.IsNullOrEmpty(deviceId))
            {
                deviceId = System.Guid.NewGuid().ToString();
                EUN.Plugin.WebGL.CookieJsBride.SetCookie(UniqueId, deviceId, 1000 * 60 * 60 * 24 * 7);
            }
#else
            deviceId = SystemInfo.deviceUniqueIdentifier;
#endif
#endif

            return GenerateMD5(deviceId);
        }

        private static string GenerateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}
