import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';

import { StoreComponent } from './store.component';
import { StoreHomeComponent } from './home/home.component';
import { StoreNavigationComponent } from './navigation/navigation.component';
import { StoreProductsComponent } from './products/products.component';
import { StoreProductDetailComponent } from './product-detail/product-detail.component';
import { StoreShoppingCardComponent } from './shopping-card/shopping-card.component';
import { StoreOrdersComponent } from './orders/orders.component';
import { StoreOrderDetailComponent } from './order-detail/order-detail.component';
import { StoreReportsComponent } from './reports/reports.component';

import { AccordionComponent } from '../shared/accordion/accordion.component';
import { ProductTableRowComponent } from '../shared/product-table-row/product-table-row.component';

import { StoreStateService } from './store.service';

const storeRoutes: Routes = [
  { path: '', component: StoreHomeComponent },
  { path: 'produkte', component: StoreProductsComponent },
  { path: 'bestellungen', component: StoreOrdersComponent },
  { path: 'berichte', component: StoreReportsComponent },
  { path: 'produkt/:id', component: StoreProductDetailComponent },
  { path: 'erstellen', component: StoreProductDetailComponent },
];

@NgModule({
  imports: [
    FormsModule,
    CommonModule,
    RouterModule.forChild(storeRoutes)
  ],
  declarations: [
    StoreComponent,
    StoreReportsComponent,
    StoreHomeComponent,
    StoreNavigationComponent,
    StoreProductsComponent,
    StoreProductDetailComponent,
    StoreShoppingCardComponent,
    StoreOrdersComponent,
    StoreOrderDetailComponent,
    AccordionComponent,
    ProductTableRowComponent,
  ],
  exports: [StoreComponent],
  providers: [StoreStateService],
  bootstrap: [StoreComponent],
})
export class StoreModule { }

export function StoreEntrypoint() {
  return StoreModule;
}
