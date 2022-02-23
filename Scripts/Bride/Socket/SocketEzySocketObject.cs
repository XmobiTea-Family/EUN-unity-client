namespace EUN.Bride.Socket
{
    using com.tvd12.ezyfoxserver.client;
    using com.tvd12.ezyfoxserver.client.config;
    using com.tvd12.ezyfoxserver.client.constant;
    using com.tvd12.ezyfoxserver.client.entity;
    using com.tvd12.ezyfoxserver.client.evt;
    using com.tvd12.ezyfoxserver.client.factory;
    using com.tvd12.ezyfoxserver.client.handler;
    using com.tvd12.ezyfoxserver.client.request;

    using EUN.Constant;

    public class SocketEzySocketObject : EzySocketObject
    {
        private EzyClient socketClient;
        internal static int udpPort;

        protected override void OnCustomStart()
        {
            base.OnCustomStart();
        }

        public override void Init(string _zoneName, string _appName)
        {
            base.Init(_zoneName, _appName);

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
        }

        public override void Connect(string username, string password, EzyData data, string host, int port, int udpPort)
        {
            base.Connect(username, password, data, host, port, udpPort);

            SocketEzySocketObject.udpPort = udpPort;

            socketClient.connect(host, port);
        }

        public override void Send(EzyObject request, bool reliable = true)
        {
            var app = socketClient.getApp();
            if (app != null)
            {
                if (reliable) app.send(Commands.RequestCmd, request);
                else app.udpSend(Commands.RequestCmd, request);
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

        void Update()
        {
            if (socketClient != null) socketClient.processEvents();
        }

        internal class ResponseHandler : EzyAbstractAppDataHandler<EzyArray>
        {
            protected override void process(EzyApp app, EzyArray data)
            {
                onResponse?.Invoke(data);
            }
        }

        internal class EventHandler : EzyAbstractAppDataHandler<EzyArray>
        {
            protected override void process(EzyApp app, EzyArray data)
            {
                onEvent?.Invoke(data);
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
                    data);
                //,
                //    EzyEntityFactory.newArrayBuilder()
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

                onLoginError?.Invoke(data);
            }
        }

        internal class AppAccessHandler : EzyAppAccessHandler
        {
            protected override void postHandle(EzyApp app, EzyArray data)
            {
                onAppAccess?.Invoke(data);
            }
        }
    }
}