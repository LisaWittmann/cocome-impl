import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { StateService } from "src/services/StateService";

interface CashDeskState {
    expressMode: boolean;
    discount: number,
}

const initialState: CashDeskState = {
    expressMode: true,
    discount: 0.5,
}

@Injectable({providedIn: 'root'})
export class CashDeskStateService extends StateService<CashDeskState> {
    expressMode$: Observable<boolean> = this.select(state => state.expressMode);
    discount$: Observable<number> = this.select(state => state.discount);

    constructor() {
        super(initialState);
    }

    setExpressMode(expressMode: boolean) {
        this.setState({ expressMode: expressMode, discount: this.getDiscount(expressMode) });
    }

    private getDiscount(expressMode: boolean) {
        if (expressMode) return 0.5;
        return 0;
    }
}