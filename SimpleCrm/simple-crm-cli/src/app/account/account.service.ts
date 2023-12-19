import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { PlatformLocation } from '@angular/common';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { MatSnackBar } from '@angular/material/snack-bar';

import {
  CredentialsViewModel,
  GoogleOptions,
  MicrosoftOptions,
  UserSummaryViewModel,
  anonymousUser,
} from './account.model';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  private baseUrl: string;
  private cachedUser = new BehaviorSubject<UserSummaryViewModel>(
    anonymousUser()
  );
  // BehaviorSubject is a type of Observable you can easily set the next value on.
  // Note the one above initializes it to the result of method call anonymousUser()

  constructor(
    private http: HttpClient, // part of Angular to make Http requests
    private router: Router, // part of Angular router, for navigating the user within the app
    private platformLocation: PlatformLocation,
    private snackBar: MatSnackBar
  ) {
    this.baseUrl = environment.server + environment.apiUrl + 'auth/';
    // this.cachedUser.next(anonymousUser()); // Already performed this action above
    const cu = localStorage.getItem('currentUser'); // <- localStorage is really useful
    if (cu) {
      // if already logged in from before, just use that. It has a JWT in it.
      this.cachedUser.next(JSON.parse(cu)); // <- JSON is built into JavaScript and always available.
      this.verifyUser(this.cachedUser.value);
    }
  }

  get user(): BehaviorSubject<UserSummaryViewModel> {
    // components can pipe off of this to get a new value as they login/logout
    return this.cachedUser;
  }
  setUser(user: UserSummaryViewModel): void {
    // called by your components that process a login from password, Google, Microsoft
    this.cachedUser.next(user);
    localStorage.setItem('currentUser', JSON.stringify(user));
  }
  get isAnonymous() {
    return this.cachedUser.pipe(
      map((user) => {
        if (user.name === 'Anonymous') {
          return false;
        }
        return true;
      })
    );
  }

  // TODO Optional: add other methods that calculate if the current cachedUser has
  // a specific role or permission in the app

  // !! Ensure both match your C# Api signatures from the prior course.
  // An API to load the Options needed to login with Microsoft.
  public loginMicrosoftOptions(): Observable<MicrosoftOptions> {
    return this.http.get<MicrosoftOptions>(this.baseUrl + 'external/microsoft');
  }
  // An API to load the Options needed to login with Google.
  public loginGoogleOptions(): Observable<GoogleOptions> {
    return this.http.get<GoogleOptions>(this.baseUrl + 'external/google');
  }

  /**
   * Name and password login API call.
   * If a successful login is completed you may want to call loginComplete
   * to handle updates to the current user and redirect to where they
   * originally wanted to go.
   * @param param0 credentials from login form (email and password)
   */
  public loginPassword(
    credentials: CredentialsViewModel
  ): Observable<UserSummaryViewModel> {
    this.cachedUser.next(anonymousUser());
    localStorage.removeItem('currentUser');
    return this.http.post<UserSummaryViewModel>(
      this.baseUrl + 'login',
      credentials
    );
  }

  /**
   * Call this to update the current login user information,
   * check they have appropriate access and then send them to
   * their originally requested page (if they have access.)
   * @param data The user data from the login call
   */
  public loginComplete(
    data: UserSummaryViewModel,
    successMessage: string
  ): void {
    this.cachedUser.next(data);
    localStorage.setItem('currentUser', JSON.stringify(data));
    if (!data.roles || data.roles.length === 0) {
      this.router.navigate(['account', 'not-authorized']);
      this.snackBar.open('No Access', '', { duration: 3000 });
    } else {
      this.snackBar.open(successMessage, '', { duration: 3000 });
      const returnUrl = localStorage.getItem('loginReturnUrl') || './customers';
      console.log(returnUrl);
      this.router.navigate([returnUrl]);
    }
  }

  /**
   * An API to complete the login from a Microsoft login code
   * @param code The login session verification code from Microsoft
   * @param state Any state that needs to be passed around, typically empty/null.
   */
  public loginMicrosoft(
    code: string,
    state: string
  ): Observable<UserSummaryViewModel> {
    const body = {
      accessToken: code,
      state,
      baseHref: this.platformLocation.getBaseHrefFromDOM(),
    };
    return this.http.post<UserSummaryViewModel>(
      this.baseUrl + 'external/microsoft',
      body
    );
  }

  /**
   * An API to complete the login from a Google login code
   * @param code The login session verification code from Google
   * @param state Any state that needs to be passed around, typically empty/null.
   */
  public loginGoogle(
    code: string,
    state: string
  ): Observable<UserSummaryViewModel> {
    const body = {
      accessToken: code,
      state,
      baseHref: this.platformLocation.getBaseHrefFromDOM(),
    };
    return this.http.post<UserSummaryViewModel>(
      this.baseUrl + 'external/google',
      body
    );
  }

  /**
   * Removes any cached login data and sends the user to the main website home page.
   */
  public logout(options: { navigate: boolean } = { navigate: true }): void {
    this.cachedUser.next(anonymousUser());
    localStorage.removeItem('currentUser');
    this.snackBar.open('You have been logged out.', '', { duration: 3000 });
    if (options && options.navigate) {
      this.router.navigate(['./customers']);
    }
  }

  /**
   * An API call to verify a login token is still valid
   * and then refresh the roles they have access to.
   * If the login token is actually expired and this is not called,
   * NO HARM is done, since the API will not return data they no
   * longer have access to!
   * After this successfully returns, please set the user property
   * to the resulting updated user (don't call loginComplete since that navigates).
   * @param user The user account to verify is still logged in
   */
  public verifyUser(
    user: UserSummaryViewModel
  ): Observable<UserSummaryViewModel> {
    const model = {};
    const options =
      !user || !user.jwtToken
        ? {}
        : { headers: { Authorization: 'Bearer ' + user.jwtToken } };
    return this.http.post<UserSummaryViewModel>(
      this.baseUrl + 'verify',
      model,
      options
    );
  }
}
