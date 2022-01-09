import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { Product } from "src/services/Product";
import { StateService } from "src/services/StateService";

interface CashDeskState {
    expressMode: boolean;
    shoppingCard: Product[],
}

const initialState: CashDeskState = {
    expressMode: true,
    shoppingCard: [],
}

@Injectable({providedIn: 'root'})
export class CashDeskStateService extends StateService<CashDeskState> {
    expressMode$: Observable<boolean> = this.select(state => state.expressMode);
    shoppingCard$: Observable<Product[]> = this.select(state => state.shoppingCard);

    constructor() {
        super(initialState);
    }

    setExpressMode(expressMode: boolean) {
        this.setState({ expressMode: expressMode });
    }

    addProduct(product: Product) {
        if (this.state.expressMode && this.state.shoppingCard.length > 8) return;
        this.setState({ shoppingCard: [...this.state.shoppingCard, product] })
    }

    removeProduct(product: Product) {
        this.setState({ shoppingCard: [...this.state.shoppingCard.filter(
            cardItem => cardItem.id != product.id 
        )]});
    }

    closeCheckoutSession() {
        this.setState({ shoppingCard: [] });
    }

    get discount() {
        return this.state.expressMode ? 0.5 : 0;
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