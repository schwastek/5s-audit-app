import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { range } from '../../utilities/utilities';
import { NgClass } from '@angular/common';

@Component({
  selector: 'app-pagination',
  standalone: true,
  imports: [NgClass],
  templateUrl: './pagination.component.html',
  styleUrl: './pagination.component.scss'
})
export class PaginationComponent implements OnInit, OnChanges {
  @Input() currentPage = 1;
  @Input() totalPages = 1;
  @Input() pageSiblings = 2;
  @Input() visiblePages = 5;
  @Output() pageChange = new EventEmitter<number>();

  // By default, page navigation is displayed as: < [1] >.
  pages: number[] = [1];

  ngOnInit(): void {
    this.pages = this.calculatePageNumbers(this.currentPage, this.totalPages, this.pageSiblings, this.visiblePages);
  }

  ngOnChanges(changes: SimpleChanges): void {
    console.log(changes);
    console.log('currentPage', this.currentPage);
    console.log('totalPages', this.totalPages);
    this.pages = this.calculatePageNumbers(this.currentPage, this.totalPages, this.pageSiblings, this.visiblePages);
    console.log('pages', this.pages);
  }

  handlePageChange(pageNumber: number, event: Event) {
    // Prevent reloading the page.
    event.preventDefault();

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
