namespace GecsEng.Messages {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using Core;
using Math;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public class CollisionMsg: IMessage {
    /*-------------------------------------
     * PUBLIC PROPERTIES
     *-----------------------------------*/

    public Vector2 Contact { get; }

    public Entity EntityA { get; }

    public Entity EntityB { get; }

    public Vector2 Normal { get; }

    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public CollisionMsg(Entity entityA, Entity entityB, Vector2 contact,
                        Vector2 normal)
    {
        EntityA = entityA;
        EntityB = entityB;
        Contact = contact;
        Normal  = normal;
    }

}

}
