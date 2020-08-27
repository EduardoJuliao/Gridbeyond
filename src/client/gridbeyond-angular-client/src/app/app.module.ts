import { MarketDataComponentNotifierService } from './Shared/Services/market-data-component-notifier.service';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { ChartModule } from 'angular2-chartjs';


import { AppComponent } from './app.component';
import { MarketDataService } from './Shared/Services/market-data.service';
import { MarketDataComponent } from './Core/Components/market-data/market-data.component';
import { DatePipe } from '@angular/common';
import { HeaderComponent } from './Core/Components/header/header.component';
import { FileUploadComponent } from './Core/Components/file-upload/file-upload.component';
import { Ng2IziToastModule } from 'ng2-izitoast';
import { LatestRecordsComponent } from './Core/Components/latest-records/latest-records.component';
import { LoaderComponent } from './Shared/Components/loader/loader.component';
import { ReportDataComponent } from './Core/Components/report-data/report-data.component';

@NgModule({
  declarations: [
    AppComponent,
    MarketDataComponent,
    HeaderComponent,
    FileUploadComponent,
    LatestRecordsComponent,
    LoaderComponent,
    ReportDataComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    ChartModule,
    Ng2IziToastModule
  ],
  providers: [MarketDataService,
    MarketDataComponentNotifierService,
    DatePipe],
  bootstrap: [AppComponent]
})
export class AppModule { }
