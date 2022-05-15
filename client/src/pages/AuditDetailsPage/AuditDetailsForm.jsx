import React from 'react';
import { Rating } from '../../components/Rating';

function AuditDetailsForm({ answers }) {
  return (
    <>
      {answers.map((answer) => (
        <div key={answer.answerId} className="d-flex pt-3 border-bottom lh-1 text-muted">
          <div className="flex-grow-1">
            <p className="pb-3 mb-0 lh-sm">{answer.questionText}</p>
          </div>
          <div>
            <Rating name={answer.questionId} max={5} initial={Number(answer.answerText)} disabled></Rating>
          </div>
        </div>
      ))}
    </>
  );
}

export { AuditDetailsForm };
