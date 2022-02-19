import { AfterViewInit, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Product } from 'src/services/Models';
import { CashDeskStateService } from '../cashdesk.service';

@Component({
  selector: 'app-cashdesk-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.scss'],
})
export class CashDeskCheckoutComponent implements AfterViewInit, OnInit {
  @ViewChild('barcodeInput', { static: false }) barcodeInput: ElementRef<HTMLInputElement>;
  @ViewChild('productList', { static: false }) productList: ElementRef<HTMLElement>;
  expressMode: boolean;
  discount: number;
  barcode: string;

  products: Product[] = [];
  pageIndex = 1;
  lastPageIndex = 1;
  pageSize = 18;

  constructor(private cashdeskState: CashDeskStateService) {
    this.cashdeskState.expressMode$.subscribe(mode => {
      this.expressMode = mode;
      this.discount = this.cashdeskState.discount;
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

  updateProductList() {
    this.cashdeskState.getProducts(this.pageIndex, this.pageSize).subscribe(result => {
      this.products = [...this.products, ...result.data];
      this.pageIndex = result.pageNumber + 1;
      this.lastPageIndex = result.totalPages;
    }, error => console.error(error));
  }

  ngOnInit() {
    this.updateProductList();
  }

  ngAfterViewInit() {
    this.barcodeInput.nativeElement.focus();
    this.productList.nativeElement.addEventListener('scroll', (event) => {
      const container = event.target as HTMLElement;
      const reachedEnd = container.offsetHeight + container.scrollTop >= container.scrollHeight
      const reachedLastPage = this.pageIndex > this.lastPageIndex;
      if (reachedEnd && !reachedLastPage) {
        this.updateProductList();
      }
    });
  }
}
