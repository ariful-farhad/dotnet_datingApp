import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { NavigationExtras, Router } from '@angular/router';
import { ToastrModule, ToastrService } from 'ngx-toastr';
import { catchError } from 'rxjs';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);
  const toastr = inject(ToastrService);
  return next(req).pipe(
    catchError((error) => {
      if (error) {
        switch (error.status) {
          case 400:
            // normally it is a validation error when we are inputting wrong password or uname, so we want to add the
            // error messages in a array,
            if (error.error.errors) {
              const modalStateErrors = [];
              for (const key in error.error.errors) {
                if (error.error.errors[key]) {
                  modalStateErrors.push(error.error.errors[key]);
                }
              }
              throw modalStateErrors.flat();
            } else {
              toastr.error(error.error, error.status);
            }
            break;
          case 401:
            toastr.error('unauthorised', error.status);
            break;
          case 404:
            router.navigateByUrl('/not-found');
            break;
          case 500:
            // we want to send the status as well of the error, to show something when we navigate to the page
            const navigationExtras: NavigationExtras = {
              state: { error: error.error },
            };

            router.navigateByUrl('/server-error', navigationExtras);
            break;
          default:
            toastr.error('something unexpected went wrong');
            break;
        }
      }
      throw error;
    })
  );
};
