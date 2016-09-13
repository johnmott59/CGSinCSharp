# CGSinCSharp

This code is a set of classes for handling Constructive Solid Geometry (CSG) of a polygon mesh in C# and some helper code to render the results in ASP.NET MVC using three.js

It is a C# port of the wonderful Javascript code located http://evanw.github.io/csg.js/docs/

The CSG work is done in the CSG classes, files beginning with CSG. There are a few other files to help render the results in ASP.NET MVC, but I've intentionally kept the classes pure code not dependent on what kind of app they are run in. 

The code contains a cube, sphere and cylinder for testing but you can load any polygon mesh into the classes and it will work -- the code doesn't know that the mesh came from a sphere, cylinder or cube. The meshes do need to be closed meshes in order to work properly.

The output of the operations is a list of CSGPolygons, they can be sent to wherever they are needed. In my case I convert them to a JSON object and send them to the viewer to render in three.js

If you only want the CSG code and want to see how its called get all of the CSG*.cs files and the file P0830Controller.cs to see how its called and used. 

If you want to build an MVC app there is a controller file:
P0830Controller.cs

View file P0830.cshtml and _Layout.cshtml file
The include files required for the view are
   + 3JSInclude.js  (routines for building the scene)
   + the Three.js library
   + JQuery 

The MVC app assumes the presence Newtonsoft JSON library a JSON object to pass data from the controller to the view.

ThreeJS, JQuery and Newtonsoft are all available via NuGet.

To see a working sample visit http://meshola.com/prototype/p0830

Many props to Evan for the Javascript work. I think I'm a solid programmer, and I have flashes of insight at times but the code he wrote gets to the very heart of the problem and solves it with an elegance that is unlike anything I've ever seen, and in Javacript to boot.
