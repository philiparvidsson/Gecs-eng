namespace GecsEng.Math.Geom {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;
using System.Collections.Generic;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public class Shape {
    /*-------------------------------------
     * PUBLIC PROPERTIES
     *-----------------------------------*/

    public Vector2[] Points { get; set; }

    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public static Shape Rectangle(float width, float height) {
        var hh = 0.5f*height;
        var hw = 0.5f*width;

        var points = new [] {
            new Vector2( hw,  hh),
            new Vector2(-hw,  hh),
            new Vector2(-hw, -hh),
            new Vector2( hw, -hh)
        };
        
        return new Shape { Points = points };
    }

    public static Shape Ngon(float radius, int n) {
        var points = new Vector2[n];

        var k = 2.0f*(float)Math.PI / n;
        for (var i = 0; i < n; i++) {
            var x = (float)Math.Cos(k*i)*radius;
            var y = (float)Math.Sin(k*i)*radius;

            points[i] = new Vector2(x, y);
        }

        return new Shape { Points = points };
    }

    public Shape[] Split(int i0, int i1) {
        if (i0 > i1) {
            var tmp = i0;
            i0 = i1;
            i1 = tmp;
        }

        var points0 = new List<Vector2>();
        var points1 = new List<Vector2>();

        for (var i = i0; i <= i1; i++) {
            points0.Add(Points[i]);
        }

        while (i1 != i0) {
            points1.Add(Points[i1]);
            
            i1 = (i1 + 1) % Points.Length;
        }

        return new [] {
            new Shape { Points = points0.ToArray() },
            new Shape { Points = points1.ToArray() }
        };
    }
}

}
