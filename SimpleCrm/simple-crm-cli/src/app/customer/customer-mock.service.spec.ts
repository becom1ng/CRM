import { TestBed } from '@angular/core/testing';
import { CustomerMockService } from './customer-mock.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('CustomerMockService', () => {
  let service: CustomerMockService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [CustomerMockService]
    });
    service = TestBed.inject(CustomerMockService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});