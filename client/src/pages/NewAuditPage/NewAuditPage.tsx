import React, { useReducer, useEffect, useRef } from 'react';
import { AuditActionsView } from './AuditActionsView';
import { NewAuditForm } from './NewAuditForm';
import { Questions } from '../../api/Questions';
import { Audits } from '../../api/Audits';
import { v4 as uuidv4 } from 'uuid';

type Question = {
  questionId: string,
  questionText: string
};

type Answer = {
  answerId: string,
  questionId: string,
  answerText: string,
  answerType: string
};

type AuditAction = {
  actionId: string,
  auditId: string,
  description: string
};

type State = {
  questions: Question[],
  actions: AuditAction[],
  error: Object,
  status: string
};

type Action = {
  type: string,
  payload?: any
};

function generateInitialAnswers(questions: Question[]): Answer[] {
  const answers = questions.map((question: Question) => {
    const answer = {
      answerId: uuidv4(),
      questionId: question.questionId,
      answerText: '3',
      answerType: 'number'
    };

    return answer;
  });

  return answers;
}

const initialState: State = {
  questions: [],
  actions: [],
  error: {},
  status: 'idle'
};

function actionsReducer(state: State, action: Action) {
  switch (action.type) {
    case 'FETCH_QUESTIONS_START': {
      return {
        ...state,
        status: 'loading'
      };
    }

    case 'FETCH_QUESTIONS_SUCCESS': {
      return {
        ...state,
        questions: action.payload,
        status: 'success'
      };
    }

    case 'FETCH_QUESTIONS_ERROR': {
      return {
        ...state,
        error: action.payload,
        status: 'error'
      };
    }

    case 'SUBMIT_AUDIT_START': {
      return {
        ...state,
        status: 'submitting'
      };
    }

    case 'SUBMIT_AUDIT_SUCCESS': {
      return {
        ...state,
        status: 'success'
      };
    }

    case 'SUBMIT_AUDIT_ERROR': {
      return {
        ...state,
        error: action.payload,
        status: 'error'
      };
    }

    case 'CREATE_ACTION': {
      const actions = [...state.actions, action.payload];

      return {
        ...state,
        actions,
        status: 'success'
      };
    }

    case 'DELETE_ACTION': {
      const actions = state.actions.filter(a => a.actionId !== action.payload);

      return {
        ...state,
        actions,
        status: 'success'
      };
    }

    default: {
      throw new Error(`Unhandled action type: ${action.type}`);
    }
  }
}

function NewAuditPage() {
  const [state, dispatch] = useReducer(actionsReducer, initialState);
  const answers = useRef<Answer[]>([]);
  const auditId = useRef(uuidv4());
  const selectedArea = useRef('assembly');
  const auditStartDate = useRef(new Date());

  useEffect(() => {
    const controller = new AbortController();

    async function fetchQuestions() {
      dispatch({type: 'FETCH_QUESTIONS_START'});

      try {
        const questions = await Questions.list(controller);
        dispatch({type: 'FETCH_QUESTIONS_SUCCESS', payload: questions});
      } catch (error) {
        dispatch({type: 'FETCH_QUESTIONS_ERROR', payload: error});
      }
    }

    fetchQuestions();

    return function cleanUp() {
      controller.abort();
    };
  }, []);

  useEffect(() => {

    // We need answers to all questions in advance.
    // In case the user submits the form immediately.
    answers.current = generateInitialAnswers(state.questions);
  }, [state.questions]);

  async function handleSubmitForm() {
    dispatch({type: 'SUBMIT_AUDIT_START'});

    // Generate audit
    const audit = {
      auditId: auditId.current,
      area: selectedArea.current,
      startDate: auditStartDate.current,
      author: 'John',
      endDate: new Date(),
      answers: answers.current,
      actions: state.actions
    };

    // POST audit
    try {
      await Audits.create(audit);
      dispatch({type: 'SUBMIT_AUDIT_SUCCESS'});
    } catch (error) {
      dispatch({type: 'SUBMIT_AUDIT_ERROR', payload: error});
    }
  }

  function handleRate(questionId: string, rate: number) {

      // Generate answer
      const answer = {
        answerId: uuidv4(),
        questionId: questionId,
        answerText: rate.toString(),
        answerType: 'number'
      };

      // Upsert answer (add or replace)
      const i = answers.current.findIndex(_answer => _answer.questionId === answer.questionId);
      if (i > -1) answers.current[i] = answer;
      else answers.current.push(answer);
  }

  function handleAreaSelect(area: string) {
    selectedArea.current = area;
  }

  function handleSubmitAction(description: string) {
    const actionId = uuidv4();

    const action = {
      actionId: actionId,
      auditId: auditId.current,
      description: description
    };

    dispatch({type: 'CREATE_ACTION', payload: action});
  }

  function handleDeleteAction(actionId: string) {
    dispatch({type: 'DELETE_ACTION', payload: actionId});
  }

  if (state.status === 'idle' || state.status === 'loading') {
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

  return (
    <div className="container">
      <div className="my-3">
        {state.status === 'error' && (
          <div className="alert alert-danger" role="alert">
            {state.error.message}
          </div>
        )}
        <div className="row">
          <div className="col-12">
            <div className="d-flex align-items-center mb-3 p-3 text-white bg-secondary rounded shadow-sm">
              <h1 className="h6 lh-1 mb-0 text-white">New audit</h1>
            </div>
          </div>
        </div>
        <div className="row">
          <div className="col-8">
            <div className="mb-3 px-3 py-3 bg-body rounded shadow-sm">
              <NewAuditForm
                questions={state.questions}
                onSubmitAudit={handleSubmitForm}
                onSelectArea={handleAreaSelect}
                onRateQuestion={handleRate}
              />
            </div>
          </div>
          <div className="col-4">
            <AuditActionsView
              actions={state.actions}
              onSubmitAction={handleSubmitAction}
              onDeleteAction={handleDeleteAction}
            />
          </div>
        </div>
      </div>
    </div>
  );
}

export { NewAuditPage };
