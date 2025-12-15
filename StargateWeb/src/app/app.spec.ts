import { TestBed } from '@angular/core/testing';
import { App } from './app';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';

describe('App', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [App, ToastModule],
      providers: [MessageService]
    }).compileComponents();
  });

  it('should create', () => {
    const fixture = TestBed.createComponent(App);
    const app = fixture.componentInstance;
    expect(app).toBeTruthy();
  });
});
