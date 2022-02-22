import { Product } from "./Product";
import { Provider } from "./Provider";
import { Store } from "./Store";
export interface OrderElement {
    product: Product;
    amount: number;
}

export interface Order {
    id: number;
    elements: OrderElement[];
    store: Store;
    provider: Provider;
    placingDate: Date;
    deliveringDate: Date;
    closed: boolean;
}
