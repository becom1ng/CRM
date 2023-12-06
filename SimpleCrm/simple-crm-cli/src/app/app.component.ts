import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AccountService } from './account/account.service';
import { Observable } from 'rxjs';
import { UserSummaryViewModel } from './account/account.model';

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
  ) {}

  ngOnInit(): void {
    // TODO: Should pipe should be used? Since <BehaviorSubject> is implemented in account
    // service, then I might be able to use next() somehow??
    this.userInfo$ = this.accountService.user.asObservable();
  }

  public onAccount(): void {
    this.router.navigate(['./login']);
  }

  public onLogout(): void {
    this.accountService.logout();
  }
}
