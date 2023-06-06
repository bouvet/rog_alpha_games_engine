import * as THREE from "three";
import {camera, scene} from "./Rendering.ts";
import {DynamicTypeHandlers, StaticTypeHandlers} from "./ObjectTypeHandler.ts";

let dynamicObjects: THREE.Mesh[] = [];
let staticObjects: THREE.Mesh[] = [];

export let playerId = -1;

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
    objects.forEach((staticObject: any) => {
        let obj = null;

        if(StaticTypeHandlers[staticObject.Type.toLowerCase()] !== undefined) {
            obj = StaticTypeHandlers[staticObject.Type.toLowerCase()](staticObject);
        }

        if(obj){
            SetMatrix(obj, staticObject);
            obj.userData.id = staticObject.Id;

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

export function AddDynamicObjects(objects: any[]) {
    objects.forEach(dynamicObject => {
        let obj = null;

        if(DynamicTypeHandlers[dynamicObject.Type.toLowerCase()] !== undefined) {
            obj = DynamicTypeHandlers[dynamicObject.Type.toLowerCase()](dynamicObject);
        }

        if(obj){
            SetMatrix(obj, dynamicObject);
            obj.userData.id = dynamicObject.Id;


            if (dynamicObject.Id === playerId) {
                camera.position.set(obj.position.x, obj.position.y, 5);
            }

            scene.add(obj);
            dynamicObjects.push(obj);
        }
    })
}

function SetMatrix(obj: THREE.Mesh, gameObject: any){
    obj.position.x = gameObject.WorldMatrix._matrix.M41;
    obj.position.y = gameObject.WorldMatrix._matrix.M42;
    obj.position.z = gameObject.WorldMatrix._matrix.M43;

    obj.rotation.x = gameObject.WorldMatrix._matrix.M11;
    obj.rotation.y = gameObject.WorldMatrix._matrix.M12;
    obj.rotation.z = gameObject.WorldMatrix._matrix.M13;
}