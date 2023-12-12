import { Component, OnInit } from '@angular/core';
import { Customer } from '../customer.model';
import { CustomerService } from '../customer.service';
import { Observable } from 'rxjs';
import { debounceTime, startWith, switchMap, tap } from 'rxjs/operators';
import { MatDialog } from '@angular/material/dialog';
import { CustomerCreateDialogComponent } from '../customer-create-dialog/customer-create-dialog.component';
import { Router } from '@angular/router';
import { FormControl } from '@angular/forms';
import { Store } from '@ngrx/store';
import {
  addCustomerAction,
  searchCustomersAction,
} from '../store/customer.store';
import {
  selectCriteria,
  selectCustomers,
} from '../store/customer.store.selectors';
import { CustomerState } from '../store/customer.store.model';

@Component({
  selector: 'crm-customer-list-page',
  templateUrl: './customer-list-page.component.html',
  styleUrls: ['./customer-list-page.component.scss'],
})
export class CustomerListPageComponent implements OnInit {
  filteredCustomers$: Observable<Customer[]>;
  filterInput = new FormControl();
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

  // TODO: Is there a better way to implement these?
  allCustomers$ = this.store.select(selectCustomers);
  searchCriteria!: string;

  constructor(
    private store: Store<CustomerState>,
    private customerService: CustomerService,
    public dialog: MatDialog,
    private router: Router
  ) {
    this.filteredCustomers$ = this.filterInput.valueChanges.pipe(
      startWith(''),
      debounceTime(700),
      tap((filterTerm: string) => {
        this.searchCustomers(filterTerm);
      }),
      switchMap((filterTerm: string) => {
        return this.customerService.search(filterTerm);
      })
    );
  }

  ngOnInit(): void {
    // ? TODO: Consider using term from the store so that the most recent search persists (state.criteria)
    // ? Current implementation of tap (above) with live search (startWith) overwrites the store term.

    // this.store.dispatch(searchCustomersAction({ criteria: { term: '' } }));
    this.store.select(selectCriteria).subscribe(({ term }) => {
      this.searchCriteria = term;
    });
    this.searchCustomers(this.searchCriteria);
  }

  searchCustomers(term: string): void {
    this.store.dispatch(searchCustomersAction({ criteria: { term: term } }));
  }

  openDetail(item: Customer): void {
    if (item) {
      // ? TODO: Use selector for new detail display instead of router.navigate?
      // var selectedCust$ = this.store.select(
      //   selectCustomerById(item.customerId.toString())
      // );
      this.router.navigate([`./customers/${item.customerId}`]);
    }
  }

  addCustomer(): void {
    const dialogRef = this.dialog.open(CustomerCreateDialogComponent, {
      width: '260px',
      data: null,
    });
    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        // Add/create customer using dialog data
        this.store.dispatch(addCustomerAction(result));
      }
    });
  }
}
