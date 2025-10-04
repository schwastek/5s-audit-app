import { Directive, inject, TemplateRef } from '@angular/core';

@Directive({
  selector: '[appRatingTemplate]'
})
export class RatingTemplateDirective {
  public readonly template = inject(TemplateRef<unknown>);
}
