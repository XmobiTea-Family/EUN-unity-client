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

    using UnityEngine;
    using System.Linq;

    public partial class NetworkingPeer
    {
        private Dictionary<int, IServerEventHandler> serverEventHandlerDic;

        internal List<EzyView> ezyViewLst;
        internal List<EzyManagerBehaviour> ezyManagerBehaviourLst;

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

        void OnEventHandler(CustomArray obj)
        {
            var eventCode = obj.GetInt(0);
            if (!serverEventHandlerDic.ContainsKey(eventCode))
            {
                return;
            }

            var operationEvent = new OperationEvent(eventCode, obj.GetCustomHashtable(1));

            Debug.Log("[EVENT] " + operationEvent.ToString());

            var serverEventHandler = serverEventHandlerDic[eventCode];
            serverEventHandler.Handle(operationEvent, this);
        }

        void OnResponseHandler(CustomArray obj)
        {
            var responseId = obj.GetInt(2);

            if (operationWaitingResponseDic.ContainsKey(responseId))
            {
                var returnCode = obj.GetInt(0);

                CustomHashtable parameters = null;
                string debugMessage = null;

                if (returnCode == 0) parameters = obj.GetCustomHashtable(1);
                else debugMessage = obj.GetString(1);

                var operationPending = operationWaitingResponseDic[responseId];

                var operationResponse = new OperationResponse(operationPending.GetOperationRequest(), returnCode, debugMessage, parameters);

                Debug.Log("[RECV] " + operationResponse.ToString());

                operationPending.GetCallback()?.Invoke(operationResponse);

                operationWaitingResponseDic.Remove(responseId);
            }
            else
            {
                Debug.LogError("OnResponseHandler " + obj);
            }
        }

        void OnLoginErrorHandler(CustomArray obj)
        {
            isConnected = false;

            foreach (var behaviour in ezyManagerBehaviourLst)
            {
                if (behaviour) behaviour.OnEzyLoginError();
            }
        }

        void OnDisconnectionHandler(EzyDisconnectReason obj)
        {
            isConnected = false;

            foreach (var behaviour in ezyManagerBehaviourLst)
            {
                if (behaviour) behaviour.OnEzyDisconnected(obj);
            }

            room = null;
            playerId = -1;
        }

        void OnConnectionFailureHandler(EzyConnectionFailedReason obj)
        {
            isConnected = false;

            foreach (var behaviour in ezyManagerBehaviourLst)
            {
                if (behaviour) behaviour.OnEzyConnectionFailure(obj);
            }

            room = null;
            playerId = -1;
        }

        void OnConnectionSuccessHandler()
        {
            foreach (var behaviour in ezyManagerBehaviourLst)
            {
                if (behaviour) behaviour.OnEzyZoneConnected();
            }
        }

        void OnAppAccessHandler(CustomArray obj)
        {
            isConnected = true;

            var outputCustomArray = obj.GetCustomArray(2);
            serverTimeStamp = outputCustomArray.GetLong(0);

            foreach (var behaviour in ezyManagerBehaviourLst)
            {
                if (behaviour) behaviour.OnEzyConnected();
            }
        }

        internal void SubscriberEzyView(EzyView view)
        {
            if (!ezyViewLst.Contains(view)) ezyViewLst.Add(view);
        }

        internal void UnSubscriberEzyView(EzyView view)
        {
            if (ezyViewLst.Contains(view)) ezyViewLst.Remove(view);
        }

        internal void SubscriberEzyBehaviour(EzyManagerBehaviour behaviour)
        {
            if (!ezyManagerBehaviourLst.Contains(behaviour)) ezyManagerBehaviourLst.Add(behaviour);
        }

        internal void UnSubscriberEzyBehaviour(EzyManagerBehaviour behaviour)
        {
            if (ezyManagerBehaviourLst.Contains(behaviour)) ezyManagerBehaviourLst.Remove(behaviour);
        }
    }
}