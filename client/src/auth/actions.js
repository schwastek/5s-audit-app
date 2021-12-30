import { Account } from '../api/Account';
import { saveToken, removeToken, startRefreshTokenTimer } from './token';

function login({username, password}) {
  return async function loginThunk(dispatch) {
    try {
      const user = await Account.login({email: username, password});
      saveToken(user.token);
      startRefreshTokenTimer(user.token);
      dispatch({ type: 'LOGIN_SUCCESS', payload: user});
    } catch (error) {
      throw(error);
    }
  };
}

function logout() {
  return function logoutThunk(dispatch) {
    removeToken();
    dispatch({ type: 'LOGOUT' });
  };
}

function getCurrentUser() {
  return async function getCurrentUserThunk(dispatch) {
    try {
      const user = await Account.current();
      saveToken(user.token);
      startRefreshTokenTimer(user.token);
      dispatch({ type: 'SET_CURRENT_USER', payload: user});
    } catch (error) {
      console.error('Couldn\'t get current user:', error.message);
    }
  };
}

export {
  login,
  logout,
  getCurrentUser
};
