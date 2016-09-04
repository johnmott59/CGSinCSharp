using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGV1
{
    public partial class CSGVector
    {
        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }


        public CSGVector(CSGVertex v)
        {
            this.x = v.pos.x;
            this.y = v.pos.y;
            this.z = v.pos.z;
        }
        public CSGVector(float[] coords)
        {
            this.x = coords[0];
            this.y = coords[1];
            this.z = coords[2];
        }

        public CSGVector(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
#if false
        clone: function()
        {
            return new CSG.Vector(this.x, this.y, this.z);
        },
#endif
        public CSGVector clone()
        {
            return new CSGVector(this.x, this.y, this.z);
        }
#if false
 negated: function() {
    return new CSG.Vector(-this.x, -this.y, -this.z);
  },

#endif
        public CSGVector negated ()
        {
            return new CSGVector(-this.x, -this.y, -this.z);
        }
#if false
        plus: function(a) {
    return new CSG.Vector(this.x + a.x, this.y + a.y, this.z + a.z);
  },
#endif
        public CSGVector plus(CSGVector a)
        {
            return new CSGVector(this.x + a.x, this.y + a.y, this.z + a.z);
        }

#if false
        minus: function(a) {
    return new CSG.Vector(this.x - a.x, this.y - a.y, this.z - a.z);
  },
#endif
        public CSGVector minus(CSGVector a)
        {
            return new CSGVector(this.x - a.x, this.y - a.y, this.z - a.z);
        }
#if false

        times: function(a) {
    return new CSG.Vector(this.x * a, this.y * a, this.z * a);
  },
#endif
        public CSGVector times(float a)
        {
            return new CSGVector(this.x * a, this.y * a, this.z * a);
        }
#if false
  dividedBy: function(a) {
    return new CSG.Vector(this.x / a, this.y / a, this.z / a);
  },
#endif
        public CSGVector dividedBy(float a)
        {
            return new CSGVector(this.x / a, this.y / a, this.z /a);
        }
#if false
  dot: function(a) {
    return this.x * a.x + this.y * a.y + this.z * a.z;
  },
#endif
        public float dot(CSGVector a)
        {
            return this.x* a.x + this.y * a.y + this.z * a.z;
        }
#if false
  lerp: function(a, t) {
    return this.plus(a.minus(this).times(t));
  },
#endif
        public CSGVector lerp(CSGVector a,float t)
        {
            return this.plus(a.minus(this).times(t));
        }
#if false
  length: function() {
    return Math.sqrt(this.dot(this));
  },
#endif
        public float length ()
        {
            return (float) Math.Sqrt(this.dot(this));
        }
#if false
  unit: function() {
    return this.dividedBy(this.length());
  },
#endif
        public CSGVector unit()
        {
            return this.dividedBy(this.length());
        }
#if false
  cross: function(a) {
    return new CSG.Vector(
      this.y * a.z - this.z * a.y,
      this.z * a.x - this.x * a.z,
      this.x * a.y - this.y * a.x
    );
  }
#endif
        public CSGVector cross(CSGVector a)
        {
            return new CSGVector(
                this.y * a.z - this.z * a.y,
                this.z * a.x - this.x * a.z,
                this.x * a.y - this.y * a.x);
        }
    }
}
