import { AfterViewInit, Component } from '@angular/core';
import { Chart } from 'chart.js';
import { Order } from 'src/models/Order';
import { StockItem, Store } from 'src/models/Store';
import { Report } from 'src/models/Transfer';
import { monthValues } from 'src/services/Month';

import { StoreStateService } from '../store.service';
@Component({
  selector: 'app-store-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class StoreHomeComponent implements AfterViewInit {
  store: Store;
  runningOutOfStock: StockItem[];
  orders: Order[];

  chart: HTMLCanvasElement;
  salesChart: Chart;
  report: Report;

  constructor(
    private storeStateService: StoreStateService) {
    this.storeStateService.store$.subscribe(store => {
      this.store = store;
      if (store) {
        this.storeStateService.getLatestProfits().subscribe(profits => {
          this.report = profits;
          this.initChart();
        });
      }
    });
    this.storeStateService.inventory$.subscribe(() => {
      this.runningOutOfStock = this.storeStateService.runningOutOfStock;
    });
    this.storeStateService.orders$.subscribe(orders => {
      this.orders = orders;
    });
  }

  get openOrders() {
    return this.orders.filter(order => !order.closed);
  }

  title = (order: Order) => {
    return `Bestellung ${order.id} vom
      ${new Date(order.placingDate).toLocaleDateString('de-DE')}`;
  }

  initChart() {
    this.chart = (document.getElementById('salesChart') as HTMLCanvasElement);
    if (this.chart) {
      this.salesChart = new Chart(this.chart.getContext('2d'), {
        type: 'line',
        data: {
          datasets: [{
            label: this.report.label,
            data: this.report.dataset
          }],
          labels: monthValues,
        },
        options: { responsive: true }
      });
    }
  }

  ngAfterViewInit() {
    if (this.report && !this.salesChart) {
      this.initChart();
    }
  }
}
