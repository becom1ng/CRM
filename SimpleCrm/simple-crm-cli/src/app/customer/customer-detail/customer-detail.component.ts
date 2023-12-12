import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Customer } from '../customer.model';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Observable } from 'rxjs';
import { CustomerState } from '../store/customer.store.model';
import { Store } from '@ngrx/store';
import { selectCustomerById } from '../store/customer.store.selectors';
import { updateCustomerAction } from '../store/customer.store';

@Component({
  selector: 'crm-customer-detail',
  templateUrl: './customer-detail.component.html',
  styleUrls: ['./customer-detail.component.scss'],
})
export class CustomerDetailComponent implements OnInit {
  customerId!: number;
  customer!: Customer;
  detailForm!: FormGroup;
  selectedCust$!: Observable<Customer | undefined>;

  constructor(
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private snackBar: MatSnackBar,
    private store: Store<CustomerState>
  ) {
    this.createForm();
  }

  public createForm(): void {
    this.detailForm = this.fb.group({
      firstName: ['', [Validators.required, Validators.maxLength(50)]],
      lastName: ['', [Validators.required, Validators.maxLength(50)]],
      phoneNumber: ['', [Validators.minLength(7), Validators.maxLength(12)]],
      emailAddress: [
        '',
        [Validators.required, Validators.email, Validators.maxLength(100)],
      ],
      preferredContactMethod: ['', Validators.required],
    });
  }

  ngOnInit(): void {
    this.selectedCust$ = this.store.select(
      selectCustomerById(this.route.snapshot.params['id'])
    );
    this.selectedCust$.subscribe((cust) => {
      if (cust) {
        this.detailForm.patchValue(cust);
        this.customer = cust;
      }
    });
  }

  public save() {
    if (!this.detailForm.valid) {
      return;
    }
    const customer: Customer = { ...this.customer, ...this.detailForm.value };
    this.store.dispatch(updateCustomerAction({ item: customer }));
    // TODO: Fix this functionality/multiple subscribers
    this.selectedCust$.subscribe({
      // next: (result) => {
      //   this.snackBar.open('Customer saved', 'OK');
      // },
      // error: (err) => {
      //   this.snackBar.open('An error occurred: ' + err, 'OK');
      // },
    });
  }
}
