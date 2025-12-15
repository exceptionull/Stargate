import { FormControl } from "@angular/forms";

  export function isInvalid(control: FormControl, formSubmitted: boolean) {
      return control?.invalid && (control.touched || formSubmitted);
  }