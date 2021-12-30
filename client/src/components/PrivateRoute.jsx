import { Redirect, Route } from 'react-router-dom';
import { getToken } from '../auth/token';

function PrivateRoute({ children, ...rest }) {
  const isLoggedIn = getToken() !== null ? true : false;

  // Pass current `location` to redirect user later,
  // to the initial page he's trying to access.
  return (
    <Route
      {...rest}
      render={({ location }) =>
        isLoggedIn
        ? (children)
        : (<Redirect
            to={{
              pathname: '/login',
              state: { from: location }
            }}
          />)
      }
    />
  );
}

export { PrivateRoute };
