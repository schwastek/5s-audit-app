import axios from 'axios';
import { PaginatedResult } from '../models/PaginatedResult';
import { getToken, removeToken } from '../auth/token';
import { wait } from '../utilities/wait';

const instance = axios.create({
  baseURL: process.env.REACT_APP_API_URL,
});

instance.defaults.headers.common['Content-Type'] = 'application/json';
instance.defaults.headers.common['Accept'] = 'application/json';

instance.interceptors.request.use(function(config) {
  const token = getToken();
  if (token) config.headers.Authorization = `Bearer ${token}`;

  return config;
});

instance.interceptors.response.use(async response => {
  if (process.env.NODE_ENV === 'development') await wait(1000);

  const hasPaginationHeader = response.headers['x-pagination'] !== undefined;
  if (hasPaginationHeader) {
    const paginationHeader = JSON.parse(response.headers['x-pagination']);
    response.data = new PaginatedResult(response.data, paginationHeader);

    return response;
  }

  return response;
}, (error) => {
  if (error.response) {
    // Request was made and the server responded with a status code
    // that falls out of the range of 2xx

    const { status, headers } = error.response;

    // Access token is expired
    if (status === 401 && headers['www-authenticate']?.startsWith('Bearer error="invalid_token"')) {

      // Logout
      removeToken();

      return Promise.reject('Session expired, please login again.');
    }
  } else if (error.request) {
    // The request was made but no response was received (e.g. API server is turned off)

    console.error(error.message);
  } else {
    // Something happened in setting up the request that triggered an Error

    console.error('Error', error.message);
  }

  return Promise.reject(error);
});

export { instance };
