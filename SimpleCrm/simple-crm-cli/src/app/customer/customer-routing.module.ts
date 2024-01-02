import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CustomerListPageComponent } from './customer-list-page/customer-list-page.component';
import { CustomerDetailComponent } from './customer-detail/customer-detail.component';
import { AuthenticatedGuard } from '../account/authenticated.guard';
// import { CustomerListPageAltComponent } from './customer-list-page-alt/customer-list-page-alt.component';

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    component: CustomerListPageComponent,
  },
  {
    path: ':id', // id parameter
    pathMatch: 'full',
    component: CustomerDetailComponent,
    canActivate: [AuthenticatedGuard],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CustomerRoutingModule {}
