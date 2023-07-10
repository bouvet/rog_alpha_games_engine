import {GameObject, IGameObject} from "./GameObject.ts";

export interface IStaticGameObject extends IGameObject{
}

export class StaticGameObject extends GameObject implements IStaticGameObject{
}