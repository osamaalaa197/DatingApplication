import { Router } from '@angular/router';
import { AccountService } from './../Services/Account.service';
import { Component, inject, OnInit } from '@angular/core';
import { RegisterComponent } from '../register/register.component';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RegisterComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
})
export class HomeComponent {
  registerToggel = true;
  public accountService = inject(AccountService);
  private route = inject(Router);
  constructor() {}

  Register() {
    this.registerToggel = !this.registerToggel;
  }
  cancelRegisterMode(event: boolean) {
    this.registerToggel = event;
  }
}
