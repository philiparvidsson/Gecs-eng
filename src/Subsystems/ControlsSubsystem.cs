namespace PrimusGE.Subsystems {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using Components.Input;
using Core;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public abstract class ControlsSubsystem: Subsystem {
    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public override void Update(float dt) {
        base.Update(dt);

        var entities = Game.Inst.Scene.GetEntities<ControlsComponent>();
        foreach (var entity in entities) {
            var controls = entity.GetComponent<ControlsComponent>();

            UpdateControls(entity, controls, dt);
        }
    }

    protected abstract void UpdateControls(Entity            entity,
                                           ControlsComponent controls,
                                           float             dt);
}

}
