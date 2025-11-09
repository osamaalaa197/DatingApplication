import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AccountService } from '../Services/Account.service';

export const jwtInterceptor: HttpInterceptorFn = (req, next) => {
  const accountServece = inject(AccountService);
  console.log(accountServece.currenUser());
  if (accountServece.currenUser()) {
    const authReq = req.clone({
      setHeaders: {
        Authorization: `Bearer  ${accountServece.currenUser()?.token}`,
      },
    });
    return next(authReq);
  }
  return next(req);
};
