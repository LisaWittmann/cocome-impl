import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { Chart } from 'chart.js';
import { Outlet } from 'src/services/Outlet';
import { monthValues } from 'src/services/Month';
import { Order } from 'src/services/Order';
import { Product } from 'src/services/Product';
import { StoreStateService } from '../store.service';

@Component({
  selector: 'app-store-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class StoreHomeComponent implements OnInit {
  outlet: Outlet;
  runningOutOfStock: Product[];
  orders: Order[];
  chart: HTMLCanvasElement;
  salesChart: Chart;

  constructor(private storeStateService: StoreStateService) {
    this.storeStateService.outlet$.subscribe(outlet => {
      this.outlet = outlet;
    });
    this.storeStateService.inventory$.subscribe(() => {
      this.runningOutOfStock = this.storeStateService.runningOutOfStock;
    });
    this.storeStateService.orders$.subscribe(orders => {
      this.orders = orders;
    });
  }

  get pendingOrders() {
    return this.orders.filter(order => order.delivered && !order.closed);
  }

  title = (order: Order) => {
    return `Bestellung ${order.id} vom
            ${order.placingDate.toLocaleDateString('de-DE')}`;
  }

  ngOnInit() {
    this.chart = (document.getElementById('salesChart') as HTMLCanvasElement);
    this.salesChart = new Chart(this.chart.getContext('2d'), {
      type: 'line',
      data: {
        datasets: this.storeStateService.salesDataset,
        labels: monthValues,
      },
      options: { responsive: true }
    });
  }
}
