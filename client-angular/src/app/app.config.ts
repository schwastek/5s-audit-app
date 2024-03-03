import { ApplicationConfig, importProvidersFrom } from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { authInterceptor } from './core/interceptors/jwt.interceptor';
import { ApiModule } from './api/api.module';
import { environment } from '../environments/environment';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideHttpClient(
      // The interceptors are chained together in the order they are listed.
      withInterceptors([authInterceptor])
    ),
    importProvidersFrom(ApiModule.forRoot({ rootUrl: environment.apiUrl }),)
  ]
};
