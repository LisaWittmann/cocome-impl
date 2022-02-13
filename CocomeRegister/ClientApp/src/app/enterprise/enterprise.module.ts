import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';
import { CommonModule } from '@angular/common';

import { EnterpriseComponent } from './enterprise.component';
import { EnterpriseNavigationComponent } from './navigation/navigation.component';
import { EnterpriseHomeComponent } from './home/home.component';
import { EnterpriseStateService } from './enterprise.service';
import { EnterpriseStoresComponent } from './stores/stores.component';
import { EnterpriseStoreDetailComponent } from './store-detail/store-detail.component';
import { EnterpriseReportsComponent } from './reports/reports.component';
import { EnterpriseProductsComponent } from './products/products.component';
import { EnterpriseProductDetailComponent } from './product-detail/product-detail.component';
import { EnterpriseProvidersComponent } from './providers/providers.component';
import { EnterpriseProviderDetailComponent } from './provider-detail/provider-detail.component';

const enterpriseRoutes: Routes = [
  { path: '', component: EnterpriseHomeComponent },
  { path: 'filialen', component: EnterpriseStoresComponent },
  { path: 'lieferanten', component: EnterpriseProvidersComponent },
  { path: 'lieferant/:id', component: EnterpriseProviderDetailComponent },
];
@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    RouterModule.forChild(enterpriseRoutes)
  ],
  declarations: [
    EnterpriseComponent,
    EnterpriseHomeComponent,
    EnterpriseNavigationComponent,
    EnterpriseStoresComponent,
    EnterpriseStoreDetailComponent,
    EnterpriseProductsComponent,
    EnterpriseProductDetailComponent,
    EnterpriseProvidersComponent,
    EnterpriseProductDetailComponent,
    EnterpriseReportsComponent,
  ],
  exports: [EnterpriseComponent],
  providers: [EnterpriseStateService],
  bootstrap: [EnterpriseComponent]
})
export class EnterpriseModule { }
