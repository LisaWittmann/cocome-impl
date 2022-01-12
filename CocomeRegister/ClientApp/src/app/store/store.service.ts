import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { Order } from "src/services/Order";
import { Product } from "src/services/Product";
import { StateService } from "src/services/StateService";

interface StoreState {
    inventory: Map<Product, number>,
    currentOrder: Product[],
    orders: Order[],
}

const initialState: StoreState = {
    inventory: new Map<Product, number>(),
    currentOrder: [],
    orders: [],
}

@Injectable({providedIn: 'root'})
export class StoreStateService extends StateService<StoreState> {
    inventory$: Observable<Map<Product, number>> = this.select(state => state.inventory);
    currentOrder$: Observable<Product[]> = this.select(state => state.currentOrder);
    orders$: Observable<Order[]> = this.select(state => state.orders);

    constructor() {
        super(initialState);
        // test data
        const inventory = new Map<Product, number>();
        for (const product of testProducts) {
            inventory.set(product, Math.round(Math.random() * 100));
        }
        this.setState({ inventory: inventory });
    }

    get closedOrders() {
        return this.state.orders.filter(order => order.closed);
    }

    get pendingOrders() {
        return this.state.orders.filter(order => order.delivered && !order.closed);
    }

    get openOrders() {
        return this.state.orders.filter(order => !order.delivered);
    }

    get availableProducts() {
        const availableProducts = [];
        for (const [key, value] of this.state.inventory) {
            if (value > 0) {
                console.log("available", key.name);
                availableProducts.push(key);
            }
        }
        console.log(availableProducts)
        return availableProducts;
    }

    closeOrder(orderId: number) {
        const orders = this.state.orders;
        for (const order of orders) {
            if (order.id == orderId) order.closed = true;
        }
        const products = this.state.inventory;
        for (const key of products.keys()) {
            if (this.containsProduct(orderId, key.id)) {
                const amount = products.get(key) + this.getAmount(orderId, key.id);
                products.set(key, amount);
            }
        }
        this.setState({ orders: orders, inventory: products })
    }

    private getAmount(productId: number, orderId: number) {
        const order = this.state.orders.find(o => o.id == orderId);
        return order.products.filter(product => product.id == productId).length;
    }

    private containsProduct(orderId: number, productId: number) {
        const order = this.state.orders.find(o => o.id == orderId);
        return order.products.some(product => product.id == productId);
    }

    changePrice(productId: number, price: number) {
        const products = this.state.inventory;
        for (const key of products.keys()) {
            if (key.id == productId) key.price = price;
        }
        this.setState({ inventory: products });
    }

    removeProducts(products: Product[]) {
        const storeProducts = this.state.inventory;
        for (const key of storeProducts.keys()) {
            const productsToKey = products.filter(product => product.id == key.id); 
            storeProducts.set(key, storeProducts.get(key) - productsToKey.length);
        }
        this.setState({ inventory: storeProducts });
        console.log(this.state.inventory);
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