import { Routes } from '@angular/router';
import { PersonHome } from './features/person/person-home/person-home';
import { PersonView } from './features/person/person-view/person-view';

export const routes: Routes = [
    {    path: '',    component: PersonHome,  },
    {    path: 'person/:name',    component: PersonView,  },
];
