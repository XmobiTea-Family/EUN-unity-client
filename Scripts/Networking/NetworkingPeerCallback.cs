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
    using XmobiTea.EUN.Logger;

    public partial class NetworkingPeer
    {
        private Dictionary<int, IServerEventHandler> serverEventHandlerDic;

        internal List<EUNView> eunViewLst;
        internal List<IEUNManagerBehaviour> eunManagerBehaviourLst;

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

            EUNDebug.Log("[EVENT] " + operationEvent.ToString());

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

                EUNDebug.Log("[RECV] " + operationResponse.ToString());

                operationPending.GetCallback()?.Invoke(operationResponse);

                operationWaitingResponseDic.Remove(responseId);
            }
            else
            {
                EUNDebug.LogError("OnResponseHandler " + obj);
            }
        }

        void OnLoginErrorHandler(EUNArray obj)
        {
            isConnected = false;

            for (var i = 0; i < eunManagerBehaviourLst.Count; i++)
            {
                var behaviour = eunManagerBehaviourLst[i];
                if (behaviour != null) behaviour.OnEUNLoginError();
            }
        }

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

        void OnConnectionSuccessHandler()
        {
            for (var i = 0; i < eunManagerBehaviourLst.Count; i++)
            {
                var behaviour = eunManagerBehaviourLst[i];
                if (behaviour != null) behaviour.OnEUNZoneConnected();
            }
        }

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

        internal void SubscriberEUNView(EUNView view)
        {
            if (!eunViewLst.Contains(view)) eunViewLst.Add(view);
        }

        internal void UnSubscriberEUNView(EUNView view)
        {
            if (eunViewLst.Contains(view)) eunViewLst.Remove(view);
        }

        internal void SubscriberEUNBehaviour(IEUNManagerBehaviour behaviour)
        {
            if (!eunManagerBehaviourLst.Contains(behaviour)) eunManagerBehaviourLst.Add(behaviour);
        }

        internal void UnSubscriberEUNBehaviour(IEUNManagerBehaviour behaviour)
        {
            if (eunManagerBehaviourLst.Contains(behaviour)) eunManagerBehaviourLst.Remove(behaviour);
        }
    }
}
