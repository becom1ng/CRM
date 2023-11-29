import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
} from '@angular/common/http';
import { Observable, tap } from 'rxjs';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  /**
   * Identifies and handles a given HTTP request.
   * @param req The outgoing request object to handle.
   * @param next The next interceptor in the chain, or the backend
   * if no interceptors remain in the chain.
   * @returns An observable of the event stream.
   */
  constructor() {}

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
      return next.handle(clonedRequest);
    } else {
      return next.handle(request);
    }

    // return next.handle(request).pipe(
    //   tap(
    //     (event: HttpEvent<any>) => {},
    //     (err: any) => {
    //       // TODO Exercise: this is where you can check for a 401 or other types of errors
    //       // If the response returns a 401 Unauthorized, the interceptor should navigate the user to the login page.
    //       // this.router.navigate(['login']) ??
    //     }
    //   )
    // );
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
