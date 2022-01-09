import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { CashDeskComponent } from './cashdesk.component';
import { CashDeskHomeComponent } from './home/home.component';
import { CashDeskCheckoutComponent } from './checkout/checkout.component';
import { CashDeskPaymentComponent } from './payment/payment.component';
import { CashDeskShoppingCardComponent } from './shopping-card/shopping-card.component';

import { ProductListComponent } from '../product-list/product-list.component';
import { ProductCardComponent } from '../product-card/product-card.component';

@NgModule({
  declarations: [
    CashDeskComponent,
    CashDeskHomeComponent,
    CashDeskCheckoutComponent,
    CashDeskShoppingCardComponent,
    CashDeskPaymentComponent,
    ProductListComponent,
    ProductCardComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: 'cashdesk', component: CashDeskHomeComponent },
      { path: 'cashdesk/checkout', component: CashDeskCheckoutComponent },
      { path: 'cashdesk/payment', component: CashDeskPaymentComponent },
    ])
  ],
  providers: [],
  bootstrap: [CashDeskHomeComponent]
})
export class CashDeskModule { }
