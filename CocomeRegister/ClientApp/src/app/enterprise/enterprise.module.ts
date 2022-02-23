import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';

import { EnterpriseComponent } from './enterprise.component';
import { EnterpriseNavigationComponent } from './navigation/navigation.component';
import { EnterpriseHomeComponent } from './home/home.component';

import { EnterpriseProductsComponent } from './products/products.component';
import { EnterpriseProductComponent } from './product/product.component';
import { EnterpriseCreateProductComponent } from './create-product/create-product.component';

import { EnterpriseStoresComponent } from './stores/stores.component';
import { EnterpriseProvidersComponent } from './providers/providers.component';
import { EnterpriseReportsComponent } from './reports/reports.component';

import { EnterpriseStateService } from './enterprise.service';
import { AuthorizeService } from '../api-authorization/authorize.service';
import { EnterpriseGuard } from '../api-authorization/authorize.guard';

const enterpriseRoutes: Routes = [
  { path: 'home', component: EnterpriseHomeComponent },
  { path: 'produkte', component: EnterpriseProductsComponent },
  { path: 'produkte/bearbeiten/:id', component: EnterpriseProductComponent },
  { path: 'produkte/neu', component: EnterpriseCreateProductComponent },
  { path: 'filialen', component: EnterpriseStoresComponent },
  { path: 'lieferanten', component: EnterpriseProvidersComponent },
  { path: 'statistik', component: EnterpriseReportsComponent },
];
@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    SharedModule,
    RouterModule.forChild(enterpriseRoutes)
  ],
  declarations: [
    EnterpriseComponent,
    EnterpriseHomeComponent,
    EnterpriseNavigationComponent,
    EnterpriseStoresComponent,
    EnterpriseProductsComponent,
    EnterpriseProductComponent,
    EnterpriseCreateProductComponent,
    EnterpriseProvidersComponent,
    EnterpriseReportsComponent,
  ],
  exports: [EnterpriseComponent],
  providers: [
    EnterpriseStateService,
    AuthorizeService
  ],
  bootstrap: [EnterpriseComponent]
})
export class EnterpriseModule { }
