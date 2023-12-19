import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { CustomerService } from '../customer.service';
import { switchMap, map } from 'rxjs';
import {
  addCustomerAction,
  addCustomerCompleteAction,
  searchCustomersAction,
  searchCustomersCompleteAction,
  updateCustomerAction,
  updateCustomerCompleteAction,
} from './customer.store';
import { Customer } from '../customer.model';

@Injectable()
export class CustomerStoreEffects {
  constructor(
    private actions$: Actions, // <-- this event stream is where to listen for dispatched actions
    private custSvc: CustomerService // <-- this is your service to be called for some actions
  ) {}

  // TODO: Combine effects?
  searchCustomers$ = createEffect(() =>
    this.actions$.pipe(
      ofType(searchCustomersAction),
      // use rxjs, accept action payload
      switchMap(({ criteria }) =>
        // make service call
        this.custSvc.search(criteria.term).pipe(
          // create action payload with API response data
          map((data) => searchCustomersCompleteAction({ result: data }))
        )
      )
    )
  );

  addCustomer$ = createEffect(() =>
    this.actions$.pipe(
      ofType(addCustomerAction),
      switchMap(({ item }) =>
        this.custSvc
          .insert(item)
          .pipe(map((data) => addCustomerCompleteAction({ result: data })))
      )
    )
  );

  updateCustomer$ = createEffect(() =>
    this.actions$.pipe(
      ofType(updateCustomerAction),
      switchMap(({ item }) =>
        this.custSvc.update(item).pipe(
          map((data: Customer) =>
            updateCustomerCompleteAction({
              result: { id: data.customerId, changes: { ...data } },
            })
          )
        )
      )
    )
  );
}
