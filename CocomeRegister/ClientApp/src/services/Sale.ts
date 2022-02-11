import { Product } from "./Product";
import { Store } from "./Store";

export interface SaleElement {
    product: Product;
    amount: number;
}
export interface Sale {
    id: number;
    store: Store;
    saleElements: SaleElement[];
    timeStamp: Date;
}
