import { Component, input, OnInit } from '@angular/core';
import { MemberDto } from '../../SharedClass/member-dto';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-member-cart',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './member-cart.component.html',
  styleUrl: './member-cart.component.css',
})
export class MemberCartComponent {
  member = input.required<MemberDto>();
}
