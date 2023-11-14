import { inject } from '@angular/core';
import { Router } from '@angular/router';

export const AuthenticatedGuard = () => {
  const router = inject(Router); // inject like this instead of in a constructor

  console.log("Access allowed by AuthenticatedGuard.");
  return true;
}
