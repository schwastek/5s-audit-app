import React, { useState } from 'react';
import { Star } from './Star';

function Rating(props) {
  const {
    name,
    max = 5,
    initial = 0,
    disabled = false,
    onRate
  } = props;

  const [clickedStar, setClickedStar] = useState(initial);

  function handleChange(e, value) {
    if (disabled) return;
    setClickedStar(value);
    if (onRate) onRate(value);
  }

  function renderStar(i) {
    const starValue = i + 1;

    // When you click 4th star, you want all 4 stars to be selected
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
        onChange={(e) => handleChange(e, starValue)}
      >
      </Star>
    );
  }

  return (
    <span className="rating-icons">
      {Array.from(new Array(max)).map((x, i) =>
        renderStar(i)
      )}
    </span>
  );
}

export { Rating };
