import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Customer } from '../customer.model';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'crm-customer-create-dialog',
  templateUrl: './customer-create-dialog.component.html',
  styleUrls: ['./customer-create-dialog.component.scss'],
})
export class CustomerCreateDialogComponent implements OnInit {
  detailForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<CustomerCreateDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Customer | null
  ) {
    // the group method takes an JSON object like the following
    this.detailForm = this.fb.group({
      // target form field name is the property name
      firstName: ['', [Validators.required, Validators.maxLength(50)]],
      lastName: ['', [Validators.required, Validators.maxLength(50)]],
      phoneNumber: ['', [Validators.minLength(7), Validators.maxLength(12)]],
      emailAddress: [
        '',
        [Validators.required, Validators.email, Validators.maxLength(100)],
      ],
      preferredContactMethod: ['', Validators.required],
    });
    if (this.data) {
      this.detailForm.patchValue(this.data);
    } // the patchValue function updates the form input values.
  }

  save() {
    if (!this.detailForm.valid) {
      return;
    } // do nothing if form is not valid
    const customer = {
      ...this.data,
      ...this.detailForm.value,
    }; // merge values from form
    this.dialogRef.close(customer); // pass in the data to give back to the parent, or nothing
  }

  cancel(): void {
    this.dialogRef.close();
  }

  ngOnInit(): void {}
}
