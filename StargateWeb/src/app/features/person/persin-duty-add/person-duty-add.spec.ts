import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MessageService } from 'primeng/api';
import { AstronautDutyService } from '../../../api';
import { of, throwError } from 'rxjs';
import { provideRouter } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { errorMessageDefault } from '../../../shared/constants/error';
import { PersonDutyAdd } from './person-duty-add';

describe('PersonDutyAdd', () => {
  let fixture: ComponentFixture<PersonDutyAdd>;
  let component: PersonDutyAdd;

  let astronautDutyService: AstronautDutyService;
  let messageService: MessageService;  

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PersonDutyAdd],
      providers: [MessageService, provideRouter([])],
    })
    .compileComponents();

    astronautDutyService = TestBed.inject(AstronautDutyService);
    messageService = TestBed.inject(MessageService);

    fixture = TestBed.createComponent(PersonDutyAdd);

    fixture.autoDetectChanges();

    component = fixture.componentInstance;

    component.visible.set(true);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should reset form, set formSubmitted to false, and set visible to false on cancel', () => {
    const formResetSpy = vi.spyOn(component.form, 'reset');
    component.formSubmitted = true;

    component.cancel();

    expect(formResetSpy).toHaveBeenCalledOnce();
    expect(component.formSubmitted).toBe(false);
    expect(component.visible()).toBe(false);
  });

  describe('on add', () => {
    it('should set formSubmitted to true and call form.markAllAsTouched() when form invalid', () => {
        const formMarkAllAsTouchedSpy = vi.spyOn(component.form, 'markAllAsTouched');

        component.add();

        expect(formMarkAllAsTouchedSpy).toHaveBeenCalledOnce();
        expect(component.formSubmitted).toBe(true);
    });

    it('should set formSubmitted to true and display error toast when api call returns error', () => {
        const formValue = {
            name: 'Jon Doe',
            rank: '1LT',
            dutyTitle: 'Commander',
            dutyStartDate: new Date()
        };
        component.form.patchValue(formValue);
        const messageServiceAddSpy = vi.spyOn(messageService, 'add');
        const error = new HttpErrorResponse({ error: { message: undefined } });
        const astronautDutyPostSpy = vi.spyOn(astronautDutyService, 'astronautDutyPost').mockReturnValue(throwError(() => error) as any)

        component.add();

        expect(astronautDutyPostSpy).toHaveBeenCalledExactlyOnceWith(formValue);
        expect(component.formSubmitted).toBe(true);
        expect(messageServiceAddSpy).toHaveBeenCalledExactlyOnceWith({ severity: 'error', summary: 'Error', detail: errorMessageDefault });
    });

    it('should set formSubmitted to true and display error toast when api call returns error message', () => {
        const formValue = {
            name: 'Jon Doe',
            rank: '1LT',
            dutyTitle: 'Commander',
            dutyStartDate: new Date()
        };
        component.form.patchValue(formValue);
        const messageServiceAddSpy = vi.spyOn(messageService, 'add');
        const errorMessage = 'Fake error!';
        const error = new HttpErrorResponse({ error: { message: errorMessage } });
        const astronautDutyPostSpy = vi.spyOn(astronautDutyService, 'astronautDutyPost').mockReturnValue(throwError(() => error) as any)

        component.add();

        expect(astronautDutyPostSpy).toHaveBeenCalledExactlyOnceWith(formValue);
        expect(component.formSubmitted).toBe(true);
        expect(messageServiceAddSpy).toHaveBeenCalledExactlyOnceWith({ severity: 'error', summary: 'Error', detail: errorMessage });
    });

    it('should display success toast, reset form, set formSubmitted to false, emit success, and set visible to false when api call returns success', () => {
        const formValue = {
            name: 'Jon Doe',
            rank: '1LT',
            dutyTitle: 'Commander',
            dutyStartDate: new Date()
        };
        component.form.patchValue(formValue);
        const messageServiceAddSpy = vi.spyOn(messageService, 'add');
        const successEmitSpy = vi.spyOn(component.success, 'emit');
        const formResetSpy = vi.spyOn(component.form, 'reset');
        const astronautDutyPostSpy = vi.spyOn(astronautDutyService, 'astronautDutyPost').mockReturnValue(of({ id: 1 }) as any)

        component.add();

        expect(astronautDutyPostSpy).toHaveBeenCalledExactlyOnceWith(formValue);
        expect(messageServiceAddSpy).toHaveBeenCalledExactlyOnceWith({ severity: 'success', summary: 'Success', detail: 'Duty added.' });
        expect(formResetSpy).toHaveBeenCalledOnce();
        expect(component.formSubmitted).toBe(false);
        expect(successEmitSpy).toHaveBeenCalledOnce();
        expect(component.visible()).toBe(false);
    });
  });  
});
