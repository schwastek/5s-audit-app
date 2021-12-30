import React from 'react';
import { NavLink, useHistory } from 'react-router-dom';
import { logout } from '../auth/actions';
import { useAuthState, useAuthDispatch } from '../auth/context';
import { classNames } from '../utilities/classNames';

function Header() {
  const history = useHistory();
  const dispatch = useAuthDispatch();
  const { user } = useAuthState();

  function handleLogout() {
    dispatch(logout());
    history.push('/');
  }

  // Don't display secondary navigation when user is not logged in
  const secondaryNavClasses = classNames([
    'nav-scroller bg-body shadow-sm',
    {
      'd-none': (user ? false : true)
    }
  ]);

  return (
    <>
      <nav className="navbar navbar-expand navbar-dark bg-dark" aria-label="Main navigation">
        <div className="container">

          <NavLink to="/" className="navbar-brand fw-bold d-flex align-items-center">
            <svg xmlns="http://www.w3.org/2000/svg" className="me-1" width="24" height="24" viewBox="0 0 24 24" style={{fill: 'rgba(255,255,255)'}}>
              <path d="m2.394 13.742 4.743 3.62 7.616-8.704-1.506-1.316-6.384 7.296-3.257-2.486zm19.359-5.084-1.506-1.316-6.369 7.279-.753-.602-1.25 1.562 2.247 1.798z"></path>
            </svg>
            5S Audit App
          </NavLink>
          {user &&
            <div className="d-flex align-items-center">
              <span className="text-light me-2">Hello, <strong>{user}</strong></span>
              <button type="button" onClick={handleLogout} className="btn btn-outline-light">Logout</button>
            </div>
          }
        </div>
      </nav>

        <div className={secondaryNavClasses}>
          <nav className="nav nav-underline container" aria-label="Secondary navigation">
            {user &&
              <>
                <NavLink exact to="/audits" className="nav-link" activeClassName="active">Browse</NavLink>
                <NavLink to="/audits/new" className="nav-link" activeClassName="active">New</NavLink>
              </>
            }
          </nav>
        </div>
    </>
  );
}

export { Header };
