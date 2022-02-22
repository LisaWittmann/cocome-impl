import { Product } from "./Product";

export interface Store {
    id: number;
    name: string;
    city: string | undefined;
    postalCode: number | undefined;
}

export interface StockItem {
    store: Store;
    product: Product;
    stock: number;
}