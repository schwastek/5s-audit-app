import { instance } from './axios';

function list(controller) {
  return instance.get('/questions', { signal: controller.signal }).then((response) => response.data);
};

export const Questions = {
  list
};
