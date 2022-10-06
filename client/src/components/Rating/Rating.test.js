import React from 'react';
import { render, screen, within } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import '@testing-library/jest-dom';
import { Rating } from './Rating';

beforeEach(() => {

  // When the error's thrown a bunch of console.errors are called even though
  // the error boundary handles the error. This makes the test output noisy,
  // so we'll mock out console.error
  // Source: https://codesandbox.io/s/github/kentcdodds/react-testing-library-examples?
  jest.spyOn(console, 'error');
  console.error.mockImplementation(() => {});
});

afterEach(() => {
  console.error.mockRestore();
});

it('Should render', () => {
  render(<Rating name='rating' />);
  const item = screen.getByTestId('rating');

  expect(item).toBeInTheDocument();
});

it('Should render 3 stars', () => {
  render(<Rating name={'rating'} max={3} />);
  const inputs = screen.queryAllByRole('radio');

  expect(inputs).toHaveLength(3);
});

it('Should apply CSS classes', async () => {
  const { container } = render(<Rating name={'rating'} initial={3} max={5} />);
  const active = container.querySelectorAll('.rating-icon-filled');
  const inactive = container.querySelectorAll('.rating-icon-empty');

  expect(active).toHaveLength(3);
  expect(inactive).toHaveLength(2);
});

it('Should select the rating', async () => {
  const user = userEvent.setup();
  const handleRate = jest.fn();
  render(<Rating name={'rating'} initial={3} max={5} onRate={handleRate} />);

  const label = screen.getByLabelText('2 Stars');
  await user.click(label);
  expect(handleRate).toBeCalledTimes(1);
  expect(handleRate).toBeCalledWith(2);

  const item = screen.getByTestId('rating-star-2');
  const input = within(item).getByRole('radio');
  expect(input).toBeChecked();
});

it('Should select the initial rating', () => {
  render(<Rating name={'rating'} initial={3} max={5} />);
  const item = screen.getByTestId('rating-star-3');
  const input = within(item).getByRole('radio');

  expect(input).toBeChecked();
});

it('Should ensure a `name` attribute', () => {
  render(<Rating name={'rating'} initial={3} max={5} />);
  const items = screen.getAllByRole('radio');

  expect(items[0]).toHaveAttribute('name', 'rating');
});

it('Should have accessibility label', () => {
  render(<Rating name={'rating'} max={5} />);
  const item = screen.getByTestId('rating-star-3');
  const input = within(item).getByRole('radio');

	expect(input).toHaveAccessibleName('3 Stars');
});

it('Should not render if max stars greater than 5', () => {
  render(<Rating name={'rating'} max={6} />);
	const item = screen.queryByTestId('rating');

	expect(item).not.toBeInTheDocument();
});

it('Should not render if initial greater than max stars', () => {
  render(<Rating name={'rating'} max={5} initial={6} />);
	const item = screen.queryByTestId('rating');

	expect(item).not.toBeInTheDocument();
});







