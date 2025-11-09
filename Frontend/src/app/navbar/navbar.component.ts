import { Component, inject, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { AccountService } from '../Services/Account.service';
import { ResponsAPI } from '../SharedClass/respons-api';
import { ToastrService } from 'ngx-toastr';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    BsDropdownModule,
    RouterLink,
    RouterLinkActive,
  ],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css',
})
export class NavbarComponent {
  loggedIn: string | null = null;
  loginForm: FormGroup;
  responseAPI: ResponsAPI | null = null;
  accountService = inject(AccountService);
  private router = inject(Router);
  constructor(private formbuilder: FormBuilder, private toastr: ToastrService) {
    this.loginForm = this.formbuilder.group({
      gmail: [''],
      password: [''],
    });
  }

  onSubmit() {
    if (this.loginForm.valid) {
      this.accountService.LogIn(this.loginForm.value).subscribe({
        next: (response: ResponsAPI) => {
          console.log(response);
          if (response.success) {
            this.router.navigateByUrl('members');
          } else {
            this.toastr.error(response.message);
          }
        },
        error: (err) => this.toastr.error(err),
      });
    }
  }
  logOut() {
    console.log('das');
    this.accountService.Logout();
    this.router.navigateByUrl('');
  }
}
