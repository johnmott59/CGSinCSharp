# CGSinCSharp

This code is a set of classes for handling Constructive Solid Geometry (CSG) in C# and some helper code to render the results in ASP.NET MVC

It is a C# port of the wonderful Javascript code located http://evanw.github.io/csg.js/docs/

The CSG work is done in the CSG classes, files beginning with CSG. There are a few other files to help render the results in ASP.NET MVC, but I've intentionally kept the classes pure code not dependent on where its being run.

If you only want the CSG code and want to see how its called get all of the CSG*.cs files and the file UpdateCShape.cs to see how its called and used. The List<CSGPolygon> lsit is the final output of the operation.

If you want to build an MVC app there is a controller file:
P0830Controller.cs
  + helper file UpdateCShape.cs
  
View file P0830.cshtml
The include files required for the view are
   + 3JSInclude.js  (routines for building the scene)
   + the Three.js library

The MVC app assumes the presence of two things: Newtonsoft JSON library in order to build a JSON object to pass data from the controller to the view and Three.js for rendering the shapes. Both are available from NuGet

To see a working sample visit http://meshola.com/prototype/p0830

Many props to Evan for the Javascript work. I think I'm a solid programmer, and I have flashes of insight at times but the code he wrote gets to the very heart of the problem and solves it with an elegance that is unlike anything I've ever seen, and in Javacript to boot.
