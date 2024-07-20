import { Directive, TemplateRef } from '@angular/core';

@Directive({
  standalone: true,
  selector: '[appRatingTemplate]'
})
export class RatingTemplateDirective {
  constructor(public templateRef: TemplateRef<unknown>) { }
}
