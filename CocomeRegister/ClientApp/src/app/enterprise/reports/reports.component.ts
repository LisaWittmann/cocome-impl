import { Component, OnInit } from '@angular/core';
import * as Chart from 'chart.js';
import { interpolateColors, toRGBA } from 'src/services/ColorGenerator';
import { Report } from 'src/models/Transfer';
import { monthValues } from 'src/services/Month';
import { EnterpriseStateService } from '../enterprise.service';

@Component({
  selector: 'app-enterprise-reports',
  templateUrl: './reports.component.html',
})
export class EnterpriseReportsComponent implements OnInit {
  deliveryReport: Report[];
  deliveryChart: Chart;

  salesReport: Report[];
  salesChart: Chart;

  constructor(private enterpriseService: EnterpriseStateService) {
    this.enterpriseService.getDeliveryReport().subscribe(report => {
      this.deliveryReport = report;
      this.initDeliveryChart();
    });
    this.enterpriseService.getProfitReport().subscribe(report => {
      this.salesReport = report;
      this.initSalesChart();
    });
  }

  getAverage(report: Report) {
    if (report.dataset.length > 1) {
      return report.dataset.reduce((x, y) => x + y) / report.dataset.length;
    } else if (report.dataset.length === 0) {
      return 0;
    }
    return report.dataset[0];
  }

  getSum(report: Report) {
    if (report.dataset.length === 0) {
      return 0;
    } else {
      return report.dataset.reduce((x, y) => x + y);
    }
  }

  get deliveryDataset() {
    const chartColors = interpolateColors(this.deliveryReport.length, { colorStart: 0.6, colorEnd: 0.8 });
    return this.deliveryReport.map(data => ({
      label: data.label,
      data: data.dataset,
      borderColor: chartColors[this.deliveryReport.indexOf(data)],
      backgroundColor: toRGBA(chartColors[this.deliveryReport.indexOf(data)], 0.5),
      fill: false,
    }));
  }

  get salesDataset() {
    const chartColors = interpolateColors(this.salesReport.length, { colorStart: 0.4, colorEnd: 0.8 });
    return this.salesReport.map(data => ({
      label: data.label,
      data: data.dataset,
      borderColor: chartColors[this.salesReport.indexOf(data)],
      backgroundColor: toRGBA(chartColors[this.salesReport.indexOf(data)], 0.4),
    }));
  }

  initDeliveryChart() {
    const canvas = document.getElementById('deliveryChart') as HTMLCanvasElement;
    if (canvas) {
      this.deliveryChart = new Chart(canvas.getContext('2d'), {
        type: 'line',
        data: { datasets: this.deliveryDataset },
        options: { responsive: true }
      });
    }
  }

  initSalesChart() {
    const canvas = document.getElementById('salesChart') as HTMLCanvasElement;
    if (canvas) {
      this.salesChart = new Chart(canvas.getContext('2d'), {
        type: 'line',
        data: {
          datasets: this.salesDataset,
          labels: monthValues
        },
        options: { responsive: true }
      });
    }
  }

  ngOnInit() {
    if (this.deliveryReport && !this.deliveryChart) {
      this.initDeliveryChart();
    }
    if (this.salesReport && !this.salesChart) {
      this.initSalesChart();
    }
  }
}
