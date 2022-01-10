import { Product } from "./Product";

export interface Order {
    id: number,
    products: Product[],
    placingDate: Date,
    deliveringDate: Date,
    delivered: boolean,
    closed: boolean,
}