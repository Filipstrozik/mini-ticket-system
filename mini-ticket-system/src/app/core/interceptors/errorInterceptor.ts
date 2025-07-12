import {
  HttpErrorResponse,
  HttpInterceptor,
  HttpInterceptorFn,
} from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { catchError, throwError } from 'rxjs';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const snackBar = inject(MatSnackBar);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      const message =
        error?.error?.message || error?.message || 'Unexpected error';
      snackBar.open(message, 'Close', {
        duration: 4000,
        panelClass: ['snackbar-error'], // optionally styled
      });

      return throwError(() => error);
    })
  );
};
