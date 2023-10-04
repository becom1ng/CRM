import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'statusIcon'
})
export class StatusIconPipe implements PipeTransform {
  transform(value: unknown, ...args: unknown[]): string {
    if (typeof value !== "string") { return 'user'; }
    if (value.search(/prospect/i) === 0) {
      return 'question';
    }
    if (value.search(/purchased/i) === 0) {
      return 'shield';
    }
    return 'user';
  }
}