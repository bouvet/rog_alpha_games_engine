import * as THREE from "three";

export interface IVector {
    GetX(): number;
    GetY(): number;
    GetZ(): number;
}

export class Vector implements IVector{
    vector: THREE.Vector3;

    constructor(x: number, y: number, z: number){
        this.vector = new THREE.Vector3(x, y, z);
    }

    GetX(): number {
        return this.vector.x;
    }

    GetY(): number {
        return this.vector.y;
    }

    GetZ(): number {
        return this.vector.z;
    }
}