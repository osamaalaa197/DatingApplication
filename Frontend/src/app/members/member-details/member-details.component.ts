import { TimeagoModule } from 'ngx-timeago';
import { Component, inject, OnInit } from '@angular/core';
import { MemberDto } from '../../SharedClass/member-dto';
import { AccountService } from '../../Services/Account.service';
import { ActivatedRoute } from '@angular/router';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-member-details',
  standalone: true,
  imports: [TabsModule, DatePipe, TimeagoModule],
  templateUrl: './member-details.component.html',
  styleUrl: './member-details.component.css',
})
export class MemberDetailsComponent implements OnInit {
  private accountService = inject(AccountService);
  private router = inject(ActivatedRoute);
  member?: MemberDto;

  ngOnInit(): void {
    this.LoadMember();
  }
  LoadMember() {
    const id = this.router.snapshot.paramMap.get('id');
    console.log(id);
    if (!id) {
      return;
    }
    this.accountService.GetUserById(id).subscribe({
      next: (member) => {
        if (member.success == true) {
          this.member = member.data;
        }
      },
      error: (error) => console.log(error),
    });
  }
}
