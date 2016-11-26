namespace PrimusGE.Components.AI {
    
/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public sealed class BrainComponent {
    /*-------------------------------------
     * PUBLIC FIELDS
     *-----------------------------------*/

    public float ThinkTimer;

    /*-------------------------------------
     * PUBLIC PROPERTIES
     *-----------------------------------*/

    public Action ThinkFunc { get; set; }

    public float InvThinkRate { get; set; } = 1.0f/30.0f;
}

}
