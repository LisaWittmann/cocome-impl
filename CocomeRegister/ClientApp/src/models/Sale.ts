import { Product } from './Product';

export enum PaymentMethod {
    CASH, CARD
}
export interface SaleElement {
    product: Product;
    amount: number;
    discount: number;
}

export interface Sale {
    saleElements: SaleElement[];
    paymentMethod: PaymentMethod;
    total: number;
    payed: number;
}
