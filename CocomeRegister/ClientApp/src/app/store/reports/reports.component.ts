import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { Chart } from 'chart.js';
import { ColorRange, interpolateColors } from 'src/services/ColorGenerator';
import { Month } from 'src/services/Month';
import { Product } from 'src/services/Product';
import { StoreStateService } from '../store.service';

@Component({
  selector: 'app-store-reports',
  templateUrl: './reports.component.html',
  styleUrls: ['./reports.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class StoreReportsComponent implements OnInit {
  
  sales: Map<number, Map<Month, number>>;
  inventory: Map<Product, number>;

  salesCanvas: HTMLCanvasElement;
  salesChart: Chart;

  inventoryCanvas: HTMLCanvasElement;
  inventoryChart: Chart;

  salesData = this.storeStateService.salesDataset;
  salesLegend = Object.keys(Month)
                .filter(m => isNaN(Number(m)))
                .map(m => m.charAt(0).toUpperCase() + m.slice(1).toLowerCase());

  constructor(private storeStateService: StoreStateService) {
    this.storeStateService.sales$.subscribe(sales => {
        this.sales = sales;
    });
    this.storeStateService.inventory$.subscribe(inventory => {
        this.inventory = inventory;
    });
  }

  get date() {
    return new Date(Date.now()).toLocaleDateString('de-DE');
  }

  ngOnInit() {
    this.salesCanvas = (document.getElementById('salesChart') as HTMLCanvasElement);
    this.salesChart = new Chart(this.salesCanvas.getContext('2d'), {
      type: 'line',
      data: {
        datasets: this.salesData,
        labels: this.salesLegend,
      },
      options: { responsive: true }
    });

    const colorRange: ColorRange = { colorStart: 0.2, colorEnd: 1 };
    const pieColors = interpolateColors(this.inventory.size, colorRange);
    this.inventoryCanvas = (document.getElementById('inventoryChart') as HTMLCanvasElement);
    this.inventoryChart = new Chart(this.inventoryCanvas.getContext('2d'), {
        type: 'pie',
        data: {
            labels: [...this.inventory.keys()].map(p => p.name),
            datasets: [
                {
                    backgroundColor: pieColors,
                    data: [...this.inventory.values()] 
                }
            ]
        },
        options: {
            responsive: true,
            plugins: {
                legend: {
                    position: 'bottom',
                    align: 'start',
                }
            } 
        }
    })
  }


}