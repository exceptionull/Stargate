import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MessageService } from 'primeng/api';
import { AstronautDuty, AstronautDutyService, GetAstronautDutiesByNameResult, PersonAstronaut } from '../../../api';
import { of } from 'rxjs';
import { PersonView } from '../person-view/person-view';
import { provideRouter } from '@angular/router';

describe('PersonView', () => {
  let fixture: ComponentFixture<PersonView>;
  let component: PersonView;

  let astronautDutyService: AstronautDutyService;

  let astronautDutyNameGetSpy: any;

  const person = { personId: 1, name: 'Jon Doe' } as PersonAstronaut;
  const astronautDuties = [
    { 
      id: 1,
      personId: 1,
      rank: '1LT',
      dutyTitle: 'Commander',
      dutyStartDate: new Date(),
      dutyEndDate: null
    },
    { 
      id: 2,
      personId: 1,
      rank: '1LT',
      dutyTitle: 'Commander',
      dutyStartDate: new Date(),
      dutyEndDate: null
    }
  ] as AstronautDuty[];

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PersonView],
      providers: [provideRouter([]), MessageService],
    })
    .compileComponents();

    astronautDutyService = TestBed.inject(AstronautDutyService);
    astronautDutyNameGetSpy = vi.spyOn(astronautDutyService, 'astronautDutyNameGet')
      .mockReturnValueOnce(of({ person: person, astronautDuties: astronautDuties } as GetAstronautDutiesByNameResult) as any)
      .mockReturnValueOnce(of({ person: person, astronautDuties: [astronautDuties[0]] } as GetAstronautDutiesByNameResult) as any);

    fixture = TestBed.createComponent(PersonView);

    fixture.autoDetectChanges();

    component = fixture.componentInstance;    
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load astronaut duty on init', () => {
    expect(component.astronautDuty()).toStrictEqual({ person: person, astronautDuties: astronautDuties });
  });

  it('should load astronaut duty on refresh', () => {
    component.refresh$.next();    

    expect(component.astronautDuty()).toStrictEqual({ person: person, astronautDuties: [astronautDuties[0]] });
  });

  it('should set person duty add components visible property to true', () => {
    component.addDuty();

    expect(component.personDutyAdd.visible()).toBe(true);
  });
});
