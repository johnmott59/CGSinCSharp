using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGV1
{
    public partial class CSG
    {
        public List<CSGPolygon> polygons { get; set; } = new List<CSGPolygon>();

        public static CSG fromPolygons(List<CSGPolygon> polygons)
        {
            return new CSG() { polygons = polygons };
        }
        /*
         * Return a new instance of the CSG object, but break multi sided polygons into triangles.
         * If you're fine with polygons of more than 3 sides you can just call fromPolygons
         * Don't do this until we're ready for the output of the CSG, the larger polygons are fine during the CSG operations
         */
        public static CSG fromTriangles(List<CSGPolygon> polygons)
        {
            List<CSGPolygon> TriangleList = new List<CSGPolygon>();
            foreach (CSGPolygon p in polygons)
            {
                TesselatePolygon(TriangleList, p);
            }
            /*
             * Flip each polygon so that the shapes are all two sided. 
             */
            foreach(CSGPolygon p in polygons)
            {
               // TriangleList.Add(p.clone().flip());
            }
            return new CSG() { polygons = TriangleList };
        }
        /*
         * Tesselate a convex multisided polygon
         */
        private static void TesselatePolygon(List<CSGPolygon> TriangleList,CSGPolygon p)
        {
            if (p.vertices.Count == 3)
            {
                TriangleList.Add(p);
                return;
            }
            /*
             * Retrieve the sequences of vertices to delete in order to snip a triangle
             */
            List<int[]> sequenceList = GetSequence(p.vertices.Count);
            /*
             * For each sequence, make a copy of the polygon and remove the vertex indices
             */
            foreach (int[] sequence in sequenceList)
            {
                CSGPolygon c = p.clone();
                foreach (int ndx in sequence) c.vertices.RemoveAt(ndx);
                TriangleList.Add(c);
            }
        }
        /*
         * Create a sequence of vertices to remove in order to clip triangles from a convex polygon with more than 3 sides
         * The sequence is creaated by selecting marching 1,2 through the sequence of vertices beginning with 0  adding 1 each time.
         * the 0th point is our anchor point, the first triangle is 0,1,2. then its 0,2,3 etc.
         *
         * The sequence consists of the numbers before and after the current 1,2, those are the values to clip
         *
         * For a 4 sided polygon the sequence of values to clip is
         * 3         [keep 1,2]
         * 1         [keep 2,3]

         * for a 5 sided its
         * 3 4       [keep 1,2]
         * 1 4       [keep 2,3]
         * 1 2       [keep 3,4]

         * for 7 side its
         * 3 4 5 6   [keep 1,2]
         * 1 4 5 6   [keep 2,3]
         * 1 2 5 6   [keep 3,4]
         * 1 2 3 6   [keep 4,5]
         * 1 2 3 4   [keep 5,6]
         * 
         */
        private static List<int[]> GetSequence(int count)
        {
            int[] sequence;
            List<int[]> sequenceList = new List<int[]>();

            int SequencePairStartIndex = 1;

            for (int i = 0; i < count - 2; i++, SequencePairStartIndex++)
            {
                sequence = new int[count - 3];      // we skip 0 and the current sequence pair

                int k = 0;

                for (int j = 1; j < SequencePairStartIndex; j++) sequence[k++] = j ;
             
                // now skip the current index and the next

                for (int j = SequencePairStartIndex + 2; j < count; j++) sequence[k++] = j;

                /*
                 * Reverse the array so that the higher number indices come first, we pull off of the end of the list
                 */
                Array.Reverse(sequence);

                sequenceList.Add(sequence);

            }

            return sequenceList;
        }

        public CSG clone()
        {
            CSG csg = new CSG();
            foreach (CSGPolygon p in polygons)
            {
                csg.polygons.Add(p.clone());
            }

            return csg;
        }

        public List<CSGPolygon> toPolygons()
        {
            return this.polygons;
        }

        public CSG union(CSG csg)
        {
            CSGNode a = new CSGNode(this.clone().polygons);
            CSGNode b = new CSGNode(csg.clone().polygons);
            a.clipTo(b);
            b.clipTo(a);
            b.invert();
            b.clipTo(a);
            b.invert();
            a.build(b.allPolygons());
            return CSG.fromTriangles(a.allPolygons());

        }

        public CSG subtract(CSG csg)
        {
            CSGNode a = new CSGNode(this.clone().polygons);
            CSGNode b = new CSGNode(csg.clone().polygons);
            a.invert();
            a.clipTo(b);
            b.clipTo(a);
            b.invert();
            b.clipTo(a);
            b.invert();
            a.build(b.allPolygons());
            a.invert();
            return CSG.fromTriangles(a.allPolygons());
        }

        public CSG intersect(CSG csg) { 
            CSGNode a = new CSGNode(this.clone().polygons);
            CSGNode b = new CSGNode(csg.clone().polygons);
            a.invert();
            b.clipTo(a);
            b.invert();
            a.clipTo(b);
            b.clipTo(a);
            a.build(b.allPolygons());
            a.invert();
            return CSG.fromTriangles(a.allPolygons());
        }

        public CSG inverse()
        {
            var csg = this.clone();
            foreach (CSGPolygon p in csg.polygons)
            {
                p.flip();
            }
            return csg;
        }


    }
}
