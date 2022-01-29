import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common'; 
import { RouterModule, Routes } from '@angular/router';

import { StoreComponent } from './store.component';
import { StoreHomeComponent } from './home/home.component';
import { StoreInventoryComponent } from './inventory/inventory.component';
import { StoreOrdersComponent } from './orders/orders.component';
import { StoreOrderDetailComponent } from './order-detail/order-detail.component';
import { AccordionComponent } from '../accordion/accordion.component';

import { StoreStateService } from './store.service';

const storeRoutes: Routes = [
  { path: 'store', component: StoreHomeComponent },
  { path: 'store/inventory', component: StoreInventoryComponent },
  { path: 'store/orders', component: StoreOrdersComponent }
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
    StoreInventoryComponent,
    StoreOrdersComponent,
    StoreOrderDetailComponent,
    AccordionComponent,
  ],
  exports: [StoreComponent],
  providers: [StoreStateService],
  bootstrap: [StoreComponent]
})
export class StoreModule { }