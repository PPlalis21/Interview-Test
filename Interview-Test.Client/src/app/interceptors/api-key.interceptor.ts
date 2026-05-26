import { HttpInterceptorFn } from '@angular/common/http';

const X_API_KEY = 'interview-test-api-key';

export const apiKeyInterceptor: HttpInterceptorFn = (req, next) => {
  const cloned = req.clone({
    setHeaders: { 'x-api-key': X_API_KEY }
  });
  return next(cloned);
};
