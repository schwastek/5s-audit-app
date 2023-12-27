import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuditListComponent } from './audit-list/audit-list.component';
import { AuditRoutingModule } from './audit-routing.module';
import { ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    AuditListComponent,
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    AuditRoutingModule
  ]
})
export class AuditModule { }
