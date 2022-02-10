import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule, Routes } from '@angular/router';

import { AppComponent } from './app.component';
import { NavigationComponent } from './navigation/navigation.component';
import { StoreComponent } from './store/store.component';
import { CashDeskComponent } from './cashdesk/cashdesk.component';
import { EnterpriseComponent } from './enterprise/enterprise.component';

import { StoreEntrypoint } from './store/store.module';
import { EnterpriseEntrypoint } from './enterprise/enterprise.module';
import { CashDeskEntrypoint } from './cashdesk/cashdesk.module';

const appRoutes: Routes = [
  { path: '', redirectTo: 'kasse', pathMatch: 'full' },
  { path: 'kasse', component: CashDeskComponent, loadChildren: () => import('./cashdesk/cashdesk.module').then(m => m.CashDeskModule) },
  { path: 'filiale', component: StoreComponent, loadChildren: () => import('./store/store.module').then(m => m.StoreModule) },
  { path: 'admin', component: EnterpriseComponent, loadChildren: () => import('./enterprise/enterprise.module').then(m => m.EnterpriseModule)},
];

@NgModule({
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot(appRoutes,  { scrollPositionRestoration: 'top' } )
  ],
  declarations: [
    AppComponent,
    NavigationComponent,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
