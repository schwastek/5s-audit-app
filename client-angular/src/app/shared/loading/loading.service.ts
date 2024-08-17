import { Injectable, signal } from '@angular/core';

@Injectable()
export class LoadingService {
  private _isLoading = signal(false);
  public isLoading = this._isLoading.asReadonly();

  start() {
    this._isLoading.set(true);
  }

  complete() {
    this._isLoading.set(false);
  }
}
