import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Router } from '@angular/router';
import { Product } from 'src/services/Product';
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
    shoppingCard: Product[];
    shoppingCardSum: number;
    totalDiscount: number;
    totalPrice: number;

    constructor(
        private router: Router,
        private cashDeskState: CashDeskStateService,
    ) {
        this.cashDeskState.shoppingCard$.subscribe(shoppingCard => {
            this.shoppingCard = shoppingCard;
            this.shoppingCardSum = this.cashDeskState.shoppingCardSum;
            this.totalDiscount = this.cashDeskState.totalDiscount;
            this.totalPrice = this.cashDeskState.totalPrice;
        });
    }

    get productSet() {
        return [...new Set(this.shoppingCard)];
    }

    getAmount(product: Product): number {
        return this.shoppingCard.filter(p => p.id === product.id).length;
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
        this.router.navigate(['cashdesk/payment']);
    }
}
