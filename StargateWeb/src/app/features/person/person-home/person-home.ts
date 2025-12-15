import { ChangeDetectionStrategy, Component, CUSTOM_ELEMENTS_SCHEMA, inject, ViewChild } from '@angular/core';
import { PersonService } from '../../../api';
import { toSignal } from '@angular/core/rxjs-interop';
import { TableModule } from 'primeng/table';
import { RouterModule } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { BehaviorSubject, map, switchMap } from 'rxjs';
import { DatePipe } from '@angular/common';
import { MessageService } from 'primeng/api';
import { PersonAdd } from "../person-add/person-add";
import { PersonEdit } from '../person-edit/person-edit';

@Component({
  selector: 'person-home',
  templateUrl: './person-home.html',
  styleUrl: './person-home.css',
  imports: [TableModule, RouterModule, ButtonModule, DatePipe, PersonAdd, PersonEdit],
  changeDetection: ChangeDetectionStrategy.OnPush,
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class PersonHome {
    @ViewChild(PersonAdd) personAdd!: PersonAdd;
    @ViewChild(PersonEdit) personEdit!: PersonEdit;

    protected readonly peopleService = inject(PersonService);

    refresh$ = new BehaviorSubject<void>(undefined);
    people$ = this.refresh$.pipe(
        switchMap(() => this.peopleService.personGet().pipe(
            map((r) => r.people ?? [])
        ))
    );
    people = toSignal(this.people$, { initialValue: [] });

    addPerson() {
        this.personAdd.visible.set(true);
    }

    editPerson(name: string) {
        this.personEdit.visible.set(true);
        this.personEdit.form.controls.name.setValue(name);
        this.personEdit.form.controls.newName.setValue(name);
    }
}
