import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { ExtraOptions, RouterModule, Routes } from '@angular/router';

import { CashDeskModule } from './cashdesk/cashdesk.module';
import { StoreModule } from './store/store.module';
import { EnterpriseModule } from './enterprise/enterprise.module';

import { AppComponent } from './app.component';
import { NavigationComponent } from './navigation/navigation.component';

const routerOptions: ExtraOptions = {
  scrollPositionRestoration: 'enabled',
  anchorScrolling: 'enabled',
  scrollOffset: [0, 64],
};

const appRoutes: Routes = [
  { path: '', redirectTo: '/cashdesk', pathMatch: 'full' },
  { path: 'cashdesk', loadChildren: () => import('./cashdesk/cashdesk.module').then(m => m.CashDeskModule) },
  { path: 'store', loadChildren: () => import('./store/store.module').then(m => m.StoreModule) },
  { path: 'enterprise', loadChildren: () => import('./enterprise/enterprise.module').then(m => m.EnterpriseModule)},
];
@NgModule({
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    CashDeskModule,
    StoreModule,
    EnterpriseModule,
    FormsModule,
    RouterModule.forRoot(appRoutes, routerOptions)
  ],
  declarations: [
    AppComponent,
    NavigationComponent,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
