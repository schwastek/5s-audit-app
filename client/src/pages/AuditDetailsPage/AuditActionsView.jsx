import React from 'react';
import { AuditActionSingle } from './AuditActionSingle';
import { AuditActionsNewForm } from './AuditActionsNewForm';

function AuditActionsView({ actions: auditActions, onSubmitAction, onDeleteAction, onCompleteAction, onIncompleteAction }) {
  return (
    <>
      <AuditActionsNewForm onSubmitAction={onSubmitAction} />

      <ul className="list-group">
        {auditActions.map((action) => (
          <AuditActionSingle
            key={action.actionId}
            action={action}
            onCompleteAction={onCompleteAction}
            onIncompleteAction={onIncompleteAction}
            onDeleteAction={onDeleteAction}
          />
        ))}
      </ul>
    </>

  );
}

export { AuditActionsView };
