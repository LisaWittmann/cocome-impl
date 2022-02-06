import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { interpolateColors, toRGBA } from 'src/services/ColorGenerator';
import { Month } from 'src/services/Month';
import { Order } from 'src/services/Order';
import { Product } from 'src/services/Product';
import { StateService } from 'src/services/StateService';

/** test data */
const testProducts = [
    {
        id: 12345,
        name: 'Salatgurke',
        price: 0.20,
        salePrice: 0.59,
        description: '',
        imageUrl: '',
    },
    {
        id: 35656,
        name: 'Endiviensalat',
        price: 0.20,
        salePrice: 0.99,
        description: '',
        imageUrl: '',
    },
    {
        id: 7263,
        name: 'Kräuterbaguette',
        price: 0.20,
        salePrice: 0.99,
        description: '',
        imageUrl: '',
    },
    {
        id: 8843,
        name: 'Schokoriegel',
        price: 0.20,
        salePrice: 1.99,
        description: '',
        imageUrl: '',
    },
    {
        id: 8443,
        name: 'Bircher Müsli',
        price: 0.20,
        salePrice: 1.99,
        description: '',
        imageUrl: '',
    },
    {
        id: 91233,
        name: 'Papaya',
        price: 0.20,
        salePrice: 1.19,
        description: '',
        imageUrl: '',
    },
    {
        id: 75236,
        name: 'Capri Sonne Orange',
        price: 0.20,
        salePrice: 0.79,
        description: '',
        imageUrl: '',
    },
    {
        id: 75231,
        name: 'Kartoffeln',
        price: 0.20,
        salePrice: 1.39,
        description: '',
        imageUrl: '',
    },
    {
        id: 23484,
        name: 'Kirschtomaten',
        price: 0.20,
        salePrice: 1.69,
        description: '',
        imageUrl: '',
    },
    {
        id: 23484,
        name: 'Eistee Pfirsich',
        price: 0.20,
        salePrice: 0.69,
        description: '',
        imageUrl: '',
    },
    {
        id: 23484,
        name: 'Eistee Zitrone',
        price: 0.20,
        salePrice: 0.69,
        description: '',
        imageUrl: '',
    },
    {
        id: 26782,
        name: 'Erdbeeren',
        price: 0.20,
        salePrice: 3.69,
        description: '',
        imageUrl: '',
    },
    {
        id: 82921,
        name: 'Laugenbrezel',
        price: 0.20,
        salePrice: 0.49,
        description: '',
        imageUrl: '',
    },
    {
        id: 72361,
        name: 'Katzenstreu',
        price: 0.20,
        salePrice: 2.79,
        description: '',
        imageUrl: '',
    },
    {
        id: 32170,
        name: 'Schlagsahne',
        price: 0.20,
        salePrice: 0.29,
        description: '',
        imageUrl: '',
    }
];

const testOrders = [
    {
        id: 139853,
        products: new Map([
            [testProducts[0], 100],
            [testProducts[1], 13],
            [testProducts[2], 200],
            [testProducts[3], 57],
            [testProducts[4], 40],
        ]),
        placingDate: new Date(2022, 1, 20),
        deliveringDate: undefined,
        delivered: false,
        closed: false,
    },
    {
        id: 12356,
        products: new Map([
            [testProducts[9], 100],
            [testProducts[6], 130],
            [testProducts[2], 300],
            [testProducts[5], 30],
            [testProducts[4], 55],
        ]),
        placingDate: new Date(2021, 6, 10),
        deliveringDate: new Date(2021, 7, 1),
        delivered: true,
        closed: true,
    },
    {
        id: 723645,
        products: new Map([
            [testProducts[9], 23],
            [testProducts[7], 111],
            [testProducts[2], 546],
            [testProducts[5], 30],
            [testProducts[3], 55],
        ]),
        placingDate: new Date(2022, 1, 10),
        deliveringDate: new Date(2022, 1, 14),
        delivered: true,
        closed: false,
    },
    {
        id: 523489,
        products: new Map([
            [testProducts[0], 100],
            [testProducts[1], 13],
            [testProducts[2], 200],
            [testProducts[3], 57],
            [testProducts[4], 40],
        ]),
        placingDate: new Date(2021, 11, 30),
        deliveringDate: new Date(2022, 1, 14),
        delivered: true,
        closed: false,
    }
];

const testSales = new Map<number, Map<Month, number>>([
    [
        2022,
        new Map<Month, number>([
            [Month.JANUARY, 210],
            [Month.FEBRUARY, 233],
            [Month.MARCH, null],
            [Month.APRIL, null],
            [Month.MAY, null],
            [Month.JUNE, null],
            [Month.JULY, null],
            [Month.AUGUST, null],
            [Month.SEPTEMBER, null],
            [Month.OCTOBER, null],
            [Month.NOVEMBER, null],
            [Month.DECEMBER, null]
        ])
    ],
    [
        2021,
        new Map<Month, number>([
            [Month.JANUARY, 116],
            [Month.FEBRUARY, 165],
            [Month.MARCH, 187],
            [Month.APRIL, 139],
            [Month.MAY, 199],
            [Month.JUNE, 165],
            [Month.JULY, 244],
            [Month.AUGUST, 155],
            [Month.SEPTEMBER, 160],
            [Month.OCTOBER, 143],
            [Month.NOVEMBER, 177],
            [Month.DECEMBER, 321]
        ])
    ],
]);

interface StoreState {
    storeId: number;
    inventory: Map<Product, number>;
    currentOrder: Map<Product, number>;
    orders: Order[];
    sales: Map<number, Map<Month, number>>;
}

const initialState: StoreState = {
    storeId: Math.round(Math.random() * 1000),
    inventory: new Map<Product, number>(),
    currentOrder: new Map<Product, number>(),
    orders: [],
    sales: new Map<number, Map<Month, number>>(),
};

@Injectable({providedIn: 'root'})
export class StoreStateService extends StateService<StoreState> {
    storeId$: Observable<number> = this.select(state => state.storeId);
    inventory$: Observable<Map<Product, number>> = this.select(state => state.inventory);
    currentOrder$: Observable<Map<Product, number>> = this.select(state => state.currentOrder);
    orders$: Observable<Order[]> = this.select(state => state.orders);
    sales$: Observable<Map<number, Map<Month, number>>> = this.select(state => state.sales);

    constructor() {
        super(initialState);
        // test data
        const inventory = new Map<Product, number>();
        for (const product of testProducts) {
            inventory.set(product, Math.round(Math.random() * 100));
        }
        this.setState({ inventory: inventory, orders: testOrders, sales: testSales });
    }

    get availableProducts() {
        return [...this.state.inventory.keys()].filter(product => this.state.inventory.get(product) > 0);
    }

    get runningOutOfStock() {
        return [...this.state.inventory.keys()].filter(product => this.state.inventory.get(product) < 10);
    }

    get salesDataset() {
        const colorRange = { colorStart: 0.6, colorEnd: 0.8 };
        const chartColors = interpolateColors(this.state.sales.size, colorRange);
        const dataset = [];
        for (const [year, sales] of this.state.sales) {
            dataset.push({
                label: year,
                data: [...sales.values()],
                borderColor: chartColors[dataset.length],
                backgroundColor: toRGBA(chartColors[dataset.length], 0.5)
            });
        }
        return dataset;
    }

    private getProduct(id: number) {
        return [...this.state.inventory.keys()].find(p => p.id === id);
    }

    private getOrder(id: number) {
        return this.state.orders.find(order => order.id === id);
    }

    addToCard(product: Product, amount = 1, replace = false) {
        const currentOrder = this.state.currentOrder;
        const cardProduct = [...currentOrder.keys()].find(p => p.id === product.id);
        if (cardProduct) {
            const productAmount = replace ? amount : currentOrder.get(cardProduct) + amount;
            currentOrder.set(cardProduct, productAmount);
        } else {
            currentOrder.set(product, amount);
        }
        this.setState({ currentOrder: currentOrder });
    }

    removeFromCard(product: Product) {
        const currentOrder = this.state.currentOrder;
        const cardProduct = this.getProduct(product.id);
        if (cardProduct && currentOrder.get(cardProduct) > 1) {
            currentOrder.set(cardProduct, currentOrder.get(cardProduct) - 1);
        }
        this.setState({ currentOrder: currentOrder });

    }

    removeAllFromCard(product: Product) {
        const currentOrder = this.state.currentOrder;
        currentOrder.delete(product);
        this.setState({ currentOrder: currentOrder });
    }

    placeNewOrder() {
        if (this.state.currentOrder.size === 0) {
            return;
        }
        const orders = this.state.orders;
        orders.push({
            id: Math.floor(Math.random() * 5000),
            products: this.state.currentOrder,
            placingDate: new Date(Date.now()),
            deliveringDate: undefined,
            delivered: false,
            closed: false,
        });
        this.setState({ currentOrder: new Map<Product, number>() });
        this.setState({ orders: orders });
    }

    closeOrder(orderId: number) {
        const orders = this.state.orders;
        const inventory = this.state.inventory;

        const foundOrder = this.getOrder(orderId);
        foundOrder.closed = true;
        for (const [product, amount] of foundOrder.products) {
            inventory.set(product, inventory.get(product) + amount);
        }
        this.setState({ orders: orders });
        this.setState({ inventory: inventory });
    }

    removeProducts(products: Product[]) {
        const storeProducts = this.state.inventory;
        for (const key of storeProducts.keys()) {
            const productsToKey = products.filter(product => product.id === key.id);
            storeProducts.set(key, storeProducts.get(key) - productsToKey.length);
        }
        this.setState({ inventory: storeProducts });
    }

    updateinventory() {
        this.setState({ inventory: this.state.inventory });
    }
}
