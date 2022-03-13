namespace EUN.Bride.WebSocket
{
#if EUN
    using com.tvd12.ezyfoxserver.client.constant;
    using com.tvd12.ezyfoxserver.client.entity;
    using com.tvd12.ezyfoxserver.client.factory;
#else
    using EUN.Entity.Support;
#endif

    using EUN.Common;
    using EUN.Constant;
    using EUN.Helper;
    using EUN.Plugin.WebGL;

    using System.Collections.Generic;

    public class WebSocketEzySocketObject : EzySocketObject
    {
        protected override void OnCustomStart()
        {
            base.OnCustomStart();

#if EUN
            EzyClientJsBride.EzyLibrary();
#endif
        }

        public override void Init(string _zoneName, string _appName)
        {
            base.Init(_zoneName, _appName);
#if EUN
            EzyClientJsBride.EzyInit(this.gameObject.name, _zoneName, _appName);
#endif
        }

        public override void Connect(string username, string password, CustomData data, string host, int port, int udpPort)
        {
            base.Connect(username, password, data, host, port, udpPort);
#if EUN
            var data1 = data == null ? EzyEntityFactory.EMPTY_ARRAY : data.ToEzyData();

            EzyClientJsBride.EzyConnect(username, password, data1.ToString(), host);
#endif
        }

#if EUN
        public override void Send(EzyObject request, bool reliable = true)
        {
            if (EzyClientJsBride.EzySend(Serializer.Serialize(request.toDict<object, object>())))
            {
                //if (reliable) app.send(Commands.RequestCmd, request);
                //else app.udpSend(Commands.RequestCmd, request);
            }
            else
            {
                var data = request.get<EzyArray>(Commands.Data);

                var customArray = new CustomArray();
                customArray.Add((int)ReturnCode.AppNullRequest);
                customArray.Add((string)null);
                if (data.size() > 2) customArray.Add(data.get<int>(2));
                //var ezyData = EzyEntityFactory.newArray();
                //ezyData.add((int)ReturnCode.AppNullRequest);
                //ezyData.add((string)null);
                //if (data.size() > 2) ezyData.add(data.get<int>(2));

                onResponse?.Invoke(customArray);
            }
        }
#endif

        private void handleOnConnectionSuccess()
        {
            onConnectionSuccess?.Invoke();
        }

        private void handleOnDisconnection(int reason)
        {
            onDisconnection?.Invoke((EzyDisconnectReason)reason);
        }

        private void handleOnConnectionFailure(int reason)
        {
            onConnectionFailure.Invoke((EzyConnectionFailedReason)reason);
        }

        private void handleOnLoginError(string jsonArr)
        {
            onLoginError?.Invoke(new CustomArray.Builder().AddAll(getObjectLstFromJsonArr(jsonArr)).Build());
        }

        private void handleOnAppAccess(string jsonArr)
        {
            onAppAccess?.Invoke(new CustomArray.Builder().AddAll(getObjectLstFromJsonArr(jsonArr)).Build());
        }

        private void handleOnResponse(string jsonArr)
        {
            onResponse?.Invoke(new CustomArray.Builder().AddAll(getObjectLstFromJsonArr(jsonArr)).Build());
        }

        private void handleOnEvent(string jsonArr)
        {
            onEvent?.Invoke(new CustomArray.Builder().AddAll(getObjectLstFromJsonArr(jsonArr)).Build());
        }

        private IList<object> getObjectLstFromJsonArr(string jsonArr)
        {
            var jsonString = "{\"data\":$data}".Replace("$data", jsonArr);
            var jsonDic = Parser.Parse(jsonString) as IDictionary<string, object>;
            return jsonDic["data"] as IList<object>;
        }
    }
}