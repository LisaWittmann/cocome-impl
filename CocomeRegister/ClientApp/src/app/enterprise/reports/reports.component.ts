import { Component, OnInit } from '@angular/core';
import * as Chart from 'chart.js';
import { interpolateColors, toRGBA } from 'src/services/ColorGenerator';
import { Statistic } from 'src/models/Transfer';
import { monthValues } from 'src/services/Month';
import { EnterpriseStateService } from '../enterprise.service';

@Component({
  selector: 'app-enterprise-reports',
  templateUrl: './reports.component.html',
})
export class EnterpriseReportsComponent implements OnInit {
  deliveryStatistic: Statistic[];
  deliveryChart: Chart;

  salesStatistic: Statistic[];
  salesChart: Chart;

  constructor(private enterpriseService: EnterpriseStateService) {
    this.enterpriseService.getDeliveryStatistic().subscribe(statistic => {
      this.deliveryStatistic = statistic;
      this.initDeliveryChart();
    });
    this.enterpriseService.getProfitStatistic().subscribe(statistic => {
      this.salesStatistic = statistic;
      this.initSalesChart();
    });
  }

  getAverage(statistic: Statistic) {
    if (statistic.dataset.length > 1) {
      return statistic.dataset.reduce((x, y) => x + y) / statistic.dataset.length;
    } else if (statistic.dataset.length === 0) {
      return 0;
    }
    return statistic.dataset[0];
  }

  getSum(statistic: Statistic) {
    if (statistic.dataset.length === 0) {
      return 0;
    } else {
      return statistic.dataset.reduce((x, y) => x + y);
    }
  }

  get deliveryDataset() {
    const chartColors = interpolateColors(this.deliveryStatistic.length, { colorStart: 0.6, colorEnd: 0.8 });
    return this.deliveryStatistic.map(data => ({
      label: data.label,
      data: data.dataset,
      borderColor: chartColors[this.deliveryStatistic.indexOf(data)],
      backgroundColor: toRGBA(chartColors[this.deliveryStatistic.indexOf(data)], 0.5),
      fill: false,
    }));
  }

  get salesDataset() {
    const chartColors = interpolateColors(this.salesStatistic.length, { colorStart: 0.4, colorEnd: 0.8 });
    return this.salesStatistic.map(data => ({
      label: data.label,
      data: data.dataset,
      borderColor: chartColors[this.salesStatistic.indexOf(data)],
      backgroundColor: toRGBA(chartColors[this.salesStatistic.indexOf(data)], 0.4),
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
    if (this.deliveryStatistic && !this.deliveryChart) {
      this.initDeliveryChart();
    }
    if (this.salesStatistic && !this.salesChart) {
      this.initSalesChart();
    }
  }
}
