import { SpinnerComponent } from './../../spinner/spinner.component';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { AccountService } from '../../Services/Account.service';
import { MemberDto } from './../../SharedClass/member-dto';
import { Component, inject, OnInit, ViewChild } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { PhotoEditorComponent } from '../photo-editor/photo-editor.component';
@Component({
  selector: 'app-member-edit',
  standalone: true,
  imports: [TabsModule, FormsModule, NgxSpinnerModule, PhotoEditorComponent],
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css'], // Corrected property name
})
export class MemberEditComponent implements OnInit {
  @ViewChild('editForm') editForm?: NgForm;
  member?: MemberDto;
  isLoading: boolean = false;
  private accountService = inject(AccountService);
  private toastr = inject(ToastrService);
  ngOnInit(): void {
    this.loadMember();
  }
  loadMember() {
    //this.isLoading = true;
    this.accountService.GetProfileData().subscribe({
      next: (response) => {
        if (response.success) {
          this.member = response.data;
        } else {
          console.log(response.message);
        }
      },
      error: (err) => {
        console.log(err);
      },
    });
  }
  updateMember() {
    this.isLoading = true;
    this.accountService.UpdateProfile(this.member).subscribe({
      next: (response) => {
        if (response.success) {
          this.toastr.success('Profile Updated Successfully');
        } else {
          this.toastr.error('Some thing went wrong');
        }
      },
      error: (err) => console.log(err),
    });
  }

  loadchangedMember($event: MemberDto) {
    this.member = $event;
  }
}
