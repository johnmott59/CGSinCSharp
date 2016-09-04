using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGV1
{
  

    public partial class CSG
    {
        static CSGVector CubeCenter = new CSGVector(0, 0, 0);
        static int[] CubeDimension= new int[] { 20, 20, 20 };



        public static CSG Cube()
        {
            return CSG.Cube(new CSGVector(0, 0, 0));
        }

        public static CSG Cube(int size)
        {
            return CSG.Cube(new CSGVector(0, 0, 0), size);
        }

        public static CSG Cube(CSGVector center)
        {
            return CSG.Cube(center, 20);
        }

        public static CSG Cube(CSGVector center,int size)
        {
            return CSG.Cube(center, size,size,size);
        }

        public static CSG Cube(CSGVector center,int r1,int r2,int r3) 
        {
            CubeCenter = center;

            CubeDimension[0] = r1;
            CubeDimension[1] = r2;
            CubeDimension[2] = r3;
            
            List<CubeMaker> cubemakerList = new List<CubeMaker>();

            cubemakerList.Add(new CubeMaker(0, 4, 6, 2, -1, 0, 0));
            cubemakerList.Add(new CubeMaker(1, 3, 7, 5, +1, 0, 0));
            cubemakerList.Add(new CubeMaker(0, 1, 5, 4, 0, -1, 0));
            cubemakerList.Add(new CubeMaker(2, 6, 7, 3, 0, +1, 0));
            cubemakerList.Add(new CubeMaker(0, 2, 3, 1, 0, 0, -1));
            cubemakerList.Add(new CubeMaker(4, 5, 7, 6, 0, 0, +1));

            List<CSGPolygon> polygonList = new List<CSGPolygon>();

            foreach (CubeMaker cm in cubemakerList)
            {
                List<CSGVertex> vertexList = new List<CSGVertex>();

                for (int i=0; i < 4; i++)
                {
                    float x = CubeCenter.x + CubeDimension[0] * 2 * ((cm.param[i] & 1) != 0 ? 1 : 0) - 1;
                    float y = CubeCenter.y + CubeDimension[1] * 2 * ((cm.param[i] & 2) != 0 ? 1 : 0) - 1;
                    float z = CubeCenter.z + CubeDimension[2] * 2 * ((cm.param[i] & 4) != 0 ? 1 : 0) - 1;

                    CSGVector pos = new CSGVector(x, y, z);
                    CSGVector n = new CSGVector(cm.nx, cm.ny, cm.nz);

                    vertexList.Add(new CSGVertex(pos, n));
                }

                polygonList.Add(new CSGPolygon(vertexList, 0));
            }

            return CSG.fromPolygons(polygonList);
        }



        class CubeMaker
        {
            public int[] param = new int[4];
            public int nx, ny, nz;
            public CubeMaker(int i1,int i2, int i3, int i4, int nx, int ny,int nz)
            {
                param[0] = i1;
                param[1] = i2;
                param[2] = i3;
                param[3] = i4;

                this.nx = nx;
                this.ny = ny;
                this.nz = nz;
            }
 
        }
    }
}
