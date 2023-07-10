import {IVector, Vector} from "../Math/Vector.ts";
import {Transform} from "class-transformer";
import {GameObject, IGameObject} from "./GameObject.ts";

export interface IDynamicGameObject extends IGameObject{
    Motion: IVector;
}

export class DynamicGameObject extends GameObject implements IDynamicGameObject {
    // @ts-ignore
    @Transform(({value}: {value: {X: number, Y: number, Z: number}}) => new Vector(value.X, value.Y, value.Z), {toClassOnly: true})
    Motion!: Vector;
}