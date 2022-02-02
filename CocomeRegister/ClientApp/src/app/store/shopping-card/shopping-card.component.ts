import { ChangeDetectionStrategy, Component } from "@angular/core";

@Component({
    selector: 'store-shopping-card',
    templateUrl: './shopping-card.component.html',
    styleUrls: ['./shopping-card.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
  })
  export class StoreShoppingCardComponent {
      
  }