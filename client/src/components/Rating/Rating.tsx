import React, { useState } from 'react';
import { Star } from './Star';
import { getErrors } from './getErrors';
import { RatingProps } from './interfaces';

function getTestId() {
  if (process.env.NODE_ENV === 'test') {
    return {
      'data-testid': 'rating'
    };
  }

  return {};
}

function Rating({
  name,
  max = 5,
  initial = 0,
  disabled = false,
  onRate
}: RatingProps) {
  const [clickedStar, setClickedStar] = useState(initial);
  const { shouldRender, message: renderErrorMessage } = getErrors(max, initial);

  if (!shouldRender) {
    console.error(renderErrorMessage);
    return null;
  }

  function handleRate(event: React.ChangeEvent<HTMLInputElement>) {
      if (disabled) return;
      let value = Number(event.target.value);
      setClickedStar(value);
      if (onRate) onRate(value);
  }

  function renderStars() {
    const stars = [];

    for (let i = 0; i < max; i++) {
      stars.push(renderStar(i));
    }

    return stars;
  }

  function renderStar(i: number) {
    const starValue = i + 1;

    // When you click 4th star, you want all 4 stars to be filled
    const isFilled = clickedStar >= starValue;

    // Is radio checked
    const isChecked = starValue === clickedStar;

    return (
      <Star
        key={i}
        name={name}
        value={starValue}
        isFilled={isFilled}
        isChecked={isChecked}
        isDisabled={disabled}
        onRate={handleRate}
      />
    );
  }

  return (
    <span {...getTestId()} className="rating-icons">
      {renderStars()}
    </span>
  );
}

export { Rating };
