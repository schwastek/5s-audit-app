import React, { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import { Audits } from '../api/Audits';
import { Rating } from '../components/Rating';

function AuditDetailsPage() {
  const [audit, setAudit] = useState({});
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState(null);
  const { id: auditId } = useParams();

  useEffect(() => {
    async function fetchData() {
      try {
        const response = await Audits.details(auditId);
        setAudit(response);
      } catch (error) {
        setError(error);
      } finally {
        setIsLoading(false);
      }
    }

    fetchData();
  }, [auditId]);

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
        <div className="alert alert-danger my-0" role="alert">
          {error.message}
        </div>
      );
    }
  }

  function renderData() {
    const showData = (audit.answers && !error && !isLoading);

    if (!showData) {
      return null;
    }

    return (
      <>
        <h2 className="h6 border-bottom pb-2 mb-0">Audit details</h2>
        {audit.answers.map((answer) => (
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

  return (
    <div className="container">
      <div className="my-3 px-3 py-3 bg-body rounded shadow-sm">
        <div className="row justify-content-center">
          <div className="col-12">
            {renderLoading()}
            {renderError()}
            {renderData()}
          </div>
        </div>
      </div>
    </div>
  );
}

export { AuditDetailsPage };
