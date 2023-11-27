import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'crm-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.scss']
})
export class RegistrationComponent {
  registrationForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private router: Router,
  ) {
    this.registrationForm = this.fb.group({
      emailAddress: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]],
      // TODO: ADD PASSWORD VERIFICATION FIELD
    })
  }

  onSubmit(): void {
    if (!this.registrationForm.valid) {
      return;
    }
    // const creds = { ...this.registrationFormForm.value };
    // this.accountService.loginPassword(creds).subscribe({
    //   next: (result) => {
    //     this.accountService.loginComplete(result, 'Login Complete');
    //   },
    //   error: (_) => {
    //     // _ is an error, interceptor shows snackbar based on api response
    //     console.log(_);
    //   },
    // });
  }

  cancel(): void {
    this.router.navigate(['./login'])
  }
}
