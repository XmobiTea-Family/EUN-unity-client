namespace XmobiTea.EUN
{
    using XmobiTea.EUN.VoiceChat;
    using System;
    using UnityEngine;

    [RequireComponent(typeof(AudioSource))]
    public class EUNVoiceChatBehaviour : EUNBehaviour
    {
        [SerializeField]
        [Range(16, 128)]
        private int bufferSize = 64;
        public int BufferSize
        {
            set { bufferSize = value; }
            get => bufferSize;
        }

        private EUNMicSpeaker micSpeaker;
        /// <summary>
        /// The mic speaker
        /// </summary>
        public EUNMicSpeaker Speaker => micSpeaker;

        protected override void OnCustomAwake()
        {
            base.OnCustomAwake();

            var audioSource = GetComponent<AudioSource>();

            if (audioSource == null) throw new Exception("EUN init microphone failed");

            micSpeaker = new EUNMicSpeaker(audioSource);
        }

        protected override void OnCustomStart()
        {
            if (eunView != null) eunView.SubscriberEUNBehaviour(this);
        }

        protected override void OnCustomDestroy()
        {
            if (eunView != null) eunView.UnSubscriberEUNBehaviour(this);
        }

        private void Update()
        {
            OnCustomUpdate();
        }

        /// <summary>
        /// This is a MonoBehaviour.Update()
        /// </summary>
        protected virtual void OnCustomUpdate()
        {
            Speaker?.Service();
        }

        public override object GetSynchronizationData()
        {
            var frame = new float[BufferSize];
            
            if (EUNMicRecord.DetectedVoice(frame))
            {
                var buffer = new short[frame.Length];

                for (var i = 0; i < frame.Length; i++)
                {
                    buffer[i] = (short)(frame[i] * short.MaxValue);
                }

                return buffer;
            }

            return null;
        }

        public override void OnEUNSynchronization(object voiceChatData)
        {
            if (Speaker != null && voiceChatData != null)
            {
                if (voiceChatData is short[] buffer)
                {
                    var frame = new float[buffer.Length];

                    for (var i = 0; i < buffer.Length; i++)
                    {
                        frame[i] = (float)buffer[i] / short.MaxValue;
                    }

                    Speaker.OnAudioFrame(frame);
                }
            }
        }
    }
}
