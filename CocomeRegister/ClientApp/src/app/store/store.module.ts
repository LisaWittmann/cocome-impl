import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { StoreComponent } from './store.component';
import { StoreHomeComponent } from './home/home.component';


@NgModule({
  declarations: [
      StoreComponent,
      StoreHomeComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forChild([
        { path: 'store', component: StoreHomeComponent }
    ])
  ],
  providers: [],
  bootstrap: [StoreComponent]
})
export class StoreModule { }