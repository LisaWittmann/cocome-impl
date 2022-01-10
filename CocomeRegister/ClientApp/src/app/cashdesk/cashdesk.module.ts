import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';
import { CommonModule } from '@angular/common'; 

import { CashDeskComponent } from './cashdesk.component';
import { CashDeskHomeComponent } from './home/home.component';
import { CashDeskCheckoutComponent } from './checkout/checkout.component';
import { CashDeskPaymentComponent } from './payment/payment.component';
import { CashDeskShoppingCardComponent } from './shopping-card/shopping-card.component';

import { ProductListComponent } from '../product-list/product-list.component';
import { ProductCardComponent } from '../product-card/product-card.component';
import { CashDeskStateService } from './cashdesk.service';

const cashDeskRoutes: Routes = [
  { path: 'cashdesk', component: CashDeskHomeComponent },
  { path: 'cashdesk/checkout', component: CashDeskCheckoutComponent },
  { path: 'cashdesk/payment', component: CashDeskPaymentComponent },
];

@NgModule({
  imports: [
    FormsModule,
    CommonModule,
    RouterModule.forChild(cashDeskRoutes),
  ],
  declarations: [
    CashDeskComponent,
    CashDeskHomeComponent,
    CashDeskCheckoutComponent,
    CashDeskShoppingCardComponent,
    CashDeskPaymentComponent,
    ProductListComponent,
    ProductCardComponent,
  ],
  providers: [CashDeskStateService],
  bootstrap: [CashDeskComponent]
})
export class CashDeskModule { }
