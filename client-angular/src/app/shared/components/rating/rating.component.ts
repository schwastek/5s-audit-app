import {CommonModule, NgTemplateOutlet} from '@angular/common';
import {Component, ContentChild, EventEmitter, forwardRef, Input, OnChanges, OnInit, Output, SimpleChanges, TemplateRef} from '@angular/core';
import {ControlValueAccessor, NG_VALUE_ACCESSOR} from '@angular/forms';

export interface GetLabelTextFn extends Function {
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
}

// A radio group with its fields visually hidden. It contains six radio buttons by default,
// one for each star, and another for 0 stars that is checked by default.
// See: https://www.w3.org/WAI/tutorials/forms/custom-controls/#a-star-rating
@Component({
  selector: 'app-rating',
  standalone: true,
  imports: [CommonModule, NgTemplateOutlet],
  templateUrl: './rating.component.html',
  styleUrls: ['./rating.component.scss'],
  providers: [RATING_VALUE_ACCESSOR]
})
export class RatingComponent implements ControlValueAccessor, OnInit, OnChanges {

  // Number of stars.
  @Input() max: number = 5;

  // If `true`, the rating can't be changed or focused.
  @Input() isDisabled: boolean = false;

  // If `true`, the rating can't be changed.
  // Note: Input elements of type `radio` does not have a `readonly` attribute.
  @Input() isReadOnly: boolean = false;

  // Returns the label text for the current rating value, e.g. "2 Stars".
  @Input({alias: 'getLabelText'}) getLabelTextCustomFn?: GetLabelTextFn;

  // CSS classes for the element wrapping the rate icon when filled.
  @Input() starFilledClass?: string;

  // CSS classes for the element wrapping the rate icon when empty.
  @Input() starEmptyClass?: string;

  // Emitted when the rating is changed.
  @Output() rateChange = new EventEmitter<number>();

  // The template to override the star icon.
  // Put an <ng-template> as the only child of this rating component.
  @ContentChild(TemplateRef, { static: false }) starTemplate?: TemplateRef<any>;

  value: number = 0;
  stars: number[] = [];
  name: string = 'rating';
  onChange: Function = (_: any): void => {};
  onTouched: Function = (): void => {};

  get isInteractive(): boolean {
    return !this.isDisabled && !this.isReadOnly;
  }

  ngOnInit(): void {
    this.updateNumberOfStars();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['max']) {
      this.updateNumberOfStars();
    }
  }

  // Called by the parent form to set a value in the child control.
  writeValue(value: any): void {
    if (!value) value = 0;
    this.value = value;
  }

  // The child control can notify the parent form that a new value is available via the callback function.
  registerOnChange(fn: Function): void {
    this.onChange = fn;
  }

  // The child control can notify its touched status back to the parent form via the callback function.
  registerOnTouched(fn: Function): void {
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

  onRateChange($event: Event) {
    const value: string = ($event.target as HTMLInputElement).value;
    this.updateRating(Number(value));
  }

  updateRating(value: number): void {
    if (!this.isInteractive) return;

    this.value = value;
    this.rateChange.emit(this.value);
    this.onChange(this.value);
    this.onTouched();
  }

  getCssClassesFor(value: number): { [key: string]: boolean } {
    const starFilledClass = this.starFilledClass || 'rating-icon-filled';
    const starEmptyClass = this.starEmptyClass || 'rating-icon-empty';

    // Currently selected star >= passed star's value? Fill the passed star.
    let isFilled = this.value >= value;

    return {
      [starFilledClass]: isFilled,
      [starEmptyClass]: !isFilled
    }
  }

  getLabelFor(value: number): string {
    return `${this.name}-${value}`;
  }

  getLabelText(value: number): string {
    if (this.getLabelTextCustomFn) {
      return this.getLabelTextCustomFn(value);
    }

    const isPlural = value != 1;

    // => "0 Stars", "1 Star"
    return `${value} Star${isPlural ? 's' : ''}`;
  }
}
