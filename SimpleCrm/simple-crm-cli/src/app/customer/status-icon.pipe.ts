import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'statusIcon'
})
export class StatusIconPipe implements PipeTransform {
  transform(value: unknown, ...args: unknown[]): string {
    if (value === 'Prospect') {
      return 'question';
    }
    if (value === 'Confirmed') {
      return 'shield';
    }
    return 'user';
  }
}