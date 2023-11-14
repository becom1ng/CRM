import { inject } from '@angular/core';
import { Router } from '@angular/router';

export const AuthenticatedGuard = () => {
  const router = inject(Router); // inject like this instead of in a constructor

  console.log("** Access denied by AuthenticatedGuard.");
  return router.createUrlTree(['not-authorized']);
}
