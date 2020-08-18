import { IMarketDataModel } from './../../../Shared/Models/IMarketDataModel';
import { MarketDataService } from './../../../Shared/Services/market-data.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-market-data',
  templateUrl: './market-data.component.html',
  styleUrls: ['./market-data.component.scss']
})
export class MarketDataComponent implements OnInit {

  chartData = {};
  options = {
    responsive: true,
    maintainAspectRatio: false
  };
  constructor(private service: MarketDataService) { }

  ngOnInit() {
    this.service.GetAllData()
      .subscribe(result => {
        console.log(result)
        this.chartData = {
          //labels: ["January", "February", "March", "April", "May", "June", "July"],
          datasets: [
            {
              label: "My First dataset",
              data: result.map(x => x.marketValueEX1)
            }
          ]
        };
      });
  }
}
