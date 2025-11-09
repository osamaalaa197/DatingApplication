import { AuthViewModel } from './../SharedClass/auth-view-model';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { map, Observable } from 'rxjs';
import { environment } from '../../../enviroment';
import { LoginViewModel } from '../SharedClass/login-view-model';
import { RegisterViewModel } from '../SharedClass/register-view-model';
import { ResponsAPI } from '../SharedClass/respons-api';
import { ToastrService } from 'ngx-toastr';
import { MemberDto } from '../SharedClass/member-dto';
import { PaginationData } from '../SharedClass/PagimationData';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  constructor(private _http: HttpClient) {}
  currenUser = signal<AuthViewModel | null>(null);
  token = signal<string | null>(null);
  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
    }),
  };
  setCuurentUser(user: AuthViewModel) {
    this.currenUser.set(user);
  }
  LogIn(data: LoginViewModel) {
    return this._http
      .post<ResponsAPI<AuthViewModel>>(
        `${environment.apiUrl}Account/LogIn`,
        data,
        this.httpOptions
      )
      .pipe(
        map((response) => {
          if (response.success == true) {
            localStorage.setItem('token', response.data.token);
            this.currenUser.set(response.data);
            this.token.set(response.data.token);
          }
          return response;
        })
      );
  }

  Register(data: any) {
    return this._http.post<ResponsAPI<object>>(
      `${environment.apiUrl}Account/Register`,
      data,
      this.httpOptions
    );
  }

  Logout() {
    localStorage.removeItem('token');
    this.currenUser.set(null);
  }
  GetAllMembers(pageNumber: number, pageSize: number) {
    const params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());
    return this._http.get<ResponsAPI<PaginationData<MemberDto>>>(
      `${environment.apiUrl}Account/List`,
      { params }
    );
  }
  GetMemberByUserName(userName: string) {
    return this._http.get<ResponsAPI<Object>>(
      `${environment.apiUrl}Account/GetUserByUserName?userName=${userName}`,
      this.httpOptions
    );
  }
  GetProfileData() {
    return this._http.get<ResponsAPI<MemberDto>>(
      `${environment.apiUrl}Account/Profile`
    );
  }

  GetUserById(id: string) {
    return this._http.get<ResponsAPI<MemberDto>>(
      `${environment.apiUrl}Account/GetUserById/${id}`
    );
  }

  UpdateProfile(data?: MemberDto) {
    return this._http.post<ResponsAPI<object>>(
      `${environment.apiUrl}Account/UpdateProfile`,
      data
    );
  }

  SetMainPhoto(photoId: number) {
    return this._http.put<ResponsAPI<object>>(
      `${environment.apiUrl}Account/SetPhotoMain/${photoId}`,
      null
    );
  }

  DeletePhoto(photoId: number) {
    return this._http.delete<ResponsAPI<object>>(
      `${environment.apiUrl}Account/DeletePhoto/${photoId}`
    );
  }
}
