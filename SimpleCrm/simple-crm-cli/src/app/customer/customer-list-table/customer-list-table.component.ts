import {
  Component,
  EventEmitter,
  Input,
  Output,
  ChangeDetectionStrategy,
} from '@angular/core';
import { Customer } from '../customer.model';

@Component({
  selector: 'crm-customer-list-table',
  templateUrl: './customer-list-table.component.html',
  styleUrls: ['./customer-list-table.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CustomerListTableComponent {
  @Input() customers!: Customer[];
  @Output() openCustomer = new EventEmitter<Customer>();

  // the column names must match the matColumnDef names in the html
  displayColumns = [
    'statusIcon',
    'name',
    'phoneNumber',
    'emailAddress',
    'status',
    'lastContactDate',
    'openDetailButton',
  ];

  constructor() {}

  openDetailClick(customer: Customer) {
    this.openCustomer.emit(customer);
  }

  trackByUserId(index: number, item: Customer) {
    return item.customerId; // some unique value on the array item
  }
}
