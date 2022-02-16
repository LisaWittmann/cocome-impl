import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';

import { StoreComponent } from './store.component';
import { StoreSelectComponent } from './select/select.component';
import { StoreDashboardComponent } from './dashboard/dashboard.component';
import { StoreNavigationComponent } from './navigation/navigation.component';
import { StoreProductsComponent } from './products/products.component';
import { StoreProductComponent } from './product/product.component';
import { StoreShoppingCardComponent } from './shopping-card/shopping-card.component';
import { StoreOrdersComponent } from './orders/orders.component';
import { StoreOrderDetailComponent } from './order-detail/order-detail.component';
import { StoreReportsComponent } from './reports/reports.component';
import { StoreStateService } from './store.service';
import { SharedModule } from '../shared/shared.module';
import { AuthorizeService } from '../api-authorization/authorize.service';
import { StoreGuard } from '../api-authorization/authorize.guard';

const storeRoutes: Routes = [
  { path: 'home', component: StoreDashboardComponent, canActivate: [StoreGuard] },
  { path: 'sortiment', component: StoreProductsComponent, canActivate: [StoreGuard] },
  { path: 'sortiment/:id', component: StoreProductComponent, canActivate: [StoreGuard] },
  { path: 'bestellungen', component: StoreOrdersComponent, canActivate: [StoreGuard] },
  { path: 'berichte', component: StoreReportsComponent, canActivate: [StoreGuard] },
];

@NgModule({
  imports: [
    FormsModule,
    CommonModule,
    SharedModule,
    RouterModule.forChild(storeRoutes)
  ],
  declarations: [
    StoreComponent,
    StoreSelectComponent,
    StoreReportsComponent,
    StoreDashboardComponent,
    StoreNavigationComponent,
    StoreProductsComponent,
    StoreProductComponent,
    StoreShoppingCardComponent,
    StoreOrdersComponent,
    StoreOrderDetailComponent,
  ],
  exports: [StoreComponent],
  providers: [
    StoreStateService,
    AuthorizeService
  ],
  bootstrap: [StoreComponent],
})
export class StoreModule { }
