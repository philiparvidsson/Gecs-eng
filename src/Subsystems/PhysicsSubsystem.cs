namespace GecsEng.Subsystems {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;
using System.Collections.Generic;

using Components.Physical;
using Core;
using Math;
using Math.Calc;
using Math.Geom;
using Messages;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

internal struct CollisionInfo {
    public bool IsColliding;
    public Vector2 Normal;
    public Entity EntityA;
    public Entity EntityB;
    public Vector2 ContactA;
    public Vector2 ContactB;
    public Vector2 ContactWorld;
}

public class PhysicsSubsystem: Subsystem {
    /*-------------------------------------
     * NON-PUBLIC FIELDS
     *-----------------------------------*/

    private Rectangle m_WorldBounds;

    /*-------------------------------------
     * PUBIC PROPERTIES
     *-----------------------------------*/

    public IIntegrator Integrator = new RungeKutta4Integrator();

    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public PhysicsSubsystem(Rectangle worldBounds) {
        m_WorldBounds = worldBounds;
    }

    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public override void Update(float dt) {
        base.Update(dt);

        var collisions = new List<CollisionInfo>();
        var entities   = Game.Inst.Scene.GetEntities<BodyComponent>();

        foreach (var entity in entities) {
            var body = entity.GetComponent<BodyComponent>();
            Array.Copy(body.State, body.PrevState, body.State.Length);
        }

        var a = 1;
        var b = 1;

        while (dt > 1.0/1000000.0f) {
            float x = (float)a/b;

            foreach (var entity in entities) {
                var body = entity.GetComponent<BodyComponent>();

                if (body.IsStatic) {
                    continue;
                }

                Array.Copy(body.PrevState, body.State, body.State.Length);
                Integrator.Solve(entity.GetComponent<BodyComponent>(), dt*x, body.DerivsFn);
            }

            if (FindCollisions(entities, collisions) > 0) {
                if ((b <<= 1) == (1 << 8)) {
                    ResolveCollisions(collisions);

                    foreach (var entity in entities) {
                        var body = entity.GetComponent<BodyComponent>();

                        if (body.IsStatic) {
                            continue;
                        }

                        body.PrevState[3] = body.State[3];
                        body.PrevState[4] = body.State[4];
                        body.PrevState[5] = body.State[5];
                    }

                    a = b = 1;
                }
            }
            else {
                foreach (var entity in entities) {
                    var body = entity.GetComponent<BodyComponent>();

                    if (body.IsStatic) {
                        continue;
                    }

                    Array.Copy(body.State, body.PrevState, body.State.Length);
                }

                dt -= dt*x;
                a <<= 1;
            }
        }
    }

    /*-------------------------------------
     * NON-PUBLIC METHODS
     *-----------------------------------*/

    private Vector2[] BestEdge(Vector2[] points, Vector2 normal) {
        var result = new Vector2[3];

        var n = points.Length;

        var max = float.MinValue;
        var index = 0;

        for (var i = 0; i < n; i++) {
            var x = points[i];
            var p = normal.Dot(x);
            if (p > max) {
                max = p;
                index = i;
            }
        }

        var next = (index+1) % points.Length;
        var prev = index-1;
        if (prev < 0) {
            prev = points.Length - 1;
        }

        Vector2 v = points[index];
        Vector2 v1 = points[next];
        Vector2 v0 = points[prev];
        Vector2 l = v - v1;
        Vector2 r = v - v0;

        l.Normalize();
        r.Normalize();

        if (r.Dot(normal) <= l.Dot(normal)) {
            result[0] = v;
            result[1] = v0;
            result[2] = v;
        }
        else {
            result[0] = v;
            result[1] = v;
            result[2] = v1;
        }

        return result;
    }

    private void FindBodyBodyCollision(Entity entityA, Entity entityB, IList<CollisionInfo> collisions) {
        var bodyA = entityA.GetComponent<BodyComponent>();
        var bodyB = entityB.GetComponent<BodyComponent>();

        var shapeA = bodyA.Shape;
        var shapeB = bodyB.Shape;

        if (shapeA == null || shapeB == null) {
            return;
        }

        var numA = shapeA.Points.Length;
        var numB = shapeB.Points.Length;

        var pointsA = new Vector2[numA];
        var pointsB = new Vector2[numB];

        var axes = new Vector2[numA + numB];
        var posA = bodyA.Position;
        var posB = bodyB.Position;

        Matrix3x3 mA = Matrix3x3.Translate(posA.X, posA.Y)
                     * Matrix3x3.RotateZ(bodyA.Angle);
        Matrix3x3 mB = Matrix3x3.Translate(posB.X, posB.Y)
                     * Matrix3x3.RotateZ(bodyB.Angle);

        for (var i = 0; i < numA; i++) {
            pointsA[i] = (mA*new Vector3(shapeA.Points[i])).XY;
        }

        for (var i = 0; i < numB; i++) {
            pointsB[i] = (mB*new Vector3(shapeB.Points[i])).XY;
        }

        for (var i = 0; i < numA; i++) {
            var j = (i+1) % numA;
            var p1 = pointsA[i];
            var p2 = pointsA[j];
            var edge = p2 - p1;
            var n = edge.Perp();
            axes[i] = n;
        }

        for (var i = 0; i < numB; i++) {
            var j = (i+1) % numB;
            var p1 = pointsB[i];
            var p2 = pointsB[j];
            var edge = p2 - p1;
            var n = edge.Perp();
            axes[numA + i] = n;
        }

        var overlapMin = float.MaxValue;
        var mtv = new Vector2(0.0f, 0.0f);

        foreach (Vector2 axis in axes) {
            float p1Min, p1Max;
            float p2Min, p2Max;

            Project(pointsA, axis, out p1Min, out p1Max);
            Project(pointsB, axis, out p2Min, out p2Max);

            if (((p2Max < p1Min) || (p1Max < p2Min))) {
                return;
            }

            var overlap = 0.0f;
            if (p1Max > p2Min) {
                overlap = p1Max - p2Min;
            }
            else {
                overlap = p1Min - p2Max; 
            }

            var r2 = axis.Dot(axis);
            var overlap2 = overlap*overlap/r2;

            
            if (overlap2 < overlapMin) {
                overlapMin = overlap2;
                mtv = axis*(overlap/r2);
            }
        }

        // -----------------------------------------------------------
        mtv.Normalize();

        var eA = BestEdge(pointsA, mtv);
        var eB = BestEdge(pointsB, mtv * -1.0f);

        var flip = false;

        Vector2[] eRef, eInc;
        if ((float)Math.Abs((eA[2] - eA[1]).Dot(mtv)) <= (float)Math.Abs((eB[2] - eB[1]).Dot(mtv))) {
            eRef = eA;
            eInc = eB;
        }
        else {
            eRef = eB;
            eInc = eA;

            flip = true;
        }

        Vector2 vRef = eRef[2] - eRef[1];
        vRef.Normalize();

        var o1 = vRef.Dot(eRef[1]);

        var cPoints = Clip(eInc[1], eInc[2], vRef, o1);

        if (cPoints.Count < 2) {
            return;
        }

        var o2 = vRef.Dot(eRef[2]);

        cPoints = Clip(cPoints[0].XY, cPoints[1].XY, vRef * -1.0f, -o2);

        if (cPoints.Count < 2) {
            return;
        }

        Vector2 refNorm = (eRef[2] - eRef[1]);
        var rx = refNorm.X;
        var ry = refNorm.Y;
        refNorm.X = -ry;
        refNorm.Y = rx;

        if (flip) {
            //refNorm *= -1.0f;
        }

        float max = refNorm.Dot(eRef[0]);

        cPoints[0] = new Vector3(cPoints[0].XY, refNorm.Dot(cPoints[0].XY) - max);
        cPoints[1] = new Vector3(cPoints[1].XY, refNorm.Dot(cPoints[1].XY) - max);

        /*if (cPoints[0].Z < 0.0f) {
            cPoints.RemoveAt(0);

            if (cPoints[0].Z < 0.0f) {
                cPoints.RemoveAt(0);
            }
        }
        else {
            if (cPoints[1].Z < 0.0f) {
                cPoints.RemoveAt(1);
            }
        }*/


        var mAi = Matrix3x3.Translate(-posA.X, -posA.Y);
        var mBi = Matrix3x3.Translate(-posB.X, -posB.Y);
        var avgA = new Vector2(0.0f, 0.0f);
        var avgB = new Vector2(0.0f, 0.0f);
        var avgW = new Vector2(0.0f, 0.0f);

        int num = 0;
        foreach (var p in cPoints) {
            if (p.Z >= 0.0f) {
                avgA += p.XY;
                avgB += p.XY;
                avgW += p.XY;
                num++;
            }
        }

        avgA *= 1.0f/num;
        avgB *= 1.0f/num;
        avgW *= 1.0f/num;

        avgA = (mAi*new Vector3(avgA)).XY;
        avgB = (mBi*new Vector3(avgB)).XY;

        var nA = mtv * -1.0f;
        var nB = mtv;

        if (num == 0) {
            return;
        }

        collisions.Add(new CollisionInfo {
            ContactA = avgA,
            ContactB = avgB,
            ContactWorld = avgW,
            EntityA = entityA,
            EntityB = entityB,
            IsColliding = true,
            Normal = nA
        });
    }

    private List<Vector3> Clip(Vector2 v1, Vector2 v2, Vector2 n, float o) {
        var cp = new List<Vector3>();

        var d1 = n.Dot(v1) - o;
        var d2 = n.Dot(v2) - o;

        if (d1 >= 0.0f) cp.Add(new Vector3(v1, 0.0f));
        if (d2 >= 0.0f) cp.Add(new Vector3(v2, 0.0f));

        if (d1*d2 < 0.0f) {
            Vector2 e = v2 - v1;
            var u = d1/(d1 - d2);
            e *= u;
            e += v1;
            cp.Add(new Vector3(e, 0.0f));
        }

        return cp;
    }

    private CollisionInfo FindBodyWorldCollision(Entity entity) {
        var body  = entity.GetComponent<BodyComponent>();
        var shape = body.Shape;
        
        if (shape == null) {
            return default (CollisionInfo);
        }

        int numContacts = 0;
        var ci = new CollisionInfo();

        ci.EntityA = entity;

        Matrix2x2 rot = Matrix2x2.RotateZ(body.Angle);

        foreach (var p in shape.Points) {
            Vector2 localPos = rot*p;
            Vector2 worldPos = localPos + body.Position;

            bool f = false;

            if (worldPos.X < m_WorldBounds.Left  ) { f = true; ci.Normal += new Vector2( 1.0f,  0.0f); }
            if (worldPos.X > m_WorldBounds.Right ) { f = true; ci.Normal += new Vector2(-1.0f,  0.0f); }
            if (worldPos.Y < m_WorldBounds.Bottom) { f = true; ci.Normal += new Vector2( 0.0f,  1.0f); }
            if (worldPos.Y > m_WorldBounds.Top   ) { f = true; ci.Normal += new Vector2( 0.0f, -1.0f); }

            if (f) {
                numContacts++;
                ci.ContactA += localPos;
            }
        }

        if (numContacts > 0) {
            ci.IsColliding = true;
            ci.ContactA *= 1.0f / numContacts;
            ci.Normal.Normalize();
        }

        return ci;
    }

    private int FindCollisions(IReadOnlyList<Entity> entities, IList<CollisionInfo> collisions) {
        collisions.Clear();

        var numEntities = entities.Count;
        for (var i = 0; i < numEntities; i++) {
            var entityA  = entities[i];
            var hasShape = entityA.GetComponent<BodyComponent>().Shape != null;

            if (!hasShape) {
                continue;
            }

            var ci = FindBodyWorldCollision(entityA);
            if (ci.IsColliding) {
                collisions.Add(ci);
            }

            for (var j = (i+1); j < numEntities; j++) {
                var entityB = entities[j];

                FindBodyBodyCollision(entityA, entityB, collisions);
            }
        }

        return collisions.Count;
    }
    
    private void Project(Vector2[] points, Vector2 axis, out float min, out float max) {
        min = axis.Dot(points[0]);
        max = min;

        var numPoints = points.Length;
        for (var i = 1; i < numPoints; i++) {
            var p = axis.Dot(points[i]);
            if (p < min) {
                min = p;
            }
            if (p > max) {
                max = p;
            }
        }
    }

    private void ResolveCollisions(ICollection<CollisionInfo> collisions) {
        foreach (var ci in collisions) {
            var a = ci.EntityA.GetComponent<BodyComponent>();
            Vector2 velA = ci.ContactA.Perp();
            Vector2 velB = new Vector2(0.0f, 0.0f);

            velA *= a.AngularVelocity;
            velA += a.Velocity;

            var velAB = velB - velA;

            Vector2 rA = ci.ContactA;
            Vector2 rB = new Vector2(0.0f, 0.0f);

            var invMassA = a.InvMass;
            var invMassB = 0.0f;
            var invMoIA = a.InvMoI;
            var invMoIB = 0.0f;
            var e = a.Restitution;

            BodyComponent b = null;
            if (ci.EntityB != null) {
                b = ci.EntityB.GetComponent<BodyComponent>();
                velB = ci.ContactB.Perp();
                velB *= b.AngularVelocity;
                velB += b.Velocity;
                velAB = velB - velA;
                rB = ci.ContactB;
                invMassB = b.InvMass;
                invMoIB = b.InvMoI;

                e = (float)Math.Max(e, b.Restitution);
            }

            float j = -(1.0f + e) * velAB.Dot(ci.Normal);
                  j /=  ci.Normal.Dot(ci.Normal) * (invMassA + invMassB)
                      + invMoIA*(float)Math.Pow(rA.PerpDot(ci.Normal), 2.0f)
                      + invMoIB*(float)Math.Pow(rB.PerpDot(ci.Normal), 2.0f);

            Vector2 impulseA = -j*ci.Normal;
            Vector2 impulseB = j*ci.Normal;
            
            a.ApplyImpulse(impulseA, ci.ContactA);

            if (b != null) {
                b.ApplyImpulse(impulseB, ci.ContactB);
            }

            Game.Inst.PostMessage(new CollisionMsg(ci.EntityA, ci.EntityB, ci.ContactWorld, ci.Normal));
        }
    }
}

}
