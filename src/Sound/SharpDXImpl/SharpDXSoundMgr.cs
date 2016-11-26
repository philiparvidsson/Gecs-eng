namespace GecsEng.Sound.SharpDXImpl {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System.Collections.Generic;
using System.IO;

using SharpDX.Multimedia;
using SharpDX.XAudio2;


/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

internal class SharpDXSoundMgr: ISoundMgr {
    /*-------------------------------------
     * NON-PUBLIC FIELDS
     *-----------------------------------*/

    private MasteringVoice m_MasteringVoice;

    private List<SharpDXSound> m_Sounds = new List<SharpDXSound>();

    private Queue<SourceVoice> m_VoicePool = new Queue<SourceVoice>();

    private XAudio2 m_XAudio2;


    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public void Cleanup() {
        if (m_MasteringVoice != null) {
            m_MasteringVoice.Dispose();
            m_MasteringVoice = null;
        }

        if (m_XAudio2 != null) {
            m_XAudio2.Dispose();
            m_XAudio2 = null;
        }
    }

    public SourceVoice GetVoice() {
        lock (m_VoicePool) {
            return m_VoicePool.Dequeue();
        }
    }

    public void Init() {
        m_XAudio2        = new XAudio2();
        m_MasteringVoice = new MasteringVoice(m_XAudio2);

        var defWaveFormat = new WaveFormat(96000, 24, 2);
        for (var i = 0; i < 10; i++) {
            var voice = new SourceVoice(m_XAudio2, defWaveFormat);

            voice.BufferEnd += (ptr) => {
                lock (m_VoicePool) {
                    m_VoicePool.Enqueue(voice);
                }
            };

            m_VoicePool.Enqueue(voice);
        }
    }

    public ISound Load(string path) {
        using (var stream = new SoundStream(File.OpenRead(path))) {
            var buf   = new AudioBuffer(stream);
            var sound = new SharpDXSound(this, buf, stream.DecodedPacketsInfo);

            m_Sounds.Add(sound);

            return sound;
        }
    }
}

}
