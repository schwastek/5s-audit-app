import { Component, inject } from '@angular/core';
import { LoadingService } from './loading.service';
import { toObservable } from '@angular/core/rxjs-interop';
import { delay, of, switchMap } from 'rxjs';
import { AsyncPipe } from '@angular/common';

@Component({
  selector: 'app-loading',
  standalone: true,
  imports: [AsyncPipe],
  templateUrl: './loading.component.html'
})
export class LoadingComponent {
  loadingService = inject(LoadingService);
  showLoadingAfterMs = 900;
  isLoading$ = toObservable(this.loadingService.isLoading).pipe(
    switchMap((isLoading) => {
      if (!isLoading) return of(false);
      return of(true).pipe(delay(this.showLoadingAfterMs));
    })
  );
}
