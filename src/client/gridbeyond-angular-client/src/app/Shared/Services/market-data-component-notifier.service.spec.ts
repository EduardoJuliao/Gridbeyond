import { TestBed } from '@angular/core/testing';

import { MarketDataComponentNotifierService } from './market-data-component-notifier.service';

describe('MarketDataComponentNotifierService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: MarketDataComponentNotifierService = TestBed.get(MarketDataComponentNotifierService);
    expect(service).toBeTruthy();
  });
});
