import { MemberDto } from './../../SharedClass/member-dto';
import { ResponsAPI } from './../../SharedClass/respons-api';
import { AccountService } from './../../Services/Account.service';
import { Component, inject, OnInit } from '@angular/core';
import { MemberCartComponent } from '../member-cart/member-cart.component';
import { PaginationData } from '../../SharedClass/PagimationData';
import { PaginationModule } from 'ngx-bootstrap/pagination';

@Component({
  selector: 'app-member-list',
  standalone: true,
  imports: [MemberCartComponent, PaginationModule],
  templateUrl: './member-list.component.html',
  styleUrl: './member-list.component.css',
})
export class MemberListComponent implements OnInit {
  data: MemberDto[] = [];
  currentPage = 1;
  pageSize = 2;
  totalCount = 0;
  totalPages = 0;
  private accountService = inject(AccountService);

  ngOnInit(): void {
    this.LoadMembers();
  }
  LoadMembers() {
    this.accountService
      .GetAllMembers(this.currentPage, this.pageSize)
      .subscribe({
        next: (response: ResponsAPI<PaginationData<MemberDto>>) => {
          if (response.success) {
            this.data = response.data.data;
            console.log(response.data);
            this.totalCount = response.data.paginationMetadata.totalCount;
            this.totalPages = response.data.paginationMetadata.totalPages;
          }
        },
        error: (err: any) => console.log(err),
      });
  }

  onPageChange(page: number): void {
    this.currentPage = page;
    this.LoadMembers();
  }
}
