import { instance } from './axios';

function list() {
  return instance.get('/questions').then((response) => response.data);
};

export const Questions = {
  list
};
