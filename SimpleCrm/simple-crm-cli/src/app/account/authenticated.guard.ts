import { CanActivateFn } from '@angular/router';

export const authenticatedGuard: CanActivateFn = (route, state) => {
  console.log("Guard activated. Access allowed (true).")
  return true;
};
