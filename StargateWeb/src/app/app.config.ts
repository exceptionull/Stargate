import { ApplicationConfig, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideDefaultClient } from './api';
import { providePrimeNG } from 'primeng/config';
import Aura from '@primeuix/themes/aura';
import { MessageService } from 'primeng/api';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes),
    provideDefaultClient({ basePath: 'http://localhost:5204' }),
    providePrimeNG({
        theme: {
            preset: Aura,
            options: {
                cssLayer: {
                    name: 'primeng',
                    order: 'theme, base, primeng'
                }
            }
        }
    }),
    MessageService
  ]
};
