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
    // TODO: Generates 404 as /search is not used in API. Either adjust this or add API endpoint.
    // return this.http.get<Customer[]>('/api/customers/search?term=' + term);

    // ? TODO: implement CustomerListParameters to match Api
    if (term !== '') {
      term = '?Term=' + term;
    }
    return this.http.get<Customer[]>('/api/customers' + term);
  }

  get(customerId: number): Observable<Customer | undefined> {
    return this.http.get<Customer>('/api/customers/' + customerId);
  }

  insert(customer: Customer): Observable<Customer> {
    // ! TODO: Fix resulting 415 (Unsupported Media Type)
    return this.http.post<Customer>('/api/customers/', customer);
  }

  update(customer: Customer): Observable<Customer> {
    // ! TODO: Fix resulting 422 (see API ifMatch check)
    return this.http.put<Customer>(
      `/api/customers/${customer.customerId}`,
      customer
    );
  }
}
