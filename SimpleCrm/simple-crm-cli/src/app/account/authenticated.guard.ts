import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { map } from 'rxjs';

import { AccountService } from './account.service';

export const AuthenticatedGuard = () => {
  const router = inject(Router);
  const accountService = inject(AccountService);

  return accountService.user.pipe(
    map((user) => {
      if (user.name === 'Anonymous') {
        router.navigate(['./login']);
        // TODO? Consider retrieving the requested URL and saving (to localStorage?)
        // so that the customer can be navigated to that once login is successful
        return false;
      }
      // TODO: may want to add more extensive role checks per route here
      // add some role checks, if your app has roles
      //if (!user || !user.roles || user.roles.length === 0) {
      //  this.router.navigate(['not-authorized']);
      //  return false;
      //}
      return true; // true, didn't find a reason to prevent access to the route
    })
  );
};
