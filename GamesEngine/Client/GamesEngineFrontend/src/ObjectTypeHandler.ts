import * as THREE from 'three';
import {StaticGameObject} from "./Objects/GameObjects/StaticGameObject.ts";
import {DynamicGameObject} from "./Objects/GameObjects/DynamicGameObject.ts";

type StaticHandler = {
    [key:string]: (param: StaticGameObject) => THREE.Object3D;
}

type DynamicHandler = {
    [key:string]: (param: DynamicGameObject) => THREE.Object3D;
}

export let StaticTypeHandlers: StaticHandler = {};
export let DynamicTypeHandlers: DynamicHandler = {};

export function AddTypeHandlers(){

    //Dynamic Objects
    // @ts-ignore
    DynamicTypeHandlers["player"] = (character: DynamicGameObject) => {
        const coneGeom = new THREE.ConeGeometry(0.5, 1, 10);
        coneGeom.translate(0, 0, -0.5);
        coneGeom.rotateX(Math.PI / 2);
        const coneMat = new THREE.MeshNormalMaterial();
        const cone = new THREE.Mesh(coneGeom, coneMat);
        cone.lookAt(new THREE.Vector3(0, 1, 0));
        return cone;
    }

    // @ts-ignore
    DynamicTypeHandlers["obstacle"] = (staticObject: DynamicGameObject) => {
        const objectGeom = new THREE.BoxGeometry(1, 1, 1);
        const objectMat = new THREE.MeshPhongMaterial({color: staticObject.Colliding ? 0xff0000 : 0xffffff});

        objectGeom.translate(0,0, 0.5); // pivot point is shifted
        return new THREE.Mesh(objectGeom, objectMat);
    }


    StaticTypeHandlers["orb"] = (staticObject: StaticGameObject) => {
        const objectGeom = new THREE.SphereGeometry(0.5, 10, 10);
        const objectMat = new THREE.MeshPhongMaterial({color: `rgba(${staticObject.MapMaterial.Color.R}, ${staticObject.MapMaterial.Color.G}, ${staticObject.MapMaterial.Color.B}, ${staticObject.MapMaterial.Color.A})`});

        objectGeom.translate(0,0, 0.5); // pivot point is shifted
        return new THREE.Mesh(objectGeom, objectMat);
    }

    //Static Objects
    // @ts-ignore
    StaticTypeHandlers["wall"] = (staticObject: StaticGameObject) => {
        const objectGeom = new THREE.BoxGeometry(1, 1, 1);
        const objectMat = new THREE.MeshPhongMaterial({color: 0x888888});

        objectGeom.translate(0,0, 0.5); // pivot point is shifted
        return new THREE.Mesh(objectGeom, objectMat);
    }

    // @ts-ignore
    StaticTypeHandlers["floor"] = (staticObject: StaticGameObject) => {
        const objectGeom = new THREE.BoxGeometry(1, 1, 1);
        const objectMat = new THREE.MeshPhongMaterial({color: staticObject.Id % 3 == 0 ? 0x888888 : 0x828282});
        objectGeom.translate(0,0, 0.5); // pivot point is shifted
        return new THREE.Mesh(objectGeom, objectMat);
    }
}