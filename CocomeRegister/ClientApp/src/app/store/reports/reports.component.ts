import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { Chart } from 'chart.js';
import { jsPDF } from 'jspdf';
import { ColorRange, interpolateColors } from 'src/services/ColorGenerator';
import { Month, monthOrdinals, monthValues } from 'src/services/Month';
import { Product } from 'src/services/Product';
import { StoreStateService } from '../store.service';
import html2canvas from 'html2canvas';

@Component({
  selector: 'app-store-reports',
  templateUrl: './reports.component.html',
  styleUrls: ['./reports.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class StoreReportsComponent implements OnInit {

  sales: Map<number, Map<Month, number>>;
  inventory: Map<Product, number>;

  salesChart: Chart;
  inventoryChart: Chart;

  salesData = this.storeStateService.salesDataset;
  salesLegend = monthOrdinals;

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
    const salesContext = (document.getElementById('salesChart') as HTMLCanvasElement).getContext('2d');
    this.salesChart = new Chart(salesContext, {
      type: 'line',
      data: {
        datasets: this.salesData,
        labels: monthValues,
      },
      options: { responsive: true }
    });

    const colorRange: ColorRange = { colorStart: 0.2, colorEnd: 1 };
    const chartColors = interpolateColors(this.inventory.size, colorRange);
    const inventoryContext = (document.getElementById('inventoryChart') as HTMLCanvasElement).getContext('2d');
    this.inventoryChart = new Chart(inventoryContext, {
        type: 'pie',
        data: {
            labels: [...this.inventory.keys()].map(p => p.name),
            datasets: [{
                backgroundColor: chartColors,
                data: [...this.inventory.values()]
            }]
        },
        options: { responsive: true }
    });
  }

  generatePDF() {
    const saleReport = document.getElementById('store-reports-sales');
    const inventoryReport = document.getElementById('store-reports-inventory');
    const pdf = new jsPDF('p', 'pt', 'a4');
    const fileWidth = 500;
    let fileHeight = fileWidth;
    const posY = 20;
    const posX = fileWidth / 10;

    html2canvas(saleReport).then((page1Canvas) => {
        const page1Image = page1Canvas.toDataURL('image/png');
        fileHeight = page1Canvas.height * fileWidth / page1Canvas.width;
        pdf.addImage(page1Image, 'JPEG', posX, posY, fileWidth, fileHeight);

        html2canvas(inventoryReport).then((page2Canvas) => {
            const page2Image = page2Canvas.toDataURL('image/png');
            fileHeight = page2Canvas.height * fileWidth / page2Canvas.width;
            pdf.addPage('a4', 'p');
            pdf.addImage(page2Image, 'JPEG', posX, posY, fileWidth, fileHeight);
            pdf.save(`Report-${this.date}.pdf`);
        });
    });
  }
}
