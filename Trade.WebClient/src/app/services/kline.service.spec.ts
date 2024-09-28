import { TestBed } from '@angular/core/testing';

import { KlineService } from './kline.service';

describe('KlineService', () => {
  let service: KlineService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(KlineService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
