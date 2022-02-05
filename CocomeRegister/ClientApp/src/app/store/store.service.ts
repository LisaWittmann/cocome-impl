import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { Order } from "src/services/Order";
import { Product } from "src/services/Product";
import { StateService } from "src/services/StateService";

interface StoreState {
    inventory: Map<Product, number>,
    currentOrder: Map<Product, number>,
    orders: Order[],
}

const initialState: StoreState = {
    inventory: new Map<Product, number>(),
    currentOrder: new Map<Product, number>(),
    orders: [],
}

@Injectable({providedIn: 'root'})
export class StoreStateService extends StateService<StoreState> {
    inventory$: Observable<Map<Product, number>> = this.select(state => state.inventory);
    currentOrder$: Observable<Map<Product, number>> = this.select(state => state.currentOrder);
    orders$: Observable<Order[]> = this.select(state => state.orders);

    constructor() {
        super(initialState);
        // test data
        const inventory = new Map<Product, number>();
        for (const product of testProducts) {
            inventory.set(product, Math.round(Math.random() * 100));
        }
        const orders = testOrders;
        this.setState({ inventory: inventory, orders: orders });
        console.log(orders)
    }

    get availableProducts() {
        return [...this.state.inventory.keys()].filter(product => this.state.inventory.get(product) > 0);
    }

    get runningOutOfStock() {
        return [...this.state.inventory.keys()].filter(product => this.state.inventory.get(product) < 10);
    }

    addToCard(product: Product, amount = 1) {
        const currentOrder = this.state.currentOrder;
        const cardProduct = [...currentOrder.keys()].find(p => p.id == product.id);
        if (cardProduct) {
            currentOrder.set(cardProduct, currentOrder.get(cardProduct) + amount);
        } else {
            currentOrder.set(product, amount);
        }
        this.setState({ currentOrder: currentOrder });
    }

    removeFromCard(product: Product) {
        const currentOrder = this.state.currentOrder;
        const cardProduct = [...currentOrder.keys()].find(p => p.id == product.id);
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
        if (this.state.currentOrder.size == 0) return;
        const orders = this.state.orders;
        orders.push({
            id: Math.floor(Math.random() * 5000),
            products: this.state.currentOrder,
            placingDate: new Date(Date.now()),
            deliveringDate: undefined,
            delivered: false,
            closed: false,
        })
        this.setState({ currentOrder: new Map<Product, number>() });
        console.log(this.state.currentOrder)
        this.setState({ orders: orders });
    }

    closeOrder(orderId: number) {
        const orders = this.state.orders;
        const inventory = this.state.inventory;

        const order = orders.find(order => order.id == orderId);
        order.closed = true;
        console.log(order);
        
        for (const [product, amount] of order.products) {
            inventory.set(product, inventory.get(product) + amount);
        }
        console.log(inventory);
        this.setState({ orders: orders });
        this.setState({ inventory: inventory })
    }

    removeProducts(products: Product[]) {
        const storeProducts = this.state.inventory;
        for (const key of storeProducts.keys()) {
            const productsToKey = products.filter(product => product.id == key.id); 
            storeProducts.set(key, storeProducts.get(key) - productsToKey.length);
        }
        this.setState({ inventory: storeProducts });
    }
}

const testProducts = [
    {
        id: 12345,
        name: "Salatgurke",
        price: 0.59,
        description: ""
    },
    {
        id: 35656,
        name: "Endiviensalat",
        price: 0.99,
        description: ""
    },
    {
        id: 7263,
        name: "Kräuterbaguette",
        price: 0.99,
        description: ""
    },
    {
        id: 8843,
        name: "Schokoriegel",
        price: 1.99,
        description: ""
    },
    {
        id: 8443,
        name: "Bircher Müsli",
        price: 1.99,
        description: ""
    },
    {
        id: 91233,
        name: "Papaya",
        price: 1.19,
        description: ""
    },
    {
        id: 75236,
        name: "Capri Sonne Orange",
        price: 0.79,
        description: ""
    },
    {
        id: 75231,
        name: "Kartoffeln",
        price: 1.39,
        description: ""
    },
    {
        id: 23484,
        name: "Kirschtomaten 500g",
        price: 1.69,
        description: ""
    },
    {
        id: 23484,
        name: "Eistee Pfirsich",
        price: 0.69,
        description: ""
    },
    {
        id: 23484,
        name: "Eistee Zitrone",
        price: 0.69,
        description: ""
    },
    {
        id: 26782,
        name: "Erdbeeren",
        price: 3.69,
        description: ""
    },
    {
        id: 82921,
        name: "Laugenbrezel",
        price: 0.49,
        description: ""
    },
    {
        id: 72361,
        name: "Katzenstreu",
        price: 2.79,
        description: ""
    },
    {
        id: 32170,
        name: "Schlagsahne",
        price: 0.29,
        description: ""
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