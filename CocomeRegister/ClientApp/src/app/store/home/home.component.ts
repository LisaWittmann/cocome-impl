import { HttpClient } from '@angular/common/http';
import { Component, Inject } from '@angular/core';
import { Router } from '@angular/router';
import { Store } from 'src/services/Models';
import { StoreStateService } from '../store.service';

@Component({
  selector: 'app-store',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class StoreHomeComponent {
  stores: Store[];
  selectedStore: Store;

  constructor(
    private http: HttpClient,
    private router: Router,
    private storeStateService: StoreStateService,
    @Inject('BASE_URL') baseUrl: string
  ) {
    this.http.get<Store[]>(`${baseUrl}api/store`).subscribe(result => {
      this.stores = result;
      console.log(this.stores);
    }, error => console.error(error));
  }

  selectStore() {
    this.storeStateService.setStore(this.selectedStore);
    this.router.navigate(['/filiale/dashboard']);
  }
}
