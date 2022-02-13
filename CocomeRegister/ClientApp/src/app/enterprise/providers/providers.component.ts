import { Component } from '@angular/core';
import { Provider } from 'src/services/Models';
import { EnterpriseStateService } from '../enterprise.service';

@Component({
  selector: 'app-enterprise-providers',
  templateUrl: './providers.component.html',
  styleUrls: ['./providers.component.scss'],
})
export class EnterpriseProvidersComponent {
  providers: Provider[];

  constructor(private enterpriseService: EnterpriseStateService) {
    enterpriseService.providers$.subscribe(provider => {
      this.providers = provider;
    })
  }
}
