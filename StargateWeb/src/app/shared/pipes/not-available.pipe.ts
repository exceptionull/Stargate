import { Pipe, PipeTransform } from "@angular/core";

@Pipe({  name: 'na',})
export class NotAvailablePipe implements PipeTransform {
    naValue = 'N/A';

    transform(value: any): string {
        if (typeof value === "string" && value.length === 0) {
            return this.naValue;
        }
        return value ?? this.naValue;
    }
}