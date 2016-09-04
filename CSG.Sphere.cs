using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGV1
{
    public partial class CSG
    {


        static CSGVector center = new CSGVector(0, 30, 0);
        static int SphereRadius = 20;

        public static CSG Sphere()
        {
            int slices = 16;
            int stacks = 8;

            List<CSGPolygon> polygons = new List<CSGPolygon>();

            for (int i=0; i < slices; i++)
            {
                for (int j=0; j < stacks; j++)
                {
                    List<CSGVertex> vertexList = new List<CSGVertex>();
                    vertexList.Add(AddVertex( (double) i / (double) slices, (double) j / (double) stacks));
                    if (j > 0) vertexList.Add(AddVertex((double) (i + 1) / (double)slices, (double)j / (double)stacks));
                    if (j < stacks - 1) vertexList.Add(AddVertex((double)(i + 1) / (double)slices, (double)(j + 1) / (double)stacks));
                    vertexList.Add(AddVertex((double) i / (double) slices, (double)(j + 1) / (double)stacks));
                    polygons.Add(new CSGPolygon(vertexList,0));
                }
            }

            return CSG.fromPolygons(polygons);
        }

        private static CSGVertex AddVertex(double theta,double phi)
        {
            theta *= Math.PI * 2;
            phi *= Math.PI;

            CSGVector dir = new CSGVector(
                (float) (Math.Cos(theta) * Math.Sin(phi)),
                (float) Math.Cos(phi),
                (float) (Math.Sin(theta) * Math.Sin(phi)));

            return new CSGVertex(center.plus(dir.times(SphereRadius)), dir);
        }
       
    }
}
