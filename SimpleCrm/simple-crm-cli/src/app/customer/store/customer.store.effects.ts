import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { CustomerService } from '../customer.service';
import { switchMap, map } from 'rxjs';

// Do NOT register any specific effects class in multiple places.
@Injectable()
export class CustomerStoreEffects {
  constructor(
    private actions$: Actions, // <-- this event stream is where to listen for dispatched actions
    private custSvc: CustomerService // <-- this is your service to be called for some actions
  ) {}

  searchCustomers$ = createEffect(() =>
    this.actions$.pipe(
      ofType(searchCustomersAction),
      switchMap(
        (
          { criteria } // use rxjs, accept action payload
        ) =>
          this.custSvc.search(criteria.term).pipe(
            // make service call
            map(
              // create action payload with API response data
              (data) => searchCustomersCompleteAction({ result: data })
            )
          )
      )
    )
  );
}
