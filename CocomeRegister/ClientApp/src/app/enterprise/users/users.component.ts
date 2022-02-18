import { HttpClient } from '@angular/common/http';
import { Component, Inject } from '@angular/core';
import { User } from 'src/services/Models';
import { EnterpriseStateService } from '../enterprise.service';

@Component({
  selector: 'app-enterprise-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.scss'],
})
export class EnterpriseUsersComponent {
  users: User[];
  newUser = {} as User;
  newUserRole = {} as string;
  newUserPassword = {} as string;

  constructor(private enterpriseService: EnterpriseStateService, private http: HttpClient, @Inject("BASE_URL") baseUrl: string) {
    this.http.get<User[]>(`${baseUrl}api/user`).subscribe(users => { this.users = users });
    this.enterpriseService.users$.subscribe(users => {
      this.users = users;
    });
  }

  saveRoleChanges(user: User, role: string) {
    this.enterpriseService.updateUserRole(user, role);
  }

  submitUser() {
    this.enterpriseService.addUser(this.newUser, this.newUserRole, this.newUserPassword);
    this.newUser = {} as User;
  }
  
}
