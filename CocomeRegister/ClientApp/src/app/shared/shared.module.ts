import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';

import { AccordionComponent } from './accordion/accordion.component';
import { ProductDetailComponent } from './product-detail/product-detail.component';
import { ProductTableRowComponent } from './product-table-row/product-table-row.component';

import { LazyLoadDirective } from './lazy-load.directive';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
  ],
  declarations: [
    AccordionComponent,
    ProductDetailComponent,
    ProductTableRowComponent,
    LazyLoadDirective,
  ],
  exports: [
    AccordionComponent,
    ProductDetailComponent,
    ProductTableRowComponent,
    LazyLoadDirective,
  ]
})
export class SharedModule { }
