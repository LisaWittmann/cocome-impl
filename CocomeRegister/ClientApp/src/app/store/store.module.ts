import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common'; 
import { RouterModule, Routes } from '@angular/router';

import { StoreComponent } from './store.component';
import { StoreHomeComponent } from './home/home.component';
import { StoreNavigationComponent } from './navigation/navigation.component';
import { StoreInventoryComponent } from './inventory/inventory.component';
import { StoreShoppingCardComponent } from './shopping-card/shopping-card.component';
import { StoreOrdersComponent } from './orders/orders.component';
import { StoreOrderDetailComponent } from './order-detail/order-detail.component';

import { AccordionComponent } from '../accordion/accordion.component';
import { ProductTableRowComponent } from '../product-table-row/product-table-row.component';

import { StoreStateService } from './store.service';

export const storeRoutes: Routes = [
  { path: '', component: StoreHomeComponent },
  { path: 'inventory', component: StoreInventoryComponent },
  { path: 'orders', component: StoreOrdersComponent }
];

@NgModule({
  imports: [
    FormsModule,
    CommonModule,
    RouterModule.forChild(storeRoutes)
  ],
  declarations: [
    StoreComponent,
    StoreHomeComponent,
    StoreNavigationComponent,
    StoreInventoryComponent,
    StoreShoppingCardComponent,
    StoreOrdersComponent,
    StoreOrderDetailComponent,
    AccordionComponent,
    ProductTableRowComponent,
  ],
  providers: [StoreStateService],
  bootstrap: [StoreComponent]
})
export class StoreModule { }

export function StoreEntrypoint() {
  return StoreModule;
}
