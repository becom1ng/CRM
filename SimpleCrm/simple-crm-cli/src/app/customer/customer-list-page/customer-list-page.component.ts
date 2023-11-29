import { Component, OnInit } from '@angular/core';
import { Customer } from '../customer.model';
import { CustomerService } from '../customer.service';
import { Observable } from 'rxjs';
import { debounceTime, startWith, switchMap } from 'rxjs/operators';
import { MatDialog } from '@angular/material/dialog';
import { CustomerCreateDialogComponent } from '../customer-create-dialog/customer-create-dialog.component';
import { Router } from '@angular/router';
import { FormControl } from '@angular/forms';

@Component({
  selector: 'crm-customer-list-page',
  templateUrl: './customer-list-page.component.html',
  styleUrls: ['./customer-list-page.component.scss'],
})
export class CustomerListPageComponent implements OnInit {
  filteredCustomers$: Observable<Customer[]>;
  // the column names must match the matColumnDef names in the html
  displayColumns = [
    'icon',
    'name',
    'phoneNumber',
    'emailAddress',
    'status',
    'lastContactDate',
    'details',
  ];
  filterInput = new FormControl();

  constructor(
    private customerService: CustomerService,
    public dialog: MatDialog,
    private router: Router
  ) {
    this.filteredCustomers$ = this.filterInput.valueChanges.pipe(
      startWith(''),
      debounceTime(700),
      switchMap((filterTerm: string) => {
        return this.customerService.search(filterTerm);
      })
    );
  }

  ngOnInit(): void {}

  openDetail(item: Customer): void {
    if (item) {
      this.router.navigate([`./customer/${item.customerId}`]);
    }
  }

  addCustomer(): void {
    const dialogRef = this.dialog.open(CustomerCreateDialogComponent, {
      width: '260px',
      data: null,
    });
  }
}
