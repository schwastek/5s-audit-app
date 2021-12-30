import { getToken } from './token';

const token = getToken();
const isLoggedIn = token === null ? false : true;

const initialState = {
  user: null,
  token: token,
  isLoggedIn: isLoggedIn
};

function authReducer(state, action) {
  switch (action.type) {
    case 'LOGIN_SUCCESS':
      return {
        ...state,
        user: action.payload.displayName,
        isLoggedIn: true,
        token: action.payload.token
      };

    case 'LOGOUT':
      return {
        ...state,
        user: null,
        isLoggedIn: false,
        token: null
      };

    case 'SET_CURRENT_USER':
      return {
        ...state,
        user: action.payload.displayName,
        isLoggedIn: true,
        token: action.payload.token
      };

    default:
      throw new Error(`Unhandled action type: ${action.type}`);
  }
};

export { authReducer, initialState };
