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

        public CSGVertex(float[] pos,float[] normal)
        {
            this.pos = new CSGVector(pos);
            this.normal = new CSGVector(normal);
        }

        public CSGVertex clone()
        {
            return new CSGVertex(this.pos.clone(), this.normal.clone());
        }

        public void flip()
        {
            this.normal = this.normal.negated();
        }

        public CSGVertex interpolate(CSGVertex other,float t)
        {
            return new CSGVertex(this.pos.lerp(other.pos, t), this.normal.lerp(other.normal, t));
        }

    }
}
