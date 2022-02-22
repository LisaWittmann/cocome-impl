import { Component } from '@angular/core';
import { AuthorizeService } from '../api-authorization/authorize.service';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
})
export class NavigationComponent {
  userName: Observable<string>;

  isAuthenticated: boolean;
  isAdmin: boolean;
  isCashier: boolean;
  isManager: boolean;

  constructor(private authService: AuthorizeService) {
    this.userName = this.authService.getUser().pipe(map(u => u && u.name));
    this.authService.isAuthenticated().subscribe(auth => {
      this.isAuthenticated = auth;
    });
    this.authService.isAdmin().subscribe(admin => {
      this.isAdmin = admin;
    });
    this.authService.isManager().subscribe(manager => {
      this.isManager = manager;
    });
    this.authService.isCashier().subscribe(cashier => {
      this.isCashier = cashier;
    });
  }
}
