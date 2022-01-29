import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { Product } from "src/services/Product";
import { StateService } from "src/services/StateService";
import { StoreStateService } from "../store/store.service";

interface CashDeskState {
    availableProducts: Product[],
    expressMode: boolean;
    shoppingCard: Product[],
}

const initialState: CashDeskState = {
    expressMode: true,
    shoppingCard: [],
    availableProducts: [],
}

const expressModeDiscount = 0.5;
const expressModeMaxItems = 8;

@Injectable({providedIn: 'root'})
export class CashDeskStateService extends StateService<CashDeskState> {
    expressMode$: Observable<boolean> = this.select(state => state.expressMode);
    shoppingCard$: Observable<Product[]> = this.select(state => state.shoppingCard);
    availableProducts$: Observable<Product[]> = this.select(state => state.availableProducts);

    constructor(private storeState: StoreStateService) {
        super(initialState);
        this.storeState.inventory$.subscribe(() => { 
            // not working yet
            this.setState({ availableProducts: this.storeState.availableProducts })
        });
    }

    setExpressMode(expressMode: boolean) {
        this.setState({ expressMode: expressMode });
    }

    addProduct(product: Product) {
        if (this.state.expressMode && this.state.shoppingCard.length > expressModeMaxItems) return;
        this.setState({ shoppingCard: [...this.state.shoppingCard, product] })
    }

    removeProduct(product: Product) {
        this.setState({ shoppingCard: [...this.state.shoppingCard.filter(
            cardItem => cardItem.id != product.id 
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
            sum += product.price;
        }
        return sum;
    }

    get totalDiscount(): number {
        return this.shoppingCardSum * this.discount;
    }
}