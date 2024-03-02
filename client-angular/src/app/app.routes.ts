import { Routes } from '@angular/router';
import { LoginComponent } from './account/login/login.component';
import { AuditListComponent } from './audit/audit-list/audit-list.component';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  { path: '', component: AuditListComponent, canActivate: [authGuard] },
  { path: 'login', component: LoginComponent },
  { path: 'audits', component: AuditListComponent, canActivate: [authGuard] },
  { path: '**', redirectTo: '/audits', pathMatch: 'full' },
];
