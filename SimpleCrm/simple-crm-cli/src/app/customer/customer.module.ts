import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatCardModule } from '@angular/material/card';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';

import { CustomerRoutingModule } from './customer-routing.module';
import { CustomerListPageComponent } from './customer-list-page/customer-list-page.component';
import { HttpClientModule } from '@angular/common/http';
import { CustomerService } from './customer.service';
import { CustomerMockService } from './customer-mock.service';
import { environment } from 'src/environments/environment';

@NgModule({
  declarations: [
    CustomerListPageComponent
  ],
  imports: [
    CommonModule,
    CustomerRoutingModule,
    MatTableModule,
    MatCardModule,
    MatPaginatorModule,
    MatSortModule,
    HttpClientModule
  ],
  providers: [
    ...environment.providers
  ]
})
export class CustomerModule { }