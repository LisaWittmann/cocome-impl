import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output } from '@angular/core';
import { Router } from '@angular/router';
import { Product } from 'src/services/Product';

@Component({
  selector: 'app-cashdesk-shopping-card',
  templateUrl: './shopping-card.component.html',
  styleUrls: ['./shopping-card.component.scss']
})
export class CashDeskShoppingCardComponent {
    @Input() products: Array<Product>;
    @Input() discount: number;

    @Output() addProductEvent = new EventEmitter<Product>();
    @Output() removeProductEvent = new EventEmitter<Product>();

    selectedProduct: Product | undefined = undefined;

    constructor(private router: Router) {}

    /**
     * get amount of product in product list
     * products are compared by their id
     * @param product product for which id the list will be filtered
     * @returns amount of product in list
     */
    getAmount(product: Product): number {
        return this.products.filter(p => p.id == product.id).length;
    }

    /**
     * transform product list into a set
     * @returns array without dublicated items
     */
    getProducts(): Array<Product> {
        return [...new Set(this.products)];
    }

    /**
     * get sum of all product prices
     * @returns total price of product list
     */
    getTotalPrice(): number {
        return this.getSum() - this.getDiscount();
    }

    getSum(): number {
        let sum = 0;
        for (const product of this.products) {
            sum += product.price;
        }
        return sum;
    }

    getDiscount(): number {
        return this.getSum() * this.discount;
    }

    /**
     * emit event to add product to product list
     * @param product product that should be added
     */
    addProduct(product: Product): void {
        this.selectedProduct = undefined;
        this.addProductEvent.emit(product);
    }

    /**
     * emit event to remove product to product list
     * @param product product that should be removed
     */
    removeProduct(product: Product): void {
        this.selectedProduct = undefined;
        this.removeProductEvent.emit(product);
    }

    select(product: Product): void {
        this.selectedProduct = product;
    } 

    isSelected(product: Product): boolean {
        return this.selectedProduct == product;
    }

    completeCheckout() {
        this.router.navigate(['cashdesk/payment'])
    }
 } 