namespace GecsEng.Graphics {

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public sealed class Font {
    /*-------------------------------------
     * NON-PUBLIC FIELDS
     *-----------------------------------*/
    
    internal readonly System.Drawing.Font m_Font;

    private static Font s_Default;

    /*-------------------------------------
     * PUBLIC PROPERTIES
     *-----------------------------------*/

    public static Font Default {
        get {
            if (s_Default != null) {
                return s_Default;
            }

            s_Default = new Font(System.Drawing.SystemFonts.DefaultFont);

            return s_Default;
        }
    }

    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public Font(System.Drawing.Font font) {
        m_Font = font;
    }
}

}
