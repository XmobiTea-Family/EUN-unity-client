namespace XmobiTea.EUN.Bride.Socket
{
#if EUN_USING_ONLINE
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
#if EUN_USING_ONLINE
        private EzyClient socketClient;
#endif

        /// <summary>
        /// only socket connection need udp port
        /// </summary>
        internal static int udpPort;

        public override void init(string _zoneName, string _appName)
        {
            base.init(_zoneName, _appName);
#if EUN_USING_ONLINE
            var config = EzyClientConfig.builder().clientName(zoneName).build();

            this.socketClient = new EzyUTClient(config);

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

            EzyClients.getInstance().addClient(this.socketClient);
#endif
        }

        public override void connect(string username, string password, IEUNData data, string host, int port, int udpPort)
        {
            base.connect(username, password, data, host, port, udpPort);

            SocketEUNSocketObject.udpPort = udpPort;
#if EUN_USING_ONLINE
            this.socketClient.connect(host, port);
#endif
        }

        public override void disconnect()
        {
            base.disconnect();

#if EUN_USING_ONLINE
            this.socketClient.disconnect((int)EzyDisconnectReason.CLOSE);
#endif
        }

        public override int getPing()
        {
            return base.getPing();
        }

        public override long getTotalRecvBytes()
        {
#if EUN_USING_ONLINE
            var networkStats = this.socketClient.getNetworkStatistics().getSocketStats().getNetworkStats();

            return networkStats.getReadBytes();
#else
            return base.getTotalRecvBytes();
#endif
        }

        public override long getTotalSendBytes()
        {
#if EUN_USING_ONLINE
            var networkStats = this.socketClient.getNetworkStatistics().getSocketStats().getNetworkStats();

            return networkStats.getWrittenBytes();
#else
            return base.getTotalSendBytes();
#endif
        }

        public override long getTotalRecvPackets()
        {
#if EUN_USING_ONLINE
            var networkStats = this.socketClient.getNetworkStatistics().getSocketStats().getNetworkStats();

            return networkStats.getReadPackets();
#else
            return base.getTotalRecvPackets();
#endif
        }

        public override long getTotalSendPackets()
        {
#if EUN_USING_ONLINE
            var networkStats = this.socketClient.getNetworkStatistics().getSocketStats().getNetworkStats();

            return networkStats.getWrittenPackets();
#else
            return base.getTotalSendPackets();
#endif
        }

#if EUN_USING_ONLINE
        public override void send(EzyObject request, bool reliable = true)
        {
            base.send(request, reliable);

            var app = this.socketClient.getApp();
            if (app != null)
            {
                if (!reliable && this.socketClient.isUdpConnected()) app.udpSend(Commands.RequestCmd, request);
                else app.send(Commands.RequestCmd, request);
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
        }
#endif

        public override void service()
        {
            base.service();

#if EUN_USING_ONLINE
            if (this.socketClient != null) this.socketClient.processEvents();
#endif
        }

#if EUN_USING_ONLINE
        internal class ResponseHandler : EzyAbstractAppDataHandler<EzyArray>
        {
            protected override void process(EzyApp app, EzyArray data)
            {
                onResponse?.Invoke(new EUNArray.Builder().addAll(data.toList<object>()).build());
            }
        }

        internal class EventHandler : EzyAbstractAppDataHandler<EzyArray>
        {
            protected override void process(EzyApp app, EzyArray data)
            {
                onEvent?.Invoke(new EUNArray.Builder().addAll(data.toList<object>()).build());
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
                    (EzyData)data.toEzyData());
            }
        }

        internal class LoginSuccessHandler : EzyLoginSuccessHandler
        {
            protected override void handleLoginSuccess(EzyData responseData)
            {
                this.client.udpConnect(udpPort);
            }
        }

        internal class UdpHandshakeHandler : EzyUdpHandshakeHandler
        {
            protected override void onAuthenticated(EzyArray data)
            {
                var request = new EzyAppAccessRequest(appName);
                this.client.send(request);
            }
        }

        internal class LoginErrorHandler : EzyLoginErrorHandler
        {
            protected override void handleLoginError(EzyArray data)
            {
                base.handleLoginError(data);

                onLoginError?.Invoke(new EUNArray.Builder().addAll(data.toList<object>()).build());
            }
        }

        internal class AppAccessHandler : EzyAppAccessHandler
        {
            protected override void postHandle(EzyApp app, EzyArray data)
            {
                onAppAccess?.Invoke(new EUNArray.Builder().addAll(data.toList<object>()).build());
            }
        }

#endif
    }

}
