var scene;
var camera;
var geometry;
var material;
var tube1;
var cameraControl;
var normals;

// Our Javascript will go here.


function init() {

    // create a scene, that will hold all our elements such as objects, cameras and lights.
    scene = new THREE.Scene();

    // create a camera, which defines where we're looking at.
    //  camera = new THREE.PerspectiveCamera(45, window.innerWidth / window.innerHeight, 0.1, 1000); // to do whole screen
    camera = new THREE.PerspectiveCamera(45, $("#3dContent").width() / $("#3dContent").height(), 0.1, 1000);



    // create a render, sets the background color and the size
    renderer = new THREE.WebGLRenderer();
    renderer.setClearColor(0x000000, 1.0);
    //  renderer.setSize(window.innerWidth, window.innerHeight);  // to do whole screen
    renderer.setSize($("#3dContent").width() , $("#3dContent").height());
    renderer.shadowMapEnabled = true;

    // create the ground plane
    var planeMaterial = new THREE.MeshLambertMaterial({ color: 0x111111 });

    var axisGeometry = new THREE.PlaneGeometry(100, 100);
   
    var xaxis = new THREE.Mesh(axisGeometry, planeMaterial);
    xaxis.receiveShadow = true;

    // rotate and position the plane
    xaxis.rotation.x = -0.5 * Math.PI;
    xaxis.position.x = 0;
    xaxis.position.y = -2;
    xaxis.position.z = 0;

  //  // add the plane to the scene
    scene.add(xaxis);

  //  plane.rotation.x = +0.5 * Math.PI;
  //  scene.add(plane);

    // position and point the camera to the center of the scene
    camera.position.x = 150;
    camera.position.y = 160;
    camera.position.z = 130;
    camera.lookAt(scene.position);

    // add spotlight for the shadows
    var spotLight = new THREE.SpotLight(0xffffff);
    spotLight.position.set(100, 200, 200);
    spotLight.shadowCameraNear = 20;
    spotLight.shadowCameraFar = 50;
    spotLight.castShadow = true;

    scene.add(spotLight);

    // add spotlight for the shadows
    spotLight = new THREE.SpotLight(0xffffff);
    spotLight.position.set(-100, 200, 200);
    spotLight.shadowCameraNear = 20;
    spotLight.shadowCameraFar = 50;
    spotLight.castShadow = true;

    scene.add(spotLight);

    // add ambient light 
    var ambient = new THREE.AmbientLight( 0x404040 ); // soft white light
    scene.add( ambient );

    // add the output of the renderer to the html element
    document.getElementById("3dContent").appendChild(renderer.domElement);

    // call the render function, after the first render, interval is determined
    // by requestAnimationFrame
    render();
}

var Rho = 150;
var Phi = 45;
var Theta = 30;

function degInRad(deg) {
    return deg * Math.PI / 180;
}

function render() {

    /*
     * Compute the camera position from the spherical coordinates. Y is up, X and Z are the plane of the horizon
     */

    camera.position.x = Rho * Math.sin(degInRad(Phi)) * Math.cos(degInRad(Theta));
    camera.position.z = Rho * Math.sin(degInRad(Phi)) * Math.sin(degInRad(Theta));
    camera.position.y = Rho * Math.cos(degInRad(Phi));

    camera.lookAt(scene.position);
    renderer.render(scene, camera);

}


function AddJSONShape(oShape) {

    var geometry = new THREE.Geometry();

    for (i = 0; i < oShape.VertexList.length; i++) {
        geometry.vertices.push(new THREE.Vector3(oShape.VertexList[i][0] , oShape.VertexList[i][1] , oShape.VertexList[i][2] ));
    }

    var faceIndex = 0;
    for (i = 0; i < oShape.TriangleIndexList.length; i++,faceIndex++) {

        geometry.faces.push(new THREE.Face3(oShape.TriangleIndexList[i][0], oShape.TriangleIndexList[i][1], oShape.TriangleIndexList[i][2]));

        var face = geometry.faces[faceIndex];
        /*
         * Add the normal
         */
        var ndxA = face.a;
        var normal = new THREE.Vector3();
        normal.x = oShape.NormalList[ndxA][0];
        normal.y = oShape.NormalList[ndxA][1];
        normal.z = oShape.NormalList[ndxA][2];
        face.vertexNormals[0] = normal;

        var ndxB = face.b;
        normal = new THREE.Vector3();
        normal.x = oShape.NormalList[ndxB][0];
        normal.y = oShape.NormalList[ndxB][1];
        normal.z = oShape.NormalList[ndxB][2];
        face.vertexNormals[1] = normal;

        var ndxC = face.c;
        normal = new THREE.Vector3();
        normal.x = oShape.NormalList[ndxC][0];
        normal.y = oShape.NormalList[ndxC][1];
        normal.z = oShape.NormalList[ndxC][2];
        face.vertexNormals[2] = normal;

    }
          
    /*
     * we want the texture vertices for the vertices in each face
     */
    for (i = 0; i < oShape.TriangleIndexList.length; i++) {
        /*
         * Get the index values of each vertex in this face
         */
        var v1 = oShape.TriangleIndexList[i][0];
        var v2 = oShape.TriangleIndexList[i][1];
        var v3 = oShape.TriangleIndexList[i][2];
        /*
         * Now get the 2-d texture values for each vertex
         */
        var uva = oShape.TextureList[v1];
        var uvb = oShape.TextureList[v2];
        var uvc = oShape.TextureList[v3];
        /*
         * Now create 2d vectors of those texture values
         */
        var vuva = new THREE.Vector2(uva[0], uva[1]);
        var vuvb = new THREE.Vector2(uvb[0], uvb[1]);
        var vuvc = new THREE.Vector2(uvc[0], uvc[1]);
        /*
         * Add the texttures to the geometries
         */
        geometry.faceVertexUvs[0].push(vuva, vuvb, vuvc);

    }

    geometry.computeFaceNormals();

    material = new THREE.MeshLambertMaterial({ color: 0xcccc00 });
    /*
     * This call creates the final object with material
     */
    var sceneObject = new THREE.Mesh(geometry, material);
    /*
     * These routines are helpers in 3JS to let you see the normals of vertices and faces. They can be rendered as graphics for debugging
     */
    // Get the normals as a list of lines
    //normals = new THREE.VertexNormalsHelper(sceneObject, 2, 0xff0000, 1);
    facenormals = new THREE.FaceNormalsHelper(sceneObject,6,0xff0000,1);

    // Another helper, get a wireframe of the object
     wireframe = new THREE.WireframeHelper(sceneObject, 0x00ff00);

    if (false) {
        // This is an example of rendering the wireframes and the face normals
        scene.add(wireframe);
        scene.add(facenormals);
    } else {
        scene.add(sceneObject);
    }

    render();

    if (false) {
        return wireframe;
    } else {
        return sceneObject;
    }

}


/**
 * Function handles the resize event. This make sure the camera and the renderer
 * are updated at the correct moment.
 */
function handleResize() {

    // camera.aspect = window.innerWidth / window.innerHeight;  // to do whole window
    camera.aspect = $("#3dContent").width() / $("#3dContent").height();

    camera.updateProjectionMatrix();
    // renderer.setSize(window.innerWidth, window.innerHeight); // to do whole window
    renderer.setSize($("#3dContent").width(), $("#3dContent").height());

}

// calls the init function when the window is done loading.
// window.onload = init;
// calls the handleResize function when the window is resized
window.addEventListener('resize', handleResize, false);

