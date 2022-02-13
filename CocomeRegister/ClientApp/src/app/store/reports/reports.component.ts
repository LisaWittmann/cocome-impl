import { Component, OnInit } from '@angular/core';
import { Chart } from 'chart.js';
import { jsPDF } from 'jspdf';
import { interpolateColors, toRGBA } from 'src/services/ColorGenerator';
import { Month, monthOrdinals, monthValues } from 'src/services/Month';
import { Store, StockItem, Statistic } from 'src/services/Models';
import { StoreStateService } from '../store.service';
import html2canvas from 'html2canvas';


@Component({
  selector: 'app-store-reports',
  templateUrl: './reports.component.html',
  styleUrls: ['./reports.component.scss'],
})
export class StoreReportsComponent implements OnInit {
  store: Store;
  sales: Map<number, Map<Month, number>>;
  inventory: StockItem[];

  salesChart: Chart;
  inventoryChart: Chart;

  salesData: Statistic[];
  salesLegend = monthOrdinals;

  constructor(private storeStateService: StoreStateService) {
    this.storeStateService.store$.subscribe(store => {
      this.store = store;
    });
    this.storeStateService.inventory$.subscribe(inventory => {
      this.inventory = inventory;
    });
    this.storeStateService.getProfits().subscribe(profits => {
      this.salesData = profits;
      this.initSalesChart();
      this.initInventoryChart();
    })
  }

  get date() {
    return new Date(Date.now()).toLocaleDateString('de-DE');
  }

  get salesDataset() {
    const colorRange = { colorStart: 0.6, colorEnd: 0.8 };
    const chartColors = interpolateColors(this.salesData.length, colorRange);
    return this.salesData.map(data => ({
      label: `${data.key}`,
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
    if (this.salesData && !this.salesChart) {
      this.initSalesChart();
    }
    if (this.inventory && !this.inventoryChart) {
      this.initInventoryChart();
    }
  }

  private generatePDFSection(elementId: string, pdf: jsPDF) {
    const section = document.getElementById(elementId);
    const pageHeight = 295;
    const pageWidth = 210;
    const pageMargin = 10;

    let imgData: string;
    let imgHeight: number;
    const imgWidth = pageWidth * 0.8;

    const posX = (pageWidth - imgWidth) / 2;
    let posY = pageMargin;

    return html2canvas(section).then(canvas => {
      imgData = canvas.toDataURL('image/png');
      imgHeight = canvas.height * imgWidth / canvas.width;
      let heightLeft = imgHeight;

      pdf.addImage(imgData, 'PNG', posX, posY, imgWidth, imgHeight);
      heightLeft -= pageHeight;

      while (heightLeft >= 0) {
        posY += heightLeft - imgHeight;
        pdf.addPage();
        pdf.addImage(imgData, 'PNG', posX, posY, imgWidth, imgHeight);
        heightLeft -= pageHeight;
      }
    });
  }

  async generatePDF() {
    const pdf = new jsPDF('p', 'mm', 'a4');
    await this.generatePDFSection('store-reports-sales', pdf);
    pdf.addPage();
    await this.generatePDFSection('store-reports-inventory', pdf);
    pdf.save(`Report-${this.date}.pdf`);
  }
}
