namespace XmobiTea.EUN.Networking
{
#if EUN_USING_ONLINE
    using com.tvd12.ezyfoxserver.client.constant;
    using com.tvd12.ezyfoxserver.client.entity;
#else
    using XmobiTea.EUN.Entity.Support;
#endif

    using XmobiTea.EUN.Common;

    using System;
    using System.Collections.Generic;

    using System.Linq;
    using XmobiTea.EUN.Entity;
    using XmobiTea.EUN.Logger;

    /// <summary>
    /// In this partial it will contains all callback 
    /// </summary>
    public partial class NetworkingPeer
    {
        /// <summary>
        /// all the class implement from IServerEventHandler
        /// </summary>
        private Dictionary<int, IServerEventHandler> serverEventHandlerDict;

        /// <summary>
        /// All EUNView list
        /// </summary>
        internal List<EUNView> eunViewLst;

        /// <summary>
        /// All IEUNManagerBehaviour subscriber to EUNNetwork
        /// </summary>
        internal List<IEUNManagerBehaviour> eunManagerBehaviourLst;

        /// <summary>
        /// Check and subscriber all class implement server event handler
        /// </summary>
        void subscriberServerEventHandler()
        {
            var type = typeof(IServerEventHandler);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && p != type).ToList();

            foreach (var t in types)
            {
                var srvMsg = Activator.CreateInstance(t) as IServerEventHandler;
                if (srvMsg != null) this.serverEventHandlerDict[(int)srvMsg.getEventCode()] = srvMsg;
            }
        }

        /// <summary>
        /// Handle an event from EUN Server to EUN Network
        /// </summary>
        /// <param name="obj"></param>
        void onEventHandler(EUNArray obj)
        {
            var eventCode = obj.getInt(0);
            
            lock (this.serverEventHandlerDict)
            {
                if (!this.serverEventHandlerDict.ContainsKey(eventCode))
                {
                    return;
                }
            }

            var operationEvent = new OperationEvent(eventCode, obj.getEUNHashtable(1));

            lock (this.eventHandlerPending)
            {
                this.eventHandlerPending.Add(operationEvent);
            }
        }

        /// <summary>
        /// Handle a response from EUN Server to EUN Network
        /// </summary>
        /// <param name="obj"></param>
        void onResponseHandler(EUNArray obj)
        {
            var responseId = obj.getInt(2);

            OperationPending operationPending = null;

            lock (this.operationWaitingResponseDict)
            {
                if (this.operationWaitingResponseDict.ContainsKey(responseId))
                {
                    operationPending = this.operationWaitingResponseDict[responseId];

                    this.operationWaitingResponseDict.Remove(responseId);
                }
                else
                {
                    EUNDebug.logError("OnResponseHandler, unavailable request id " + obj);
                }
            }

            if (operationPending != null)
            {
                operationPending.onRecv();

                var returnCode = obj.getInt(0);

                EUNHashtable parameters = null;
                string debugMessage = null;

                if (returnCode == 0) parameters = obj.getEUNHashtable(1);
                else debugMessage = obj.getString(1);

                var request = operationPending.getOperationRequest();

                var response = new OperationResponse(request.getOperationCode(), request.getRequestId());
                response.setReturnCode(returnCode);
                response.setDebugMessage(debugMessage);
                response.setParameters(parameters);

                operationPending.setOperationResponse(response);

                this.responseHandlerPending.Add(operationPending);
            }
        }

        /// <summary>
        /// Handle if login to EUN Server error
        /// </summary>
        /// <param name="obj"></param>
        void onLoginErrorHandler(EUNArray obj)
        {
            this.isConnected = false;

            this.loginErrorHandlerPending = obj;
        }

        /// <summary>
        /// Handle if has a disconnect call
        /// </summary>
        /// <param name="obj">EzyDisconnectReason</param>
        void onDisconnectionHandler(EzyDisconnectReason obj)
        {
            this.isConnected = false;

            this.disconnectionHandlerPending = obj;
        }

        /// <summary>
        /// Handle if connection fail
        /// </summary>
        /// <param name="obj"></param>
        void onConnectionFailureHandler(EzyConnectionFailedReason obj)
        {
            this.isConnected = false;

            this.connectionFailureHandlerPending = obj;
        }

        /// <summary>
        /// Handle connect to EUNServer success
        /// </summary>
        void onConnectionSuccessHandler()
        {
            this.connectionSuccessHandlerPending = true;
        }

        /// <summary>
        /// Handle if zone name, app name and plugin name correct and EUN Server access this client
        /// </summary>
        /// <param name="obj"></param>
        void onAppAccessHandler(EUNArray obj)
        {
            this.isConnected = true;

            this.appAccessHandlerPending = obj;
        }

        private List<OperationEvent> eventHandlerPending;
        private List<OperationPending> responseHandlerPending;
        private EUNArray loginErrorHandlerPending;
        private bool connectionSuccessHandlerPending;
        private EzyConnectionFailedReason? connectionFailureHandlerPending;
        private EzyDisconnectReason? disconnectionHandlerPending;
        private EUNArray appAccessHandlerPending;

        void serviceOnMainThread()
        {
            if (this.appAccessHandlerPending != null)
            {
                var outputEUNArray = appAccessHandlerPending.getEUNArray(2);
                this.serverTimestamp = outputEUNArray.getLong(0);

                lock (this.eunManagerBehaviourLst)
                {
                    for (var i = 0; i < this.eunManagerBehaviourLst.Count; i++)
                    {
                        var behaviour = this.eunManagerBehaviourLst[i];
                        if (behaviour != null) behaviour.onEUNConnected();
                    }
                }

                this.appAccessHandlerPending = null;
            }

            if (this.connectionSuccessHandlerPending)
            {
                lock (this.eunManagerBehaviourLst)
                {
                    for (var i = 0; i < this.eunManagerBehaviourLst.Count; i++)
                    {
                        var behaviour = this.eunManagerBehaviourLst[i];
                        if (behaviour != null) behaviour.onEUNZoneConnected();
                    }
                }

                this.connectionSuccessHandlerPending = false;
            }

            if (this.connectionFailureHandlerPending != null)
            {
                lock (this.eunManagerBehaviourLst)
                {
                    for (var i = 0; i < this.eunManagerBehaviourLst.Count; i++)
                    {
                        var behaviour = this.eunManagerBehaviourLst[i];
                        if (behaviour != null) behaviour.onEUNConnectionFailure(connectionFailureHandlerPending.GetValueOrDefault());
                    }
                }

                this.room = null;
                this.playerId = -1;
                this.eunViewDict.Clear();

                this.connectionFailureHandlerPending = null;
            }

            if (this.disconnectionHandlerPending != null)
            {
                lock (this.eunManagerBehaviourLst)
                {
                    for (var i = 0; i < this.eunManagerBehaviourLst.Count; i++)
                    {
                        var behaviour = this.eunManagerBehaviourLst[i];
                        if (behaviour != null) behaviour.onEUNDisconnected(this.disconnectionHandlerPending.GetValueOrDefault());
                    }
                }

                this.room = null;
                this.playerId = -1;
                this.eunViewDict.Clear();

                this.disconnectionHandlerPending = null;
            }

            if (this.loginErrorHandlerPending != null)
            {
                lock (this.eunManagerBehaviourLst)
                {
                    for (var i = 0; i < this.eunManagerBehaviourLst.Count; i++)
                    {
                        var behaviour = this.eunManagerBehaviourLst[i];
                        if (behaviour != null) behaviour.onEUNLoginError();
                    }
                }

                this.loginErrorHandlerPending = null;
            }

            lock (this.responseHandlerPending)
            {
                if (this.responseHandlerPending.Count != 0)
                {
                    foreach (var operationPending in this.responseHandlerPending)
                    {
                        EUNDebug.log("[RECV] " + operationPending.getOperationResponse().ToString());

                        operationPending.Invoke();
                    }

                    this.responseHandlerPending.Clear();
                }
            }

            lock (this.eventHandlerPending)
            {
                if (this.eventHandlerPending.Count != 0)
                {
                    foreach (var operationEvent in this.eventHandlerPending)
                    {
                        EUNDebug.log("[EVENT] " + operationEvent.ToString());

                        var serverEventHandler = this.serverEventHandlerDict[operationEvent.getEventCode()];
                        serverEventHandler.handle(operationEvent, this);
                    }

                    this.eventHandlerPending.Clear();
                }
            }
        }

        /// <summary>
        /// Subscriber a EUNView if eunViewLst does not contains it
        /// </summary>
        /// <param name="view"></param>
        internal void subscriberEUNView(EUNView view)
        {
            lock (this.eunViewLst) if (!this.eunViewLst.Contains(view)) this.eunViewLst.Add(view);
        }

        /// <summary>
        /// Remove subscriber a EUNView if eunViewLst contains it
        /// </summary>
        /// <param name="view"></param>
        internal void unSubscriberEUNView(EUNView view)
        {
            lock (this.eunViewLst) if(this.eunViewLst.Contains(view)) this.eunViewLst.Remove(view);
        }

        /// <summary>
        /// Subscriber a EUNManagerBehaviour if eunManagerBehaviourLst does not contains it
        /// </summary>
        /// <param name="behaviour"></param>
        internal void subscriberEUNManagerBehaviour(IEUNManagerBehaviour behaviour)
        {
            lock (this.eunManagerBehaviourLst) if (!this.eunManagerBehaviourLst.Contains(behaviour)) this.eunManagerBehaviourLst.Add(behaviour);
        }

        /// <summary>
        /// Remove subscriber a EUNManagerBehaviour if eunManagerBehaviourLst contains it
        /// </summary>
        /// <param name="behaviour"></param>
        internal void unSubscriberEUNManagerBehaviour(IEUNManagerBehaviour behaviour)
        {
            lock (this.eunManagerBehaviourLst) if(this.eunManagerBehaviourLst.Contains(behaviour)) this.eunManagerBehaviourLst.Remove(behaviour);
        }

    }

}
