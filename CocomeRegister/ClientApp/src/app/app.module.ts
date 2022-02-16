import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule, Routes } from '@angular/router';

import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { NavigationComponent } from './navigation/navigation.component';
import { StoreComponent } from './store/store.component';
import { CashDeskComponent } from './cashdesk/cashdesk.component';
import { EnterpriseComponent } from './enterprise/enterprise.component';

import { StoreModule } from './store/store.module';
import { EnterpriseModule } from './enterprise/enterprise.module';
import { CashDeskModule } from './cashdesk/cashdesk.module';

import { ApiAuthorizationModule } from './api-authorization/api-authorization.module';
import { AuthorizeGuard } from './api-authorization/authorize.guard';
import { AuthorizeInterceptor } from './api-authorization/authorize.interceptor';
import { AuthorizeService } from './api-authorization/authorize.service';

const appRoutes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'kasse', component: CashDeskComponent, canActivate: [AuthorizeGuard], loadChildren: () => import('./cashdesk/cashdesk.module').then(m => m.CashDeskModule) },
  { path: 'filiale', component: StoreComponent, canActivate: [AuthorizeGuard], loadChildren: () => import('./store/store.module').then(m => m.StoreModule) },
  { path: 'admin', component: EnterpriseComponent, canActivate: [AuthorizeGuard], loadChildren: () => import('./enterprise/enterprise.module').then(m => m.EnterpriseModule) },
];

@NgModule({
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ApiAuthorizationModule,
    StoreModule,
    CashDeskModule,
    EnterpriseModule,
    RouterModule.forRoot(appRoutes)
  ],
  declarations: [
    AppComponent,
    HomeComponent,
    NavigationComponent,
  ],
  providers: [
    AuthorizeService,
    { 
      provide: HTTP_INTERCEPTORS,
      useClass: AuthorizeInterceptor,
      multi: true 
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
