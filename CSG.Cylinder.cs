using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGV1
{
    public partial class CSG
    {
        static CSGVector axisZ;
        static CSGVector axisX;
        static CSGVector axisY;
        static CSGVector CylinderBottom = new CSGVector(0, 0, 0);
        static CSGVector CylinderTop = new CSGVector(0, 30, 0);
        static CSGVector ray;
        static int CylinderRadius = 20;

        public static CSG Cylinder()
        {
            return CSG.Cylinder(30, 20);
        }

        public static CSG Cylinder(int height, int radius)
        {
            return CSG.Cylinder(new CSGVector(0, 0, 0), new CSGVector(0, height, 0), radius);
        }

        public static CSG Cylinder(CSGVector bottom,CSGVector top, int radius) 
        {

            CylinderBottom = bottom;
            CylinderTop = top;
            CylinderRadius = radius;

            ray = CylinderTop.minus(CylinderBottom);

            int slices = 16;
            axisZ = ray.unit();
            int isY = Math.Abs(axisZ.y) > 0.5 ? 1 : 0;
            axisX = new CSGVector(isY, 1 - isY , 0).cross(axisZ).unit();
            axisY = axisX.cross(axisZ).unit();
            CSGVertex start = new CSGVertex(CylinderBottom, axisZ.negated());
            CSGVertex end = new CSGVertex(CylinderTop, axisZ.unit());
            List<CSGPolygon> polygons = new List<CSGPolygon>();

            for (int i=0; i < slices; i++)
            {
                double t0 = (double)i / (double)slices;
                double t1 = (double)(i + 1) / (double)slices;
                polygons.Add(new CSGPolygon(start, point(0, t0, -1), point(0, t1, -1)));
                polygons.Add(new CSGPolygon(point(0, t1, 0), point(0, t0, 0), point(1, t0, 0), point(1, t1, 0)));
                polygons.Add(new CSGPolygon(end, point(1, t1, 1), point(1, t0, 1)));
            }

            return CSG.fromPolygons(polygons);

        }

        private static CSGVertex point( double  stack, double  slice,int normalblend)
        {
            double angle = slice * Math.PI * 2;
            CSGVector _out = axisX.times((float) Math.Cos(angle)).plus(axisY.times((float) Math.Sin(angle)));
            CSGVector pos = CylinderBottom.plus(ray.times((float)stack)).plus(_out.times(CylinderRadius));
            CSGVector normal = _out.times(1 - Math.Abs(normalblend)).plus(axisZ.times(normalblend));
            return new CSGVertex(pos, normal);
        }
    }
}
