import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
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
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CustomerListPageComponent implements OnInit {
  filteredCustomers$: Observable<Customer[]>;
  filterInput = new FormControl();

  // TODO: Is there a better way to implement these?
  allCustomers$ = this.store.select(selectCustomers);
  allCustomers!: Customer[];
  searchCriteria$ = this.store.select(selectCriteria);
  searchCriteria!: string;

  constructor(
    private store: Store<CustomerState>,
    private customerService: CustomerService,
    public dialog: MatDialog,
    private router: Router
  ) {
    this.searchCriteria$.subscribe(({ term }) => {
      this.searchCriteria = term;
    });
    this.allCustomers$.subscribe((x) => (this.allCustomers = x));
    this.filteredCustomers$ = this.filterInput.valueChanges.pipe(
      startWith(this.searchCriteria),
      debounceTime(700),
      tap((filterTerm: string) => {
        this.searchCustomers(filterTerm);
      }),
      switchMap((filterTerm: string) => {
        return this.customerService.search(filterTerm);
        // ! TODO: Eliminate this second service call.
        // Return Observable<Customer[]> in another way: of(item), etc.
        // this.searchCustomers(filterTerm) => return type mismatch
      })
    );
  }

  ngOnInit(): void {
    this.filterInput.setValue(this.searchCriteria);
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
