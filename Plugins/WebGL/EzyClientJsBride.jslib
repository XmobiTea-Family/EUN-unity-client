mergeInto(LibraryManager.library, {
    EzyLibrary: function() {
        var ezyClientJs = window.document.getElementById("ezyclientjs");

        if (!ezyClientJs) {
            ezyClientJs = window.document.createElement("script");
            ezyClientJs.type = "text/javascript";
            ezyClientJs.src = "StreamingAssets/ezyclient-1.0.4.min.js";
            ezyClientJs.id = "ezyclientjs";
            window.document.body.append(ezyClientJs);

            ezyClientJs.onload = function() {
                window.ezyClientJsScript = ezyClientJs;
                if (window.onEzyClientJsLoaded) window.onEzyClientJsLoaded();
            };
        }
    },

    EzyInit: function(gameObjectName, zoneName, appName) {
        gameObjectName = Pointer_stringify(gameObjectName);
        zoneName = Pointer_stringify(zoneName);
        appName = Pointer_stringify(appName);

        window.ezyGameObject = gameObjectName;
        window.ezyConfig = {
            zoneName: zoneName,
            appName: appName
		};

        var unityMethodDic = unityMethodDic || {
            HandleOnConnectionSuccess: "handleOnConnectionSuccess",
            HandleOnDisconnection: "handleOnDisconnection",
            HandleOnConnectionFailure: "handleOnConnectionFailure",
            HandleOnLoginError: "handleOnLoginError",
            HandleOnAppAccess: "handleOnAppAccess",
            HandleOnResponse: "handleOnResponse",
            HandleOnEvent: "handleOnEvent"
		};
        Object.freeze(unityMethodDic);

        var afterInitDone = function() {
            var config = new EzyClientConfig();
            config.zoneName = zoneName;

            var clients = EzyClients.getInstance();
            var client = clients.newDefaultClient(config);

            var connectionSuccessHandler = new EzyConnectionSuccessHandler();
            connectionSuccessHandler.postHandle = function() {
                SendMessage(window.ezyGameObject,
                    unityMethodDic.HandleOnConnectionSuccess
                );
            };

            var disconnectionHandler = new EzyDisconnectionHandler();
            disconnectionHandler.onDisconnected = function(event) {
                SendMessage(window.ezyGameObject,
                    unityMethodDic.HandleOnDisconnection,
                    event.reason
                );
		    }

            var connectionFailureHandler = new EzyConnectionFailureHandler();
            connectionFailureHandler.onConnectionFailed = function(event) {
                SendMessage(window.ezyGameObject,
                    unityMethodDic.HandleOnConnectionFailure,
                    event.reason
                );
		    }

            var handshakeHandler = new EzyHandshakeHandler();
            handshakeHandler.getLoginRequest = function() {
                return [window.ezyConfig.zoneName, window.ezyConfig.userId, "", []];
		    }

            var loginSuccessHandler = new EzyLoginSuccessHandler();
            loginSuccessHandler.handleLoginSuccess = function(responseData) {
                var accessAppRequest = [window.ezyConfig.appName, []];
                window.ezyClient.sendRequest(EzyCommand.APP_ACCESS, accessAppRequest);
            }

            var loginErrorHandler = new EzyLoginErrorHandler();
            loginErrorHandler.handleLoginError = function(data) {
                SendMessage(window.ezyGameObject,
                    unityMethodDic.HandleOnLoginError,
                    JSON.stringify(data)
                );
		    }

            var appAccessHandler = new EzyAppAccessHandler();
            appAccessHandler.postHandle = function(app, data) {
                console.log(JSON.stringify(data));
                SendMessage(window.ezyGameObject,
                    unityMethodDic.HandleOnAppAccess,
                    JSON.stringify(data)
                );
		    }

            var setup = client.setup;
            setup.addEventHandler(EzyEventType.CONNECTION_SUCCESS, connectionSuccessHandler);
            setup.addEventHandler(EzyEventType.DISCONNECTION, disconnectionHandler);
            setup.addEventHandler(EzyEventType.CONNECTION_FAILURE, connectionFailureHandler);

            setup.addDataHandler(EzyCommand.HANDSHAKE, handshakeHandler);
            setup.addDataHandler(EzyCommand.LOGIN, loginSuccessHandler);
            setup.addDataHandler(EzyCommand.LOGIN_ERROR, loginErrorHandler);
            setup.addDataHandler(EzyCommand.APP_ACCESS, appAccessHandler);
        
            var setupApp = setup.setupApp(appName);
            setupApp.addDataHandler("r", function(app, data) {
                SendMessage(window.ezyGameObject,
                    unityMethodDic.HandleOnResponse,
                    JSON.stringify(data)
                );
            });
            setupApp.addDataHandler("e", function(app, data) {
                SendMessage(window.ezyGameObject,
                    unityMethodDic.HandleOnEvent,
                    JSON.stringify(data)
                );
            });

            window.ezyClient = client;
            if (window.onEzyClientLoaded) window.onEzyClientLoaded();
        }

        if (window.ezyClientJsScript != null) afterInitDone();
        else window.onEzyClientJsLoaded = afterInitDone;
	},

    EzyConnect: function(userId, host) {
        userId = Pointer_stringify(userId);
        host = Pointer_stringify(host);

        window.ezyConfig.userId = userId;

        var afterInitDone = function() {
            window.ezyClient.connect(host);
        }

        if (window.ezyClient != null) afterInitDone();
        else window.onEzyClientLoaded = afterInitDone;
	},

    EzySend: function(requestData) {
        var client = window.ezyClient;
        if (client == null) return false;

        var app = client.getApp();
        if (app == null) return false;

        requestData = Pointer_stringify(requestData);
        var requestJson = JSON.parse(requestData);

        app.sendRequest("", requestJson);
        return true;
	},
});

