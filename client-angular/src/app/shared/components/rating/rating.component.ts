import { CommonModule, NgTemplateOutlet } from '@angular/common';
import { Component, ContentChild, EventEmitter, forwardRef, Input, OnChanges, OnInit, Output, SimpleChanges, TemplateRef } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { RatingTemplateDirective } from './templates.directive';
import { noop } from 'rxjs';

export interface GetLabelTextFn {
  (starValue: number): string;
}

// Register the custom form control as a known value accessor in the dependency injection system.
// All value accessors within Angular Forms are registered
// with the `NG_VALUE_ACCESSOR` injection token (hence, `multi: true`).
// Use `forwardRef` when a reference to a value is needed before it is defined.
export const RATING_VALUE_ACCESSOR = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => RatingComponent),
  multi: true
};

// A radio group with its fields visually hidden. It contains six radio buttons by default,
// one for each star, and another for 0 stars that is checked by default.
// See: https://www.w3.org/WAI/tutorials/forms/custom-controls/#a-star-rating
@Component({
  selector: 'app-rating',
  templateUrl: './rating.component.html',
  styleUrls: ['./rating.component.scss'],
  providers: [RATING_VALUE_ACCESSOR],
  standalone: true,
  imports: [CommonModule, NgTemplateOutlet, RatingTemplateDirective]
})
export class RatingComponent implements ControlValueAccessor, OnInit, OnChanges {

  // The current rating. Useful if the rating component is not controlled by Angular Forms (e.g. via `formControlName`).
  @Input() rate: number = 0;

  // Number of stars.
  @Input() max: number = 5;

  // If `true`, the rating can't be changed or focused.
  @Input() isDisabled: boolean = false;

  // If `true`, the rating can't be changed.
  // Note: Input elements of type `radio` does not have a `readonly` attribute.
  @Input() isReadOnly: boolean = false;

  // Returns the label text for the current rating value, e.g. "2 Stars".
  @Input() getLabelTextFn?: GetLabelTextFn;

  // CSS classes for the element wrapping the rate icon when filled.
  @Input() starFilledClass?: string;

  // CSS classes for the element wrapping the rate icon when empty.
  @Input() starEmptyClass?: string;

  // Emitted when the rating is changed.
  @Output() rateChange = new EventEmitter<number>();

  // The template to override the rating icon.
  // Use `<ng-template [ratingTemplate]>` (custom directive) as the child of this rating component.
  @ContentChild(RatingTemplateDirective, { read: TemplateRef, static: false }) ratingTemplate?: TemplateRef<unknown>;

  stars: number[] = [];
  name: string = 'rating';
  onChange: (value: number) => void = noop;
  onTouched: () => void = noop;

  get isInteractive(): boolean {
    return !this.isDisabled && !this.isReadOnly;
  }

  ngOnInit(): void {
    // Generate random name for this radio group, e.g. "rating-ez8l7i".
    this.name = `${this.name}-${Math.random().toString(36).slice(6)}`;
    this.updateNumberOfStars();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['max']) {
      this.updateNumberOfStars();
    }
  }

  // Called by the parent form to set a value in the child control.
  writeValue(value: unknown): void {
    if (typeof value === 'number') {
      this.rate = value;
      return;
    }

    this.rate = 0;
  }

  // The child control can notify the parent form that a new value is available via the callback function.
  registerOnChange(fn: (fn: number) => void): void {
    this.onChange = fn;
  }

  // The child control can notify its touched status back to the parent form via the callback function.
  registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }

  // The parent form can enable or disable any of its child controls by calling the `setDisabledState` method.
  setDisabledState?(isDisabled: boolean): void {
    this.isDisabled = isDisabled;
  }

  updateNumberOfStars(): void {
    // Length = number of stars.
    // Value = star's value.
    this.stars = [];

    for (let i = 0; i < this.max; i++) {
      this.stars[i] = (i + 1);
    }
  }

  onRateChange($event: Event): void {
    if (!this.isInteractive) return;

    const value: string = ($event.target as HTMLInputElement).value;
    this.updateRating(Number(value));
  }

  updateRating(value: number): void {
    if (!this.isInteractive) return;

    this.rate = value;
    this.rateChange.emit(this.rate);
    this.onChange(this.rate);
    this.onTouched();
  }

  getCssClassesFor(value: number): { [key: string]: boolean } {
    const starFilledClass = this.starFilledClass || 'rating-icon-filled';
    const starEmptyClass = this.starEmptyClass || 'rating-icon-empty';

    // Currently selected star >= passed star's value? Fill the passed star.
    const isFilled = this.rate >= value;

    return {
      [starFilledClass]: isFilled,
      [starEmptyClass]: !isFilled
    };
  }

  getLabelFor(value: number): string {
    return `${this.name}-${value}`;
  }

  getLabelText(value: number): string {
    if (this.getLabelTextFn) {
      return this.getLabelTextFn(value);
    }

    const isPlural = value != 1;

    // => "0 Stars", "1 Star"
    return `${value} Star${isPlural ? 's' : ''}`;
  }
}
