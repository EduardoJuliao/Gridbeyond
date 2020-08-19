import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IReportDataModel } from '../Models/IReportDataModel';
// import { IInsertedRecordsModel } from '../Models/IInsertedRecordsModel';
import { IMarketDataModel } from '../Models/IMarketDataModel';

import clientConfig from '../../../assets/config.json';
import { IInsertedRecordsModel } from '../Models/IInsertedRecordsModel';

@Injectable({
  providedIn: 'root'
})
export class MarketDataService {
  constructor(private client: HttpClient) {
  }

  public GetReportData(): Observable<IReportDataModel> {
    return this.client.get<IReportDataModel>(`${clientConfig.api.baseUrl}/marketdata/report`);
  }

  public GetAllData(): Observable<IMarketDataModel[]> {
    return this.client.get<IMarketDataModel[]>(`${clientConfig.api.baseUrl}/marketdata`);
  }

  public GetLatestData(): Observable<IMarketDataModel[]> {
    return this.client.get<IMarketDataModel[]>(`${clientConfig.api.baseUrl}/marketdata/latest`);
  }

  public InsertRecords(records: string[]): Observable<IInsertedRecordsModel> {
    return this.client.post<IInsertedRecordsModel>(`${clientConfig.api.baseUrl}/marketdata`, records);
  }
}
