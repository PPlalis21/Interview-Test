import { bootstrapApplication } from '@angular/platform-browser';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { provideRouter } from '@angular/router';
import { routes } from './app/app.routes';
import { AppComponent } from './app/app.component';
import { apiKeyInterceptor } from './app/interceptors/api-key.interceptor';

bootstrapApplication(AppComponent, {
  providers: [
    provideHttpClient(withInterceptors([apiKeyInterceptor])),
    provideRouter(routes)
  ]
}).catch(err => console.error(err));
