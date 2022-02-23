import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';

import { StoreComponent } from './store.component';
import { StoreHomeComponent } from './home/home.component';
import { StoreNavigationComponent } from './navigation/navigation.component';
import { StoreProductsComponent } from './products/products.component';
import { StoreProductComponent } from './product/product.component';
import { StoreShoppingCardComponent } from './shopping-card/shopping-card.component';
import { StoreOrdersComponent } from './orders/orders.component';
import { StoreOrderDetailComponent } from './order-detail/order-detail.component';
import { StoreExchangeDetailComponent } from './exchange-detail/exchange-detail.component';
import { StoreReportsComponent } from './reports/reports.component';
import { StoreStateService } from './store.service';
import { SharedModule } from '../shared/shared.module';

const storeRoutes: Routes = [
  { path: 'home', component: StoreHomeComponent },
  { path: 'sortiment', component: StoreProductsComponent },
  { path: 'sortiment/:id', component: StoreProductComponent },
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
    StoreReportsComponent,
    StoreHomeComponent,
    StoreNavigationComponent,
    StoreProductsComponent,
    StoreProductComponent,
    StoreShoppingCardComponent,
    StoreOrdersComponent,
    StoreOrderDetailComponent,
    StoreExchangeDetailComponent
  ],
  exports: [StoreComponent],
  providers: [
    StoreStateService,
  ],
  bootstrap: [StoreComponent],
})
export class StoreModule { }
