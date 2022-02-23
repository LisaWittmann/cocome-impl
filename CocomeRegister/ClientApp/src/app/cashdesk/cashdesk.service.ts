import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Product } from 'src/models/Product';
import { Sale, SaleElement, PaymentMethod } from 'src/models/Sale';
import { PagedResponse} from 'src/models/Transfer';
import { StateService } from 'src/services/StateService';
import { AuthorizeService } from '../api-authorization/authorize.service';

interface CashDeskState {
    storeId: number;
    expressMode: boolean;
    shoppingCard: SaleElement[];
}

const initialState: CashDeskState = {
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
    lastSales: Sale[] = [];
    api: string;

    constructor(
    private http: HttpClient,
    private authService: AuthorizeService,
    @Inject('BASE_URL') baseUrl: string
  ) {
        super(initialState);
        this.api = baseUrl + 'api/cashdesk';
        this.authService.getUser().subscribe(user => {
            this.setState({ storeId: Number(user.store) });
        });
    }

    /**
     * convert product into a saleElement and add it to shopping card
     * or increase amount of saleElement if product is already in card
     * @param product product to add
     */
    addProduct(product: Product) {
        if (this.state.expressMode &&
            this.getCardItems() >= expressModeMaxItems) {
            return;
        }
        const cardItem = this.state.shoppingCard.find(item => item.product.id === product.id);
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
            this.setState({ shoppingCard: [...this.state.shoppingCard, {product: product, amount: 1, discount: this.discount}] });
        }
    }

    /**
     * remove saleElement with given product from shopping card
     * @param product product to remove
     */
    removeProduct(product: Product) {
        this.setState({ shoppingCard: this.state.shoppingCard.filter(
            cardItem => cardItem.product.id !== product.id
        )});
    }

    /**
     * request confirmation for new sale from shopping card elements
     * @param paymentMethod confiremed payment method of the customer
     * @param payed payed amount of the customer
     * @returns promise of the http response
     */
    async confirmCheckout(paymentMethod: PaymentMethod, payed: number) {
        const sale: Sale = {
            saleElements: this.state.shoppingCard,
            paymentMethod: paymentMethod,
            timeStamp: new Date(Date.now()),
            total: this.getTotalPrice(),
            payed: payed
        };
        return this.http.post(
            `${this.api}/checkout/${this.state.storeId}`, sale,
            { responseType: 'blob' }
        ).toPromise().then(response => {
            this.setState({ shoppingCard: [] });
            this.updateLastSales(sale);
            return response;
        });
    }

    /**
     * request product by id from api
     * @param id productId
     * @returns observable http response
     */
    getProduct(id: number) {
        return this.http.get<Product>(`${this.api}/products/${this.state.storeId}/${id}`);
    }

    /**
     * request available products in store
     * @param page current page number
     * @param size max amount of items per request
     * @param searchparam filter for products
     * @returns observable http response
     */
    getProducts(page: number, size: number, searchparam?: string) {
        let query = searchparam ? `?q=${searchparam}&` : '?';
        query += `pageNumber=${page}&pageSize=${size}`;
        return this.http.get<PagedResponse<Product[]>>(
            `${this.api}/products/${this.state.storeId}${query}`
        );
    }

    get discount() {
        return this.state.expressMode ? expressModeDiscount : 0;
    }

    /**
     * count items in shopping card
     * @returns total length of shopping card
     */
    getCardItems(): number {
        if (this.state.shoppingCard.length === 0) {
            return 0;
        }
        return this.state.shoppingCard
            .map(item => item.amount)
            .reduce((x, y) => (x + y));
    }

    /**
     * calculates sum of shopping card
     * @returns sum of all products in shopping card
     */
    getCardSum(): number {
        if (this.state.shoppingCard.length === 0) {
            return 0;
        }
        return this.state.shoppingCard
            .map(item => item.product.salePrice * item.amount)
            .reduce((x, y) => (x + y));
    }

    /**
     * calculates total discount of shoppingcard
     * @returns sum of all products discounts
     */
    getTotalDiscount(): number {
        return this.getCardSum() * this.discount;
    }

    /**
     * calculates price of shopping card minus total discount
     * @returns total price of all card items
     */
    getTotalPrice() {
        return this.getCardSum() - this.getTotalDiscount();
    }

    resetExpressMode() {
        this.setState({ expressMode: false });
    }

    updateLastSales(sale: Sale) {
        const currentTime = new Date(Date.now()).getTime();
        this.lastSales.push(sale);
        this.lastSales = this.lastSales.filter(sale => 
            ((currentTime - sale.timeStamp.getTime()) / 1000 / 60) < 60
        );
        const validSales = this.lastSales.filter(value => 
            value.saleElements.length < expressModeMaxItems &&
            value.paymentMethod == PaymentMethod.CASH
        );
        console.log(this.lastSales);
        this.setState({ expressMode: (validSales.length * 2) > this.lastSales.length });
    }
}
