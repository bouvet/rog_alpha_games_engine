import * as THREE from 'three';
import {Marker} from "./Figures/Marker.ts";
import {keys} from "./Events/InputHandler.ts";
import "./style.css";
import {dynamicObjects, LIGHT, SHADOWS} from "./SceneHandler.ts";

export const camera = new THREE.PerspectiveCamera(
    60,
    window.innerWidth / window.innerHeight,
    1,
    1000
);
camera.position.set(0, 0, 5);
camera.lookAt(new THREE.Vector3(0, 5, 0));

const renderer = new THREE.WebGLRenderer({
    antialias: true,
});
renderer.shadowMap.enabled = true;
renderer.shadowMap.type = THREE.PCFSoftShadowMap;

renderer.setSize(innerWidth, innerHeight);
renderer.setPixelRatio(2);
document.body.appendChild(renderer.domElement);


export const scene = new THREE.Scene();
export const plane = new THREE.Plane(new THREE.Vector3(0, 0, 1), 0);
export const raycaster = new THREE.Raycaster();
export const mouse = new THREE.Vector2();
export const intersectPoint = new THREE.Vector3();
export const marker = Marker(scene);

//AxesHelper(scene);
//Grid(scene);

export const pointLight = new THREE.PointLight('white', 0.5);
const worldLight = new THREE.HemisphereLight('white', "gray", SHADOWS ? 0.1 : 0.25);
worldLight.position.set(0, 0, 5);
scene.add(worldLight);

if(LIGHT){
    pointLight.position.set(0, 0, 2);
    pointLight.castShadow = true;
    scene.add(pointLight);
}


// Handle window resize
window.addEventListener('resize', () => {
    const {innerWidth, innerHeight} = window;

    renderer.setSize(innerWidth, innerHeight);
    camera.aspect = innerWidth / innerHeight;
    camera.updateProjectionMatrix();
});

export let direction = new THREE.Vector3();

// Update the direction vector based on the keys that are pressed
function updateDirection() {
    direction.set(0, 0, 0);
    if (keys.left) direction.x -= 1;
    if (keys.right) direction.x += 1;
    if (keys.up) direction.y += 1;
    if (keys.down) direction.y -= 1;

    // Normalize the direction vector
    if (direction.length() > 0) {
        direction.normalize();
    }
}

export function render() {
    updateDirection();
    requestAnimationFrame(render);
    renderer.render(scene, camera);

    dynamicObjects.forEach((object) => {
       if(object.userData.update){
          object.userData.update(object);
       }
    });
}