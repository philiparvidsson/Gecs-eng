namespace GecsEng.Subsystems {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;
using System.Collections.Generic;

using Components.Graphical;
using Components.Physical;
using Core;
using Graphics;
using Math;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public class GraphicsSubsystem: Subsystem {
    /*-------------------------------------
     * PUBLIC PROPERTIES
     *-----------------------------------*/

    public Color ClearColor { get; set; } = Color.Black;

    public Action DrawFunc { get; set; }

    public Matrix4x4 ViewMatrix { get; set; } = Matrix4x4.Identity();

    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public GraphicsSubsystem() {
        DrawFunc = () => {
            Game.Inst.Graphics.RenderTarget.Clear(ClearColor);

            var triMeshes = Game.Inst.Scene.GetEntities<TriMeshComponent>();
            foreach (var triMesh in triMeshes) {
                DrawTriMesh(triMesh);
            }
        };
    }

    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public Matrix4x4 CalcTransform(Entity entity) {
        var ode     = entity.GetComponent<BodyComponent>(); 
        var triMesh = entity.GetComponent<TriMeshComponent>();

        var a  = 0.0f;
        var m  = Matrix4x4.Identity();
        var tx = 0.0f;
        var ty = 0.0f;
        var tz = 0.0f;

        if (ode != null) {
            a  = ode.Angle;
            tx = ode.Position.X;
            ty = ode.Position.Y;
        }

        if (triMesh != null) {
            m = triMesh.Transform;
        }

        m = ViewMatrix
          * Matrix4x4.Translate(tx, ty, tz)
          * Matrix4x4.RotateZ  (a)
          * m;

        return m;
    }

    public override void Cleanup() {
        base.Cleanup();
    }

    public override void Draw(float dt) {
        base.Draw(dt);

        var g = Game.Inst.Graphics;

        g.BeginFrame();
        DrawFunc();
        g.EndFrame();
    }

    public void DrawTriMesh(Entity entity) {
        var g = Game.Inst.Graphics;

        var triMesh = entity.GetComponent<TriMeshComponent>();

        var transform = CalcTransform(entity);
        transform.Transpose();

        g.DrawTriMesh(triMesh.TriMesh, transform);
    }

    public override void Init() {
        base.Init();
    }
}

}
