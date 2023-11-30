import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Customer } from './customer.model';

@Injectable({
  providedIn: 'root',
})
export class CustomerService {
  constructor(private http: HttpClient) {
    console.warn('Using CustomerService. Production environments.');
  }

  search(term: string): Observable<Customer[]> {
    // TODO: Generates 404 response. Either adjust this or add API endpoint.
    return this.http.get<Customer[]>('/api/customers/search?term=' + term);
  }

  get(customerId: number): Observable<Customer | undefined> {
    return this.http.get<Customer>('/api/customers/' + customerId);
  }

  insert(customer: Customer): Observable<Customer> {
    return this.http.post<Customer>('/api/customers/save', customer);
  }

  update(customer: Customer): Observable<Customer> {
    // example url: /api/customers/5
    return this.http.put<Customer>(
      `/api/customers/${customer.customerId}`,
      customer
    );
  }
}
