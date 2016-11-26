namespace PrimusGE.Components.Graphical {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;

using Graphics;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public sealed class TextComponent {
    /*-------------------------------------
     * PUBLIC PROPERTIES
     *-----------------------------------*/

    public Font Font { get; set; } = Font.Default;

    public Func<string> Text { get; set; }
}

}
