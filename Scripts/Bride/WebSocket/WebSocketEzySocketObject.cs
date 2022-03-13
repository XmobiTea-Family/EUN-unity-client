#if EUN
namespace EUN.Bride.WebSocket
{
    using com.tvd12.ezyfoxserver.client.constant;
    using com.tvd12.ezyfoxserver.client.entity;
    using com.tvd12.ezyfoxserver.client.factory;

    using EUN.Constant;
    using EUN.Helper;
    using EUN.Plugin.WebGL;

    using System.Collections.Generic;

    public class WebSocketEzySocketObject : EzySocketObject
    {
        protected override void OnCustomStart()
        {
            base.OnCustomStart();

            EzyClientJsBride.EzyLibrary();
        }

        public override void Init(string _zoneName, string _appName)
        {
            base.Init(_zoneName, _appName);

            EzyClientJsBride.EzyInit(this.gameObject.name, _zoneName, _appName);
        }

        public override void Connect(string username, string password, EzyData data, string host, int port, int udpPort)
        {
            base.Connect(username, password, data, host, port, udpPort);

            var data1 = data == null ? EzyEntityFactory.EMPTY_ARRAY : (EzyArray)data;

            EzyClientJsBride.EzyConnect(username, password, data1.ToString(), host);
        }

        public override void Send(EzyObject request, bool reliable = true)
        {
            if (EzyClientJsBride.EzySend(Serializer.Serialize(request.toDict<object, object>())))
            {
                //if (reliable) app.send(Commands.RequestCmd, request);
                //else app.udpSend(Commands.RequestCmd, request);
            }
            else
            {
                var data = request.get<EzyArray>("d");

                var ezyData = EzyEntityFactory.newArray();
                ezyData.add((int)ReturnCode.AppNullRequest);
                ezyData.add((string)null);
                if (data.size() > 2) ezyData.add(data.get<int>(2));

                onResponse?.Invoke(ezyData);
            }
        }

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
            var ezyArray = EzyEntityFactory.newArrayBuilder()
                .appendAll(getObjectLstFromJsonArr(jsonArr))
                .build();

            onLoginError?.Invoke(ezyArray);
        }

        private void handleOnAppAccess(string jsonArr)
        {
            var ezyArray = EzyEntityFactory.newArrayBuilder()
                .appendAll(getObjectLstFromJsonArr(jsonArr))
                .build();

            onAppAccess?.Invoke(ezyArray);
        }

        private void handleOnResponse(string jsonArr)
        {
            var ezyArray = EzyEntityFactory.newArrayBuilder()
                .appendAll(getObjectLstFromJsonArr(jsonArr))
                .build();

            onResponse?.Invoke(ezyArray);
        }

        private void handleOnEvent(string jsonArr)
        {
            var ezyArray = EzyEntityFactory.newArrayBuilder()
                .appendAll(getObjectLstFromJsonArr(jsonArr))
                .build();

            onEvent?.Invoke(ezyArray);
        }

        private IList<object> getObjectLstFromJsonArr(string jsonArr)
        {
            var jsonString = "{\"data\":$data}".Replace("$data", jsonArr);
            var jsonDic = Parser.Parse(jsonString) as IDictionary<string, object>;
            return jsonDic["data"] as IList<object>;
        }
    }
}
#endif