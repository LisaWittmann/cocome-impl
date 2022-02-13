import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { interpolateColors, toRGBA } from 'src/services/ColorGenerator';
import { Month } from 'src/services/Month';
import { StateService } from 'src/services/StateService';
import { Product, StockItem, Store, SaleElement, Order, OrderElement } from 'src/services/Models';

interface StoreState {
  store: Store;
  inventory: StockItem[];
  currentOrder: OrderElement[];
  orders: Order[];
  sales: Map<number, Map<Month, number>>;
}

const initialState: StoreState = {
  store: { name: "Test" } as Store,
  inventory: [],
  currentOrder: [],
  orders: [],
  sales: new Map<number, Map<Month, number>>(),
};
@Injectable({providedIn: 'root'})
export class StoreStateService extends StateService<StoreState> {
  store$: Observable<Store> = this.select(state => state.store);
  inventory$: Observable<StockItem[]> = this.select(state => state.inventory);
  currentOrder$: Observable<OrderElement[]> = this.select(state => state.currentOrder);
  orders$: Observable<Order[]> = this.select(state => state.orders);
  sales$: Observable<Map<number, Map<Month, number>>> = this.select(state => state.sales);
  api: string;

  constructor(
      private http: HttpClient,
      @Inject('BASE_URL') baseUrl: string
  ) {
      super(initialState);
      this.api = baseUrl + "api/store";
      this.getSession();
  }

  private fetchInventory() {
    this.http.get<StockItem[]>(`${this.api}/inventory/${this.state.store.id}`).subscribe(result => {
      console.log(result);
      this.setState({ inventory: result });
    }, error => console.error(error));
  }

  private fetchOrders() {
    this.http.get<Order[]>(`${this.api}/orders/${this.state.store.id}`).subscribe(result => {
      console.log(result);
      this.setState({ orders: result });
    }, error => console.error(error));
  }

  get availableProducts() {
    return this.state.inventory.filter(item => item.stock > 0).map(item => item.product);
  }

  get runningOutOfStock() {
    return this.state.inventory.filter(item => item.stock < 10);
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

  getSession() {
    const store: Store = JSON.parse(sessionStorage.getItem("store"));
    if (store) {
      this.setStore(store);
    }
  }

  setStore(store: Store) {
    sessionStorage.setItem("store", JSON.stringify(store));
    this.setState({ store: store });
    this.fetchInventory();
    this.fetchOrders();
    console.log(this.state);
  }

  addToCard(product: Product, amount = 1, replace = false) {
    console.log(product);
    const currentOrder = this.state.currentOrder;
    const cardProduct = currentOrder.find(element => element.product.id === product.id);
    if (cardProduct) {
      cardProduct.amount = replace ? amount : cardProduct.amount + amount;
    } else {
      currentOrder.push({ product: product, amount: amount });
    }
    this.setState({ currentOrder: currentOrder });
  }

  removeFromCard(product: Product) {
    const currentOrder = this.state.currentOrder;
    const cardProduct = this.state.currentOrder.find(element => element.product.id === product.id);
    if (cardProduct && cardProduct.amount > 1) {
      cardProduct.amount--;
    }
    this.setState({ currentOrder: currentOrder });
  }

  removeAllFromCard(product: Product) {
    this.setState({ currentOrder: this.state.currentOrder.filter(element => element.product.id != product.id) });
  }

  placeNewOrder() {
    this.http.post<Order[]>(
      `${this.api}/create-order/${this.state.store.id}`, 
      this.state.currentOrder
    ).subscribe(result => {
      this.setState({ orders: result });
      this.setState({ currentOrder: [] });
    }, error => console.error(error));
}

  closeOrder(orderId: number) {
    this.http.post<Order[]>(
      `${this.api}/close-order/${this.state.store.id}`,
      orderId
    ).subscribe(result => {
      this.setState({ orders: result });
      this.updateInventory();
    }, error => console.error(error));
  }

  updateInventory() {
    this.http.post<StockItem[]>(
      `${this.api}/update-inventory/${this.state.store.id}`,
      this.state.inventory
    ).subscribe(result => {
      this.setState({ inventory: result });
    }, error => console.error(error));
  }

  getProduct(id: number) {
    return this.http.get<Product>(`${this.api}/${this.state.store.id}/product/${id}`);
  }

  createProduct(product: Product) {
    this.http.post<StockItem[]>(
      `${this.api}/create-product/${this.state.store.id}`,
      product
    ).subscribe(result => {
       this.setState({ inventory: result });
    }, error => console.error(error));
  }
}
