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
        private int _bufferSize = 64;
        public int bufferSize
        {
            set { this._bufferSize = value; }
            get => this._bufferSize;
        }

        private EUNMicSpeaker _micSpeaker;
        /// <summary>
        /// The mic speaker
        /// </summary>
        public EUNMicSpeaker speaker => this._micSpeaker;

        protected override void onCustomAwake()
        {
            base.onCustomAwake();

            var audioSource = GetComponent<AudioSource>();

            if (audioSource == null) throw new Exception("EUN init microphone failed");

            this._micSpeaker = new EUNMicSpeaker(audioSource);
        }

        protected override void onCustomStart()
        {
            if (this.eunView != null) this.eunView.subscriberEUNBehaviour(this);
        }

        protected override void onCustomDestroy()
        {
            if (this.eunView != null) this.eunView.unSubscriberEUNBehaviour(this);
        }

        private void Update()
        {
            this.onCustomUpdate();
        }

        /// <summary>
        /// This is a MonoBehaviour.Update()
        /// </summary>
        protected virtual void onCustomUpdate()
        {
            this.speaker?.service();
        }

        public override object getSynchronizationData()
        {
            var frame = new float[this.bufferSize];
            
            if (EUNMicRecord.detectedVoice(frame))
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

        public override void onEUNSynchronization(object voiceChatData)
        {
            if (this.speaker != null && voiceChatData != null)
            {
                if (voiceChatData is short[] buffer)
                {
                    var frame = new float[buffer.Length];

                    for (var i = 0; i < buffer.Length; i++)
                    {
                        frame[i] = (float)buffer[i] / short.MaxValue;
                    }

                    this.speaker.onAudioFrame(frame);
                }
            }
        }

    }

}
