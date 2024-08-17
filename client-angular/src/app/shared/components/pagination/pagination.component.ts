import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { range } from '../../utilities/utilities';

@Component({
  selector: 'app-pagination',
  standalone: true,
  imports: [],
  templateUrl: './pagination.component.html'
})
export class PaginationComponent implements OnInit, OnChanges {

  // If `true`, pagination links will be disabled.
  @Input() disabled = false;

  // The currently selected page.
  @Input() currentPage = 1;

  // Total number of pages.
  @Input() totalPages = 1;

  // Number of pages to display on each side of the current page.
  @Input() pageSiblings = 2;

  // The maximum number of pages to display.
  @Input() visiblePages = 5;

  // An event fired when the page is changed.
  @Output() pageChange = new EventEmitter<number>();

  // By default, page navigation is displayed as: < [1] >.
  pages: number[] = [1];

  ngOnInit() {
    this.pages = this.calculatePageNumbers(this.currentPage, this.totalPages, this.pageSiblings, this.visiblePages);
  }

  ngOnChanges(_: SimpleChanges) {
    this.pages = this.calculatePageNumbers(this.currentPage, this.totalPages, this.pageSiblings, this.visiblePages);
  }

  handlePageChange(pageNumber: number, event: Event) {
    // Prevent reloading the page.
    event.preventDefault();

    if (this.disabled) return;

    this.currentPage = pageNumber;
    this.pages = this.calculatePageNumbers(this.currentPage, this.totalPages, this.pageSiblings, this.visiblePages);
    this.pageChange.emit(pageNumber);
  }

  handleMoveLeft(event: Event) {
    this.handlePageChange(this.currentPage - 1, event);
  }

  handleMoveRight(event: Event) {
    this.handlePageChange(this.currentPage + 1, event);
  }

  private calculatePageNumbers(currentPage: number, totalPages: number, pageSiblings: number, visiblePages: number): number[] {
    // Ensure a fixed total number of pages (e.g. 5 pages) is always shown.
    // Center the current page within the range when possible.
    // Show more pages on one side of the current page, when the current page is near the beginning or end.

    let startPage;
    let endPage;

    if (totalPages <= visiblePages) {
      // If total pages less than visible pages, show all pages.
      startPage = 1;
      endPage = totalPages;
    } else if (currentPage <= pageSiblings) {
      // If closer to the beginning.
      // Display: 1 [2] 3 4 5.
      startPage = 1;
      endPage = visiblePages;
    } else if (currentPage + pageSiblings >= totalPages) {
      // If closer to the end.
      // Display: 6 7 8 [9] 10
      startPage = totalPages - visiblePages + 1;
      endPage = totalPages;
    } else {
      // Current page is centered.
      // Display: 3 4 [5] 6 7
      startPage = currentPage - pageSiblings;
      endPage = currentPage + pageSiblings;
    }

    const pages = range(startPage, endPage);

    return pages;
  }
}
