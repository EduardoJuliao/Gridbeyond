import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LatestRecordsComponent } from './latest-records.component';

describe('LatestRecordsComponent', () => {
  let component: LatestRecordsComponent;
  let fixture: ComponentFixture<LatestRecordsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LatestRecordsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LatestRecordsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
