import { Component, Inject } from '@angular/core';
import { MAT_SNACK_BAR_DATA } from '@angular/material/snack-bar';

@Component({
  selector: 'app-error-snackbar',
  templateUrl: './error-snackbar.html',
  styleUrl: './error-snackbar.scss',
})
export class ErrorSnackbar {
  constructor(@Inject(MAT_SNACK_BAR_DATA) public data: string) {}
}
