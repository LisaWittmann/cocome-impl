import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { CashDeskModule } from './cashdesk/cashdesk.module';
import { StoreModule } from './store/store.module';
import { EnterpriseModule } from './enterprise/enterprise.module';

import { AppComponent } from './app.component';
import { NavigationComponent } from './navigation/navigation.component';

@NgModule({
  declarations: [
    AppComponent,
    NavigationComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    CashDeskModule,
    StoreModule,
    EnterpriseModule,
    RouterModule.forRoot([
      { path: 'cashdesk', component: CashDeskModule },
      { path: 'store', component: StoreModule },
      { path: 'enterprise', component: EnterpriseModule }
    ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
