namespace XmobiTea.EUN.Bride.WebSocket
{
#if EUN
    using com.tvd12.ezyfoxserver.client.constant;
    using com.tvd12.ezyfoxserver.client.entity;
    using com.tvd12.ezyfoxserver.client.factory;
#else
    using XmobiTea.EUN.Entity.Support;
#endif

    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;
    using XmobiTea.EUN.Helper;
    using XmobiTea.EUN.Plugin.WebGL;

    using System.Collections.Generic;

    public class WebSocketEUNSocketObject : EUNSocketObject
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

        public override void Connect(string username, string password, IEUNData data, string host, int port, int udpPort)
        {
            base.Connect(username, password, data, host, port, udpPort);
#if EUN
            var data1 = data == null ? EzyEntityFactory.EMPTY_ARRAY : data.ToEzyData();

            EzyClientJsBride.EzyConnect(username, password, data1.ToString(), host);
#endif
        }

        public override void Disconnect()
        {
            base.Disconnect();
#if EUN
            EzyClientJsBride.EzyDisconnect();
#endif
        }

#if EUN
        public override void Send(EzyObject request, bool reliable = true)
        {
            base.Send(request, reliable);

            if (EzyClientJsBride.EzySend(Serializer.Serialize(request.toDict<object, object>())))
            {
                //if (reliable) app.send(Commands.RequestCmd, request);
                //else app.udpSend(Commands.RequestCmd, request);
            }
            else
            {
                var data = request.get<EzyArray>(Commands.Data);

                var eunArray = new EUNArray();
                eunArray.Add((int)ReturnCode.AppNullRequest);
                eunArray.Add((string)null);
                if (data.size() > 2) eunArray.Add(data.get<int>(2));

                onResponse?.Invoke(eunArray);
            }
        }
#endif

        public override int GetPing()
        {
            return base.GetPing();
        }

        public override long GetTotalRecvBytes()
        {
            return base.GetTotalRecvBytes();
        }

        public override long GetTotalSendBytes()
        {
            return base.GetTotalSendBytes();
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
            onLoginError?.Invoke(new EUNArray.Builder().AddAll(getObjectLstFromJsonArr(jsonArr) as System.Collections.IList).Build());
        }

        private void handleOnAppAccess(string jsonArr)
        {
            onAppAccess?.Invoke(new EUNArray.Builder().AddAll(getObjectLstFromJsonArr(jsonArr) as System.Collections.IList).Build());
        }

        private void handleOnResponse(string jsonArr)
        {
            onResponse?.Invoke(new EUNArray.Builder().AddAll(getObjectLstFromJsonArr(jsonArr) as System.Collections.IList).Build());
        }

        private void handleOnEvent(string jsonArr)
        {
            onEvent?.Invoke(new EUNArray.Builder().AddAll(getObjectLstFromJsonArr(jsonArr) as System.Collections.IList).Build());
        }

        private IList<object> getObjectLstFromJsonArr(string jsonArr)
        {
            return (IList<object>)Parser.Parse(jsonArr);
        }
    }
}
