import { Component, Input, Output, EventEmitter } from '@angular/core';
import { Product } from 'src/services/Product';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css']
})
export class ProductListComponent {
  @Input() filter: string;
  @Input() products: Product[] = [
    {
      id: 12345,
      name: "Salatgurke",
      price: 0.59,
      description: ""
    },
    {
      id: 35656,
      name: "Endiviensalat",
      price: 0.99,
      description: ""
    },
    {
      id: 7263,
      name: "Kräuterbaguette",
      price: 0.99,
      description: ""
    },
    {
      id: 8843,
      name: "Schokoriegel",
      price: 1.99,
      description: ""
    },
    {
      id: 8443,
      name: "Bircher Müsli",
      price: 1.99,
      description: ""
    },
    {
      id: 91233,
      name: "Papaya",
      price: 1.19,
      description: ""
    },
    {
      id: 75236,
      name: "Capri Sonne Orange",
      price: 0.79,
      description: ""
    },
    {
      id: 75231,
      name: "Kartoffeln",
      price: 1.39,
      description: ""
    },
    {
      id: 23484,
      name: "Kirschtomaten 500g",
      price: 1.69,
      description: ""
    },
    {
      id: 23484,
      name: "Eistee Pfirsich",
      price: 0.69,
      description: ""
    },
    {
      id: 23484,
      name: "Eistee Zitrone",
      price: 0.69,
      description: ""
    },
    {
      id: 26782,
      name: "Erdbeeren",
      price: 3.69,
      description: ""
    },
    {
      id: 82921,
      name: "Laugenbrezel",
      price: 0.49,
      description: ""
    },
    {
      id: 72361,
      name: "Katzenstreu",
      price: 2.79,
      description: ""
    },
    {
      id: 32170,
      name: "Schlagsahne",
      price: 0.29,
      description: ""
    }
  ];
  @Output() clickProductEvent = new EventEmitter<Product>()

  clickProductCard(product: Product) {
    this.clickProductEvent.emit(product)
  }

  displayedProducts(): Array<Product> {
    if (this.filter == "") return this.products;
    return this.products.filter(p => p.id.toString().includes(this.filter) || p.name.includes(this.filter));
  }

}