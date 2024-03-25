namespace XmobiTea.EUN.Bride.WebSocket
{
#if EUN_USING_ONLINE
    using com.tvd12.ezyfoxserver.client.entity;
#if !UNITY_EDITOR && UNITY_WEBGL
    using com.tvd12.ezyfoxserver.client.factory;
    using XmobiTea.EUN.Plugin.WebGL;
    using XmobiTea.EUN.Helper;
    using XmobiTea.EUN.Constant;
#endif
#else
    using XmobiTea.EUN.Entity.Support;
#endif

    using XmobiTea.EUN.Common;

    public class WebSocketEUNSocketObject : EUNSocketObject
    {
        public WebSocketEUNSocketObject() : base()
        {
#if EUN_USING_ONLINE && !UNITY_EDITOR && UNITY_WEBGL
            EzyClientJsBride.ezyLibrary();
#endif
        }

        public override void init(string _zoneName, string _appName)
        {
            base.init(_zoneName, _appName);
#if EUN_USING_ONLINE && !UNITY_EDITOR && UNITY_WEBGL
            EzyClientJsBride.ezyInit("[EUN] ServiceUpdate", _zoneName, _appName);
#endif
        }

        public override void connect(string username, string password, IEUNData data, string host, int port, int udpPort)
        {
            base.connect(username, password, data, host, port, udpPort);
#if EUN_USING_ONLINE && !UNITY_EDITOR && UNITY_WEBGL
            var data1 = data == null ? com.tvd12.ezyfoxserver.client.factory.EzyEntityFactory.EMPTY_ARRAY : data.toEzyData();

            EzyClientJsBride.ezyConnect(username, password, data1.ToString(), host);
#endif
        }

        public override void disconnect()
        {
            base.disconnect();
#if EUN_USING_ONLINE && !UNITY_EDITOR && UNITY_WEBGL
            EzyClientJsBride.ezyDisconnect();
#endif
        }

#if EUN_USING_ONLINE
        public override void send(EzyObject request, bool reliable = true)
        {
            base.send(request, reliable);
            
#if !UNITY_EDITOR && UNITY_WEBGL
            if (EzyClientJsBride.ezySend(Serializer.Serialize(request.toDict<object, object>())))
            {
                //if (reliable) app.send(Commands.RequestCmd, request);
                //else app.udpSend(Commands.RequestCmd, request);
            }
            else
            {
                var data = request.get<EzyArray>(Commands.Data);

                var eunArray = new EUNArray();
                eunArray.add((int)ReturnCode.AppNullRequest);
                eunArray.add((string)null);
                if (data.size() > 2) eunArray.add(data.get<int>(2));

                onResponse?.Invoke(eunArray);
            }
#endif
        }
#endif

        public override int getPing()
        {
            return base.getPing();
        }

        public override long getTotalRecvBytes()
        {
            return base.getTotalRecvBytes();
        }

        public override long getTotalSendBytes()
        {
            return base.getTotalSendBytes();
        }

    }

}
