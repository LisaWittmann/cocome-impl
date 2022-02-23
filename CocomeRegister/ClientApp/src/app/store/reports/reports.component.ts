import { Component, OnInit } from '@angular/core';
import { Chart } from 'chart.js';
import { interpolateColors, toRGBA } from 'src/services/ColorGenerator';
import { Month, monthOrdinals, monthValues } from 'src/services/Month';
import { StoreStateService } from '../store.service';
import { StockItem, Store } from 'src/models/Store';
import { Report } from 'src/models/Transfer';

@Component({
  selector: 'app-store-reports',
  templateUrl: './reports.component.html',
})
export class StoreReportsComponent implements OnInit {
  store: Store;
  sales: Map<number, Map<Month, number>>;
  inventory: StockItem[];

  salesChart: Chart;
  inventoryChart: Chart;

  salesData: Report[];
  salesLegend = monthOrdinals;

  constructor(private storeStateService: StoreStateService) {
    this.storeStateService.store$.subscribe(store => {
      this.store = store;
    });
    this.storeStateService.inventory$.subscribe(inventory => {
      this.inventory = inventory;
    });
  }

  get date() {
    return new Date(Date.now()).toLocaleDateString('de-DE');
  }

  get salesDataset() {
    const chartColors = interpolateColors(this.salesData.length, { colorStart: 0.6, colorEnd: 0.8 });
    return this.salesData.map(data => ({
      label: data.label,
      data: data.dataset,
      borderColor: chartColors[this.salesData.indexOf(data)],
      backgroundColor: toRGBA(chartColors[this.salesData.indexOf(data)], 0.5)
    }));
  }

  initSalesChart() {
    const canvas = document.getElementById('salesChart') as HTMLCanvasElement;
    if (canvas) {
      this.salesChart = new Chart(canvas.getContext('2d'), {
        type: 'line',
        data: {
          datasets: this.salesDataset,
          labels: monthValues,
        },
        options: { responsive: true }
      });
    }
  }

  initInventoryChart() {
    const chartColors = interpolateColors(this.inventory.length, { colorStart: 0.2, colorEnd: 1 });
    const canvas = document.getElementById('inventoryChart') as HTMLCanvasElement;
    if (canvas) {
      this.inventoryChart = new Chart(canvas.getContext('2d'), {
          type: 'pie',
          data: {
            labels: this.inventory.map(item => item.product.name),
            datasets: [{
              backgroundColor: chartColors,
              data: this.inventory.map(item => item.stock)
            }]
          },
          options: { responsive: true }
      });
    }
  }

  ngOnInit() {
    this.storeStateService.getProfits().subscribe(profits => {
      this.salesData = profits;
      this.initSalesChart();
      this.initInventoryChart();
    });
  }
}
