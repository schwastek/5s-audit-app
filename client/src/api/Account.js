import { instance } from './axios';

function login(user) {
  return instance.post('/account/login', user).then((response) => response.data);
};

function current() {
  return instance.get('/account').then((response) => response.data);
}

function refreshToken() {
  return instance.post('/account/refreshtoken', {}).then((response) => response.data);
}

export const Account = {
  login,
  current,
  refreshToken
};
