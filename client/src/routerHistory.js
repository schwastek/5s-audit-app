import { createBrowserHistory } from 'history';

// Share the same, custom history object across the app.
// This way, we can access the history object outside of a React component.
const routerHistory = createBrowserHistory();

export { routerHistory };
