import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { EnterpriseComponent } from './enterprise.component';
import { EnterpriseHomeComponent } from './home/home.component';


@NgModule({
  declarations: [
      EnterpriseComponent,
      EnterpriseHomeComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forChild([
        { path: 'enterprise', component: EnterpriseHomeComponent }
    ])
  ],
  providers: [],
  bootstrap: [EnterpriseComponent]
})
export class EnterpriseModule { }