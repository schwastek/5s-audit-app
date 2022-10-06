interface Errors {
  shouldRender: boolean,
  message: string
};

function setErrors(errors: Errors, message: string) {
  errors['shouldRender'] = false;
  errors['message'] = message;
}

function getErrors(max: number, initial: number) {
  const errors = {
    shouldRender: true,
    message: ''
  };

  if (max < 1 || max > 5) {
    setErrors(errors, 'Invalid number of stars');
    return errors;
  }

  if (initial < 0 || initial > max) {
    setErrors(errors, 'Invalid initial value');
    return errors;
  }

  return errors;
}

export { getErrors };
