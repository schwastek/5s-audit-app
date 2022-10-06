import React from 'react';

export interface RatingProps {
  name?: string;
  max?: number;
  initial?: number;
  disabled?: boolean;
  onRate?: (value: number) => void;
}

export interface StarProps {
  name?: string;
  value: number;
  isFilled: boolean;
  isChecked: boolean;
  isDisabled: boolean;
  onRate: (event: React.ChangeEvent<HTMLInputElement>) => void;
}

