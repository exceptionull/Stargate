import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { MenubarModule } from 'primeng/menubar';
import { PrimeIcons, MenuItem } from 'primeng/api';
import { ToastModule } from 'primeng/toast';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, MenubarModule, ToastModule],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  items: MenuItem[] = [
    {
        label: 'People',
        icon: PrimeIcons.USERS,
        routerLink: './'
        
    },
  ];
}
