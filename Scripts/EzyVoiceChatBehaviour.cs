namespace EUN
{
    using EUN.VoiceChat;
    using System;
    using UnityEngine;

    [RequireComponent(typeof(AudioSource))]
    public class EzyVoiceChatBehaviour : EzyBehaviour
    {
        [SerializeField]
        [Range(16, 128)]
        private int bufferSize = 64;
        public int BufferSize
        {
            set { bufferSize = value; }
            get => bufferSize;
        }

        private EzyMicSpeaker micSpeaker;
        public EzyMicSpeaker Speaker => micSpeaker;

        protected override void Awake()
        {
            base.Awake();

            var audioSource = GetComponent<AudioSource>();

            if (audioSource == null) throw new Exception("Ezy init microphone failed");

            micSpeaker = new EzyMicSpeaker(audioSource);
        }

        protected override void Start()
        {
            if (ezyView != null) ezyView.SubscriberEzyBehaviour(this);
        }

        protected override void OnDestroy()
        {
            if (ezyView != null) ezyView.UnSubscriberEzyBehaviour(this);
        }

        protected virtual void Update()
        {
            Speaker?.Service();
        }

        public override object GetSynchronizationData()
        {
            var frame = new float[BufferSize];
            
            if (EzyMicRecord.DetectedVoice(frame))
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

        public override void OnEzySynchronization(object voiceChatData)
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

