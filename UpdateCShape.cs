using System.Collections.Generic;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using CSGV1;

namespace threejs.Controllers
{
    public partial class PrototypeController 
    {
        [HttpPost]
        /*
         * Recieve an AJAX command to select an operation and generate the desired shape
         */
        public ContentResult P0830_SelectCSG(string operation)
        {

            List<CSGPolygon> PolygonList = null;
            bool BothSides = false;

            switch (operation)
            {
                case "union":
                    PolygonList = CSG.Sphere().union(CSG.Cube()).toPolygons();
                    break;
                case "intersection":
                    PolygonList = CSG.Sphere().intersect(CSG.Cube()).toPolygons();
                    break;
                case "subtraction":
                    PolygonList = CSG.Sphere().subtract(CSG.Cube()).toPolygons();
                    BothSides = true;
                    break;
                case "subtraction2":
                    PolygonList = CSG.Cube().subtract(CSG.Sphere()).toPolygons();
                    BothSides = true;
                    break;
            }

            /*
             * Create a JSON object containing the 3D shape to send to the browser
             */
            JObject JShape = new JObject();
            /*
             * The design of the JSON object is an array of shapes. In our case we only send one
             */
            JArray shapeArray = new JArray();

            JShape.Add("ShapeList", shapeArray);

            JObject oShape = AddPolygonList(CSG.fromTriangles(PolygonList), BothSides);

            shapeArray.Add(oShape);

            return Content(JShape.ToString());
        }
        /*----------------------------------------------------------------------------------------------------------------
         * Add a polygon list to the output shape list. If desired add both sides of each face. This would be useful if a 
         * subtract operation had been done, the inner faces would normally be hidden.
         */
        protected JObject AddPolygonList(CSG oShape, bool BothSides)
        {
            JObject jo = new JObject();

            JArray vertices = new JArray();
            jo.Add(new JProperty("VertexList", vertices));

            JArray normals = new JArray();
            jo.Add(new JProperty("NormalList", normals));

            JArray faces = new JArray();
            jo.Add(new JProperty("TriangleIndexList", faces));

            JArray textures = new JArray();
            jo.Add(new JProperty("TextureList", textures));

            int vertexIndex = 0;
            foreach (CSGPolygon p in oShape.toPolygons())
            {
                // add the vertices and normals for this face
                foreach (CSGVertex v in p.vertices)
                {
                    vertices.Add(new JArray(v.pos.x, v.pos.y, v.pos.z));
                    normals.Add(new JArray(v.normal.x, v.normal.y, v.normal.z));
                    textures.Add(new JArray(0, 0));
                }
                // now add the indices of the vertices for face
                // Add each side of the face so that subtracted surfaces will show
                faces.Add(new JArray(vertexIndex, vertexIndex + 1, vertexIndex + 2));
                /*
                 * If desired add the opposite face to the display. This is useful if a subtract operation has been done
                 * When there is a subtract operation the 'outside' of the shape that was subtracted is inside another shape,
                 * and the normals for those faces point inside the shape. This is actually correct behavior but its not 
                 * intuitive for display purposes so in that case both sides of the shape need to be displayed
                 */
                if (BothSides) faces.Add(new JArray(vertexIndex, vertexIndex + 2, vertexIndex + 1));

                vertexIndex += 3;
            }

            return jo;
        }

    }
}