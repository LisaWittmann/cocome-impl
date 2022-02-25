import { EventEmitter, OnInit } from "@angular/core";
import { Component, Input, Output } from "@angular/core";
import { Store } from "src/models/Store";
import { Role, RoleSelect, User } from "src/models/User";

@Component({
    selector: 'app-enterprise-user-detail',
    templateUrl: './user-detail.component.html',
    styleUrls: ['./user-detail.component.scss'],
  })
  export class EnterpriseUserDetailComponent implements OnInit {
    @Input() user: User;
    @Input() stores: Store;
    @Output() updateUserEvent = new EventEmitter<User>();
    @Output() updateRoleEvent = new EventEmitter<any>()
    roles: RoleSelect[] = [];

    ngOnInit(): void {
        this.roles = Object.keys(Role)
        .map(r => ({
            role: Role[r],
            selected: this.user.roles.includes(Role[r])
        }) as RoleSelect);
        console.log(this.roles)
    }

    get fullName() {
        return `${this.user.firstName} ${this.user.lastName}`;
    }

    updateRole(event: any) {
        this.updateRoleEvent.emit(event);
    }

    saveChanges() {
        console.log("emit");
        this.updateUserEvent.emit(this.user);
    }
  }