import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Product, Provider } from 'src/services/Models';
import { EnterpriseStateService } from '../enterprise.service';

@Component({
  selector: 'app-enterprise-create-product',
  templateUrl: './create-product.component.html',
})
export class EnterpriseCreateProductComponent {
  product: Product = {} as Product;
  providers: Provider[];

  constructor(
    private enterpriseService: EnterpriseStateService,
    private router: Router
  ) { 
    this.enterpriseService.providers$.subscribe(providers => {
      this.providers = providers;
    });
  }

  saveProduct() {
    this.enterpriseService.addProduct(this.product);
    this.router.navigate(['/admin/produkte']);
  }
}
