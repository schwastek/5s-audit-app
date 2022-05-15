import React, { useState } from 'react';
import { Question } from '../../components/Question';
import { Rating } from '../../components/Rating';
import { FormSelect } from '../../components/FormSelect';
import { routerHistory } from '../../routerHistory';

function NewAuditForm({ questions, onSubmitAudit, onSelectArea, onRateQuestion }) {
  const [isSubmitting, setIsSubmitting] = useState(false);

  async function handleSubmit(event) {
    event.preventDefault();
    setIsSubmitting(true);

    try {
      await onSubmitAudit(event);
    } finally {
      setIsSubmitting(false);
      routerHistory.push('/audits');
    }
  }

  function handleRate(questionId) {
    return (rate) => {
      onRateQuestion(questionId, rate);
    };
  }

  return (
    <form onSubmit={handleSubmit}>
      <FormSelect onChange={onSelectArea} />
      {questions.map((question) => {
        const { questionId, questionText } = question;

        return (
          <Question key={questionId} text={questionText}>
            <Rating name={questionId} max={5} initial={3} onRate={handleRate(questionId)}></Rating>
          </Question>
        );
      })}
      {isSubmitting
        ? (<button type="submit" className="btn btn-primary mt-3" disabled>
            <span className="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>
            <span className="sr-only">Submitting...</span>
          </button>)
        : (<button className="btn btn-primary mt-3">Submit</button>)}
    </form>
  );
}

export { NewAuditForm };
