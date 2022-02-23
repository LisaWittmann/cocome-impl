import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { StateService } from 'src/services/StateService';
import { Product } from 'src/models/Product';
import { Store, StockItem } from 'src/models/Store';
import { Order, OrderElement } from 'src/models/Order';
import { Statistic } from 'src/models/Transfer';
import { StockExchange } from 'src/models/StockExchange';
import { AuthorizeService } from '../api-authorization/authorize.service';

interface StoreState {
  store: Store;
  inventory: StockItem[];
  exchanges: StockExchange[];
  currentOrder: OrderElement[];
  orders: Order[];
}

const initialState: StoreState = {
  store: undefined,
  inventory: [],
  exchanges: [],
  currentOrder: [],
  orders: [],
};

@Injectable({providedIn: 'root'})
export class StoreStateService extends StateService<StoreState> {
  store$: Observable<Store> = this.select(state => state.store);
  inventory$: Observable<StockItem[]> = this.select(state => state.inventory);
  exchanges$: Observable<StockExchange[]> = this.select(state => state.exchanges);
  currentOrder$: Observable<OrderElement[]> = this.select(state => state.currentOrder);
  orders$: Observable<Order[]> = this.select(state => state.orders);
  api: string;

  constructor(
    private http: HttpClient,
    private authService: AuthorizeService,
    @Inject('BASE_URL') baseUrl: string
  ) {
    super(initialState);
    this.api = baseUrl + 'api/store';
    this.authService.getUser().subscribe(user => {
      this.fetchStore(user.store);
    });
    this.getSession();
  }

  /**
   * request a store by it's id and set the result as state
   * @param id storeid
   */
  fetchStore(id: string) {
    this.http.get<Store>(
      `${this.api}/${id}`
    ).subscribe(result => {
      this.setStore(result);
    }, error => console.error(error));
  }

  /**
   * request the states store stockitems
   */
  fetchInventory() {
    this.http.get<StockItem[]>(
      `${this.api}/inventory/${this.state.store.id}`
    ).subscribe(result => {
      this.setState({ inventory: result });
    }, error => console.error(error));
  }

  /**
   * request the states store orders
   */
  fetchOrders() {
    this.http.get<Order[]>(
      `${this.api}/orders/${this.state.store.id}`
    ).subscribe(result => {
      this.setState({ orders: result });
    }, error => console.error(error));
  }

  /**
   * request the states stores exchanges
   */
  fetchExchanges() {
    this.http.get<StockExchange[]>(
      `${this.api}/exchanges/${this.state.store.id}`
    ).subscribe(result => {
      console.log(result);
      this.setState({ exchanges: result });
    }, error => console.error(error));
  }

  get runningOutOfStock() {
    return this.state.inventory.filter(item => 
      (item.minimum && item.stock < item.minimum) || item.stock < 10
    );
  }

  /**
   * get saved store in session storage
   */
  getSession() {
    const store: Store = JSON.parse(sessionStorage.getItem('store'));
    if (store) {
      this.setStore(store);
    }
  }


  /**
   * update state store and all depending store data
   * @param store store to set as new state store
   */
  setStore(store: Store) {
    sessionStorage.setItem('store', JSON.stringify(store));
    this.setState({ store: store });
    this.fetchInventory();
    this.fetchOrders();
    this.fetchExchanges();
  }

  /**
   * convert product into an order element and add it to current order
   * or increase elements amount if product is already in order
   * @param product product to add to order
   * @param amount amount of prodcut to add to order, default 1
   * @param replace replace existing amount of product in order, default false
   */
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

  /**
   * decrease amount of the product in current order
   * @param product product to decrease amount
   */
  removeFromCard(product: Product) {
    const currentOrder = this.state.currentOrder;
    const cardProduct = this.state.currentOrder.find(element => element.product.id === product.id);
    if (cardProduct && cardProduct.amount > 1) {
      cardProduct.amount--;
    }
    this.setState({ currentOrder: currentOrder });
  }

  /**
   * remove product completely from the current order
   * @param product product to remove all instances of
   */
  removeAllFromCard(product: Product) {
    this.setState({ currentOrder: this.state.currentOrder.filter(element => element.product.id !== product.id) });
  }

  /**
   * post current order to api and update state to result
   */
  placeNewOrder() {
    this.http.post<Order[]>(
      `${this.api}/orders/${this.state.store.id}/create`,
      this.state.currentOrder
    ).subscribe(result => {
      console.log(result);
      this.setState({ currentOrder: [] });
      this.setState({ orders: result });
    }, error => console.error(error));
  }

  /**
   * mark order as delivered
   * @param order order to close
   */
  closeOrder(order: Order) {
    this.http.post<Order[]>(
      `${this.api}/orders/${this.state.store.id}/close`,
      order
    ).subscribe(result => {
      this.setState({ orders: result });
      this.fetchInventory();
    }, error => console.error(error));
  }

  /**
   * update product entry with current data
   * @param product product containing id of entry to update
   */
  updateProduct(product: Product) {
    this.http.post<StockItem[]>(
      `${this.api}/products/${this.state.store.id}/update`,
      product
    ).subscribe(result => {
      this.setState({ inventory: result });
    }, error => console.error(error));
  }

  /**
   * request product by its id
   * @param id product id
   * @returns observale http response
   */
  getProduct(id: number) {
    return this.http.get<Product>(
      `${this.api}/${this.state.store.id}/product/${id}`
    );
  }

  /**
   * get stores latest year profits
   * @returns statistic with all monthly profits
   * as observable http response
   */
  getLatestProfits() {
    const year = new Date(Date.now()).getFullYear();
    return this.http.get<Statistic>(
      `${this.api}/profit/${this.state.store.id}/${year}`
    );
  }

  /**
   * get stores overall profits
   * @returns list of statistics with each years profit
   * as observable http response
   */
  getProfits() {
    return this.http.get<Statistic[]>(
      `${this.api}/profit/${this.state.store.id}`
    );
  }

  /**
   * mark stock exchange as placed
   * @param exchange exchange to perform changes on
   */
  startExchange(exchange: StockExchange) {
    this.http.put<StockExchange[]>(
      `${this.api}/exchanges/${this.state.store.id}/start`,
      exchange
    ).subscribe(result => {
      this.setState({ exchanges: result });
    }, error => console.error(error));
  }

  /**
   * mark stock exchange as delivered
   * @param exchange exchange to perform changes on
   */
  closeExchange(exchange: StockExchange) {
    this.http.put<StockExchange[]>(
      `${this.api}/exchanges/${this.state.store.id}/close`,
      exchange
    ).subscribe(result => {
      this.setState({ exchanges: result });
    }, error => console.error(error));
  }
}
