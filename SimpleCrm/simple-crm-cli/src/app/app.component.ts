import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AccountService } from './account/account.service';
import { Observable } from 'rxjs';
import { UserSummaryViewModel } from './account/account.model';
import { LayoutState, toggleSidenav } from './store/layout.store';
import { Store } from '@ngrx/store';

@Component({
  selector: 'crm-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  title = 'Simple CRM';
  userInfo$!: Observable<UserSummaryViewModel>;

  constructor(
    private router: Router,
    private accountService: AccountService,
    private store: Store<LayoutState>
  ) {}

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
