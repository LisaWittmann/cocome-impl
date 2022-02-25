import { HttpClient } from '@angular/common/http';
import { Component, Inject } from '@angular/core';
import { Store } from 'src/models/Store';
import { Role, RoleSelect, User } from 'src/models/User';
import { EnterpriseStateService } from '../enterprise.service';

@Component({
  selector: 'app-enterprise-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.scss'],
})
export class EnterpriseUsersComponent {
  users: User[] = [];
  stores: Store[] = [];
  newUser: User;
  selectedRoles: RoleSelect[];
  registerError: boolean;

  constructor(private enterpriseService: EnterpriseStateService, private http: HttpClient, @Inject("BASE_URL") baseUrl: string) {
    this.initEmptyUser();
    this.initSelectedRoles();
    this.enterpriseService.stores$.subscribe(stores => {
      this.stores = stores;
    });
    this.enterpriseService.getUsers().subscribe(users => {
      console.log(users);
      this.users = users;
    });
  }

  get storeRequired() {
    return this.selectedRoles.length && this.selectedRoles.some(r => r.role != Role.Admin);
  }

  initEmptyUser() {
    this.newUser = {
      firstName: undefined,
      lastName: undefined,
      email: undefined,
      roles: [],
    };
  }

  initSelectedRoles() {
    const roles = Object.keys(Role).map(r => Role[r]);
    this.selectedRoles = [...roles].map(r => ({ role: r, selected: false } as RoleSelect));
  }

  updateUser(user: User) {
    this.enterpriseService.updateUser(user).subscribe(() => {
      console.log("updated");
    }, error => {
      console.error(error);
      this.enterpriseService.getUsers().subscribe(users => {
        this.users = users;
      });
    });
  }

  updateUserRole(user: User, event: any) {
    const role: Role = event.target.value;
    const selected = event.target.checked;
    if (user.roles.includes(role) && !selected) {
      user.roles = user.roles.filter(r => r !== role);
    }
    if (!user.roles.includes(role) && selected) {
      user.roles.push(role);
    }
    console.log(user);
  }

  registerUser() {
    this.enterpriseService.addUser(this.newUser).subscribe(user => {
      this.users.push(user);
      this.initEmptyUser();
      this.registerError = false;
    }, error => {
      console.error(error);
      this.registerError = true;
    });
  }
}
