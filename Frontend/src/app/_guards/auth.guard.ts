import { ToastrService } from 'ngx-toastr';
import { AccountService } from './../Services/Account.service';
import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';

export const authGuard: CanActivateFn = (route, state) => {
  const accountService = inject(AccountService);
  const toastar = inject(ToastrService);
  if (accountService.currenUser()?.token) {
    return true;
  } else {
    toastar.error('You don`t have access to this page');
    return false;
  }
};
