import React from 'react';

function AuditActionSingle({ action, onDeleteAction }) {
  function handleDeleteAction(actionId, event) {
    event.preventDefault();
    onDeleteAction(actionId);
  }

  return (
    <li className="list-group-item py-3">
      <div className="d-flex align-items-center justify-content-between">
        <p className="mb-0 text-break pe-2">{action.description}</p>
        <div className="btn-group" role="group" aria-label="Actions">
          <button type="button" className="btn btn-sm btn-outline-danger fill-danger" onClick={(e) => handleDeleteAction(action.actionId, e)}>
            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M15 2H9c-1.103 0-2 .897-2 2v2H3v2h2v12c0 1.103.897 2 2 2h10c1.103 0 2-.897 2-2V8h2V6h-4V4c0-1.103-.897-2-2-2zM9 4h6v2H9V4zm8 16H7V8h10v12z"></path></svg>
            <span className="visually-hidden">Delete</span>
          </button>
        </div>
      </div>
    </li>
  );
}

export { AuditActionSingle };
