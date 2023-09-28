import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Customer } from '../customer.model';

@Component({
  selector: 'crm-customer-create-dialog',
  templateUrl: './customer-create-dialog.component.html',
  styleUrls: ['./customer-create-dialog.component.scss']
})
export class CustomerCreateDialogComponent implements OnInit {

  constructor(public dialogRef: MatDialogRef<CustomerCreateDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Customer | null
  ) {}

  // TODO: SAVE, get form data and return to parent component
  save() {
    const customer = {};
    this.dialogRef.close(customer);  // pass in the data to give back to the parent, or nothing
  }

  cancel(): void {
    this.dialogRef.close();
  }

  ngOnInit(): void {}
}
