import React, { useState } from 'react';
import { Redirect, useLocation } from 'react-router-dom';
import { useAuthDispatch } from '../auth/context';
import { login as loginUser } from '../auth/actions';
import { useIsMounted } from '../hooks/useIsMounted';
import { getToken } from '../auth/token';

function Login() {
  const location = useLocation();
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [errorMessage, setErrorMessage] = useState('');
  const [isSubmitting, setIsSubmitting] = useState(false);
  const dispatch = useAuthDispatch();
  const isMounted = useIsMounted();


  // After user is succesfully logged in,
  // redirect him to the page he wanted access initially.
  const isLoggedIn = getToken() !== null ? true : false;
  if (isLoggedIn === true) {
    let { from } = location.state || { from: { pathname: '/' } };
    return <Redirect to={from} />;
  }

  async function handleLogin(event) {
    event.preventDefault();
    setIsSubmitting(true);
    setErrorMessage('');

    try {
      await dispatch(loginUser({username, password}));

      // Don't perform state update on an unmounted component (user is redirected when logged in)
      if (isMounted()) {
        setIsSubmitting(false);
      }
    } catch (error) {
      if (isMounted()) {
        setIsSubmitting(false);

        // TODO Login page shouldn't be concerned with the shape Axios error object
        let message = error.message;
        if (error.response?.status === 401) {
          message = 'Wrong email or password';
        }

        setErrorMessage(message);
      }
    }
  }

  function onChangeUsername(event) {
    const username = event.target.value;
    setUsername(username);
  }

  function onChangePassword(event) {
    const password = event.target.value;
    setPassword(password);
  }

  function renderLoginButton() {
    if (isSubmitting) {
      return (
        <button type="submit" className="btn btn-primary w-100" disabled>
          <span className="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>
          <span className="sr-only">Signing in...</span>
        </button>
      );
    }

    return (
      <button type="submit" className="btn btn-primary w-100">Sign In</button>
    );
  }

  return (
      <div className="container py-5">
        <div className="row align-items-center">
          <div className="col-lg-7 text-center text-lg-start">
            <h1 className="display-4 fw-bold lh-1 mb-3">5S audits, more efficient operations</h1>
            <p className="col-lg-10 fs-4">
              In simple terms, 5S methodology helps keep the workplace organized and clean.<br/>
              This app allows you perform an audit to ensure the 5S approach is followed.
            </p>
          </div>

          <div className="col-md-10 mx-auto col-lg-5">
            <div className="bg-white p-5 rounded shadow-sm">
              <h2 className="mb-4">Sign In</h2>
              {errorMessage !== '' &&
                <div className="alert alert-danger" role="alert">
                  {errorMessage}
                </div>
              }
              <form onSubmit={handleLogin}>
                <div className="form-group mb-3">
                  <label htmlFor="inputEmail" className="form-label">Email address</label>
                  <input type="email" onChange={onChangeUsername} disabled={isSubmitting} className="form-control" id="inputEmail" placeholder="Enter email" required />
                </div>
                <div className="form-group mb-3">
                  <label htmlFor="inputPassword" className="form-label">Password</label>
                  <input type="password" onChange={onChangePassword} disabled={isSubmitting} className="form-control" id="inputPassword" placeholder="Password" required />
                </div>
                <div className="form-group">
                  {renderLoginButton()}
                </div>
              </form>
            </div>
          </div>
        </div>
      </div>
  );
}

export { Login };
