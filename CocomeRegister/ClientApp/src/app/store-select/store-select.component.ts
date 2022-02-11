import { HttpClient } from '@angular/common/http';
import { Component, Inject } from '@angular/core';
import { Router } from '@angular/router';
import { Store } from 'src/services/Store';
import { StoreStateService } from '../store/store.service';

@Component({
  selector: 'app-store-select',
  templateUrl: './store-select.component.html',
  styleUrls: ['./store-select.component.scss']
})
export class StoreSelectComponent {
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
    this.storeStateService.setStore(this.selectedStore).then(() => {
      this.router.navigate(['/filiale']);
    });
  }
}
