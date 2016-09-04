using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGV1
{
    public partial class CSGVertex
    {
        public CSGVector pos { get; set; }
        public CSGVector normal { get; set; }

        public CSGVertex(CSGVector pos,CSGVector normal)
        {
            this.pos = pos;
            this.normal = normal;

        }

#if false
        CSG.Vertex = function(pos, normal) {
            this.pos = new CSG.Vector(pos);
            this.normal = new CSG.Vector(normal);
        };
#endif
        public CSGVertex(float[] pos,float[] normal)
        {
            this.pos = new CSGVector(pos);
            this.normal = new CSGVector(normal);
        }
#if false
         clone: function() {
        return new CSG.Vertex(this.pos.clone(), this.normal.clone());
        },
#endif
        public CSGVertex clone()
        {
            return new CSGVertex(this.pos.clone(), this.normal.clone());
        }

#if false
  flip: function() {
    this.normal = this.normal.negated();
  },
#endif
        public void flip()
        {
            this.normal = this.normal.negated();
        }
#if false
    interpolate: function(other, t) {
        return new CSG.Vertex(
            this.pos.lerp(other.pos, t),
            this.normal.lerp(other.normal, t)
    );
  }
#endif
        public CSGVertex interpolate(CSGVertex other,float t)
        {
            return new CSGVertex(this.pos.lerp(other.pos, t), this.normal.lerp(other.normal, t));
        }

    }
}
