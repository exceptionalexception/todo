import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class DateValidationService {

  constructor() { }

  isValidDate(value: any): boolean {
    if (value === null || value === undefined) {
      return false;
    }
  
    const date = new Date(value);
    return !isNaN(date.getTime());
  }
}
