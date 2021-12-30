import React from 'react';

function Question(props) {
  const {
    text,
    children
  } = props;

  return (
    <div className="question mb-3">
      <span>{text}</span>
      {children}
    </div>
  );
}

export { Question };
