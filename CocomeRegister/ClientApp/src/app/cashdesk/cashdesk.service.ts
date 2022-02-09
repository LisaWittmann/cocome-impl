import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Product } from 'src/services/Product';
import { StateService } from 'src/services/StateService';
import { StoreStateService } from '../store/store.service';

interface CashDeskState {
    id: number;
    expressMode: boolean;
    shoppingCard: Product[];
}

const initialState: CashDeskState = {
    id: 1,
    expressMode: true,
    shoppingCard: [],
};

const expressModeDiscount = 0.5;
const expressModeMaxItems = 8;

@Injectable({providedIn: 'root'})
export class CashDeskStateService extends StateService<CashDeskState> {
    expressMode$: Observable<boolean> = this.select(state => state.expressMode);
    shoppingCard$: Observable<Product[]> = this.select(state => state.shoppingCard);
    api: string;

  constructor(
    private storeState: StoreStateService,
    private http: HttpClient,
    @Inject('BASE_URL') baseUrl: string
  ) {
        super(initialState);
        this.api = baseUrl + "api/cashdesk";
        this.fetchExpressMode();
    }

    private fetchExpressMode() {
        this.http.get<boolean>(`${this.api}/express/${this.state.id}`).subscribe(result => {
            this.setState({ expressMode: result});
        }, error => console.error(error))
    }

    resetExpressMode() {
        this.http.post<boolean>(`${this.api}/update-express/${this.state.id}`, false).subscribe(result => {
            this.setState({ expressMode: result });
        }, error => console.error(error));
    }

    addProduct(product: Product) {
        if (this.state.expressMode && this.state.shoppingCard.length >= expressModeMaxItems) {
            return;
        }
        console.log("add product")
        this.setState({ shoppingCard: [...this.state.shoppingCard, product] });
    }

    removeProduct(product: Product) {
        this.setState({ shoppingCard: [...this.state.shoppingCard.filter(
            cardItem => cardItem.id !== product.id
        )]});
    }

    closeCheckoutSession() {
        this.storeState.removeProducts(this.state.shoppingCard);
        this.setState({ shoppingCard: [] });
    }

    get discount() {
        return this.state.expressMode ? expressModeDiscount : 0;
    }

    get totalPrice() {
        return this.shoppingCardSum - this.totalDiscount;
    }

    get shoppingCardSum(): number {
        let sum = 0;
        for (const product of this.state.shoppingCard) {
            sum += product.salePrice;
        }
        return sum;
    }

    get totalDiscount(): number {
        return this.shoppingCardSum * this.discount;
    }
}
