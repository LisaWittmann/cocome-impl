import { Location } from '@angular/common';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Product, Provider } from 'src/services/Models';
import { EnterpriseStateService } from '../enterprise.service';

@Component({
  selector: 'app-enterprise-product',
  templateUrl: './product.component.html',
})
export class EnterpriseProductComponent {
  product: Product;
  providers: Provider[];

  constructor(
    private enterpriseService: EnterpriseStateService,
    private router: Router,
    private location: Location,
  ) {
    const productId = Number(router.url.split('/').pop());
    this.enterpriseService.products$.subscribe(products => {
      this.product = products.find(p => p.id == productId);
      console.log(this.product);
    });
    this.enterpriseService.providers$.subscribe(providers => {
      this.providers = providers;
    });
    if (!this.product) {
      location.back();
    }
  }

  updateProduct() {
    this.enterpriseService.updateProduct(this.product);
    this.router.navigate(['/admin/produkte']);
  }
}

