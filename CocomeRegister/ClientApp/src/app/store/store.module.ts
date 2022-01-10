import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

import { StoreComponent } from './store.component';
import { StoreHomeComponent } from './home/home.component';
import { StoreStateService } from './store.service';


@NgModule({
  imports: [
    FormsModule,
    RouterModule.forChild([
        { path: 'store', component: StoreHomeComponent }
    ])
  ],
  declarations: [
      StoreComponent,
      StoreHomeComponent
  ],
  exports: [StoreComponent],
  providers: [StoreStateService],
  bootstrap: [StoreComponent]
})
export class StoreModule { }