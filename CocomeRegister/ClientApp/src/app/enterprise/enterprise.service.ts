import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Product } from 'src/services/Product';
import { StateService } from 'src/services/StateService';

interface EnterpriseState {
    order: Product[];
}

const initialState: EnterpriseState = {
    order: [],
};

@Injectable({providedIn: 'root'})
export class EnterpriseStateService extends StateService<EnterpriseState> {
    order$: Observable<Product[]> = this.select(state => state.order);

    constructor() {
        super(initialState);
    }

    addProduct(product: Product) {
        this.setState({ order: [...this.state.order, product] });
    }

    removeProduct(product: Product) {
        this.setState({ order: [...this.state.order.filter(
            cardItem => cardItem.id !== product.id
        )]});
    }
}
