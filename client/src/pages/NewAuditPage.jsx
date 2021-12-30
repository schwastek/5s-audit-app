import React, { useState, useEffect, useRef } from 'react';
import { Question } from '../components/Question';
import { Rating } from '../components/Rating';
import { FormSelect } from '../components/FormSelect';
import { Questions } from '../api/Questions';
import { Audits } from '../api/Audits';
import { useIsMounted } from '../hooks/useIsMounted';
import { v4 as uuidv4 } from 'uuid';
import { routerHistory } from '../routerHistory';

// Array of answers is empty when the component loads the first time
function generateInitialAnswers(questions) {
  const answers = questions.map((question) => {
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

function NewAuditPage() {
  const [isLoading, setIsLoading] = useState(false);
  const [questions, setQuestions] = useState([]);
  const [error, setError] = useState(null);
  const [isSubmitting, setIsSubmitting] = useState(false);
  const answers = useRef([]);
  const selectedArea = useRef('assembly');
  const auditStartDate = new Date();
  const isMounted = useIsMounted();

  useEffect(() => {
    async function fetchData() {
      setIsLoading(true);

      try {
        const response = await Questions.list();

        if (isMounted()) {

          // We need all answers in advance in case the user
          // does not answer any question at all but submits the form
          answers.current = generateInitialAnswers(response);

          setQuestions(response);
          setError(null);
        }
      } catch (error) {
        if (isMounted()) {
          setError(error);
          setQuestions([]);
        }
      } finally {
        if (isMounted()) {
          setIsLoading(false);
        }
      }
    }

    fetchData();
  }, [isMounted]);

  async function handleSubmit(event) {
    event.preventDefault();

    // Generate audit
    const audit = {
      auditId: uuidv4(),
      area: selectedArea.current,
      startDate: auditStartDate,
      author: 'John',
      endDate: new Date(),
      answers: answers.current
    };

    // POST audit
    try {
      if (isMounted()) {
        setIsSubmitting(true);
        await Audits.create(audit);
        setError(null);
        setIsSubmitting(false);
        routerHistory.push('/audits');
      }
    } catch (error) {
      if (isMounted()) {
        setIsSubmitting(false);
        setError(error);
      }
    }
  }

  function handleRate(questionId, rate) {

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

  function handleAreaSelect(area) {
    selectedArea.current = area;
  }

  function renderLoading() {
    if (isLoading) {
      return (
        <p className="pb-2 mb-0">Loading...</p>
      );
    }
  }

  function renderError() {
    if (error) {
      return (
        <div className="row justify-content-center">
          <div className="col-10">
            <div className="alert alert-danger shadow-sm" role="alert">
              {error.message}
            </div>
          </div>
        </div>
      );
    }
  }

  function renderSubmitButton() {
    if (isSubmitting) {
      return (
        <button type="submit" className="btn btn-primary mt-3" disabled>
          <span className="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>
          <span className="sr-only">Submitting...</span>
        </button>
      );
    }

    return (
      <button className="btn btn-primary mt-3">Submit</button>
    );
  }

  function renderForm() {
    if (isLoading) return null;

    return (
      <form onSubmit={handleSubmit}>
        <FormSelect onChange={handleAreaSelect} />
        {questions.map((question) => {
          const { questionId, questionText } = question;

          return (
            <Question key={questionId} text={questionText}>
              <Rating name={questionId} max={5} initial={3} onRate={(rate) => handleRate(questionId, rate)}></Rating>
            </Question>
          );
        })}
        {renderSubmitButton()}
      </form>
    );
  }

  return (
    <div className="container">
      <div className="row justify-content-center">
        <div className="col-10">
          <div className="d-flex align-items-center p-3 my-3 text-white bg-secondary rounded shadow-sm">
            <h1 className="h6 lh-1 mb-0 text-white">New audit</h1>
          </div>
        </div>
      </div>
      {renderError()}
      <div className="row justify-content-center">
        <div className="col-10">
          <div className="mb-3 px-3 py-3 bg-body rounded shadow-sm">
            {renderLoading()}
            {renderForm()}
          </div>
        </div>
      </div>
    </div>
  );
}

export { NewAuditPage };
