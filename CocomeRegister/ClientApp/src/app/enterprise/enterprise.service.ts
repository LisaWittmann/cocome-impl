import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { Product, Store, Provider, Statistic } from 'src/services/Models';
import { StateService } from 'src/services/StateService';

interface EnterpriseState {
    products: Product[];
    stores: Store[],
    providers: Provider[],
}

const initialState: EnterpriseState = {
    products: [],
    stores: [],
    providers: []
};

@Injectable({providedIn: 'root'})
export class EnterpriseStateService extends StateService<EnterpriseState> {
    products$: Observable<Product[]> = this.select(state => state.products);
    stores$: Observable<Store[]> = this.select(state => state.stores);
    providers$: Observable<Provider[]> = this.select(state => state.providers);
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

    /**
     * request all products from api
     */
    private fetchProducts() {
        this.http.get<Product[]>(
            `${this.api}/products`
        ).subscribe(result => {
            this.setState({ products: result });
        }, error => console.error(error));
    }

    /**
     * request all providers from api
     */
    private fetchProviders() {
        this.http.get<Provider[]>(
            `${this.api}/provider`
        ).subscribe(result => {
            this.setState({ providers: result });
        }, error => console.error(error));
    }

    /**
     * request all stores frm api
     */
    private fetchStores() {
        this.http.get<Store[]>(
            `${this.api}/stores`
        ).subscribe(result => {
            this.setState({ stores: result });
        }, error => console.error(error));
    }

    /**
     * request adding new product data to databse
     * @param product new product to add
     */
    addProduct(product: Product) {
        this.http.post<Product>(
            `${this.api}/create-product`,
            product
        ).subscribe(result => {
            this.setState({ products: [...this.state.products, result] });
        }, error => console.error(error));
    }

    /**
     * request adding new store data to database
     * @param store new store to add
     */
    addStore(store: Store) {
        this.http.post<Store>(
            `${this.api}/create-store`,
            store,
        ).subscribe(result => {
            this.setState({ stores: [...this.state.stores, result] });
        }, error => console.error(error));
    }

    /**
     * request adding new provider data to database
     * @param provider new provider to add
     */
    addProvider(provider: Provider) {
        this.http.post<Provider>(
            `${this.api}/create-provider`,
            provider
        ).subscribe(result => {
            this.setState({ providers: [...this.state.providers, result] });
        }, error => console.error(error));
    }

    /**
     * request updating products database entry
     * @param product new product data
     */
    updateProduct(product: Product) {
        const index = this.state.products.indexOf(
            this.state.products.find(p => p.id == product.id)
        );
        this.http.put<Product>(
            `${this.api}/update-product/${product.id}`,
            product
        ).subscribe(result => {
            this.setState({ products: [
                ...this.state.products.slice(0, index),
                result,
                ...this.state.products.slice(index + 1)
            ] });
        }, () => this.fetchProducts());
    }

    /**
     * request updating stores database entry
     * @param store new store data
     */
    updateStore(store: Store) {
        const index = this.state.stores.indexOf(
            this.state.stores.find(s => s.id == store.id)
        );
        this.http.put<Store>(
            `${this.api}/update-store/${store.id}`,
            store
        ).subscribe(result => {
            this.setState({ stores: [
                ...this.state.stores.slice(0, index),
                result,
                ...this.state.stores.slice(index + 1)
            ] });
        }, () => this.fetchStores());
    }

    /**
     * request updating providers database entry
     * @param product new provider data
     */
    updateProvider(provider: Provider) {
        const index = this.state.products.indexOf(
            this.state.products.find(p => p.id == provider.id)
        );
        this.http.put<Provider>(
            `${this.api}/update-provider/${provider.id}`,
            provider
        ).subscribe(result => {
            this.setState({ providers: [
                ...this.state.providers.slice(undefined, index),
                result,
                ...this.state.providers.slice(index + 1)
            ] });
        }, () => this.fetchProviders());
    }

    /**
     * request all products the given provider delivers
     * @param provider provider to request products from
     * @returns observable http response
     */
    getProductsByProvider(provider: Provider) {
        return this.state.products.filter(p => p.provider.id == provider.id);
    }

    /**
     * request all stores that have the given product in stock
     * @param productId product id to search stock items for
     * @returns observable http response
     */
    getStoresByProduct(productId: number) {
        return this.http.get<Store[]>(
            `${this.api}/product/${productId}/stores`
        );
    }

    /**
     * request delivery statistics
     * @returns observable http response
     */
    getDeliveryStatistic() {
        return this.http.get<Statistic[]>(
            `${this.api}/provider-reports`
        );
    }

    /**
     * request profut statistics for current year
     * @returns observable http response
     */
    getProfitStatistic() {
        return this.http.get<Statistic[]>(
            `${this.api}/store-reports`
        );
    }

    /**
     * request adding a product to a stores stock
     * @param storeId store to add product to
     * @param product product to add
     * @returns observable http response
     */
    addProductToStore(storeId: number, product: Product) {
        return this.http.post<Store[]>(
            `${this.api}/create-stock/${storeId}`, product
        );
    }
}
