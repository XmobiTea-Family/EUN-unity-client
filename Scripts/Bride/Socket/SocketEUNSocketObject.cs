namespace XmobiTea.EUN.Bride.Socket
{
#if EUN
    using com.tvd12.ezyfoxserver.client;
    using com.tvd12.ezyfoxserver.client.config;
    using com.tvd12.ezyfoxserver.client.constant;
    using com.tvd12.ezyfoxserver.client.entity;
    using com.tvd12.ezyfoxserver.client.evt;
    using com.tvd12.ezyfoxserver.client.handler;
    using com.tvd12.ezyfoxserver.client.request;
#else
    using XmobiTea.EUN.Entity.Support;
#endif

    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;

    /// <summary>
    /// The EUNSocketObject for Socket connection
    /// </summary>
    public class SocketEUNSocketObject : EUNSocketObject
    {
#if EUN
        private EzyClient socketClient;
#endif

        /// <summary>
        /// only socket connection need udp port
        /// </summary>
        internal static int udpPort;

        public override void Init(string _zoneName, string _appName)
        {
            base.Init(_zoneName, _appName);
#if EUN
            var config = EzyClientConfig.builder().clientName(zoneName).build();

            socketClient = new EzyUTClient(config);

            var setup = socketClient.setup();
            setup.addEventHandler(EzyEventType.CONNECTION_SUCCESS, new ConnectionSuccessHandler());
            setup.addEventHandler(EzyEventType.CONNECTION_FAILURE, new ConnectionFailureHandler());
            setup.addEventHandler(EzyEventType.DISCONNECTION, new DisconnectionHandler());
            setup.addDataHandler(EzyCommand.HANDSHAKE, new HandshakeHandler());
            setup.addDataHandler(EzyCommand.UDP_HANDSHAKE, new UdpHandshakeHandler());
            setup.addDataHandler(EzyCommand.LOGIN, new LoginSuccessHandler());
            setup.addDataHandler(EzyCommand.LOGIN_ERROR, new LoginErrorHandler());
            setup.addDataHandler(EzyCommand.APP_ACCESS, new AppAccessHandler());

            var appSetup = setup.setupApp(appName);
            appSetup.addDataHandler(Commands.ResponseCmd, new ResponseHandler());
            appSetup.addDataHandler(Commands.EventCmd, new EventHandler());

            EzyClients.getInstance().addClient(socketClient);
#endif
        }

        public override void Connect(string username, string password, IEUNData data, string host, int port, int udpPort)
        {
            base.Connect(username, password, data, host, port, udpPort);

            SocketEUNSocketObject.udpPort = udpPort;
#if EUN
            socketClient.connect(host, port);
#endif
        }

        public override void Disconnect()
        {
            base.Disconnect();

#if EUN
            socketClient.disconnect((int)EzyDisconnectReason.CLOSE);
#endif
        }

        public override int GetPing()
        {
            return base.GetPing();
        }

        public override long GetTotalRecvBytes()
        {
#if EUN
            var networkStats = socketClient.getNetworkStatistics().getSocketStats().getNetworkStats();

            return networkStats.getReadBytes();
#else
            return base.GetTotalRecvBytes();
#endif
        }

        public override long GetTotalSendBytes()
        {
#if EUN
            var networkStats = socketClient.getNetworkStatistics().getSocketStats().getNetworkStats();

            return networkStats.getWrittenBytes();
#else
            return base.GetTotalSendBytes();
#endif
        }

        public override long GetTotalRecvPackets()
        {
#if EUN
            var networkStats = socketClient.getNetworkStatistics().getSocketStats().getNetworkStats();

            return networkStats.getReadPackets();
#else
            return base.GetTotalRecvPackets();
#endif
        }

        public override long GetTotalSendPackets()
        {
#if EUN
            var networkStats = socketClient.getNetworkStatistics().getSocketStats().getNetworkStats();

            return networkStats.getWrittenPackets();
#else
            return base.GetTotalSendPackets();
#endif
        }

#if EUN
        public override void Send(EzyObject request, bool reliable = true)
        {
            base.Send(request, reliable);

            var app = socketClient.getApp();
            if (app != null)
            {
                if (!reliable && socketClient.isUdpConnected()) app.udpSend(Commands.RequestCmd, request);
                else app.send(Commands.RequestCmd, request);
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

#if EUN
        void Update()
        {
            if (socketClient != null) socketClient.processEvents();
        }
#endif

#if EUN
        internal class ResponseHandler : EzyAbstractAppDataHandler<EzyArray>
        {
            protected override void process(EzyApp app, EzyArray data)
            {
                onResponse?.Invoke(new EUNArray.Builder().AddAll(data.toList<object>()).Build());
            }
        }

        internal class EventHandler : EzyAbstractAppDataHandler<EzyArray>
        {
            protected override void process(EzyApp app, EzyArray data)
            {
                onEvent?.Invoke(new EUNArray.Builder().AddAll(data.toList<object>()).Build());
            }
        }

        internal class ConnectionSuccessHandler : EzyConnectionSuccessHandler
        {
            protected override void postHandle()
            {
                base.postHandle();

                onConnectionSuccess?.Invoke();
            }
        }

        internal class ConnectionFailureHandler : EzyConnectionFailureHandler
        {
            protected override void onConnectionFailed(EzyConnectionFailureEvent evt)
            {
                base.onConnectionFailed(evt);

                onConnectionFailure?.Invoke(evt.getReason());
            }
        }

        internal class DisconnectionHandler : EzyDisconnectionHandler
        {
            protected override void onDisconnected(EzyDisconnectionEvent evt)
            {
                base.onDisconnected(evt);

                onDisconnection?.Invoke((EzyDisconnectReason)evt.getReason());
            }
        }

        internal class HandshakeHandler : EzyHandshakeHandler
        {
            protected override EzyRequest getLoginRequest()
            {
                return new EzyLoginRequest(
                    zoneName,
                    username,
                    password,
                    (EzyData)data.ToEzyData());
                //,
                //    EUNEntityFactory.newArrayBuilder()
                //        //.append("gameName", appName)
                //        .build()
                //    );
            }
        }

        internal class LoginSuccessHandler : EzyLoginSuccessHandler
        {
            protected override void handleLoginSuccess(EzyData responseData)
            {
                client.udpConnect(udpPort);
            }
        }

        internal class UdpHandshakeHandler : EzyUdpHandshakeHandler
        {
            protected override void onAuthenticated(EzyArray data)
            {
                var request = new EzyAppAccessRequest(appName);
                client.send(request);
            }
        }

        internal class LoginErrorHandler : EzyLoginErrorHandler
        {
            protected override void handleLoginError(EzyArray data)
            {
                base.handleLoginError(data);

                onLoginError?.Invoke(new EUNArray.Builder().AddAll(data.toList<object>()).Build());
            }
        }

        internal class AppAccessHandler : EzyAppAccessHandler
        {
            protected override void postHandle(EzyApp app, EzyArray data)
            {
                onAppAccess?.Invoke(new EUNArray.Builder().AddAll(data.toList<object>()).Build());
            }
        }
#endif
    }
}
