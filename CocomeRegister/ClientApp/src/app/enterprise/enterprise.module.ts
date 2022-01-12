import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';
import { CommonModule } from '@angular/common';  

import { EnterpriseComponent } from './enterprise.component';
import { EnterpriseHomeComponent } from './home/home.component';
import { EnterpriseStateService } from './enterprise.service';

const enterpriseRoutes: Routes = [
  { path: 'enterprise', component: EnterpriseHomeComponent }
]
@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    RouterModule.forChild(enterpriseRoutes)
  ],
  declarations: [
      EnterpriseComponent,
      EnterpriseHomeComponent,
  ],
  exports: [EnterpriseComponent],
  providers: [EnterpriseStateService],
  bootstrap: [EnterpriseComponent]
})
export class EnterpriseModule { }