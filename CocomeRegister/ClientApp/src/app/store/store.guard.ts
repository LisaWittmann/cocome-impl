import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { StoreStateService } from './store.service';

@Injectable({
  providedIn: 'root'
})
export class StoreGuard implements CanActivate {
  constructor(private storeService: StoreStateService, private router: Router) {
  }
  canActivate(
    _next: ActivatedRouteSnapshot,
  ): Observable<boolean> | Promise<boolean> | boolean {
      return this.storeService.isInitialized()
        .pipe(tap(initialzed => this.handleStoreSelect(initialzed)));
  }

  private handleStoreSelect(isInitialzed: boolean) {
    console.log("store initialized", isInitialzed);
    if (!isInitialzed) {
      this.router.navigate(["/filiale/auswahl"]);
    }
  }
}