import { environment } from './../../../../enviroment';
import { Component, inject, input, OnInit, output } from '@angular/core';
import { MemberDto } from '../../SharedClass/member-dto';
import { FileUploader, FileUploadModule } from 'ng2-file-upload';
import { DecimalPipe, NgClass, NgFor, NgIf, NgStyle } from '@angular/common';
import { AccountService } from '../../Services/Account.service';
import { ToastrService } from 'ngx-toastr';
import { PhotoDto } from '../../SharedClass/photo-dto';

@Component({
  selector: 'app-photo-editor',
  standalone: true,
  imports: [NgIf, NgClass, NgFor, FileUploadModule, DecimalPipe, NgStyle],
  templateUrl: './photo-editor.component.html',
  styleUrl: './photo-editor.component.css',
})
export class PhotoEditorComponent implements OnInit {
  private accountService = inject(AccountService);
  member = input.required<MemberDto>();
  uploader?: FileUploader;
  hasBaseDropZoneOver = false;
  baserUrl = environment.apiUrl;
  updatedMember = output<MemberDto>();
  private toastr = inject(ToastrService);
  ngOnInit(): void {
    this.intializeUploader();
  }
  fileOverBase(e: any) {
    this.hasBaseDropZoneOver = e;
  }
  intializeUploader() {
    this.uploader = new FileUploader({
      url: this.baserUrl + 'Account/AddProfilePhoto',
      authToken: 'Bearer ' + this.accountService.currenUser()?.token,
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024,
    });
    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false;
    };
    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response) {
        const photo = JSON.parse(response).data;
        const changedMember = { ...this.member() };
        changedMember.photos.push(photo);
        this.updatedMember.emit(changedMember);
        if (photo.isMain) {
          const user = this.accountService.currenUser();
          if (user) {
            user.photoUrl = photo.url;
            this.accountService.setCuurentUser(user);
          }
          changedMember.photoUrl = photo.url;
          changedMember.photos.forEach((e) => {
            if (e.isMain) e.isMain = false;
            if (e.id == photo.id) e.isMain = true;
          });
          this.updatedMember.emit(changedMember);
        }
      }
    };
  }
  SetMainPhoto(photo: PhotoDto) {
    this.accountService.SetMainPhoto(photo.id).subscribe({
      next: (response) => {
        if (response.success) {
          const user = this.accountService.currenUser();
          if (user) {
            user.photoUrl = photo.url;
            this.accountService.setCuurentUser(user);
          }
          const changedMember = { ...this.member() };
          changedMember.photoUrl = photo.url;
          changedMember.photos.forEach((e) => {
            if (e.isMain) e.isMain = false;
            if (e.id == photo.id) e.isMain = true;
          });
          this.updatedMember.emit(changedMember);
        } else {
          this.toastr.error('Some thing went wrong');
        }
      },
      error: (err) => console.log(err),
    });
  }
  deletePhoto(photo: PhotoDto) {
    this.accountService.DeletePhoto(photo.id).subscribe({
      next: (response) => {
        if (response.success) {
          const changedMember = { ...this.member() };
          changedMember.photos = changedMember.photos.filter(
            (e) => e.id != photo.id
          );
          this.updatedMember.emit(changedMember);
        } else {
          this.toastr.error('Some thing went wrong');
        }
      },
      error: (err) => console.log(err),
    });
  }
}
