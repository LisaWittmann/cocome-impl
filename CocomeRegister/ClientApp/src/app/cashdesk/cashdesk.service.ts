import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PagedResponse, PaymentMethod, Product, Sale, SaleElement } from 'src/services/Models';
import { StateService } from 'src/services/StateService';
import { AuthorizeService } from '../api-authorization/authorize.service';

interface CashDeskState {
    id: number;
    storeId: number;
    expressMode: boolean;
    shoppingCard: SaleElement[];
}

const initialState: CashDeskState = {
    id: 1,
    storeId: undefined,
    expressMode: false,
    shoppingCard: [],
};

const expressModeDiscount = 0.5;
const expressModeMaxItems = 8;

@Injectable({providedIn: 'root'})
export class CashDeskStateService extends StateService<CashDeskState> {
    expressMode$: Observable<boolean> = this.select(state => state.expressMode);
    shoppingCard$: Observable<SaleElement[]> = this.select(state => state.shoppingCard);
    api: string;

    constructor(
    private http: HttpClient,
    private authService: AuthorizeService,
    @Inject('BASE_URL') baseUrl: string
  ) {
        super(initialState);
        this.api = baseUrl + "api/cashdesk";
        this.authService.getUser().subscribe(user => {
            this.setState({ storeId: Number(user.store) });
        });
        if (this.state.id) {
            this.fetchExpressMode();
        }
    }

    private fetchExpressMode() {
        this.http.get<boolean>(
            `${this.api}/express/${this.state.id}`
        ).subscribe(result => {
            this.setState({ expressMode: result});
        }, error => console.error(error))
    }

    resetExpressMode() {
        this.http.post<boolean>(
            `${this.api}/update-express/${this.state.id}`,
            false
        ).subscribe(result => {
            this.setState({ expressMode: result });
        }, error => console.error(error));
    }

    addProduct(product: Product) {
        if (this.state.expressMode && 
            this.getCardItems() >= expressModeMaxItems) {
            return;
        }
        const cardItem = this.state.shoppingCard.find(item => item.product.id == product.id);
        if (cardItem) {
            const index = this.state.shoppingCard.indexOf(cardItem);
            cardItem.amount++;
            this.setState({ shoppingCard: 
                [
                    ...this.state.shoppingCard.slice(undefined, index), 
                    cardItem,
                    ...this.state.shoppingCard.slice(index + 1, undefined)
                ]
            });
        } else {
            this.setState({ shoppingCard: [...this.state.shoppingCard, {product: product, amount: 1}] });
        }
    }

    removeProduct(product: Product) {
        this.setState({ shoppingCard: this.state.shoppingCard.filter(
            cardItem => cardItem.product.id !== product.id
        )});
    }

    async confirmCheckout(paymentMethod: PaymentMethod, payed: number) {
        const sale: Sale = {
            saleElements: this.state.shoppingCard,
            paymentMethod: paymentMethod,
            payed: payed
        }
        return this.http.post(
            `${this.api}/checkout/${this.state.storeId}`, sale,
            { responseType: 'blob' }
        ).toPromise();
    }

    getProduct(id: number) {
        return this.http.get<Product>(`${this.api}/products/${this.state.storeId}/${id}`);
    }

    getProducts(page: number, size: number, searchparam?: string) {
        let query = searchparam? `?q=${searchparam}&` : '?';
        query += `pageNumber=${page}&pageSize=${size}`;
        return this.http.get<PagedResponse<Product[]>>(
            `${this.api}/products/${this.state.storeId}${query}`
        );
    }

    get discount() {
        return this.state.expressMode ? expressModeDiscount : 0;
    }

    getCardItems(): number {
        if (this.state.shoppingCard.length == 0) {
            return 0;
        }
        return this.state.shoppingCard
            .map(item => item.amount)
            .reduce((x, y) => (x + y));
    }

    getCardSum(): number {
        if (this.state.shoppingCard.length == 0) {
            return 0;
        }
        return this.state.shoppingCard
            .map(item => item.product.salePrice * item.amount)
            .reduce((x, y) => (x + y));
    }

    getTotalDiscount(): number {
        return this.getCardSum() * this.discount;
    }

    getTotalPrice() {
        return this.getCardSum() - this.getTotalDiscount();
    }
}
