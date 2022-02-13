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

  constructor(private enterpriseService: EnterpriseStateService) {
    this.enterpriseService.stores$.subscribe(stores => {
      this.stores = stores;
    });
  }
}
