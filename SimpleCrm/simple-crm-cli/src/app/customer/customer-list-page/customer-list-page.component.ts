import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { Customer } from '../customer.model';
import { Observable, combineLatest } from 'rxjs';
import { debounceTime, map, startWith } from 'rxjs/operators';
import { MatDialog } from '@angular/material/dialog';
import { CustomerCreateDialogComponent } from '../customer-create-dialog/customer-create-dialog.component';
import { Router } from '@angular/router';
import { FormControl } from '@angular/forms';
import { Store } from '@ngrx/store';
import {
  addCustomerAction,
  searchCustomersAction,
} from '../store/customer.store';
import { selectCustomers } from '../store/customer.store.selectors';
import { CustomerState } from '../store/customer.store.model';

@Component({
  selector: 'crm-customer-list-page',
  templateUrl: './customer-list-page.component.html',
  styleUrls: ['./customer-list-page.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CustomerListPageComponent implements OnInit {
  filteredCustomers$: Observable<Customer[]>;
  filterInput = new FormControl();
  allCustomers$ = this.store.select(selectCustomers);

  constructor(
    private store: Store<CustomerState>,
    public dialog: MatDialog,
    private router: Router
  ) {
    this.filteredCustomers$ = combineLatest([
      this.allCustomers$,
      this.filterInput.valueChanges.pipe(startWith(''), debounceTime(700)),
    ]).pipe(
      map(([customers, term]) => {
        return customers.filter(
          (x) =>
            x.lastName.toLocaleLowerCase().indexOf(term.toLocaleLowerCase()) >=
              0 ||
            x.firstName.toLocaleLowerCase().indexOf(term.toLocaleLowerCase()) >=
              0 ||
            (x.firstName + ' ' + x.lastName)
              .toLocaleLowerCase()
              .indexOf(term.toLocaleLowerCase()) >= 0 ||
            x.phoneNumber.indexOf(term) >= 0 ||
            x.emailAddress
              .toLocaleLowerCase()
              .indexOf(term.toLocaleLowerCase()) >= 0
        );
      })
    );
  }

  ngOnInit(): void {
    // Load all entities.
    this.store.dispatch(searchCustomersAction({ criteria: { term: '' } }));
  }

  // ? TODO: Remove method as it is unused. Only call is ngOnInit.
  searchCustomers(term: string): void {
    this.store.dispatch(searchCustomersAction({ criteria: { term: term } }));
  }

  openDetail(item: Customer): void {
    if (item) {
      this.router.navigate([`./customers/${item.customerId}`]);
    }
  }

  /** Add/create customer using dialog data. */
  addCustomer(): void {
    const dialogRef = this.dialog.open(CustomerCreateDialogComponent, {
      width: '260px',
      data: null,
    });
    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.store.dispatch(addCustomerAction(result));
      }
    });
  }
}
