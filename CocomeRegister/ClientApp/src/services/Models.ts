export interface Store {
    id: number;
    name: string;
    city: string | undefined;
    postalCode: number | undefined;
}

export interface Provider {
    id: number;
    name: string;
}

export interface Product {
    id: number;
    name: string;
    price: number;
    salePrice: number;
    description: string | undefined;
    imageUrl: string | undefined;
    provider: Provider,
}


export interface OrderElement {
    product: Product;
    amount: number;
}

export interface Order {
    id: number;
    orderElements: OrderElement[];
    store: Store;
    provider: Provider;
    placingDate: Date;
    deliveringDate: Date | undefined;
    closed: boolean;
}

export interface StockItem {
    id: number;
    product: Product;
    stock: number;
    store: Store;
    minimum: number | undefined;
}

export enum PaymentMethod {
    CASH, CARD
}
export interface SaleElement {
    product: Product;
    amount: number;
}

export interface Sale {
    saleElements: SaleElement[];
    paymentMethod: PaymentMethod;
    payed: number;
}

export interface Statistic {
    label: string;
    dataset: number[];
}

export interface PagedResponse<T> {
    pageNumber: number;
    pageSize: number;
    firstPage: string | undefined;
    lastPage: string | undefined;
    totalPages: number;
    totalRecords: number;
    nextPage: string | undefined;
    previousPage: string | undefined;
    data: T;
}