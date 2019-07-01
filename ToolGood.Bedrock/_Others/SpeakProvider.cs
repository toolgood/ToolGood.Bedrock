using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ToolGood.Bedrock.Tools
{
    public class SpeakProvider
    {
        private static String typeName = "System.Speech.Synthesis.SpeechSynthesizer";
        private Type _type;

        public SpeakProvider()
        {
            try {
                // 新版系统内置
                if (Environment.OSVersion.Version.Major >= 6)
                    Assembly.Load("System.Speech, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35");
                _type = Type.GetType(typeName);
                //_type = typeName.GetTypeEx(true);
            } catch (Exception ex) {
                LogUtil.Error(ex);
                //XTrace.WriteException(ex);
            }
        }

        private dynamic synth;
        void EnsureSynth()
        {
            if (synth == null) {
                try {
                    synth = Activator.CreateInstance(_type, new Object[0]);
                    synth.SetOutputToDefaultAudioDevice();
                    //synth = _type.CreateInstance(new Object[0]);
                    //synth.Invoke("SetOutputToDefaultAudioDevice", new Object[0]);
                } catch (Exception ex) {
                    LogUtil.Error(ex);
                    //XTrace.WriteException(ex);
                    _type = null;
                }
            }
        }

        public void Speak(String value)
        {
            if (_type == null) return;

            EnsureSynth();
            if (synth != null) {
                synth.Speak();

                //synth.Invoke("Speak", value);
            }
        }

        public void SpeakAsync(String value)
        {
            if (_type == null) return;

            EnsureSynth();
            if (synth != null) {
                synth.SpeakAsync();
                //synth.Invoke("SpeakAsync", value);
            }
        }

        /// <summary>
        /// 停止话音播报
        /// </summary>
        public void SpeakAsyncCancelAll()
        {
            if (_type == null) return;

            EnsureSynth();
            if (synth != null) {
                synth.SpeakAsyncCancelAll();
                //synth.Invoke("SpeakAsyncCancelAll");
            }
        }


 
    }
}
