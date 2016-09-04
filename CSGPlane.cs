using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGV1
{
    public partial class CSGPlane
    {
        public CSGVector normal { get; set; }
        public float w { get; set; }

        protected float EPSILON = .00005f;

        public CSGPlane(CSGVector normal, float w)
        {
            this.normal = normal;
            this.w = w;
        }
#if false
        CSG.Plane.fromPoints = function(a, b, c) {
            var n = b.minus(a).cross(c.minus(a)).unit();
            return new CSG.Plane(n, n.dot(a));
        };

#endif
        public static CSGPlane fromPoints(CSGVector a, CSGVector b, CSGVector c) 
        {
            CSGVector n = b.minus(a).cross(c.minus(a)).unit();
            return new CSGPlane(n, n.dot(a));
        }

#if false
        clone: function() {
            return new CSG.Plane(this.normal.clone(), this.w);
        },
#endif
        public CSGPlane clone()
        {
            return new CSGPlane(this.normal.clone(), this.w);
        }
#if false
        flip: function() {
              this.normal = this.normal.negated();
             this.w = -this.w;
         },
#endif
        public void flip()
        {
            this.normal = this.normal.negated();
            this.w = -this.w;
        }

        public void splitPolygon(CSGPolygon polygon, List<CSGPolygon> coplanarFront, List<CSGPolygon> coPlanarBack, List<CSGPolygon> front, List<CSGPolygon> back)
        {
            const int COPLANAR = 0;
            const int FRONT = 1;
            const int BACK = 2;
            const int SPANNING = 3;

            int polygonType = 0;
            List<int> types = new List<int>();
            for (int i=0; i < polygon.vertices.Count; i++)
            {
                float t = this.normal.dot(polygon.vertices[i].pos) - this.w;
                int type = (t < -EPSILON) ? BACK : (t > EPSILON) ? FRONT : COPLANAR;
                polygonType |= type;
                types.Add(type);
            }

            switch (polygonType)
            {
                case COPLANAR:
                    (this.normal.dot(polygon.plane.normal) > 0 ? coplanarFront : coPlanarBack).Add(polygon);
                    break;
                case FRONT:
                    front.Add(polygon);
                    break;
                case BACK:
                    back.Add(polygon);
                    break;
                case SPANNING:
                    List<CSGVertex> f = new List<CSGVertex>();
                    List<CSGVertex> b = new List<CSGVertex>();
                    for (int i=0; i < polygon.vertices.Count; i++)
                    {
                        int j = (i + 1) % polygon.vertices.Count;
                        int ti = types[i];
                        int tj = types[j];
                        CSGVertex vi = polygon.vertices[i];
                        CSGVertex vj = polygon.vertices[j];
                        if (ti != BACK) f.Add(vi);
                        if (ti != FRONT) b.Add(ti != BACK ? vi.clone() : vi);
                        if ((ti | tj) == SPANNING)
                        {
                            float t = (this.w - this.normal.dot(vi.pos)) / this.normal.dot(vj.pos.minus(vi.pos));
                            CSGVertex v = vi.interpolate(vj, t);
                            f.Add(v);
                            b.Add(v.clone());
                        }

                    }
                    if (f.Count >= 3) front.Add(new CSGPolygon(f, polygon.shared));
                    if (b.Count >= 3) back.Add(new CSGPolygon(b, polygon.shared));
                    break;

            }

        }

    }
}
