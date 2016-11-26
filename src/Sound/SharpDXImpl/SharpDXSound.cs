namespace GecsEng.Sound.SharpDXImpl {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;
using System.Diagnostics;

using SharpDX.Multimedia;
using SharpDX.XAudio2;


/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

internal class SharpDXSound: IDisposable, ISound {
    /*-------------------------------------
     * NON-PUBLIC FIELDS
     *-----------------------------------*/

    private AudioBuffer m_Buffer;

    private uint[] m_PacketsInfo;

    private SharpDXSoundMgr m_Sound;

    private Stopwatch m_Stopwatch = new Stopwatch();

    /*-------------------------------------
     * PUBLIC PROPERTIES
     *-----------------------------------*/

    public int MaxPlaysPerSec { get; set; } = 10;

    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public SharpDXSound(SharpDXSoundMgr sound,
                        AudioBuffer     buffer,
                        uint[]          packetsInfo)
    {
        m_Buffer      = buffer;
        m_PacketsInfo = packetsInfo;
        m_Sound       = sound;
    }

    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public void Play(float pitch=0.0f) {
        if (MaxPlaysPerSec > 0) {
            var minTime = 1.0f/MaxPlaysPerSec;
            var time    = (float)m_Stopwatch.Elapsed.TotalSeconds;

            if (m_Stopwatch.IsRunning && time < minTime) {
                return;
            }
        }

        pitch = (float)Math.Pow(2.0f, pitch - 1.0f);

        var sourceVoice = m_Sound.GetVoice();

        sourceVoice.SubmitSourceBuffer(m_Buffer, m_PacketsInfo);
        sourceVoice.SetFrequencyRatio(pitch);
        sourceVoice.Start();

        m_Stopwatch.Restart();
    }

    /*-------------------------------------
     * NON-PUBLIC METHODS
     *-----------------------------------*/

    protected virtual void Dispose(bool disposing) {
        if (disposing) {
            m_Sound = null;
        }
    }
}

}
