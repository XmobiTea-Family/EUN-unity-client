namespace XmobiTea.EUN.VoiceChat
{
    using System.Collections.Generic;

    using UnityEngine;

    public class EUNMicRecord
    {
        private AudioClip mic;
        private string device;

        public EUNMicRecord(string device, int suggestedFrequency)
        {
#if EUN_VOICE_CHAT
            if (Microphone.devices.Length < 1)
            {
                return;
            }
            this.device = device;
            int minFreq;
            int maxFreq;
            Microphone.GetDeviceCaps(device, out minFreq, out maxFreq);
            var frequency = suggestedFrequency;
            //        minFreq = maxFreq = 44100; // test like android client
            if (suggestedFrequency < minFreq || maxFreq != 0 && suggestedFrequency > maxFreq)
            {
                Debug.LogWarningFormat("[PV] MicWrapper does not support suggested frequency {0} (min: {1}, max: {2}). Setting to {2}",
                    suggestedFrequency, minFreq, maxFreq);
                frequency = maxFreq;
            }
            this.mic = Microphone.Start(device, true, 1, frequency);
#endif
        }

        public int SamplingRate => this.mic == null ? -1 : this.mic.frequency;
        public int Channels => this.mic == null ? - 1 : this.mic.channels;

        private int micPrevPos;
        private int micLoopCnt;
        private int readAbsPos;

        public bool Read(float[] buffer)
        {
#if EUN_VOICE_CHAT
            int micPos = Microphone.GetPosition(this.device);
            // loop detection
            if (micPos < micPrevPos)
            {
                micLoopCnt++;
            }
            micPrevPos = micPos;

            var micAbsPos = micLoopCnt * this.mic.samples + micPos;

            var bufferSamplesCount = buffer.Length / mic.channels;

            var nextReadPos = this.readAbsPos + bufferSamplesCount;
            if (nextReadPos < micAbsPos)
            {
                this.mic.GetData(buffer, this.readAbsPos % this.mic.samples);
                this.readAbsPos = nextReadPos;
                return true;
            }
            else
            {
                return false;
            }
#endif

            return false;
        }

        public static bool DetectedVoice(float[] buffer)
        {
#if EUN_VOICE_CHAT
            const int activityDelayValuesCount = 44100 / 1000 * 100;

            var detected = false;
            var threshold = 0.01f;
            var autoSilenceCounter = 0;

            foreach (var s in buffer)
            {
                if (s > threshold)
                {
                    detected = true;
                    autoSilenceCounter = 0;
                }
                else
                {
                    autoSilenceCounter++;
                }
            }

            if (autoSilenceCounter > activityDelayValuesCount)
            {
                detected = false;
            }
            return detected;
#endif

            return false;
        }

        public void Dispose()
        {
#if EUN_VOICE_CHAT
            Microphone.End(this.device);
#endif
        }
    }

    public class EUNMicSpeaker
    {
        const int maxPlayLagMs = 100;
        private int maxPlayLagSamples;

        private int playDelaySamples;

        private int bufferSamples;
        private int channels;
        private int frameSize;
        private int frameSamples;
        private int streamSamplePos;

        public int CurrentBufferLag { get; private set; }

        public AudioSource AudioSource => this.source;

        private int streamSamplePosAvg;

        private AudioSource source;

        public EUNMicSpeaker(AudioSource audioSource)
        {
            this.source = audioSource;
            this.frameQueue = new Queue<float[]>();
        }

        private int playSamplePos
        {
            get { return this.source.clip != null ? this.playLoopCount * this.bufferSamples + this.source.timeSamples : 0; }
            set
            {
                if (this.source.clip != null)
                {
                    var pos = value % this.bufferSamples;
                    if (pos < 0)
                    {
                        pos += this.bufferSamples;
                    }
                    this.source.timeSamples = pos;
                    this.playLoopCount = value / this.bufferSamples;
                    this.sourceTimeSamplesPrev = this.source.timeSamples;
                }

            }
        }
        private int sourceTimeSamplesPrev;
        private int playLoopCount;

        public bool IsPlaying
        {
            get { return this.source.isPlaying; }
        }

        public void Start(int frequency, int channels, int frameSamples, int playDelayMs)
        {
            this.bufferSamples = (maxPlayLagMs + playDelayMs) * frequency / 1000 + frameSamples + frequency; // frame + max delay + 1 sec. just in case

            this.channels = channels;
            this.frameSamples = frameSamples;
            this.frameSize = frameSamples * channels;

            this.maxPlayLagSamples = maxPlayLagMs * frequency / 1000 + this.frameSamples;
            this.playDelaySamples = playDelayMs * frequency / 1000 + this.frameSamples;

            this.CurrentBufferLag = this.playDelaySamples;
            this.streamSamplePosAvg = this.playDelaySamples;

            this.source.loop = true;
            this.source.clip = AudioClip.Create("AudioStreamPlayer", bufferSamples, channels, frequency, false);

            this.streamSamplePos = 0;
            this.playSamplePos = 0;

            this.source.Play();
            this.source.Pause();
        }

        public void Pause()
        {
            this.source.Pause();
        }

        public void Play()
        {
            this.source.Play();
        }

        Queue<float[]> frameQueue;

        public void Service()
        {
            if (this.source.clip != null)
            {
                while (frameQueue.Count > 0)
                {
                    var frame = frameQueue.Dequeue();
                    this.source.clip.SetData(frame, this.streamSamplePos % this.bufferSamples);
                    this.streamSamplePos += frame.Length / this.channels;
                }

                if (this.source.isPlaying)
                {
                    if (this.source.timeSamples < sourceTimeSamplesPrev)
                    {
                        playLoopCount++;
                    }
                    sourceTimeSamplesPrev = this.source.timeSamples;
                }

                var playPos = this.playSamplePos;

                this.CurrentBufferLag = (this.CurrentBufferLag * 39 + (this.streamSamplePos - playPos)) / 40;

                this.streamSamplePosAvg = playPos + this.CurrentBufferLag;
                if (this.streamSamplePosAvg > this.streamSamplePos)
                {
                    this.streamSamplePosAvg = this.streamSamplePos;
                }

                if (playPos < this.streamSamplePos - this.playDelaySamples)
                {
                    if (!this.source.isPlaying)
                    {
                        this.source.UnPause();
                    }
                }

                if (playPos > this.streamSamplePos - frameSamples)
                {
                    if (this.source.isPlaying)
                    {
                        this.source.Pause();

                        playPos = this.streamSamplePos;
                        this.playSamplePos = playPos;
                        this.CurrentBufferLag = this.playDelaySamples;
                    }
                }
                if (this.source.isPlaying)
                {
                    var lowerBound = this.streamSamplePos - this.playDelaySamples - maxPlayLagSamples;
                    if (playPos < lowerBound)
                    {                 
                        playPos = this.streamSamplePos - this.playDelaySamples;
                        this.playSamplePos = playPos;
                        this.CurrentBufferLag = this.playDelaySamples;
                    }
                }
            }
        }

        public void OnAudioFrame(float[] frame)
        {
            if (frame.Length == 0)
            {
                return;
            }
            if (frame.Length != frameSize)
            {
                return;
            }

            float[] b = new float[frame.Length];
            System.Buffer.BlockCopy(frame, 0, b, 0, frameSize * sizeof(float));
            frameQueue.Enqueue(b);
        }

        public void Stop()
        {
            this.source.Stop();
            this.source.clip = null;
        }
    }
}