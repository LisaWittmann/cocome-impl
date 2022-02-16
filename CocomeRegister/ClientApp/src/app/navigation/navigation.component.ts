import { Component } from '@angular/core';
import { AuthorizeService } from '../api-authorization/authorize.service';
import { AuthRoles } from '../api-authorization/api-authorization.constants';

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
})
export class NavigationComponent {
  userName: string;
  userRoles: string[];

  constructor(private authService: AuthorizeService) {
    this.authService.getUser().subscribe(user => {
      console.log("user", user);
      this.userName = user ? user.name : undefined;
      this.userRoles = user ? user.role : undefined;
    });
  }

  get isAdmin() {
    return this.userRoles && this.userRoles.includes(AuthRoles.Admin);
  }

  get isManager() {
    return this.userRoles && this.userRoles.includes(AuthRoles.Manager);
  }

  get isCashier() {
    return this.userRoles && this.userRoles.includes(AuthRoles.Cashier);
  }
}
