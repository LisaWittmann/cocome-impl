import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Router } from '@angular/router';
import { Product, SaleElement } from 'src/services/Models';
import { CashDeskStateService } from '../cashdesk.service';

@Component({
  selector: 'app-cashdesk-shopping-card',
  templateUrl: './shopping-card.component.html',
  styleUrls: ['./shopping-card.component.scss']
})
export class CashDeskShoppingCardComponent {
    @Input() discount: number;

    @Output() addProductEvent = new EventEmitter<Product>();
    @Output() removeProductEvent = new EventEmitter<Product>();

    selectedProduct: Product | undefined = undefined;
    shoppingCard: SaleElement[];
    shoppingCardSum: number;
    totalDiscount: number;
    totalPrice: number;

    constructor(
        private router: Router,
        private cashDeskState: CashDeskStateService,
    ) {
        this.cashDeskState.shoppingCard$.subscribe(shoppingCard => {
            this.shoppingCard = shoppingCard;
            this.shoppingCardSum = this.cashDeskState.getCardSum();
            this.totalDiscount = this.cashDeskState.getTotalDiscount();
            this.totalPrice = this.cashDeskState.getTotalPrice();
        });
    }

    addProduct(product: Product): void {
        this.selectedProduct = undefined;
        this.addProductEvent.emit(product);
    }

    removeProduct(product: Product): void {
        this.selectedProduct = undefined;
        this.removeProductEvent.emit(product);
    }

    select(product: Product): void {
        this.selectedProduct = product;
    }

    isSelected(product: Product): boolean {
        return this.selectedProduct === product;
    }

    completeCheckout() {
        this.router.navigate(['kasse/payment']);
    }
}
