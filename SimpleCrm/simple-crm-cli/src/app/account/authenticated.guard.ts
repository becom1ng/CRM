import { inject } from '@angular/core';
import { map } from 'rxjs';

import { Router } from '@angular/router';
//import { AccountService } from './account.service';

export const AuthenticatedGuard = () => {
  const router = inject(Router);
  //const accountService = inject(AccountService); // inject like this instead of in a constructor

  // then do a return of some value.  Same data types can be returned as in the older style (bool, UrlTree or observable of bool or UrlTree)
  console.log("Access allowed by AuthenticatedGuard.");
  return true;

  // you can do more based on logged in user...
  //return accountService.user.pipe(
    // if getting data from a service to make a determination, pipe from that observable and add any 'operators' needed
    //map(user => {
      // TODO: check whatever you need to about the user
      //return true; // these really return Observable<boolean> since its inside an operator
    //})
  //);
}
  
