import { DOCUMENT } from '@angular/common';
import { Directive, effect, ElementRef, inject, input } from '@angular/core';

@Directive({
  selector: '[appLoadingButton]'
})
export class LoadingButtonDirective {
  loading = input<boolean>(false, { alias: 'appLoadingButton' });
  host = inject(ElementRef);
  document: Document = inject(DOCUMENT);

  private get htmlElement(): HTMLElement {
    return this.host.nativeElement as HTMLElement;
  }

  constructor() {
    effect(() => {
      if (this.loading()) {
        this.showIcon();
      } else {
        this.hideIcon();
      }
    });
  }

  showIcon() {
    const iconElement = this.htmlElement.querySelector('.spinner-border');

    if (iconElement) {
      iconElement.classList.remove('d-none');
    } else {
      this.createIcon();
    }
  }

  createIcon() {
    const created = this.htmlElement.querySelector('.spinner-border');

    if (!created) {
      const iconElement = this.document.createElement('span');
      iconElement.className = 'spinner-border spinner-border-sm me-1';
      iconElement.setAttribute('aria-hidden', 'true');

      // Insert before text.
      this.htmlElement.insertBefore(iconElement, this.htmlElement.firstChild);
    }
  }

  hideIcon() {
    const iconElement = this.htmlElement.querySelector('.spinner-border');

    if (iconElement) {
      iconElement.classList.add('d-none');
    }
  }
}
