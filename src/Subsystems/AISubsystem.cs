namespace PrimusGE.Subsystems {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using Components.AI;
using Core;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public class AISubsystem: Subsystem {
    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public override void Update(float dt) {
        base.Update(dt);

        var entities = Game.Inst.Scene.GetEntities<BrainComponent>();
        foreach (var entity in entities) {
            var brain = entity.GetComponent<BrainComponent>();

            var t = brain.ThinkTimer + dt;

            var invThinkRate = brain.InvThinkRate;
            while (t >= invThinkRate) {
                brain.ThinkFunc?.Invoke();
                t -= invThinkRate;
            }

            brain.ThinkTimer = t;
        }
    }
}

}
