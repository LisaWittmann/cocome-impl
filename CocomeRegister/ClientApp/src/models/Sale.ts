import { Product } from './Product';

export enum PaymentMethod {
    CASH, CARD
}

export interface CreditCard {
    number: string;
    pin: number;
}

export interface SaleElement {
    product: Product;
    amount: number;
    discount: number;
}

export interface Sale {
    saleElements: SaleElement[];
    paymentMethod: PaymentMethod;
    timeStamp: Date;
    total: number;
    payed: number;
}
