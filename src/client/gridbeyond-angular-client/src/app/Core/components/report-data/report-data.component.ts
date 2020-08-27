import { IReportDataModel } from './../../../Shared/Models/IReportDataModel';
import { Component, OnInit } from '@angular/core';
import { MarketDataService } from 'src/app/Shared/Services/market-data.service';
import { MarketDataComponentNotifierService } from 'src/app/Shared/Services/market-data-component-notifier.service';
import config from '../../../../assets/config.json';

@Component({
  selector: 'app-report-data',
  templateUrl: './report-data.component.html',
  styleUrls: ['./report-data.component.scss']
})
export class ReportDataComponent implements OnInit {

  isLoading: boolean = false;
  reportData: IReportDataModel;
  readonly dateFormat: string = config.config.dateTimeFormat;

  constructor(private service: MarketDataService,
    private notifier: MarketDataComponentNotifierService) { }

  ngOnInit() {
    this.populate();
    this.notifier.currentData.subscribe(() => {
      this.populate();
    })
  }

  populate(): void {
    this.isLoading = true;
    console.log('received')
    this.service.GetReportData()
      .subscribe(result => {
        console.log(result);
        this.reportData = result;
        this.isLoading = false;
      });
  }

}
