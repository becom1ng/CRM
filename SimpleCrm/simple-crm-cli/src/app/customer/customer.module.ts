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
import { CustomerRoutingModule } from './customer-routing.module';
import { CustomerService } from './customer.service';

import { CustomerListPageComponent } from './customer-list-page/customer-list-page.component';
import { CustomerCreateDialogComponent } from './customer-create-dialog/customer-create-dialog.component';
import { CustomerDetailComponent } from './customer-detail/customer-detail.component';
import { StatusIconPipe } from './status-icon.pipe';
import { CustomerStoreEffects } from './store/customer.store.effects';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { customerFeatureKey } from './store/customer.store.selectors';
import { customerReducer } from './store/customer.store';
import { CustomerListTableComponent } from './customer-list-table/customer-list-table.component';
@NgModule({
  declarations: [
    CustomerListPageComponent,
    CustomerCreateDialogComponent,
    CustomerDetailComponent,
    StatusIconPipe,
    CustomerListTableComponent,
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
    MatSnackBarModule,
    EffectsModule.forFeature([CustomerStoreEffects]),
    StoreModule.forFeature(customerFeatureKey, customerReducer),
  ],
  providers: [
    {
      provide: CustomerService,
      useClass: CustomerService,
    },
  ],
})
export class CustomerModule {}
