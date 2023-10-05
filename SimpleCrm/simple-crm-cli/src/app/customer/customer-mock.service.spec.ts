import { TestBed, getTestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';

import { CustomerMockService } from './customer-mock.service';
import { of } from 'rxjs';
import { Customer } from './customer.model';

describe('CustomerMockService', () => {
  let injector: TestBed;
  let service: CustomerMockService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [ HttpClientTestingModule ],
      providers: [ CustomerMockService ]
    });
    injector = getTestBed();
    service = injector.inject(CustomerMockService);
    httpMock = injector.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify(); // ensures there are no outstanding requests between tests.
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('Get(1) should return observable of id 1', done => {
    const test$ = service.get(1);
    test$.subscribe(result => {
      expect(result?.customerId).toEqual(1);
      done();
    })
  });
});