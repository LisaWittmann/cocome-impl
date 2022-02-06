import { Product } from './Product';

export interface Order {
    id: number;
    products: Map<Product, number>;
    placingDate: Date;
    deliveringDate: Date | undefined;
    delivered: boolean;
    closed: boolean;
    provider: Provider;
}

export interface Provider {
    id: number;
    name: string;
}
