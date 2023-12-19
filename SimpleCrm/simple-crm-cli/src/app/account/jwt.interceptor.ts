import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
} from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { Router } from '@angular/router';
import { AccountService } from './account.service';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  /**
   * Identifies and handles a given HTTP request.
   * @param req The outgoing request object to handle.
   * @param next The next interceptor in the chain, or the backend
   * if no interceptors remain in the chain.
   * @returns An observable of the event stream.
   */
  constructor(private router: Router, private accountService: AccountService) {}

  // It should add the Authorization header to all outgoing requests, *IF* the current user is not anonymous.
  // The token to add to each request can be found in the UserSummary from localStorage.
  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    console.log('JwtInterceptor reached.');
    const jwtToken = this.getJwtToken();
    if (jwtToken) {
      const clonedRequest = request.clone({
        headers: request.headers.set('Authorization', 'Bearer ' + jwtToken),
      });
      // return next.handle(clonedRequest);
      // TODO: Refactor
      return next.handle(clonedRequest).pipe(
        tap(
          (event: HttpEvent<any>) => {},
          (err: any) => {
            if (err.status === 401 || err.status === 403) {
              this.accountService.logout({ navigate: false });
              this.router.navigate(['login']);
            } else {
              return;
            }
          }
        )
      );
    }

    return next.handle(request).pipe(
      tap(
        (event: HttpEvent<any>) => {},
        (err: any) => {
          if (err.status === 401 || err.status === 403) {
            this.accountService.logout({ navigate: false });
            this.router.navigate(['login']);
          } else {
            return;
          }
        }
      )
    );
  }

  private getJwtToken() {
    const cu = localStorage.getItem('currentUser');
    if (cu) {
      var currentUser = JSON.parse(cu);
      var token = currentUser.jwtToken;
      return token;
    }
    return false;
  }
}
