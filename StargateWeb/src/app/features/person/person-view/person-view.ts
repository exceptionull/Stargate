import { ChangeDetectionStrategy, Component, CUSTOM_ELEMENTS_SCHEMA, inject, ViewChild } from '@angular/core';
import { AstronautDutyService } from '../../../api';
import { BehaviorSubject, switchMap } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { DividerModule } from 'primeng/divider';
import { toSignal } from '@angular/core/rxjs-interop';
import { TableModule } from 'primeng/table';
import { RouterModule } from '@angular/router';
import { DatePipe } from '@angular/common';
import { NotAvailablePipe } from '../../../shared/pipes/not-available.pipe';
import { PersonDutyAdd } from "../persin-duty-add/person-duty-add";

@Component({
  selector: 'person-view',
  templateUrl: './person-view.html',
  styleUrl: './person-view.css',
  imports: [ButtonModule, CardModule, DividerModule, TableModule, RouterModule, DatePipe, NotAvailablePipe, PersonDutyAdd],
  changeDetection: ChangeDetectionStrategy.OnPush,
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class PersonView {
  @ViewChild(PersonDutyAdd) personDutyAdd!: PersonDutyAdd;

  astronautDutyService = inject(AstronautDutyService);
  route = inject(ActivatedRoute);
  name = this.route.snapshot.paramMap.get('name') ?? '';
  
  refresh$ = new BehaviorSubject<void>(undefined);
  astronautDuty$ = this.refresh$.pipe(switchMap(() => this.astronautDutyService.astronautDutyNameGet(this.name)));
  astronautDuty = toSignal(this.astronautDuty$);

  addDuty() {
      this.personDutyAdd.visible.set(true);
      this.personDutyAdd.form.controls.name.setValue(this.name);
  }
}
