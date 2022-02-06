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
