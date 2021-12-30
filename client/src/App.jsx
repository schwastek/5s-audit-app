import React, { useEffect } from 'react';
import {
  Switch,
  Route
} from 'react-router-dom';
import { PrivateRoute } from './components/PrivateRoute';
import { Header } from './components/Header';
import { Footer } from './components/Footer';
import { Login } from './pages/LoginPage';
import { NewAuditPage } from './pages/NewAuditPage';
import { BrowseAuditsPage } from './pages/BrowseAuditsPage';
import { AuditDetailsPage } from './pages/AuditDetailsPage';
import { NotFoundPage } from './pages/NotFoundPage';
import { useAuthDispatch } from './auth/context';
import { getCurrentUser } from './auth/actions';
import { getToken } from './auth/token';

function App() {
  const dispatch = useAuthDispatch();
  const isLoggedIn = getToken() !== null ? true : false;

  useEffect(() => {
    document.body.classList.add('bg-light');
  }, []);

  // When page is refreshed, store gets reset (hence, no user object in store).
  // Get current user when we render the app.
  useEffect(() => {
    async function getUser() {
      if (isLoggedIn) {
        await dispatch(getCurrentUser());
      }
    }

    getUser();
  }, [dispatch, isLoggedIn]);

  return (
    <>
      <Header />
        <Switch>
          <PrivateRoute exact path={['/', '/audits']}>
            <BrowseAuditsPage />
          </PrivateRoute>
          <PrivateRoute path="/audits/new">
            <NewAuditPage />
          </PrivateRoute>
          <PrivateRoute path="/audits/:id">
            <AuditDetailsPage />
          </PrivateRoute>
          <Route path="/login">
            <Login />
          </Route>
          <Route path="*">
            <NotFoundPage />
          </Route>
        </Switch>
      <Footer />
    </>
  );
}

export default App;
