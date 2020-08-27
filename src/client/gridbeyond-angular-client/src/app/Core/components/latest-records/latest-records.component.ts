import { MarketDataService } from 'src/app/Shared/Services/market-data.service';
import { Component, OnInit } from '@angular/core';
import { IMarketDataModel } from 'src/app/Shared/Models/IMarketDataModel';
import config from '../../../../assets/config.json';
import { MarketDataComponentNotifierService } from 'src/app/Shared/Services/market-data-component-notifier.service';

@Component({
  selector: 'app-latest-records',
  templateUrl: './latest-records.component.html',
  styleUrls: ['./latest-records.component.scss']
})
export class LatestRecordsComponent implements OnInit {

  readonly dateFormat: string = config.config.dateTimeFormat;

  isLoading: boolean = true;
  latest: IMarketDataModel[] = [];
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
    this.service.GetLatestData()
      .subscribe((latest: IMarketDataModel[]) => {
        this.latest = latest;
        this.isLoading = false;
      });
  }

}
