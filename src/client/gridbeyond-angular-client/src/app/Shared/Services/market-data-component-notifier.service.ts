import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MarketDataComponentNotifierService {

  constructor() { }

  private data = new BehaviorSubject(false);
  currentData = this.data.asObservable();

  updateMessage() {
    this.data.next(true);
  }
}
