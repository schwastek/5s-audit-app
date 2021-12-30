import React from 'react';
import ReactDOM from 'react-dom';
import { Router } from 'react-router-dom';
import { routerHistory } from './routerHistory';
import { AuthProvider } from './auth/context';
import './css/App.scss';
import App from './App';
import reportWebVitals from './reportWebVitals';

ReactDOM.render(
  <React.StrictMode>
    <AuthProvider>
      <Router history={routerHistory}>
        <App />
      </Router>
    </AuthProvider>
  </React.StrictMode>,
  document.getElementById('root')
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
