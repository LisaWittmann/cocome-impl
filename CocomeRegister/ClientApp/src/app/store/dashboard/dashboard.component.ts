import { AfterViewInit, Component } from '@angular/core';
import { Chart } from 'chart.js';
import { monthValues } from 'src/services/Month';
import { StockItem, Store, Order, Statistic } from 'src/services/Models';
import { StoreStateService } from '../store.service';
@Component({
  selector: 'app-store-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
})
export class StoreDashboardComponent implements AfterViewInit {
  store: Store;
  runningOutOfStock: StockItem[];
  orders: Order[];

  chart: HTMLCanvasElement;
  salesChart: Chart;
  statistic: Statistic;

  constructor(
    private storeStateService: StoreStateService) {
    this.storeStateService.store$.subscribe(store => {
      this.store = store;
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
    this.salesChart = new Chart(this.chart.getContext('2d'), {
      type: 'line',
      data: {
        datasets: [{
          label: this.statistic.label,
          data: this.statistic.dataset
        }],
        labels: monthValues,
      },
      options: { responsive: true }
    });
  }

  ngAfterViewInit() {
    this.storeStateService.getLatestProfits().subscribe(profits => {
      this.statistic = profits;
      this.initChart();
    });
  }
}
