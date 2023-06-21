import {Transform} from "class-transformer";
import {Matrix} from "./Math/Matrix.ts";

export interface IGameObject {
    WorldMatrix: Matrix;
    LocalMatrix: Matrix;
    Parent: GameObject;
    Children: GameObject[];
    Id: number;
    Type: string;
}

export class GameObject implements IGameObject{
    [key: string]: any;

    Children!: GameObject[];
    Id!: number;
    Parent!: GameObject;
    Type!: string;

    // @ts-ignore
    @Transform(({ value }) => new Matrix(value), { toClassOnly: true })
    WorldMatrix!: Matrix;

    // @ts-ignore
    @Transform(({ value }) => new Matrix(value), { toClassOnly: true })
    LocalMatrix!: Matrix;
}