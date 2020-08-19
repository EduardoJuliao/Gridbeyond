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

  private file: any;
  chartData = {};
  reportData = {};
  readonly dateFormat: string = 'dd/MM/yyyy hh:mm:ss';

  options = {
    responsive: true,
    maintainAspectRatio: false
  };
  constructor(private service: MarketDataService,
    private datePipe: DatePipe) {

  }

  ngOnInit() {
    this.populate();
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

  fileChanged(e) {
    this.file = e.target.files[0];
  }

  sendFile() {
    let fileReader = new FileReader();
    fileReader.onload = (e) => {
      let lines = fileReader.result.toString().split('\r\n');
      this.service.InsertRecords(lines)
        .subscribe((result) => {
          console.log(result);
          this.populate();
        });
    }
    fileReader.readAsText(this.file);
  }
}
