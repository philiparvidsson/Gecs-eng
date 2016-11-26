namespace GecsEng.Components {
    
/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public sealed class LifetimeComponent {
    /*-------------------------------------
     * PUBLIC PROPERTIES
     *-----------------------------------*/

    public float Age { get; set; }

    public Action EndOfLife { get; set; }

    public float Lifetime { get; set; }
}

}
