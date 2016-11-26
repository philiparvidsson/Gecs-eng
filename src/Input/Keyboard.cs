namespace GecsEng.Input {


/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public sealed class Keyboard {
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

    public System.Windows.Input.Key? MapKeyCode(Key key) {
        if (key == Key.Up  ) return System.Windows.Input.Key.Up;
        if (key == Key.Down) return System.Windows.Input.Key.Down;
        if (key == Key.Q   ) return System.Windows.Input.Key.Q;
        if (key == Key.E   ) return System.Windows.Input.Key.E;

        return null;
    }
}

}
