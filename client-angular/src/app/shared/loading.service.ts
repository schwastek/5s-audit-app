import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class LoadingService {
  private _isLoading = signal(false);
  public isLoading = this._isLoading.asReadonly();

  private timeout!: number;
  private showLoadingAfterMs = 900;

  start() {
    this.timeout = window.setTimeout(() => {
      this._isLoading.set(true);
    }, this.showLoadingAfterMs);
  }

  complete() {
    this._isLoading.set(false);
    window.clearTimeout(this.timeout);
  }
}
