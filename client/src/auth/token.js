import { Account } from '../api/Account';

function getToken() {
  const token = localStorage.getItem('jwt');

  return token;
};

function saveToken(token) {
  localStorage.setItem('jwt', token);
};

function removeToken() {
  localStorage.removeItem('jwt');
};

let refreshTokenTimeout = null;

async function refreshToken() {
  stopRefreshTokenTimer();

  try {
      const user = await Account.refreshToken();
      saveToken(user.token);
      startRefreshTokenTimer(user.token);
  } catch (error) {
      throw(error);
  }
}

function stopRefreshTokenTimer() {
  clearTimeout(refreshTokenTimeout);
}

function startRefreshTokenTimer(token) {
  const payload = JSON.parse(Buffer.from(token.split('.')[1], 'base64').toString());
  const expires = new Date(payload.exp * 1000);

  // Refresh token just before it expires
  const timeout = expires.getTime() - Date.now() - (10 * 1000);
  refreshTokenTimeout = setTimeout(refreshToken, timeout);
}

export { getToken, saveToken, removeToken, startRefreshTokenTimer };
