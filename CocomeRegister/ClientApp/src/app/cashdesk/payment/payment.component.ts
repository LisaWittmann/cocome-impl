import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { PaymentMethod } from 'src/services/Models';
import { CashDeskStateService } from '../cashdesk.service';

@Component({
  selector: 'app-cashdesk-payment',
  templateUrl: './payment.component.html',
  styleUrls: ['./payment.component.scss'],
})
export class CashDeskPaymentComponent {
  expressMode: boolean;
  paymentMethod: PaymentMethod | undefined = undefined;
  cardPaymentAccepted = false;
  totalPrice = 0;
  handedCash = 0;

  constructor(
    private router: Router,
    private cashdeskState: CashDeskStateService
  ) {
    this.cashdeskState.expressMode$.subscribe(mode => {
      this.expressMode = mode;
    });
    this.cashdeskState.shoppingCard$.subscribe(() => {
      this.totalPrice = this.cashdeskState.getTotalPrice();
    });
  }

  get cardPayment() {
    return this.paymentMethod === PaymentMethod.CARD;
  }

  get cashPayment() {
    return this.paymentMethod === PaymentMethod.CASH;
  }

  get cashback() {
    return this.handedCash - this.totalPrice;
  }

  get paymentAccepted() {
    if (this.cashPayment) {
      return this.handedCash >= this.totalPrice;
    }
    return this.cardPaymentAccepted;
  }

  checkoutCash() {
    this.paymentMethod = PaymentMethod.CASH;
  }

  checkoutCard() {
    this.paymentMethod = PaymentMethod.CARD;
  }

  resetPaymentMethod() {
    this.paymentMethod = undefined;
  }

  confirmPayment() {
    this.cashdeskState.confirmCheckout(
      this.paymentMethod,
      this.handedCash
    ).then(() => {
      this.router.navigate(['/kasse/home']);
    }).catch(error => console.error(error));
  }
}
