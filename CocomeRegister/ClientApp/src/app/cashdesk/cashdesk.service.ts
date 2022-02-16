import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Product, SaleElement } from 'src/services/Models';
import { StateService } from 'src/services/StateService';
import { StoreStateService } from '../store/store.service';

interface CashDeskState {
    id: number;
    storeId: number;
    expressMode: boolean;
    shoppingCard: SaleElement[];
}

const initialState: CashDeskState = {
    id: undefined,
    storeId: undefined,
    expressMode: true,
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
    private storeStateService: StoreStateService,
    private http: HttpClient,
    @Inject('BASE_URL') baseUrl: string
  ) {
        super(initialState);
        this.storeStateService.store$.subscribe(store => {
            this.state.storeId = store.id
        });
        this.api = baseUrl + "api/cashdesk";
        this.fetchExpressMode();
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
        const shoppingCard = this.state.shoppingCard;
        const cardItem = shoppingCard.find(item => item.product.id == product.id);
        if (cardItem) {
            cardItem.amount++;
            this.setState({ shoppingCard: shoppingCard });
        } else {
            this.setState({ shoppingCard: [...this.state.shoppingCard, {product: product, amount: 1}] });
        }
    }

    removeProduct(product: Product) {
        this.setState({ shoppingCard: this.state.shoppingCard.filter(
            cardItem => cardItem.product.id !== product.id
        )});
    }

    confirmCheckout() {
        this.http.post(
            `${this.api}/checkout/${this.state.storeId}`,
            this.state.shoppingCard,
        ).subscribe(() => {
            this.storeStateService.fetchInventory();
            this.setState({ shoppingCard: [] });
        }, error => console.error(error));
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
