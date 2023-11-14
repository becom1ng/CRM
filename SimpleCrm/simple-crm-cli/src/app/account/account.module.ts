import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AccountRoutingModule } from './account-routing.module';
import { NotAuthorizedComponent } from './not-authorized/not-authorized.component';


@NgModule({
  declarations: [
    NotAuthorizedComponent
  ],
  imports: [
    CommonModule,
    AccountRoutingModule
  ]
})
export class AccountModule { }
