import { Component } from '@angular/core';
import { AuthorizeService } from '../api-authorization/authorize.service';

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
})
export class NavigationComponent {
  user: string;
  isAuthenticated: string;

  constructor(private authService: AuthorizeService) {
    this.authService.getUser().subscribe(user => {
      this.user = user ? user.name : undefined;
    });
  }
}
