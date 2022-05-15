import React, { useEffect, useReducer } from 'react';
import { useParams } from 'react-router-dom';
import { Audits } from '../../api/Audits';
import { Actions } from '../../api/Actions';
import { AuditDetailsForm } from './AuditDetailsForm';
import { AuditActionsView } from './AuditActionsView';
import { v4 as uuidv4 } from 'uuid';

const initialState = {
  answers: [],
  actions: [],
  status: 'idle',
  error: {}
};

function auditReducer(state, action) {
  switch (action.type) {
    case 'FETCH_AUDIT_START':
    {
      return {
        ...state,
        status: 'pending'
      };
    }
    case 'FETCH_AUDIT_SUCCESS': {
      return {
        ...state,
        ...action.response,
        status: 'resolved'
      };
    }
    case 'FETCH_AUDIT_ERROR': {
      return {
        ...state,
        error: action.error,
        status: 'rejected'
      };
    }

    case 'CREATE_ACTION': {
      const actions = [...state.actions, action.action];

      return {
        ...state,
        actions,
        status: 'resolved'
      };
    }
    case 'DELETE_ACTION': {
      const actions = state.actions.filter(a => a.actionId !== action.actionId);

      return {
        ...state,
        actions,
        status: 'resolved'
      };
    }

    default: {
      throw new Error(`Unhandled action type: ${action.type}`);
    }
  }
}

function AuditDetailsPage() {
  const [state, dispatch] = useReducer(auditReducer, initialState);
  const { id: auditId } = useParams();

  useEffect(() => {
    const controller = new AbortController();

    async function fetchData() {
      dispatch({type: 'FETCH_AUDIT_START'});

      try {
        const response = await Audits.details(auditId, controller);
        dispatch({type: 'FETCH_AUDIT_SUCCESS', response});
      } catch (error) {
        dispatch({type: 'FETCH_AUDIT_ERROR', error});
      }
    }

    fetchData();

    return function cleanUp() {
      controller.abort();
    };
  }, [auditId]);

  async function handleSubmitAction(description) {
    const action = {
      actionId: uuidv4(),
      auditId: auditId,
      description: description
    };

    dispatch({type: 'CREATE_ACTION', action});
    await Actions.create(action);
  }

  async function handleDeleteAction(actionId) {
    dispatch({type: 'DELETE_ACTION', actionId});
    await Actions.remove(actionId);
  }

  async function handleCompleteAction(actionId) {
    const action = {
      isComplete: true
    };

    await Actions.update(actionId, action);
  }

  async function handleIncompleteAction(actionId) {
    const action = {
      isComplete: false
    };

    await Actions.update(actionId, action);
  }

  if (state.status === 'idle' || state.status === 'pending') {
    return (
      <div className="container">
        <div className="my-3">
          <div className="alert alert-dark" role="alert">
            Loading...
          </div>
        </div>
      </div>
    );
  }

  if (state.status === 'rejected') {
    return (
      <div className="container">
        <div className="my-3">
          <div className="alert alert-danger" role="alert">
            {state.error.message}
          </div>
        </div>
      </div>
    );
  }

  if (state.status === 'resolved') {
    return (
      <div className="container">
        <div className="my-3">
          <div className="row">
            <div className="col-8">
              <div className="px-3 py-3 bg-body rounded shadow-sm">
                <h2 className="h6 border-bottom pb-2 mb-0">Audit details</h2>
                <AuditDetailsForm answers={state.answers} />
              </div>
            </div>
            <div className="col-4">
              <AuditActionsView
                actions={state.actions}
                onSubmitAction={handleSubmitAction}
                onDeleteAction={handleDeleteAction}
                onCompleteAction={handleCompleteAction}
                onIncompleteAction={handleIncompleteAction}
              />
            </div>
          </div>
        </div>
      </div>
    );
  }

  return null;
}

export { AuditDetailsPage };
