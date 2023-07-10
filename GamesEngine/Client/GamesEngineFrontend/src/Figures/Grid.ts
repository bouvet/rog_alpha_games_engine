import * as THREE from 'three';


export function Grid(scene: THREE.Scene) {
    var grid = new THREE.GridHelper(20, 12, "white", "white");
    grid.rotation.x = Math.PI / 2;
    grid.position.z = 0.01;
    scene.add(grid);
}