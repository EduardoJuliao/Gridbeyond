import { MarketDataComponentNotifierService } from './../../../Shared/Services/market-data-component-notifier.service';
import { MarketDataService } from './../../../Shared/Services/market-data.service';
import { Component, OnInit } from '@angular/core';
import { DatePipe } from '@angular/common';
import config from '../../../../assets/config.json';
import * as CanvasJS from '../../../../assets/canvasjs/canvasjs.min.js';

@Component({
  selector: 'app-market-data',
  templateUrl: './market-data.component.html',
  styleUrls: ['./market-data.component.scss']
})
export class MarketDataComponent implements OnInit {

  chartData = {};
  readonly dateFormat: string = config.config.dateTimeFormat;

  options = {
    responsive: true,
    maintainAspectRatio: false
  };
  constructor(private service: MarketDataService,
    private datePipe: DatePipe,
    private notifier: MarketDataComponentNotifierService) {

  }

  ngOnInit() {
    this.populate();
    this.notifier.currentData.subscribe(() => {
      this.populate();
    });
  }

  populate() {
    this.service.GetLatestData()
      .subscribe(result => {
        var data: {}[] = result.map(r => ({
          x: new Date(r.date),
          y: r.marketPriceEX1
        }));

        let chart = new CanvasJS.Chart("chartContainer", {
          zoomEnabled: true,
          animationEnabled: true,
          exportEnabled: true,
          title: {
            text: `Latest data from ${this.datePipe.transform(result[0].date, this.dateFormat)}`
          },
          data: [{
            type: "line",
            dataPoints: data,
          }]
        });

        chart.render();
      });
  }
}
