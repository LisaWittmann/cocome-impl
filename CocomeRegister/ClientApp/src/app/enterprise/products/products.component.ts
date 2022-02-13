import { Component } from '@angular/core';
import { Product } from 'src/services/Models';
import { EnterpriseStateService } from '../enterprise.service';

@Component({
  selector: 'app-enterprise-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.scss'],
})
export class EnterpriseProductsComponent {
  products: Product[];

  constructor(private enterpriseService: EnterpriseStateService) {
    this.enterpriseService.products$.subscribe(products => {
      this.products = products;
    });
  }
}
