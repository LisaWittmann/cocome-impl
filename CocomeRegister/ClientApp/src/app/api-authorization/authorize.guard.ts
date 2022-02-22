import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthorizeService } from './authorize.service';
import { tap } from 'rxjs/operators';
import { ApplicationPaths } from './api-authorization.constants';

@Injectable({
  providedIn: 'root'
})
export class StoreGuard implements CanActivate {
  constructor(
    private authorize: AuthorizeService,
    private router: Router
  ) {}
  
  canActivate(
    _next: ActivatedRouteSnapshot): Observable<boolean> | Promise<boolean> | boolean {
      return this.authorize.isManager()
        .pipe(tap(isAuthenticated => this.handleAuthorization(isAuthenticated)));
  }

  private handleAuthorization(isAuthenticated: boolean) {
    if (!isAuthenticated) {
      this.router.navigate(ApplicationPaths.LoginPathComponents);
    }
  }
}

@Injectable({
  providedIn: 'root'
})
export class CashDeskGuard implements CanActivate {
  constructor(private authorize: AuthorizeService, private router: Router) {
  }
  canActivate(
    _next: ActivatedRouteSnapshot): Observable<boolean> | Promise<boolean> | boolean {
      return this.authorize.isCashier()
        .pipe(tap(isAuthenticated => this.handleAuthorization(isAuthenticated)));
  }

  private handleAuthorization(isAuthenticated: boolean) {
    if (!isAuthenticated) {
      this.router.navigate(ApplicationPaths.LoginPathComponents);
    }
  }
}

@Injectable({
  providedIn: 'root'
})
export class EnterpriseGuard implements CanActivate {
  constructor(private authorize: AuthorizeService, private router: Router) {
  }
  canActivate(
    _next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
      return this.authorize.isAdmin()
        .pipe(tap(isAuthenticated => this.handleAuthorization(isAuthenticated, state)));
  }

  private handleAuthorization(isAuthenticated: boolean, state: RouterStateSnapshot) {
    if (!isAuthenticated) {
      this.router.navigate(ApplicationPaths.LoginPathComponents);
    }
  }
}