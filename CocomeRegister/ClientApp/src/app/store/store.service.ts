import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { Product } from "src/services/Product";
import { StateService } from "src/services/StateService";

interface StoreState {

}

const initialState: StoreState = {
}

@Injectable({providedIn: 'root'})
export class EnterpriseStateService extends StateService<StoreState> {

    constructor() {
        super(initialState);
    }
}