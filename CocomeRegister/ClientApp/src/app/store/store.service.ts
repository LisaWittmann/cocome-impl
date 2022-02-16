import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { StateService } from 'src/services/StateService';
import { Product, StockItem, Store, Order, OrderElement, Statistic } from 'src/services/Models';

interface StoreState {
  store: Store;
  inventory: StockItem[];
  currentOrder: OrderElement[];
  orders: Order[];
}

const initialState: StoreState = {
  store: { name: "Test" } as Store,
  inventory: [],
  currentOrder: [],
  orders: [],
};

@Injectable({providedIn: 'root'})
export class StoreStateService extends StateService<StoreState> {
  store$: Observable<Store> = this.select(state => state.store);
  inventory$: Observable<StockItem[]> = this.select(state => state.inventory);
  currentOrder$: Observable<OrderElement[]> = this.select(state => state.currentOrder);
  orders$: Observable<Order[]> = this.select(state => state.orders);
  api: string;

  constructor(
      private http: HttpClient,
      @Inject('BASE_URL') baseUrl: string
  ) {
      super(initialState);
      this.api = baseUrl + "api/store";
      this.getSession();
  }

  fetchInventory() {
    this.http.get<StockItem[]>(`${this.api}/inventory/${this.state.store.id}`).subscribe(result => {
      this.setState({ inventory: result });
    }, error => console.error(error));
  }

  fetchOrders() {
    this.http.get<Order[]>(`${this.api}/orders/${this.state.store.id}`).subscribe(result => {
      this.setState({ orders: result });
    }, error => console.error(error));
  }

  get availableProducts() {
    return this.state.inventory.filter(item => item.stock > 0).map(item => item.product);
  }

  get runningOutOfStock() {
    return this.state.inventory.filter(item => item.stock < 10);
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
  }

  addToCard(product: Product, amount = 1, replace = false) {
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
      this.fetchInventory();
    }, error => console.error(error));
  }

  updateProduct(product: Product) {
    this.http.post<StockItem[]>(
      `${this.api}/update-product/${this.state.store.id}`,
      product
    ).subscribe(result => {
      this.setState({ inventory: result });
    }, error => console.error(error));
  }

  getProduct(id: number) {
    return this.http.get<Product>(`${this.api}/${this.state.store.id}/product/${id}`);
  }

  getLatestProfits() {
    const year = new Date(Date.now()).getFullYear();
    return this.http.get<Statistic>(
      `${this.api}/${this.state.store.id}/profit/${year}`
    );
  }

  getProfits() {
    return this.http.get<Statistic[]>(
      `${this.api}/profit/${this.state.store.id}`
    );
  }
}
