import { Component } from '@angular/core';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css']
})
export class ProductListComponent {
  // test data
  products: Product[] = [
    {
      id: 12345,
      name: "Bratwurstaufstrich",
      price: 4.99,
      description: ""
    },
    {
      id: 12345,
      name: "Bratwurstaufstrich",
      price: 4.99,
      description: ""
    },
    {
      id: 35656,
      name: "Leberwurstaufstrich",
      price: 2.99,
      description: ""
    },
    {
      id: 7263,
      name: "Regenschirm",
      price: 8.99,
      description: ""
    },
    {
      id: 8843,
      name: "Schokoriegel",
      price: 1.99,
      description: ""
    }
  ];

  /**
   * get amount of product in product list
   * products are compared by their id
   * @param products list that should be filtered
   * @param product product for which id the list will be filtered
   * @returns amount of product in list
   */
  getAmount(products: Array<Product>, product: Product): number {
    return products.filter(p => p.id == product.id).length;
  }

  /**
   * transform component's product list into a set
   * @returns array without dublicated items
   */
  getProductSet(): Array<Product> {
    const productSet = [];
    for (const product of this.products) {
      if (!this.getAmount(productSet, product)) productSet.push(product);
    }
    return productSet;
  }

  /**
   * get sum of all product prices
   * @returns total price of component's product list
   */
  getTotalPrice(): string {
    let sum = 0;
    for (const product of this.products) {
      sum += product.price;
    }
    return sum.toFixed(2);
  }

  /**
   * add product to component's product list
   * @param product product that should be added
   */
  addProduct(product: Product): void {
    this.products.push(product);
  }

  /**
   * remove product from component's product list
   * @param product product that should be removed
   */
  removeProduct(product: Product): void {
    this.products = this.products.filter(p => p != product);
    console.log(this.products);
  }
}

interface Product {
  id: number;
  name: string;
  price: number;
  description: string;
}
