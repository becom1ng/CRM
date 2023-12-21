import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Customer } from '../customer.model';

@Component({
  selector: 'crm-customer-list-table',
  templateUrl: './customer-list-table.component.html',
  styleUrls: ['./customer-list-table.component.scss'],
})
export class CustomerListTableComponent {
  @Input() customers!: Customer[] | null;
  @Output() openCustomer = new EventEmitter<Customer>();

  constructor() {}

  openDetailClick(customer: Customer) {
    this.openCustomer.emit(customer);
  }
}
