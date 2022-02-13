import { Component, OnInit } from '@angular/core';
import { Chart } from 'chart.js';
import { monthValues } from 'src/services/Month';
import { StockItem, Store, Order } from 'src/services/Models';
import { StoreStateService } from '../store.service';

@Component({
  selector: 'app-store-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
})
export class StoreDashboardComponent implements OnInit {
  store: Store;
  runningOutOfStock: StockItem[];
  orders: Order[];
  chart: HTMLCanvasElement;
  salesChart: Chart;

  constructor(private storeStateService: StoreStateService) {
    this.storeStateService.store$.subscribe(store => {
      console.log("setting store", store);
      this.store = store;
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
            ${new Date(order.placingDate).toLocaleDateString('de-DE')}`;
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
