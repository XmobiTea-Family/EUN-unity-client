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
    using XmobiTea.EUN.Entity;

    public partial class NetworkingPeer
    {
        private Dictionary<int, IServerEventHandler> serverEventHandlerDic;

        internal List<EUNView> eunViewLst;
        internal List<EUNManagerBehaviour> eunManagerBehaviourLst;

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

        void OnEventHandler(EUNArray obj)
        {
            var eventCode = obj.GetInt(0);
            if (!serverEventHandlerDic.ContainsKey(eventCode))
            {
                return;
            }

            var operationEvent = new OperationEvent(eventCode, obj.GetEUNHashtable(1));

            Debug.Log("[EVENT] " + operationEvent.ToString());

            var serverEventHandler = serverEventHandlerDic[eventCode];
            serverEventHandler.Handle(operationEvent, this);
        }

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

                Debug.Log("[RECV] " + operationResponse.ToString());

                operationPending.GetCallback()?.Invoke(operationResponse);

                operationWaitingResponseDic.Remove(responseId);
            }
            else
            {
                Debug.LogError("OnResponseHandler " + obj);
            }
        }

        void OnLoginErrorHandler(EUNArray obj)
        {
            isConnected = false;

            foreach (var behaviour in eunManagerBehaviourLst)
            {
                if (behaviour) behaviour.OnEUNLoginError();
            }
        }

        void OnDisconnectionHandler(EzyDisconnectReason obj)
        {
            isConnected = false;

            foreach (var behaviour in eunManagerBehaviourLst)
            {
                if (behaviour) behaviour.OnEUNDisconnected(obj);
            }

            room = null;
            playerId = -1;
        }

        void OnConnectionFailureHandler(EzyConnectionFailedReason obj)
        {
            isConnected = false;

            foreach (var behaviour in eunManagerBehaviourLst)
            {
                if (behaviour) behaviour.OnEUNConnectionFailure(obj);
            }

            room = null;
            playerId = -1;
        }

        void OnConnectionSuccessHandler()
        {
            foreach (var behaviour in eunManagerBehaviourLst)
            {
                if (behaviour) behaviour.OnEUNZoneConnected();
            }
        }

        void OnAppAccessHandler(EUNArray obj)
        {
            isConnected = true;

            var outputEUNArray = obj.GetEUNArray(2);
            serverTimeStamp = outputEUNArray.GetLong(0);

            foreach (var behaviour in eunManagerBehaviourLst)
            {
                if (behaviour) behaviour.OnEUNConnected();
            }
        }

        internal void SubscriberEUNView(EUNView view)
        {
            if (!eunViewLst.Contains(view)) eunViewLst.Add(view);
        }

        internal void UnSubscriberEUNView(EUNView view)
        {
            if (eunViewLst.Contains(view)) eunViewLst.Remove(view);
        }

        internal void SubscriberEUNBehaviour(EUNManagerBehaviour behaviour)
        {
            if (!eunManagerBehaviourLst.Contains(behaviour)) eunManagerBehaviourLst.Add(behaviour);
        }

        internal void UnSubscriberEUNBehaviour(EUNManagerBehaviour behaviour)
        {
            if (eunManagerBehaviourLst.Contains(behaviour)) eunManagerBehaviourLst.Remove(behaviour);
        }
    }
}
