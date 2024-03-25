namespace XmobiTea.EUN.Helper
{
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
            deviceId = EUN.Plugin.WebGL.CookieJsBride.getCookie(UniqueId);
            Debug.LogError(deviceId);
            if (string.IsNullOrEmpty(deviceId))
            {
                deviceId = System.Guid.NewGuid().ToString();
                EUN.Plugin.WebGL.CookieJsBride.setCookie(UniqueId, deviceId, 1000 * 60 * 60 * 24 * 7);
            }
#else
            deviceId = SystemInfo.deviceUniqueIdentifier;
#endif
#endif

            return deviceId;
        }

    }

}
