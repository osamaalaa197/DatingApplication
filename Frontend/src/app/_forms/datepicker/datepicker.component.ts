import { Component, input, Self } from '@angular/core';
import {
  ControlValueAccessor,
  FormControl,
  NgControl,
  ReactiveFormsModule,
} from '@angular/forms';
import {
  BsDatepickerConfig,
  BsDatepickerModule,
} from 'ngx-bootstrap/datepicker';

@Component({
  selector: 'app-datepicker',
  standalone: true,
  imports: [BsDatepickerModule, ReactiveFormsModule],
  templateUrl: './datepicker.component.html',
  styleUrl: './datepicker.component.css',
})
export class DatepickerComponent implements ControlValueAccessor {
  label = input<string>('');
  maxDate = input<Date>();
  bsConfig?: Partial<BsDatepickerConfig>;
  constructor(@Self() public ngcontrol: NgControl) {
    this.ngcontrol.valueAccessor = this;
    this.bsConfig = {
      containerClass: 'them-color:blue',
      dateInputFormat: 'DD/MM/YYYY',
    };
  }

  writeValue(obj: any): void {}
  registerOnChange(fn: any): void {}
  registerOnTouched(fn: any): void {}
  setDisabledState?(isDisabled: boolean): void {}

  getControl(): FormControl {
    return this.ngcontrol.control as FormControl;
  }
}
