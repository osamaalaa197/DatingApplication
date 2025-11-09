import { RegisterViewModel } from './../SharedClass/register-view-model';
import { Component, input, output } from '@angular/core';
import {
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { AccountService } from '../Services/Account.service';
import { ResponsAPI } from '../SharedClass/respons-api';
import { ToastrService } from 'ngx-toastr';
import { CommonModule } from '@angular/common';
import { DatepickerComponent } from '../_forms/datepicker/datepicker.component';
@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    FormsModule,
    CommonModule,
    ReactiveFormsModule,
    DatepickerComponent,
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
})
export class RegisterComponent {
  profileForm = new FormGroup({
    firstName: new FormControl('', Validators.required),
    lastName: new FormControl('', Validators.required),
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', Validators.required),
    gender: new FormControl('', Validators.required),
    dateOfBirth: new FormControl('', Validators.required),
    city: new FormControl('', Validators.required),
    country: new FormControl('', Validators.required),
    knownAs: new FormControl('', Validators.required),
  });
  Model: RegisterViewModel = {
    firstName: '',
    lastName: '',
    email: '',
    password: '',
    gender: '',
    dateOfBirth: '',
    city: '',
    country: '',
  };
  cancelRegister = output<boolean>();
  constructor(
    private accountService: AccountService,
    private toastr: ToastrService
  ) {}
  register() {
    const dop = this.profileForm.get('dateOfBirth')?.value;
    this.profileForm.patchValue({ dateOfBirth: dop });
    this.accountService.Register(this.profileForm.value).subscribe({
      next: (response: ResponsAPI<object>) => {
        if (response.success) {
          this.toastr.success(response.message);
          this.cancel();
        } else {
          this.toastr.error(response.message);
        }
      },
    });
    console.log(this.profileForm.value);
  }
  cancel() {
    this.cancelRegister.emit(true);
  }

  getDatOnly(dob: string | undefined) {
    if (!dob) {
      return;
    }
    return new Date(dob).toISOString().slice(0, 10);
  }
}
