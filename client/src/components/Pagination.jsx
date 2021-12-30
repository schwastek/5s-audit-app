import React, { useMemo } from 'react';
import { classNames } from '../utilities/classNames';

// Common reference to unicode character '…' to check against later
const ellipsis = 'ellipsis';

/**
 * Helper method for creating a range of numbers
 * range(1, 5) => [1, 2, 3, 4, 5]
 */
function range(from, to, step = 1) {
  let i = from;
  const range = [];

  while (i <= to) {
    range.push(i);
    i += step;
  }

  return range;
}

function calculatePageNumbers(currentPage = 1, totalPages = 1, pageSiblings = 1) {

  // full case: < 1 ... 4 5 [6] 7 8 ... 10 >
  // case: 1 ... 47 48 [96] 97 98 ... 100
  // case: 1 ... 47 48 [97] 98 99 100

  const startPage = Math.max(2, currentPage - pageSiblings);
  const endPage = Math.min(totalPages - 1, currentPage + pageSiblings);

  const hasLeftDots = startPage > 2;
  // show right dots if we are further than 1 page before the end
  const hasRightDots = endPage < totalPages - 1;

  // If the number of pages is less than the page numbers we want to show in our

  // Cases:
  // 1. Total page count is less than the page pills we want to show: 1 [2] 3
  // 2. Right dots are visible 1 2 [3] 4 ... 10
  // 4. Left dots are visible 1 ... 47 48 [49] 50 51 ... 100

  // first page + last page + current page + 2 * page siblings + previous and next page controls
  const totalPagePills = (2 * pageSiblings) + 5;

  // case: the number of total pages is less than the page pills we want to show in our navigation
  if (totalPages < totalPagePills) {
    const pages = range(1, totalPages);

    return pages;
  }

  // case: 1 ... 46 47 [48] 49 50
  if (hasLeftDots && !hasRightDots) {
    const middlePages = range(startPage, endPage);
    const pages = [1, ellipsis, ...middlePages, totalPages];

    return pages;
  }

  // case: 1 2 3 [4] 5 6 ... 50
  if (!hasLeftDots && hasRightDots) {
    const middlePages = range(startPage, endPage);
    const pages = [1, ...middlePages, ellipsis, totalPages];

    return pages;
  }

  // case: 1 ... 47 48 [49] 50 51 ... 100
  if (hasLeftDots && hasRightDots) {
    const middlePages = range(startPage, endPage);
    const pages = [1, ellipsis, ...middlePages, ellipsis, totalPages];

    return pages;
  }
}

function Pagination({currentPage, totalPages, pageSiblings, handleChangePage}) {
  function handleMoveLeft() {
    handleChangePage(currentPage - 1);
  };

  function handleMoveRight() {
    handleChangePage(currentPage + 1);
  }

  const pageNumbers = useMemo(
    () => calculatePageNumbers(currentPage, totalPages, pageSiblings),
    [currentPage, totalPages, pageSiblings],
  );

  const lastPage = pageNumbers[pageNumbers.length - 1];

  return (
    <nav aria-label="Page navigation">
      <ul className="pagination">

        {/* Previous page button */}
        <li
          className={classNames(['page-item', {
            'disabled': currentPage === 1
          }])}
        >
            <button className="page-link" onClick={handleMoveLeft} disabled={currentPage === 1}>Previous</button>
        </li>

        {/* Show page numbers */}
        {pageNumbers.map((page, index) => {
          const isCurrentPage = currentPage === page;

          // Render ellipsis '…' unicode character
          if (page === ellipsis) {
            return (
              <li key={index} className="page-item" aria-label="More">
                <span className="page-link" aria-hidden="true">&#8230;</span>
              </li>
            );
          }

          return (
            <li key={index}
              className={classNames(['page-item', {
                'active': isCurrentPage
              }])}
              // Conditionally add ARIA attribute
              aria-current={isCurrentPage ? 'true' : undefined}
            >
              <button className="page-link" onClick={() => handleChangePage(page)}>
                {page}
              </button>
            </li>
          );
        })}

        {/* Next page button */}
        <li
          className={classNames(['page-item', {
            'disabled': currentPage === lastPage
          }])}
        >
          <button className="page-link" onClick={handleMoveRight} disabled={currentPage === lastPage}>Next</button>
        </li>
      </ul>
    </nav>
  );
}

export { Pagination };
