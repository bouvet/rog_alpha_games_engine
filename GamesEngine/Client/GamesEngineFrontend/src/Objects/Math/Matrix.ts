import {IVector, Vector} from "./Vector.ts";

export interface IMatrix {
    GetPosition(): IVector;
    GetRotation(): IVector;
    GetScale(): IVector;
}

type matrixElements =
    {
        M11: number, M12: number, M13: number, M14: number,
        M21: number, M22: number, M23: number, M24: number,
        M31: number, M32: number, M33: number, M34: number,
        M41: number, M42: number, M43: number, M44: number};

export class Matrix implements IMatrix{
    matrix: matrixElements

    constructor(elements: matrixElements) {
        this.matrix = elements;
    }

    GetPosition(): IVector {
        return new Vector(this.matrix.M41, this.matrix.M42, this.matrix.M43);
    }

    GetRotation(): IVector {
        return new Vector(this.matrix.M11, this.matrix.M12, this.matrix.M13);
    }

    GetScale(): IVector {
        return new Vector(this.matrix.M21, this.matrix.M22, this.matrix.M23);
    }
}