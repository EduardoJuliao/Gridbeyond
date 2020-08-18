import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { ChartModule } from 'angular2-chartjs';


import { AppComponent } from './app.component';
import { MarketDataService } from './Shared/Services/market-data.service';
import { MarketDataComponent } from './core/components/market-data/market-data.component';

@NgModule({
  declarations: [
    AppComponent,
    MarketDataComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    ChartModule
  ],
  providers: [MarketDataService],
  bootstrap: [AppComponent]
})
export class AppModule { }
