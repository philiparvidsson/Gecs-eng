namespace PrimusGE.Subsystems {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using Components;
using Core;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public class LifetimeSubsystem: Subsystem {
    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public override void Update(float dt) {
        base.Update(dt);

        var entities = Game.Inst.Scene.GetEntities<LifetimeComponent>();
        foreach (var entity in entities) {
            var lifetime = entity.GetComponent<LifetimeComponent>();

            lifetime.Age += dt;

            if (lifetime.Age >= lifetime.Lifetime) {
                lifetime.EndOfLife?.Invoke();
                entity.Destroy();
            }
        }
    }
}

}
