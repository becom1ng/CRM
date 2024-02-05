import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AccountService } from './account/account.service';
import { Observable } from 'rxjs';
import { UserSummaryViewModel } from './account/account.model';
import {
  LayoutState,
  selectShowSideNav,
  toggleSidenav,
} from './store/layout.store';
import { Store, select } from '@ngrx/store';

@Component({
  selector: 'crm-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  title = 'Simple CRM';
  userInfo$!: Observable<UserSummaryViewModel>;
  showSideNav$: Observable<boolean>;

  // TODO: remove router and accountservice injection as they can be removed with store (see https://www.nexulacademy.com/courseware/angular-advanced/ngrx-actions-reducers)
  constructor(
    private router: Router,
    private accountService: AccountService,
    private store: Store<LayoutState>
  ) {
    this.showSideNav$ = this.store.pipe(select(selectShowSideNav));
  }

  ngOnInit(): void {
    // ? TODO: Should pipe or next() be used? Since <BehaviorSubject> is implemented in accountService
    this.userInfo$ = this.accountService.user.asObservable();
  }

  public onAccount(): void {
    this.router.navigate(['./login']);
  }

  public onLogout(): void {
    this.accountService.logout();
  }

  sideNavToggle() {
    this.store.dispatch(toggleSidenav());
  }
}
