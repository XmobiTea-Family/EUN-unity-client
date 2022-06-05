namespace XmobiTea.EUN.Networking
{
#if EUN
    using com.tvd12.ezyfoxserver.client.constant;
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
        private Dictionary<int, IServerEventHandler> serverEventHandlerDic;

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
        void SubscriberServerEventHandler()
        {
            var type = typeof(IServerEventHandler);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && p != type).ToList();

            foreach (var t in types)
            {
                var srvMsg = Activator.CreateInstance(t) as IServerEventHandler;
                if (srvMsg != null) serverEventHandlerDic[(int)srvMsg.GetEventCode()] = srvMsg;
            }
        }

        /// <summary>
        /// Handle an event from EUN Server to EUN Network
        /// </summary>
        /// <param name="obj"></param>
        void OnEventHandler(EUNArray obj)
        {
            var eventCode = obj.GetInt(0);
            if (!serverEventHandlerDic.ContainsKey(eventCode))
            {
                return;
            }

            var operationEvent = new OperationEvent(eventCode, obj.GetEUNHashtable(1));

            EUNDebug.Log("[EVENT] " + operationEvent.ToString());

            var serverEventHandler = serverEventHandlerDic[eventCode];
            serverEventHandler.Handle(operationEvent, this);
        }

        /// <summary>
        /// Handle a response from EUN Server to EUN Network
        /// </summary>
        /// <param name="obj"></param>
        void OnResponseHandler(EUNArray obj)
        {
            var responseId = obj.GetInt(2);

            if (operationWaitingResponseDic.ContainsKey(responseId))
            {
                var returnCode = obj.GetInt(0);

                EUNHashtable parameters = null;
                string debugMessage = null;

                if (returnCode == 0) parameters = obj.GetEUNHashtable(1);
                else debugMessage = obj.GetString(1);

                var operationPending = operationWaitingResponseDic[responseId];

                var operationResponse = new OperationResponse(operationPending.GetOperationRequest(), returnCode, debugMessage, parameters);

                EUNDebug.Log("[RECV] " + operationResponse.ToString());

                operationPending.GetCallback()?.Invoke(operationResponse);

                operationWaitingResponseDic.Remove(responseId);
            }
            else
            {
                EUNDebug.LogError("OnResponseHandler " + obj);
            }
        }

        /// <summary>
        /// Handle if login to EUN Server error
        /// </summary>
        /// <param name="obj"></param>
        void OnLoginErrorHandler(EUNArray obj)
        {
            isConnected = false;

            for (var i = 0; i < eunManagerBehaviourLst.Count; i++)
            {
                var behaviour = eunManagerBehaviourLst[i];
                if (behaviour != null) behaviour.OnEUNLoginError();
            }
        }

        /// <summary>
        /// Handle if has a disconnect call
        /// </summary>
        /// <param name="obj">EzyDisconnectReason</param>
        void OnDisconnectionHandler(EzyDisconnectReason obj)
        {
            isConnected = false;

            for (var i = 0; i < eunManagerBehaviourLst.Count; i++)
            {
                var behaviour = eunManagerBehaviourLst[i];
                if (behaviour != null) behaviour.OnEUNDisconnected(obj);
            }

            room = null;
            playerId = -1;
        }

        /// <summary>
        /// Handle if connection fail
        /// </summary>
        /// <param name="obj"></param>
        void OnConnectionFailureHandler(EzyConnectionFailedReason obj)
        {
            isConnected = false;

            for (var i = 0; i < eunManagerBehaviourLst.Count; i++)
            {
                var behaviour = eunManagerBehaviourLst[i];
                if (behaviour != null) behaviour.OnEUNConnectionFailure(obj);
            }

            room = null;
            playerId = -1;
        }

        /// <summary>
        /// Handle connect to EUNServer success
        /// </summary>
        void OnConnectionSuccessHandler()
        {
            for (var i = 0; i < eunManagerBehaviourLst.Count; i++)
            {
                var behaviour = eunManagerBehaviourLst[i];
                if (behaviour != null) behaviour.OnEUNZoneConnected();
            }
        }

        /// <summary>
        /// Handle if zone name, app name and plugin name correct and EUN Server access this client
        /// </summary>
        /// <param name="obj"></param>
        void OnAppAccessHandler(EUNArray obj)
        {
            isConnected = true;

            var outputEUNArray = obj.GetEUNArray(2);
            serverTimeStamp = outputEUNArray.GetLong(0);

            for (var i = 0; i < eunManagerBehaviourLst.Count; i++)
            {
                var behaviour = eunManagerBehaviourLst[i];
                if (behaviour != null) behaviour.OnEUNConnected();
            }
        }

        /// <summary>
        /// Subscriber a EUNView if eunViewLst does not contains it
        /// </summary>
        /// <param name="view"></param>
        internal void SubscriberEUNView(EUNView view)
        {
            if (!eunViewLst.Contains(view)) eunViewLst.Add(view);

            if (view != null)
            {
                if (view.RoomGameObject.IsValid())
                {
                    eunViewDic[view.RoomGameObject.ObjectId] = view;
                }
            }
        }

        /// <summary>
        /// Remove subscriber a EUNView if eunViewLst contains it
        /// </summary>
        /// <param name="view"></param>
        internal void UnSubscriberEUNView(EUNView view)
        {
            if (eunViewLst.Contains(view)) eunViewLst.Remove(view);

            if (view != null)
            {
                if (view.RoomGameObject.IsValid())
                {
                    eunViewDic.Remove(view.RoomGameObject.ObjectId);
                }
            }
        }

        /// <summary>
        /// Subscriber a EUNManagerBehaviour if eunManagerBehaviourLst does not contains it
        /// </summary>
        /// <param name="behaviour"></param>
        internal void SubscriberEUNManagerBehaviour(IEUNManagerBehaviour behaviour)
        {
            if (!eunManagerBehaviourLst.Contains(behaviour)) eunManagerBehaviourLst.Add(behaviour);
        }

        /// <summary>
        /// Remove subscriber a EUNManagerBehaviour if eunManagerBehaviourLst contains it
        /// </summary>
        /// <param name="behaviour"></param>
        internal void UnSubscriberEUNManagerBehaviour(IEUNManagerBehaviour behaviour)
        {
            if (eunManagerBehaviourLst.Contains(behaviour)) eunManagerBehaviourLst.Remove(behaviour);
        }
    }
}
