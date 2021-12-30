import React, { useState, useEffect, useRef } from 'react';
import { Audits } from '../api/Audits';
import { Pagination } from '../components/Pagination';

function formatDate(dateIsoString) {
  const date = new Date(dateIsoString);
  const formatted = new Intl.DateTimeFormat('en-GB', { dateStyle: 'full' }).format(date);

  return formatted;
}

function formatAsPercentage(value) {
  const formatter = Intl.NumberFormat('en-US', {
    style: 'percent',
  });
  const percent = formatter.format(value);

  return percent;
}

function BrowseAuditsPage() {
  const [audits, setAudits] = useState([]);
  const [paginationHeader, setPaginationHeader] = useState({});
  const [error, setError] = useState(null);
  const [isLoading, setIsLoading] = useState(true);
  const [currentPage, setCurrentPage] = useState(1);
  // Prevents updating data on an unmounted component
  const isMounted = useRef(false);

  useEffect(() => {
    isMounted.current = true;

    async function fetchData() {
      setIsLoading(true);

      try {
        const params = new URLSearchParams();
        params.append('pageSize', 10);
        params.append('pageNumber', currentPage);

        const response = await Audits.list(params);
        if (isMounted.current) {
          setAudits(response.data);
          setPaginationHeader(response.pagination);
          setError(null);
        }
      } catch (error) {
        if (isMounted.current) {
          setError(error);
          setAudits([]);
          setPaginationHeader({});
        }
      } finally {
        if (isMounted.current) {
          setIsLoading(false);
        }
      }
    }

    fetchData();

    // Clean up
    return () => isMounted.current = false;
  }, [currentPage]);

  function handleChangePage(pageNumber) {
    setCurrentPage(pageNumber);
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
        <div className="alert alert-danger my-0" role="alert">
          {error.message}
        </div>
      );
    }
  }

  function renderData() {
    const showData = (audits && !error && !isLoading);

    if (!showData) {
      return null;
    }

    return (
      <>
        <table className="table">
          <thead>
            <tr>
              <th scope="col">ID</th>
              <th scope="col">Date</th>
              <th scope="col">Author</th>
              <th scope="col">Score</th>
              <th scope="col">Actions</th>
            </tr>
          </thead>
          <tbody className="text-muted">
            {audits.map((audit) => (
              <tr key={audit.auditId}>
                <td>{audit.auditId.substring(0, 8)}</td>
                <td>{formatDate(audit.startDate)}</td>
                <td>{audit.author}</td>
                <td>{formatAsPercentage(audit.score)}</td>
                <td>
                  <a className="btn btn-sm btn-outline-secondary" href={`/audits/${audit.auditId}`}>View</a>
                </td>
              </tr>
            ))}
          </tbody>
        </table>

        <div className="d-flex justify-content-center mt-5">
          <Pagination
            currentPage={currentPage}
            totalPages={paginationHeader.totalPages}
            pageSiblings={2}
            handleChangePage={handleChangePage}
          />
        </div>
      </>
    );
  }

  return (
    <div className="container">
      <div className="my-3 px-3 py-3 bg-body rounded shadow-sm">
        {renderLoading()}
        {renderError()}
        {renderData()}
      </div>
    </div>
  );
}

export { BrowseAuditsPage };
