import React, { useState } from 'react';

function AuditActionSingle({ action, onCompleteAction, onIncompleteAction, onDeleteAction }) {
  const [isComplete, setIsComplete] = useState(action.isComplete);

  function handleIncompleteAction(event, actionId) {
    event.preventDefault();
    setIsComplete(false);
    onIncompleteAction(actionId);
  }

  function handleCompleteAction(event, actionId) {
    event.preventDefault();
    setIsComplete(true);
    onCompleteAction(actionId);
  }

  function handleDeleteAction(event, actionId) {
    event.preventDefault();
    onDeleteAction(actionId);
  }

  return (
    <li className="list-group-item py-3">
      <div className="d-flex align-items-center justify-content-between mb-2">
        <p className="mb-0 text-break pe-2">{action.description}</p>
        <div className="btn-group" role="group" aria-label="Actions">
          {isComplete ?
            (<button type="button" className="btn btn-sm btn-outline-danger fill-danger" onClick={(event) => handleIncompleteAction(event, action.actionId)}>
              <svg xmlns="http://www.w3.org/2000/svg" aria-hidden="true" focusable="false" width="24" height="24" viewBox="0 0 24 24"><path d="m16.192 6.344-4.243 4.242-4.242-4.242-1.414 1.414L10.535 12l-4.242 4.242 1.414 1.414 4.242-4.242 4.243 4.242 1.414-1.414L13.364 12l4.242-4.242z"></path></svg>
              <span className="visually-hidden">Incomplete</span>
            </button>) :
            (<button type="button" className="btn btn-sm btn-outline-success fill-success" onClick={(event) => handleCompleteAction(event, action.actionId)}>
              <svg xmlns="http://www.w3.org/2000/svg" aria-hidden="true" focusable="false" width="24" height="24" viewBox="0 0 24 24"><path d="m10 15.586-3.293-3.293-1.414 1.414L10 18.414l9.707-9.707-1.414-1.414z"></path></svg>
              <span className="visually-hidden">Complete</span>
            </button>)
          }
          <button type="button" className="btn btn-sm btn-outline-danger fill-danger" onClick={(event) => handleDeleteAction(event, action.actionId)}>
            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M15 2H9c-1.103 0-2 .897-2 2v2H3v2h2v12c0 1.103.897 2 2 2h10c1.103 0 2-.897 2-2V8h2V6h-4V4c0-1.103-.897-2-2-2zM9 4h6v2H9V4zm8 16H7V8h10v12z"></path></svg>
            <span className="visually-hidden">Delete</span>
          </button>
        </div>
      </div>
      <small className="text-muted">{isComplete ? 'Complete' : 'Incomplete'}</small>
    </li>
  );
}

export { AuditActionSingle };
