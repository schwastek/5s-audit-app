import React from 'react';
import { classNames } from '../../utilities/classNames';
import { StarProps } from './interfaces';

function getLabelText(i: number) {
  return `${i} Star${i !== 1 ? 's' : ''}`;
}

function getTestId(value: number) {
  if (process.env.NODE_ENV === 'test') {
    return {
      'data-testid': `rating-star-${value}`
    };
  }

  return {};
}

function Star({
  name,
  value,
  isFilled,
  isChecked,
  isDisabled,
  onRate
}: StarProps) {
  const labelText = getLabelText(value);
  const id = `${name}-${value}`;

  const classes = classNames([
    'rating-icon',
    {
      'rating-icon-filled': (isFilled && !isDisabled),
      'rating-icon-filled-disabled': (isFilled && isDisabled),
      'rating-icon-empty': (!isFilled && !isDisabled),
      'rating-icon-empty-disabled': (!isFilled && isDisabled),
    }
  ]);

  return (
    <div {...getTestId(value)}>
      <label htmlFor={id}>
        <span className={classes}>
          <svg className="rating-icon-svg" focusable="false" viewBox="0 0 24 24" aria-hidden="true">
            <path d="M12 17.27L18.18 21l-1.64-7.03L22 9.24l-7.19-.61L12 2 9.19 8.63 2 9.24l5.46 4.73L5.82 21z"></path>
          </svg>
        </span>
        <span className="visually-hidden">{labelText}</span>
      </label>
      <input type="radio" value={value} id={id} name={name} className="visually-hidden" onChange={onRate} checked={isChecked} disabled={isDisabled}></input>
    </div>
  );
}

export { Star };
