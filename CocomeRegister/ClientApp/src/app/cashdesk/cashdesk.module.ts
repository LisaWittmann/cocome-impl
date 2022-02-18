import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';
import { CommonModule } from '@angular/common';

import { CashDeskComponent } from './cashdesk.component';
import { CashDeskHomeComponent } from './home/home.component';
import { CashDeskCheckoutComponent } from './checkout/checkout.component';
import { CashDeskPaymentComponent } from './payment/payment.component';
import { CashDeskShoppingCardComponent } from './shopping-card/shopping-card.component';

import { ProductListComponent } from '../shared/product-list/product-list.component';
import { ProductCardComponent } from '../shared/product-card/product-card.component';
import { CashDeskStateService } from './cashdesk.service';

import { AuthorizeService } from '../api-authorization/authorize.service';
import { CashDeskGuard } from '../api-authorization/authorize.guard';

export const cashdeskRoutes: Routes = [
  { path: 'home', component: CashDeskHomeComponent, canActivate: [CashDeskGuard] },
  { path: 'checkout', component: CashDeskCheckoutComponent, canActivate: [CashDeskGuard] },
  { path: 'payment', component: CashDeskPaymentComponent, canActivate: [CashDeskGuard] },
];

@NgModule({
  imports: [
    FormsModule,
    CommonModule,
    RouterModule.forChild(cashdeskRoutes),
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
  exports: [CashDeskComponent],
  providers: [
    CashDeskStateService
  ],
  bootstrap: [CashDeskComponent]
})
export class CashDeskModule { }
