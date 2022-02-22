import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Product } from 'src/models/Product';
import { Provider } from 'src/models/Provider';
import { EnterpriseStateService } from '../enterprise.service';

@Component({
  selector: 'app-enterprise-providers',
  templateUrl: './providers.component.html',
  styleUrls: ['./providers.component.scss'],
})
export class EnterpriseProvidersComponent {
  providers: Provider[];
  newProvider = {} as Provider;

  constructor(
    private enterpriseService: EnterpriseStateService,
    private router: Router
  ) {
    enterpriseService.providers$.subscribe(provider => {
      this.providers = provider;
    })
  }

  getProducts(provider: Provider) {
    return this.enterpriseService.getProductsByProvider(provider);
  }

  toProductPage(product: Product) {
    this.router.navigate(['/admin/produkte/bearbeiten', product.id]);
  }

  saveChanges(provider: Provider) {
    this.enterpriseService.updateProvider(provider);
  }

  submitProvider() {
    this.enterpriseService.addProvider(this.newProvider);
    this.newProvider = {} as Provider;
  }
}
