import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';

import { StoreComponent } from './store.component';
import { StoreHomeComponent } from './home/home.component';
import { StoreDashboardComponent } from './dashboard/dashboard.component';
import { StoreNavigationComponent } from './navigation/navigation.component';
import { StoreProductsComponent } from './products/products.component';
import { StoreProductDetailComponent } from './product-detail/product-detail.component';
import { StoreShoppingCardComponent } from './shopping-card/shopping-card.component';
import { StoreOrdersComponent } from './orders/orders.component';
import { StoreOrderDetailComponent } from './order-detail/order-detail.component';
import { StoreReportsComponent } from './reports/reports.component';
import { StoreStateService } from './store.service';
import { SharedModule } from '../shared/shared.module';

const storeRoutes: Routes = [
  { path: '', component: StoreHomeComponent },
  { path: 'dashboard', component: StoreDashboardComponent },
  { path: 'sortiment', component: StoreProductsComponent },
  { path: 'sortiment/:id', component: StoreProductDetailComponent },
  { path: 'bestellungen', component: StoreOrdersComponent },
  { path: 'berichte', component: StoreReportsComponent },
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
    StoreHomeComponent,
    StoreReportsComponent,
    StoreDashboardComponent,
    StoreNavigationComponent,
    StoreProductsComponent,
    StoreProductDetailComponent,
    StoreShoppingCardComponent,
    StoreOrdersComponent,
    StoreOrderDetailComponent,
  ],
  exports: [StoreComponent],
  providers: [StoreStateService],
  bootstrap: [StoreComponent],
})
export class StoreModule { }
