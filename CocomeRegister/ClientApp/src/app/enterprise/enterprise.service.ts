import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { Product, Store, Provider, Statistic, User } from 'src/services/Models';
import { StateService } from 'src/services/StateService';

interface EnterpriseState {
    products: Product[];
    stores: Store[],
    providers: Provider[],
    users: User[],
}

const initialState: EnterpriseState = {
    products: [],
    stores: [],
    providers: [],
    users: []
};

@Injectable({providedIn: 'root'})
export class EnterpriseStateService extends StateService<EnterpriseState> {
    products$: Observable<Product[]> = this.select(state => state.products);
    stores$: Observable<Store[]> = this.select(state => state.stores);
    providers$: Observable<Provider[]> = this.select(state => state.providers);
    users$: Observable<User[]> = this.select(state => state.users);
    api: string;

    constructor(
        private router: Router,
        private http: HttpClient,
        @Inject('BASE_URL') baseUrl: string
    ) {
        super(initialState);
        this.api = baseUrl + "api/enterprise";
        this.fetchProducts();
        this.fetchStores();
        this.fetchProviders();
    }

    private fetchProducts() {
        this.http.get<Product[]>(
            `${this.api}/products`
        ).subscribe(result => {
            this.setState({ products: result });
        }, error => console.error(error));
    }

    private fetchProviders() {
        this.http.get<Provider[]>(
            `${this.api}/provider`
        ).subscribe(result => {
            this.setState({ providers: result });
        }, error => console.error(error));
    }

    private fetchStores() {
        this.http.get<Store[]>(
            `${this.api}/stores`
        ).subscribe(result => {
            this.setState({ stores: result });
        }, error => console.error(error));
    }

    private fetchUsers() {
      this.http.get<User[]>(
        `${this.api}/user`
      ).subscribe(result => {
        this.setState({ stores: result });
      }, error => console.error(error));
    }

    addProduct(product: Product) {
        this.http.post<Product[]>(
            `${this.api}/create-product`,
            product
        ).subscribe(result => {
            this.setState({ products: result });
        }, error => console.error(error));
    }

    addStore(store: Store) {
        this.http.post<Store[]>(
            `${this.api}/create-store`,
            store,
        ).subscribe(result => {
            this.setState({ stores: result });
        }, error => console.error(error));
    }

    addProvider(provider: Provider) {
        this.http.post<Provider[]>(
            `${this.api}/create-provider`,
            provider
        ).subscribe(result => {
            this.setState({ providers: result });
        }, error => console.error(error));
    }

    updateProduct(product: Product) {
        this.http.post<Product[]>(
            `${this.api}/update-product/${product.id}`,
            product
        ).subscribe(result => {
            this.setState({ products: result });
        }, error => console.error(error));
    }

    updateStore(store: Store) {
        this.http.post<Store[]>(
            `${this.api}/update-store/${store.id}`,
            store
        ).subscribe(result => {
            this.setState({ stores: result });
        }, error => console.error(error));
    }

    updateProvider(provider: Provider) {
        this.http.post<Provider[]>(
            `${this.api}/update-provider/${provider.id}`,
            provider
        ).subscribe(result => {
            this.setState({ providers: result });
        }, error => console.error(error));
    }

    getProductsByProvider(provider: Provider) {
        return this.state.products.filter(p => p.provider.id == provider.id);
    }

    getStoresByProduct(productId: number) {
        return this.http.get<Store[]>(
            `${this.api}/product/${productId}/stores`
        );
    }

    getDeliveryStatistic() {
        return this.http.get<Statistic[]>(
            `${this.api}/provider-reports`
        );
    }

    getProfitStatistic() {
        return this.http.get<Statistic[]>(
            `${this.api}/store-reports`
        );
    }

    addProductToStore(storeId: number, product: Product) {
        return this.http.post<Store[]>(
            `${this.api}/create-stock/${storeId}`, product
        );
    }

    addUser(newUser: User, role: string, password: string) {
      this.http.post<User[]>(
        `${this.api}/user`,
        [newUser, role, password]
      ).subscribe(result => {
        this.setState({ users: result });
      }, error => console.error(error));
    }

    updateUserRole(user: User, role: string) {
      this.http.post<User[]>(
        `${this.api}/role/`,
        [user, role]
      ).subscribe(result => {
        this.setState({ users: result });
      }, error => console.error(error));
  }
}
