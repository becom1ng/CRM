import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatCardModule } from '@angular/material/card';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';
import { HttpClientModule } from '@angular/common/http';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { ReactiveFormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { environment } from 'src/environments/environment';
import { CustomerRoutingModule } from './customer-routing.module';
import { CustomerService } from './customer.service';
import { CustomerMockService } from './customer-mock.service';

import { CustomerListPageComponent } from './customer-list-page/customer-list-page.component';
import { CustomerListPageAltComponent } from './customer-list-page-alt/customer-list-page-alt.component';
import { CustomerCreateDialogComponent } from './customer-create-dialog/customer-create-dialog.component';
import { CustomerDetailComponent } from './customer-detail/customer-detail.component';
import { StatusIconPipe } from './status-icon.pipe';
@NgModule({
  declarations: [
    CustomerListPageComponent,
    CustomerListPageAltComponent,
    CustomerCreateDialogComponent,
    CustomerDetailComponent,
    StatusIconPipe
  ],
  imports: [
    CommonModule,
    CustomerRoutingModule,
    HttpClientModule,
    MatTableModule,
    MatCardModule,
    MatPaginatorModule,
    MatSortModule,
    MatIconModule,
    MatListModule,
    MatButtonModule,
    MatDialogModule,
    ReactiveFormsModule,
    MatInputModule,
    MatSelectModule,
    MatSnackBarModule
  ],
  providers: [{
    provide: CustomerService,
    useClass: environment.production ? CustomerService : CustomerMockService,
  }]
})
export class CustomerModule { }