import { Component } from '@angular/core';
import { CashDeskStateService } from '../cashdesk.service';


@Component({
  selector: 'app-cashdesk-display',
  templateUrl: './display.component.html',
  styleUrls: ['./display.component.scss'],
})
export class CashDeskDisplayComponent {
    expressMode: boolean;

    constructor(private cashdeskService: CashDeskStateService) {
        this.cashdeskService.expressMode$.subscribe(express => {
            this.expressMode = express;
            console.log(express);
        });
    }
}
