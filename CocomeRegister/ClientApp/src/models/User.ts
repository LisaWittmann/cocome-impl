import { Store } from "./Store";

export interface User {
    firstName: string;
    lastName: string;
    email: string;
    roles: Role[];
    store?: Store;
    password?: string;
}

export enum Role {
    Admin = "Admisitrator",
    Manager = "Filialleiter",
    Cashier = "Kassierer",
}

export interface RoleSelect {
    role: Role,
    selected: boolean;
};
