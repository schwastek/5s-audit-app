export function isDefined(value: unknown): boolean {
  return value !== undefined && value !== null;
}

// Helper method for creating a range of numbers
// range(1, 5) => [1, 2, 3, 4, 5]
export function range(from: number, to: number, step = 1): number[] {
  let i = from;
  const range = [];

  while (i <= to) {
    range.push(i);
    i += step;
  }

  return range;
}

// Utility functions copied from: https://github.com/primefaces/primeng/blob/f48e9767c65d3d55b0c9f78c86b078ff6f48e616/src/app/components/utils/objectutils.ts
// Copyright (c) 2016-2022 PrimeTek
// License: MIT
export function isDate(input: unknown): boolean {
  return Object.prototype.toString.call(input) === '[object Date]';
}

export function isEmpty(value: unknown): boolean {
  return value === null || value === undefined || value === '' || (Array.isArray(value) && value.length === 0) || (!isDate(value) && typeof value === 'object' && Object.keys(value).length === 0);
}

export function isNotEmpty(value: unknown): boolean {
  return !isEmpty(value);
}
