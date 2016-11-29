namespace PrimusGE.Input.DefaultImpl {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using Input;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public sealed class DefaultKeyboard: IKeyboard {
    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public bool IsKeyPressed(Key key) {
        var r = MapKeyCode(key);

        if (!r.HasValue) {
            return false;
        }

        return System.Windows.Input.Keyboard.IsKeyDown(r.Value);
    }

    /*-------------------------------------
     * NON-PUBLIC METHODS
     *-----------------------------------*/

    private System.Windows.Input.Key? MapKeyCode(Key key) {
        // FIXME: Use a dictionary map instead for faster lookups.

        if (key == Key.A     ) return System.Windows.Input.Key.A;
        if (key == Key.D     ) return System.Windows.Input.Key.D;
        if (key == Key.Down  ) return System.Windows.Input.Key.Down;
        if (key == Key.E     ) return System.Windows.Input.Key.E;
        if (key == Key.Left  ) return System.Windows.Input.Key.Left;
        if (key == Key.Q     ) return System.Windows.Input.Key.Q;
        if (key == Key.Right ) return System.Windows.Input.Key.Right;
        if (key == Key.S     ) return System.Windows.Input.Key.S;
        if (key == Key.Up    ) return System.Windows.Input.Key.Up;
        if (key == Key.W     ) return System.Windows.Input.Key.W;

        return null;
    }
}

}
