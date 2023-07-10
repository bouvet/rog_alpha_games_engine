import * as THREE from "three";
import {camera, pointLight, scene} from "./Rendering.ts";
import {DynamicTypeHandlers, StaticTypeHandlers} from "./ObjectTypeHandler.ts";
import {MeshBasicMaterial} from "three";
import {plainToInstance} from "class-transformer";
import {GameObject} from "./Objects/GameObjects/GameObject.ts";
import {StaticGameObject} from "./Objects/GameObjects/StaticGameObject.ts";
import {DynamicGameObject} from "./Objects/GameObjects/DynamicGameObject.ts";

export let dynamicObjects: THREE.Object3D[] = [];
export let staticObjects: THREE.Object3D[] = [];

export let SHOW_BOUNDS = false;
export let SHADOWS = false;
export let LIGHT = true;

export let playerId = -1;
let cameraPosition = new THREE.Vector3();

setInterval(() => {
    if(camera.position != cameraPosition){
        camera.position.lerp(cameraPosition, 0.25);
    }
}, 50);


export function SetPlayerId(id: number){
    playerId = id;
}

export function RemoveStaticObjects(){
    staticObjects.forEach(obj => {
        scene.remove(obj);
    });
    staticObjects = [];
}

export function AddStaticObjects(objects: any[]){
    let gameObjects = plainToInstance(GameObject, objects);
    gameObjects.forEach((staticObject: any) => {
        let obj = null;

        if(StaticTypeHandlers[staticObject.Type.toLowerCase()] !== undefined) {
            obj = StaticTypeHandlers[staticObject.Type.toLowerCase()](plainToInstance(StaticGameObject, staticObject));
        }

        if(obj){
            SetMatrix(obj, staticObject);
            obj.userData.id = staticObject.Id;

            if(SHADOWS){
                obj.receiveShadow = true;
            }

            if(SHOW_BOUNDS) {
                RenderBounds(obj, true);
            }

            scene.add(obj);
            staticObjects.push(obj);
        }
    });
}

export function RemoveDynamicObjects() {
    dynamicObjects.forEach(obj => {
        scene.remove(obj);
    });
    dynamicObjects = [];
}

let lastPositions = new Map<number, THREE.Vector3>();

function RenderBounds(obj : any, staticObj: boolean) {
    let rotation = new THREE.Euler(
        obj.rotation.x,
        obj.rotation.y,
        obj.rotation.z,
        'XYZ' // This is the order of rotations
    );

    let dimensions = new THREE.Vector3(obj.scale.x, obj.scale.y, obj.scale.z);
    let pivot = new THREE.Vector3(dimensions.x / 2, obj.rotation.y != 0.0 ? 0 : dimensions.y / 2, obj.rotation.x == 0.0 ? 0 : dimensions.z / 2);

    let corners = [
        new THREE.Vector3(0, 0, 0).sub(pivot),
        new THREE.Vector3(dimensions.x, 0, 0).sub(pivot),
        new THREE.Vector3(0, dimensions.y, 0).sub(pivot),
        new THREE.Vector3(0, 0, dimensions.z).sub(pivot),
        new THREE.Vector3(dimensions.x, dimensions.y, 0).sub(pivot),
        new THREE.Vector3(dimensions.x, 0, dimensions.z).sub(pivot),
        new THREE.Vector3(0, dimensions.y, dimensions.z).sub(pivot),
        new THREE.Vector3(dimensions.x, dimensions.y, dimensions.z).sub(pivot)
    ];

    let start = new THREE.Vector3(obj.position.x, obj.position.y, obj.position.z);
    let matrix = new THREE.Matrix4().makeRotationFromEuler(rotation);
    let endRelative = dimensions.applyMatrix4(matrix);
    let end = start.clone().add(endRelative);
    let worldCorners = corners.map(corner => corner.applyMatrix4(matrix).add(start));
    let min = worldCorners[0].clone();
    let max = worldCorners[0].clone();

    // Find the minimum and maximum x, y, and z values
    worldCorners.forEach(corner => {
        min.min(corner);
        max.max(corner);
    });

    start = min;
    end = max;

    dimensions = end.clone().sub(start.clone());

    /*
    let point = (pos: THREE.Vector3, color: number) => {
        let startPoint = new THREE.SphereGeometry(0.1, 32, 32);
        let startPointMesh = new THREE.Mesh(startPoint, new MeshBasicMaterial({color: color, depthTest: false}));
        startPointMesh.position.set(pos.x, pos.y, pos.z);
        scene.add(startPointMesh);
        dynamicObjects.push(startPointMesh);
    }

    point(start, 0x00ff00);
    point(end, 0xff0000);
    point(new THREE.Vector3(start.x + dimensions.x / 2, start.y + dimensions.y / 2, start.z + dimensions.z / 2), 0x0000ff);
     */

    let geometry = new THREE.BoxGeometry(dimensions.x, dimensions.y, dimensions.z);
    let boundingBox = new THREE.Mesh(geometry, new MeshBasicMaterial({color: 0x00ff00, wireframe: true}));
    boundingBox.position.addVectors(min, max).multiplyScalar(0.5);
    scene.add(boundingBox);

    if(staticObj){
        staticObjects.push(boundingBox);
    }else{
        dynamicObjects.push(boundingBox);
    }
}

export function AddDynamicObjects(objects: any[]) {
    let gameObjects = plainToInstance(GameObject, objects);
    gameObjects.forEach(dynamicObject => {
        let obj = null;

        if(DynamicTypeHandlers[dynamicObject.Type.toLowerCase()] !== undefined) {
            obj = DynamicTypeHandlers[dynamicObject.Type.toLowerCase()](plainToInstance(DynamicGameObject, dynamicObject));
        }

        if(obj) {
            SetMatrix(obj, dynamicObject);

            if(lastPositions.get(dynamicObject.Id) !== undefined){
                let lastPos = lastPositions.get(dynamicObject.Id);
                if(lastPos != null){
                    let newPos = new THREE.Vector3(obj.position.x, obj.position.y, obj.position.z);
                    obj.position.set(lastPos.x, lastPos.y, lastPos.z);

                    obj.userData.update = (obj: THREE.Mesh) => {
                        obj.position.lerp(obj.userData.newPos, 0.1);
                        lastPositions.set(obj.userData.id, new THREE.Vector3(obj.position.x, obj.position.y, obj.position.z));
                    };
                    obj.userData.newPos = newPos;
                }
            }

            obj.userData.id = dynamicObject.Id;

            if (dynamicObject.Id === playerId) {
                cameraPosition = new THREE.Vector3(obj.position.x, obj.position.y - 5, 5);

                if(LIGHT)
                    pointLight.position.set(obj.position.x, obj.position.y, obj.position.z + 1);
            }


            if(SHOW_BOUNDS) {
                RenderBounds(obj, false);
            }

            if(SHADOWS){
                obj.castShadow = true;
                obj.receiveShadow = true;
            }

            scene.add(obj);
            dynamicObjects.push(obj);
        }
    })

    lastPositions.clear();
    dynamicObjects.forEach(obj => {
        lastPositions.set(obj.userData.id, new THREE.Vector3(obj.position.x, obj.position.y, obj.position.z));
    });
}

function SetMatrix(obj: THREE.Object3D, gameObject: GameObject){
    console.log(gameObject.WorldMatrix.GetPosition())

    obj.position.x = gameObject.WorldMatrix.GetPosition().GetX();
    obj.position.y = gameObject.WorldMatrix.GetPosition().GetY();
    obj.position.z = gameObject.WorldMatrix.GetPosition().GetZ();

    obj.rotation.x = THREE.MathUtils.degToRad(gameObject.WorldMatrix.GetRotation().GetX());
    obj.rotation.y = THREE.MathUtils.degToRad(gameObject.WorldMatrix.GetRotation().GetY());
    obj.rotation.z = THREE.MathUtils.degToRad(gameObject.WorldMatrix.GetRotation().GetZ());

    obj.scale.x = gameObject.WorldMatrix.GetScale().GetX();
    obj.scale.y = gameObject.WorldMatrix.GetScale().GetY();
    obj.scale.z = gameObject.WorldMatrix.GetScale().GetZ();
}