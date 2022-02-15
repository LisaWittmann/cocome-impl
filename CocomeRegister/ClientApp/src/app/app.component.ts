import { Component } from '@angular/core';
import { AuthorizeService } from './api-authorization/authorize.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent {
  title = 'app';
  user: string;

  constructor(private authService: AuthorizeService) {
    this.authService.getUser().subscribe(user => {
      console.log(user);
      this.user = user.name;
    });
  }
}
