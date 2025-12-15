import { ChangeDetectionStrategy, Component, inject, model, output } from '@angular/core';
import { PersonService } from '../../../api';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { MessageModule } from 'primeng/message';
import { InputTextModule } from 'primeng/inputtext';
import { FormGroup, FormControl, Validators, ReactiveFormsModule } from '@angular/forms';
import { take, catchError, EMPTY } from 'rxjs';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';
import { isInvalid } from '../../../shared/helpers/form';

@Component({
  selector: 'person-edit',
  templateUrl: './person-edit.html',
  styleUrl: './person-edit.css',
  imports: [ButtonModule, DialogModule, MessageModule, ReactiveFormsModule, InputTextModule, ToastModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PersonEdit {
  protected readonly personService = inject(PersonService);
  protected readonly messageService = inject(MessageService);

  visible = model<boolean>(false);
  success = output<void>();

  form = new FormGroup({
    name: new FormControl<string | undefined>(undefined, { nonNullable: true, validators: Validators.required }),
    newName: new FormControl<string | undefined>(undefined, { nonNullable: true, validators: Validators.required }),
  });
  formSubmitted = false;

  isInvalid = isInvalid;

  cancel() {
      this.form.reset();
      this.formSubmitted = false;
      this.visible.set(false);    
  }

  edit() {
    this.formSubmitted = true;
    if (!this.form.valid) {
        this.form.markAllAsTouched();
        return;
    }

    this.personService.personPut(this.form.value).pipe(
            take(1), 
            catchError((err) => {
                this.messageService.add({ severity: 'error', summary: 'Error', detail: !!err?.error?.message ? err.error.message : 'An internal error has occurred please try again.' });
                return EMPTY;
            })
        ).subscribe(() => {
            this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Person updated.' });
            this.form.reset();
            this.formSubmitted = false;
            this.success.emit();
            this.visible.set(false); 
        });
  }
}
