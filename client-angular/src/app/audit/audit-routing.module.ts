import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuditListComponent } from './audit-list/audit-list.component';
import { authGuard } from '../core/guards/auth.guard';

const routes: Routes = [
  {path: 'audits', component: AuditListComponent, canActivate: [authGuard]},
];

@NgModule({
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [
    RouterModule
  ]
})
export class AuditRoutingModule { }
