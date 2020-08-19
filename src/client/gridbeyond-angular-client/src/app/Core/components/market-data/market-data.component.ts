import { MarketDataComponentNotifierService } from './../../../Shared/Services/market-data-component-notifier.service';
import { IReportDataModel } from './../../../Shared/Models/IReportDataModel';
import { IMarketDataModel } from './../../../Shared/Models/IMarketDataModel';
import { MarketDataService } from './../../../Shared/Services/market-data.service';
import { Component, OnInit } from '@angular/core';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-market-data',
  templateUrl: './market-data.component.html',
  styleUrls: ['./market-data.component.scss']
})
export class MarketDataComponent implements OnInit {


  chartData = {};
  reportData = {};
  readonly dateFormat: string = 'dd/MM/yyyy hh:mm:ss';

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
        this.chartData = {
          labels: result.map(x => this.datePipe.transform(x.date, this.dateFormat)),
          datasets: [
            {
              label: "Showing latests 50",
              data: result.map(x => x.marketPriceEX1)
            }
          ]
        };
      });

    this.service.GetReportData()
      .subscribe(result => {
        this.reportData = result;
      });
  }
}
