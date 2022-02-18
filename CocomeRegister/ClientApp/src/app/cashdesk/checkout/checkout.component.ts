import { AfterViewInit, Component, ElementRef, ViewChild } from '@angular/core';
import { Product } from 'src/services/Models';
import { CashDeskStateService } from '../cashdesk.service';

@Component({
  selector: 'app-cashdesk-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.scss'],
})
export class CashDeskCheckoutComponent implements AfterViewInit {
  @ViewChild('barcodeInput', { static: false }) barcodeInput: ElementRef<HTMLInputElement>;
  expressMode: boolean;
  discount: number;
  products: Product[];
  barcode: string;

  constructor(private cashdeskState: CashDeskStateService) {
    this.cashdeskState.expressMode$.subscribe(mode => {
      this.expressMode = mode;
      this.discount = this.cashdeskState.discount;
    });
    this.cashdeskState.storeProducts$.subscribe(storeProducts => {
      this.products = storeProducts;
    });
  }

  addToCard(product: Product) {
    this.cashdeskState.addProduct(product);
    this.barcodeInput.nativeElement.focus();
  }

  removeFromCard(product: Product) {
    this.cashdeskState.removeProduct(product);
    this.barcodeInput.nativeElement.focus();
  }

  onSubmit() {
    const product = this.products.find(p => p.id == Number(this.barcode));
    if (product) {
      this.addToCard(product);
      this.barcode = undefined;
    }
    this.barcodeInput.nativeElement.focus();
  }

  ngAfterViewInit() {
    this.barcodeInput.nativeElement.focus();
  }
}
