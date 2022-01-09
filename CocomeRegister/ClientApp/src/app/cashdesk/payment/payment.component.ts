import { ChangeDetectionStrategy, Component } from '@angular/core';
import { Router } from '@angular/router';
import { CashDeskStateService } from '../cashdesk.service';

@Component({
  selector: 'app-cashdesk-payment',
  templateUrl: './payment.component.html',
  styleUrls: ['./payment.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CashDeskPaymentComponent {
  expressMode: boolean;
  paymentMethod: PaymentMethod | undefined = undefined;
  cardPaymentAccepted = false;
  totalPrice: number = 0;
  handedCash: number = 0;
  
  constructor(private router: Router, private cashdeskState: CashDeskStateService) {
    this.cashdeskState.expressMode$.subscribe(mode => {
      this.expressMode = mode;
    })
    this.cashdeskState.shoppingCard$.subscribe(() => {
      this.totalPrice = this.cashdeskState.totalPrice;
    })
  }

  get cardPayment() {
    return this.paymentMethod == PaymentMethod.CARD;
  }

  get cashPayment() {
    return this.paymentMethod == PaymentMethod.CASH;
  }

  get cashback() {
    return this.handedCash - this.totalPrice;
  }

  get paymentAccepted() {
    if (this.cashPayment) return this.handedCash >= this.totalPrice;
    return this.cardPaymentAccepted;
  }

  checkoutCash() {
    this.paymentMethod = PaymentMethod.CASH;
    console.log(this.paymentMethod);
  }

  checkoutCard() {
    this.paymentMethod = PaymentMethod.CARD;
  }

  resetPaymentMethod() {
    this.paymentMethod = undefined;
  }

  confirmPayment() {
    console.log("payment confirmed")
    this.cashdeskState.closeCheckoutSession();
    this.router.navigate(['cashdesk']);
  }

}

enum PaymentMethod {
  CASH, CARD,
}