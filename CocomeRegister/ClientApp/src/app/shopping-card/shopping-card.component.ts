import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Product } from 'src/services/Product';

@Component({
  selector: 'app-shopping-card',
  templateUrl: './shopping-card.component.html',
  styleUrls: ['./shopping-card.component.css']
})
export class ShoppingCardComponent {
    @Input() products: Array<Product>;
    @Output() addProductEvent = new EventEmitter<Product>();
    @Output() removeProductEvent = new EventEmitter<Product>();

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
    getProductSet(): Array<Product> {
        return [...new Set(this.products)];
    }

    /**
     * get sum of all product prices
     * @returns total price of product list
     */
    getTotalPrice(): string {
        let sum = 0;
        for (const product of this.products) {
            sum += product.price;
        }
        return sum.toFixed(2);
    }

    /**
     * emit event to add product to product list
     * @param product product that should be added
     */
    addProduct(product: Product): void {
        this.addProductEvent.emit(product);
    }

    /**
     * emit event to remove product to product list
     * @param product product that should be removed
     */
    removeProduct(product: Product): void {
        this.removeProductEvent.emit(product);
    }
}