import { Store } from "./Store";

export interface User {
    firstName: string;
    lastName: string;
    userName: string;
    email: string;
    store: Store;
}
