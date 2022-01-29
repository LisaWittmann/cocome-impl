import { ChangeDetectionStrategy, Component } from "@angular/core";

@Component({
    selector: 'app-store-shopping-card',
    templateUrl: './shopping-card.component.html',
    styleUrls: ['./shopping-card.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
  })
  export class StoreShoppingCardComponent {
      
  }