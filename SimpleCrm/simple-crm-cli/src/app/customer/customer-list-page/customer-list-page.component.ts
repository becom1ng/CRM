import { Component, OnInit } from '@angular/core';
import { Customer } from '../customer.model';
import { CustomerService } from '../customer.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'crm-customer-list-page',
  templateUrl: './customer-list-page.component.html',
  styleUrls: ['./customer-list-page.component.scss']
})
export class CustomerListPageComponent implements OnInit {
  customers$: Observable<Customer[]>;
  displayColumns = ['name', 'phoneNumber', 'emailAddress', 'status'];
  // the above column names must match the matColumnDef names in the html
  
  constructor(private customerService: CustomerService) {
    this.customers$ = this.customerService.search('');
  }

  ngOnInit(): void {}
}