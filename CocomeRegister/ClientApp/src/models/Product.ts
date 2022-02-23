import { Provider } from './Provider';

export interface Product {
    id: number;
    name: string;
    price: number;
    salePrice: number;
    description: string | undefined;
    imageUrl: string | undefined;
    provider: Provider;
}
