export function isDefined(value: any): boolean {
  return value !== undefined && value !== null;
}

// Utility functions copied from: https://github.com/primefaces/primeng/blob/f48e9767c65d3d55b0c9f78c86b078ff6f48e616/src/app/components/utils/objectutils.ts
// Copyright (c) 2016-2022 PrimeTek
// License: MIT
export function isDate(input: any): boolean {
  return Object.prototype.toString.call(input) === '[object Date]';
}

export function isEmpty(value: any): boolean {
  return value === null || value === undefined || value === '' || (Array.isArray(value) && value.length === 0) || (!isDate(value) && typeof value === 'object' && Object.keys(value).length === 0);
}

export function isNotEmpty(value: any): boolean {
  return !isEmpty(value);
}