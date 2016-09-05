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

        public CSGVector clone()
        {
            return new CSGVector(this.x, this.y, this.z);
        }

        public CSGVector negated ()
        {
            return new CSGVector(-this.x, -this.y, -this.z);
        }

        public CSGVector plus(CSGVector a)
        {
            return new CSGVector(this.x + a.x, this.y + a.y, this.z + a.z);
        }

        public CSGVector minus(CSGVector a)
        {
            return new CSGVector(this.x - a.x, this.y - a.y, this.z - a.z);
        }

        public CSGVector times(float a)
        {
            return new CSGVector(this.x * a, this.y * a, this.z * a);
        }

        public CSGVector dividedBy(float a)
        {
            return new CSGVector(this.x / a, this.y / a, this.z /a);
        }

        public float dot(CSGVector a)
        {
            return this.x* a.x + this.y * a.y + this.z * a.z;
        }

        public CSGVector lerp(CSGVector a,float t)
        {
            return this.plus(a.minus(this).times(t));
        }

        public float length ()
        {
            return (float) Math.Sqrt(this.dot(this));
        }

        public CSGVector unit()
        {
            return this.dividedBy(this.length());
        }

        public CSGVector cross(CSGVector a)
        {
            return new CSGVector(
                this.y * a.z - this.z * a.y,
                this.z * a.x - this.x * a.z,
                this.x * a.y - this.y * a.x);
        }
    }
}
