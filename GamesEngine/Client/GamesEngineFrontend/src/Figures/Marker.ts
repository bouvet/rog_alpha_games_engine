import * as THREE from 'three';


export function Marker(scene: THREE.Scene) {
    const markerGeometry = new THREE.SphereGeometry(0.1, 4, 4)
    const markerMaterial = new THREE.MeshBasicMaterial({
        color: "red"
    });
    const marker = new THREE.Mesh(markerGeometry, markerMaterial);
    scene.add(marker);
    return marker;
}
