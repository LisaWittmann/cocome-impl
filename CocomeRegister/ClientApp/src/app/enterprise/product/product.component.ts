import { Location } from '@angular/common';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Product, Provider, Store } from 'src/services/Models';
import { EnterpriseStateService } from '../enterprise.service';

@Component({
  selector: 'app-enterprise-product',
  templateUrl: './product.component.html',
})
export class EnterpriseProductComponent {
  product: Product;
  providers: Provider[];
  stores: Store[];
  availableStores: Store[];

  newStore = {} as Store;

  constructor(
    private enterpriseService: EnterpriseStateService,
    private router: Router,
    private location: Location,
  ) {
    const productId = Number(router.url.split('/').pop());
    this.enterpriseService.products$.subscribe(products => {
      this.product = products.find(p => p.id == productId);
      if (!this.product) {
        location.back();
      }
    });
    this.enterpriseService.providers$.subscribe(providers => {
      this.providers = providers;
    });
    this.enterpriseService.stores$.subscribe(stores => {
      this.stores = stores;
    });
    this.enterpriseService.getStoresByProduct(productId).subscribe(stores => {
      this.availableStores = stores;
      this.stores = this.stores.filter(store => !this.availableStores.some((a) => (a.id === store.id)));
    })
  }

  updateProduct() {
    this.enterpriseService.updateProduct(this.product);
    this.router.navigate(['/admin/produkte']);
  }

  addToStore() {
    if (this.newStore) {
      this.enterpriseService.addProductToStore(this.product.id, this.newStore.id).subscribe(stores => {
        this.availableStores = stores;
      });
    }
  }
}

