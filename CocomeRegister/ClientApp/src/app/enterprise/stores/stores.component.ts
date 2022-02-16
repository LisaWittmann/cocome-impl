import { Component } from '@angular/core';
import { Store } from 'src/services/Models';
import { EnterpriseStateService } from '../enterprise.service';

@Component({
  selector: 'app-enterprise-stores',
  templateUrl: './stores.component.html',
  styleUrls: ['./stores.component.scss'],
})
export class EnterpriseStoresComponent {
  stores: Store[];
  newStore = {} as Store;

  constructor(private enterpriseService: EnterpriseStateService) {
    this.enterpriseService.stores$.subscribe(stores => {
      this.stores = stores;
    });
  }

  saveChanges(store: Store) {
    this.enterpriseService.updateStore(store);
  }

  submitStore() {
    this.enterpriseService.addStore(this.newStore);
    this.newStore = {} as Store;
  }
}
