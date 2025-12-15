import { ComponentFixture, TestBed } from '@angular/core/testing';
import { PersonHome } from './person-home';
import { GetPeopleResult, PersonAstronaut, PersonService } from '../../../api';
import { of } from 'rxjs';
import { provideRouter } from '@angular/router';
import { MessageService } from 'primeng/api';

describe('PersonHome', () => {
  let fixture: ComponentFixture<PersonHome>;
  let component: PersonHome;

  let personService: PersonService;

  const people = [{ name: 'Jon Doe' }, { name: 'Jane Doe' }] as PersonAstronaut[]

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PersonHome],
      providers: [provideRouter([]), MessageService],
    })
    .compileComponents();

    personService = TestBed.inject(PersonService);
    vi.spyOn(personService, 'personGet')
      .mockReturnValueOnce(of({ people: people } as GetPeopleResult) as any)
      .mockReturnValueOnce(of({ people: [people[0]] } as GetPeopleResult) as any);

    fixture = TestBed.createComponent(PersonHome);

    fixture.autoDetectChanges();

    component = fixture.componentInstance;    
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load people on init', () => {
    expect(component.people()).toStrictEqual(people);
  });

  it('should load people on refresh', () => {
    component.refresh$.next();    

    expect(component.people()).toStrictEqual([people[0]]);
  });

  it('should set person add components visible property to true', () => {
    component.addPerson();

    expect(component.personAdd.visible()).toBe(true);
  });

  it('should set person edit components visible property to true and form values', () => {
    const name = 'Mike Doe';
    component.editPerson(name);

    expect(component.personEdit.visible()).toBe(true);
    expect(component.personEdit.form.controls.name.value).toBe(name);
    expect(component.personEdit.form.controls.newName.value).toBe(name);
  });
});
