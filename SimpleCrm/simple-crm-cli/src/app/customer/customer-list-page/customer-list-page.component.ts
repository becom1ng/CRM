import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { Customer } from '../customer.model';
import { CustomerService } from '../customer.service';
import { Observable, combineLatest } from 'rxjs';
import { debounceTime, map, startWith, switchMap, tap } from 'rxjs/operators';
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
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CustomerListPageComponent implements OnInit {
  filteredCustomers$: Observable<Customer[]>;
  filterInput = new FormControl();

  // allCustomers!: Customer[];
  // searchCriteria$ = this.store.select(selectCriteria);
  // searchCriteria!: string;
  // Store data
  allCustomers$ = this.store.select(selectCustomers);

  constructor(
    private store: Store<CustomerState>,
    private customerService: CustomerService,
    public dialog: MatDialog,
    private router: Router
  ) {
    // this.searchCriteria$.subscribe(({ term }) => {
    //   this.searchCriteria = term;
    // });
    // this.allCustomers$.subscribe((x) => (this.allCustomers = x));

    this.filteredCustomers$ = combineLatest([
      this.allCustomers$,
      this.filterInput.valueChanges.pipe(startWith(''), debounceTime(700)),
    ]).pipe(
      map(([customers, term]) => {
        return customers.filter(
          // TODO: fix case sensitivity (tolowerinvariant)
          // The term AND the array data needs to be transformed
          (x) =>
            x.lastName.indexOf(term.tolowerinvariant) >= 0 ||
            x.firstName.indexOf(term.tolowerinvariant) >= 0 ||
            (x.firstName + ' ' + x.lastName).indexOf(term.tolowerinvariant) >=
              0 ||
            x.phoneNumber.indexOf(term) >= 0 ||
            x.emailAddress.indexOf(term.tolowerinvariant) >= 0
          // ||
          // x.statusCode.indexOf(term) >= 0
        );
      })
    );
  }

  ngOnInit(): void {
    this.searchCustomers('');
  }

  searchCustomers(term: string): void {
    this.store.dispatch(searchCustomersAction({ criteria: { term: term } }));
  }

  openDetail(item: Customer): void {
    if (item) {
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
