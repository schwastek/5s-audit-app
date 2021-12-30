import React from 'react';
import { useReducerWithThunk } from '../hooks/useReducerWithThunk';
import { authReducer, initialState } from './reducer';

// Contains the authentication token and user details
const StateContext = React.createContext();

// Passes the dispatch method given to us by the useReducer()
const DispatchContext = React.createContext();

function useAuthState() {
  const context = React.useContext(StateContext);
  if (context === undefined) {
    throw new Error('useAuthState must be used within a AuthProvider');
  }

  return context;
}

function useAuthDispatch() {
  const context = React.useContext(DispatchContext);
  if (context === undefined) {
    throw new Error('useAuthDispatch must be used within a AuthProvider');
  }

  return context;
}

function AuthProvider({ children }) {
  const [state, dispatch] = useReducerWithThunk(authReducer, initialState);

  return (
    /**
     * Don't use inline object for `value` prop `value={{state, dispatch}}`. Because:
     * `App` re-renders -> new object on the `value` prop -> any component consuming that context also re-renders.
     * Use `useMemo` to memoize the value given to the provider.
     * Or make two separate contexts to provide state and dispatch independently.
     */
    <DispatchContext.Provider value={dispatch}>
      <StateContext.Provider value={state}>
        {children}
      </StateContext.Provider>
    </DispatchContext.Provider>
  );
};

export { AuthProvider, useAuthState, useAuthDispatch };
