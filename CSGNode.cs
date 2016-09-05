using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGV1
{
    public partial class CSGNode
    {
        public CSGPlane plane { get; set; } = null;
        public CSGNode front { get; set; } = null;
        public CSGNode back { get; set; } = null;
        List<CSGPolygon> polygons { get; set; } = new List<CSGPolygon>();

        public CSGNode()
        {

        }
        public CSGNode(List<CSGPolygon> polygons)
        {
            if (polygons != null) this.build(polygons);
        }

        public CSGNode clone()
        {
            CSGNode node = new CSGNode();
            node.plane = this.plane == null ? null : this.plane.clone();
            node.front = this.front == null ? null : this.front.clone();
            node.back = this.back == null ? null : this.back.clone();

            node.polygons = new List<CSGPolygon>();

            foreach (CSGPolygon p in this.polygons) node.polygons.Add(p.clone());

            return node;
        }

        public void invert()
        {
            for (int i = 0; i < this.polygons.Count; i++) this.polygons[i].flip();
            this.plane.flip();
            if (this.front != null) this.front.invert();
            if (this.back != null) this.back.invert();
            var temp = this.front;
            this.front = this.back;
            this.back = temp;
        }

        public List<CSGPolygon> clipPolygons(List<CSGPolygon> polygons)
        {
            if (this.plane == null) {
                /* 
                 * Create a shallow copy of the list
                 */
                CSGPolygon[] plist = new CSGPolygon[polygons.Count];

                polygons.CopyTo(plist);

                return plist.ToList<CSGPolygon>();

            }

            List<CSGPolygon> front = new List<CSGPolygon>();
            List<CSGPolygon> back = new List<CSGPolygon>();

            for (int i=0; i < polygons.Count; i++)
            {
                this.plane.splitPolygon(polygons[i], front, back, front, back);   
            }

            if (this.front != null)
            {
                front = this.front.clipPolygons(front);
            }
            if (this.back != null)
            {
                back = this.back.clipPolygons(back);
            }
            else back = new List<CSGPolygon>();

            foreach (CSGPolygon p in back)
            {
                front.Add(p);
            }

            return front;
        }

        public void clipTo(CSGNode bsp)
        {
            this.polygons = bsp.clipPolygons(this.polygons);
            if (this.front != null) this.front.clipTo(bsp);
            if (this.back != null) this.back.clipTo(bsp);
        }

        public List<CSGPolygon> allPolygons()
        {
            /* 
             * Create a shallow copy of the list
             */
            CSGPolygon[] plist = new CSGPolygon[this.polygons.Count];

            polygons.CopyTo(plist);

            polygons =  plist.ToList<CSGPolygon>();

            if (this.front != null) {
                foreach (CSGPolygon p in this.front.allPolygons())
                {
                    polygons.Add(p);
                }
            }

            if (this.back != null) {
                foreach (CSGPolygon p in this.back.allPolygons())
                {
                    polygons.Add(p);
                }
            }

            return polygons;
        }

        public void build(List<CSGPolygon> polygons)
        {
            if (polygons.Count == 0) return;
            if (this.plane == null) this.plane = polygons[0].plane.clone();
            List<CSGPolygon> front = new List<CSGPolygon>();
            List<CSGPolygon> back = new List<CSGPolygon>();

            for (int i=0; i < polygons.Count; i++)
            {
                /*
                 * Examine each polygon in the list. place it into 1 of three lists:f
                 * 1. this.polygons - coplaner, part of this node
                 * 2. this.front - polygons in front of this plane
                 * 3. this.back - polygons in back of this plane
               
                 * The 0th iteration of this loop should be a co-planer polygon since the test plane is equal to that polygon

                 */
                this.plane.splitPolygon(polygons[i], this.polygons, this.polygons, front, back);
            }
            if (front.Count > 0)
            {
                if (this.front == null) this.front = new CSGNode();
                this.front.build(front);
            }
            if (back.Count > 0)
            {
                if (this.back == null) this.back = new CSGNode();
                this.back.build(back);
            }
        }
    }
}
