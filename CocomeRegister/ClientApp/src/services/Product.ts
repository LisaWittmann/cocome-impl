import { Store } from "./Store";

export interface Product {
    id: number;
    name: string;
    price: number;
    salePrice: number;
    description: string | undefined;
    imageUrl: string | undefined;
}

export interface StockItem {
    id: number;
    product: Product;
    stock: number;
    store: Store;
    minimum: number | undefined;
}
