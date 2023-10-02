import { Injectable } from '@angular/core';
import { CustomerService } from './customer.service';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { Customer } from './customer.model';

@Injectable()
export class CustomerMockService extends CustomerService {
  customers: Customer[] = [];
  
  constructor(http: HttpClient) {
    super(http);
    console.warn('Warning: You are using the CustomerMockService, not intended for production use.');

    const localCustomers = localStorage.getItem('customers');
    if (localCustomers) {
       this.customers = JSON.parse(localCustomers);
    } else {
      this.customers.push({
        customerId: 1,
        firstName: 'John',
        lastName: 'Smith',
        phoneNumber: '314-555-1234',
        emailAddress: 'john@nexulacademy.com',
        statusCode: 'Prospect',
        preferredContactMethod: 'phone',
        lastContactDate: new Date().toISOString()
      });
    }
  }

  override search(term: string): Observable<Customer[]> {
    // returns array of results
    const items = this.customers.filter(x =>
      x.lastName.indexOf(term) >= 0
      || x.firstName.indexOf(term) >= 0
      || (x.firstName + ' ' + x.lastName).indexOf(term) >= 0
      || x.phoneNumber.indexOf(term) >= 0
      || x.emailAddress.indexOf(term) >= 0
      || x.statusCode.indexOf(term) >= 0);
    // convert to observable
    return of(items);
  }
  
  override get(customerId: number): Observable<Customer | undefined> {
    const item = this.customers.find(x => x.customerId === customerId);
    return of(item);
  }

  override insert(customer: Customer): Observable<Customer> {
    customer.customerId = Math.max(...this.customers.map(x => x.customerId)) + 1;
    this.customers = [...this.customers, customer];
    localStorage.setItem('customers', JSON.stringify(this.customers));
    return of(customer);
  }

  override update(customer: Customer): Observable<Customer> {
    const match = this.customers.find(x => x.customerId === customer.customerId);
    if (match) {
      this.customers = this.customers.map(x => x.customerId === customer.customerId ? customer : x);
    } else {
      this.customers = [...this.customers, customer];
    }
    localStorage.setItem('customers', JSON.stringify(this.customers));
    return of(customer);
  }

}