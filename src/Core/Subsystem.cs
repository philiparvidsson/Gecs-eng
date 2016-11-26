namespace PrimusGE.Core {

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public abstract class Subsystem {
    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public virtual void Cleanup() {
    }

    public virtual void Draw(float dt) {
    }

    public virtual void Init() {
    }

    public virtual void Update(float dt) {
    }
}

}
