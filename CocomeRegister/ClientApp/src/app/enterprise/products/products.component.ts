import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Product } from 'src/models/Product';
import { EnterpriseStateService } from '../enterprise.service';

@Component({
  selector: 'app-enterprise-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.scss'],
})
export class EnterpriseProductsComponent {
  products: Product[];

  constructor(
    private enterpriseService: EnterpriseStateService,
    private router: Router
  ) {
    this.enterpriseService.products$.subscribe(products => {
      this.products = products;
    });
  }

  toDetailPage(product: Product) {
    this.router.navigate(['/admin/produkte/bearbeiten', product.id]);
  }

  createProduct() {
    this.router.navigate(['/admin/produkte/neu']);
  }
}
