namespace GecsEng.Subsystems {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System.Diagnostics;

using Core;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public class PerformanceInfoSubsystem: Subsystem {
    /*-------------------------------------
     * NON-PUBLIC FIELDS
     *-----------------------------------*/

    private int m_NumDraws;

    private int m_NumUpdates;

    private Stopwatch m_Stopwatch;

    private string m_Text;

    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public override void Cleanup() {
        base.Cleanup();
        
        Game.Inst.Window.Text = m_Text;
    }

    public override void Draw(float dt) {
        base.Draw(dt);

        m_NumDraws++;

        if (m_Stopwatch.Elapsed.TotalSeconds >= 1.0) {
            Game.Inst.Window.Text = string.Format("{0} ({1}, {2} draws/s, {3} updates/s, {4} entities)", m_Text, Game.Inst.Graphics.Name, m_NumDraws, m_NumUpdates, Game.Inst.Scene.Entities.Count);

            m_NumDraws   = 0;
            m_NumUpdates = 0;

            m_Stopwatch.Restart();
        }
    }

    public override void Init() {
        base.Init();
        
        m_Stopwatch = Stopwatch.StartNew();
        m_Text      = Game.Inst.Window.Text;
    }

    public override void Update(float dt) {
        base.Update(dt);

        m_NumUpdates++;
    }
}

}
