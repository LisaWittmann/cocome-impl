import { Product } from "./Product";
import { Store } from "./Store";

export interface Trade<T> {
    id: number;
    elements: TradeElement[];
    store: Store;
    provider: T;
    placingDate: Date;
    deliveringDate: Date;
    closed: boolean;
}

export interface TradeElement {
    product: Product;
    amount: number;
}