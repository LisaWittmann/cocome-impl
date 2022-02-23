import { HttpClient } from '@angular/common/http';
import { Component, Inject } from '@angular/core';
import { Store } from 'src/models/Store';
import { User } from 'src/models/User';
import { EnterpriseStateService } from '../enterprise.service';

@Component({
  selector: 'app-enterprise-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.scss'],
})
export class EnterpriseUsersComponent {
  users: User[];
  stores: Store[];
  newUser = {} as User;
  newUserRole: string;
  newUserPassword: string;

  constructor(private enterpriseService: EnterpriseStateService, private http: HttpClient, @Inject("BASE_URL") baseUrl: string) {
    this.enterpriseService.stores$.subscribe(stores => {
      this.stores = stores;
    });
    this.enterpriseService.getUsers().subscribe(users => {
      this.users = users;
    });
  }

  saveRoleChanges(user: User, role: string) {
    this.enterpriseService.updateUserRole(user, role);
  }

  submitUser() {
    this.enterpriseService.addUser(this.newUser, this.newUserRole, this.newUserPassword).subscribe(user => {
      this.users.push(user);
    });
    this.newUser = {} as User;
  }
  
}
