import { Routes } from '@angular/router';
import { LoginComponent } from './account/login/login.component';
import { AuditListComponent } from './audit/audit-list/audit-list.component';
import { authGuard } from './core/guards/auth.guard';
import { AuditDetailComponent } from './audit/audit-detail/audit-detail.component';
import { AuditNewComponent } from './audit/audit-new/audit-new.component';

export const routes: Routes = [
  { path: '', redirectTo: 'audits', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'audits', component: AuditListComponent, canActivate: [authGuard] },
  { path: 'audits/new', component: AuditNewComponent, canActivate: [authGuard] },
  { path: 'audits/:id', component: AuditDetailComponent,  canActivate: [authGuard] },
  { path: '**', redirectTo: 'audits' }
];
