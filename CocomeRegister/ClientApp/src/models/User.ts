import { Store } from "./Store";

export interface User {
    firstName: string;
    lastName: string;
    userName: string;
    email: string;
    store: Store;
    roles: Role[];
}

export enum Role {
    Admin = "Admisitrator",
    Manager = "Filialleiter",
    Cashier = "Kassierer",
}
