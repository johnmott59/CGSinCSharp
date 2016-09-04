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
#if false
CSG.Node.prototype = {
  clone: function() {
    var node = new CSG.Node();
    node.plane = this.plane && this.plane.clone();
    node.front = this.front && this.front.clone();
    node.back = this.back && this.back.clone();
    node.polygons = this.polygons.map(function(p) { return p.clone(); });
    return node;
  },
#endif
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

#if false

         invert: function() {
    for (var i = 0; i < this.polygons.length; i++) {
      this.polygons[i].flip();
    }
    this.plane.flip();
    if (this.front) this.front.invert();
    if (this.back) this.back.invert();
    var temp = this.front;
    this.front = this.back;
    this.back = temp;
  },
#endif
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

#if false
  clipPolygons: function(polygons) {
    if (!this.plane) return polygons.slice();
    var front = [], back = [];
    for (var i = 0; i < polygons.length; i++) {
      this.plane.splitPolygon(polygons[i], front, back, front, back);
    }
    if (this.front) front = this.front.clipPolygons(front);
    if (this.back) back = this.back.clipPolygons(back);
    else back = [];
    return front.concat(back);
  },
#endif
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

#if false
        clipTo: function(bsp) {
    this.polygons = bsp.clipPolygons(this.polygons);
    if (this.front) this.front.clipTo(bsp);
    if (this.back) this.back.clipTo(bsp);
  },
#endif
        public void clipTo(CSGNode bsp)
        {
            this.polygons = bsp.clipPolygons(this.polygons);
            if (this.front != null) this.front.clipTo(bsp);
            if (this.back != null) this.back.clipTo(bsp);
        }

#if false
 allPolygons: function() {
    var polygons = this.polygons.slice();
    if (this.front) polygons = polygons.concat(this.front.allPolygons());
    if (this.back) polygons = polygons.concat(this.back.allPolygons());
    return polygons;
  },
#endif
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
#if false
        build: function(polygons) {
    if (!polygons.length) return;
    if (!this.plane) this.plane = polygons[0].plane.clone();
    var front = [], back = [];
    for (var i = 0; i < polygons.length; i++) {
      this.plane.splitPolygon(polygons[i], this.polygons, this.polygons, front, back);
    }
    if (front.length) {
      if (!this.front) this.front = new CSG.Node();
      this.front.build(front);
    }
    if (back.length) {
      if (!this.back) this.back = new CSG.Node();
      this.back.build(back);
    }
  }
#endif
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
