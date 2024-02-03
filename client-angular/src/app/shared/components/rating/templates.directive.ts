import { Directive, TemplateRef } from '@angular/core';

@Directive({
  standalone: true,
  selector: '[ratingTemplate]'
})
export class RatingTemplateDirective {
  constructor(public templateRef: TemplateRef<unknown>) { }
}
