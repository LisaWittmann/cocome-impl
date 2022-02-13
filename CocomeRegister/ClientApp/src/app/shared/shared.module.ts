import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";
import { NgModule } from "@angular/core";

import { AccordionComponent } from "./accordion/accordion.component";
import { ProductTableRowComponent } from "./product-table-row/product-table-row.component";

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
    ],
    declarations: [
      AccordionComponent,
      ProductTableRowComponent
    ],
    exports: [
        AccordionComponent,
        ProductTableRowComponent,
    ]
  })
  export class SharedModule { }
  