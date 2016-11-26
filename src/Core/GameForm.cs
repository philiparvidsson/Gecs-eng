namespace GecsEng.Core {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System.Windows.Forms;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public class GameForm: Form {
    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public GameForm() {
        DoubleBuffered  = true;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox     = false;

        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint,
                 true);
    }

    /*-------------------------------------
     * NON-PUBLIC METHODS
     *-----------------------------------*/

    protected override void OnFormClosing(FormClosingEventArgs e) {
        base.OnFormClosing(e);

        Game.Inst.Exit();
    }
}

}
