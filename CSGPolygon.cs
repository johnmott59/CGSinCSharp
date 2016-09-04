using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGV1
{
    public partial class CSGPolygon
    {
        public List<CSGVertex> vertices { get; set; } = new List<CSGVertex>();
        public int shared { get; set; }
        public CSGPlane plane { get; set; }

        public CSGPolygon()
        {

        }
#if false
CSG.Polygon = function(vertices, shared) {
  this.vertices = vertices;
  this.shared = shared;
  this.plane = CSG.Plane.fromPoints(vertices[0].pos, vertices[1].pos, vertices[2].pos);
};
#endif
        public CSGPolygon(List<CSGVertex> vertices,int shared)
        {
            this.vertices = vertices;
            this.shared = shared;
            this.plane = CSGPlane.fromPoints(vertices[0].pos, vertices[1].pos, vertices[2].pos);                        
        }

        public CSGPolygon(CSGVertex P0, CSGVertex P1, CSGVertex P2)
        {
            vertices.Add(P0);
            vertices.Add(P1);
            vertices.Add(P2);

            shared = 0;

            this.plane = CSGPlane.fromPoints(P0.pos, P1.pos, P2.pos);
        }

        public CSGPolygon(CSGVertex P0, CSGVertex P1, CSGVertex P2,CSGVertex P3)
        {
            vertices.Add(P0);
            vertices.Add(P1);
            vertices.Add(P2);
            vertices.Add(P3);

            shared = 0;

            this.plane = CSGPlane.fromPoints(P0.pos, P1.pos, P2.pos);
        }
#if false
        clone: function()
        {
            var vertices = this.vertices.map(function(v) { return v.clone(); });
            return new CSG.Polygon(vertices, this.shared);
        },
#endif
        public CSGPolygon clone()
        {
            CSGPolygon oClone = new CSGPolygon() { shared = this.shared, plane=this.plane.clone() };
            foreach (CSGVertex v in this.vertices)
            {
                oClone.vertices.Add(v.clone());
            }

            return oClone;

        }

#if false
        flip: function() {
    this.vertices.reverse().map(function(v) { v.flip(); });
    this.plane.flip();
  }
#endif
        public CSGPolygon flip()
        {
            for (int i=this.vertices.Count-1; i >= 0; i--)
            {
                this.vertices[i].flip();
            }
            this.plane.flip();

            return this;
        }
    }
}
