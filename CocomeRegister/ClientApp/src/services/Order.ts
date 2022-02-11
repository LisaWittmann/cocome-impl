import { Store } from './Store';
import { Product } from './Product';

export interface Order {
    id: number;
    product: Product,
    amount: number;
    store: Store;
    provider: Provider;
    placingDate: Date;
    deliveringDate: Date | undefined;
    delivered: boolean;
    closed: boolean;
}
export interface Provider {
    id: number;
    name: string;
}
