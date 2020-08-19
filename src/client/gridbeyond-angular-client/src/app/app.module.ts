import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { ChartModule } from 'angular2-chartjs';


import { AppComponent } from './app.component';
import { MarketDataService } from './Shared/Services/market-data.service';
import { MarketDataComponent } from './Core/Components/market-data/market-data.component';
import { DatePipe } from '@angular/common';

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
  providers: [MarketDataService,
    DatePipe],
  bootstrap: [AppComponent]
})
export class AppModule { }
