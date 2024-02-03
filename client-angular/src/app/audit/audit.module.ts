import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuditListComponent } from './audit-list/audit-list.component';
import { AuditRoutingModule } from './audit-routing.module';
import { ReactiveFormsModule } from '@angular/forms';
import { RatingTemplateDirective } from '../shared/components/rating/templates.directive';

@NgModule({
  declarations: [
    AuditListComponent,
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    AuditRoutingModule
  ]
    declarations: [
        AuditListComponent,
    ],
    imports: [
        CommonModule,
        ReactiveFormsModule,
        AuditRoutingModule,
        RatingComponent,
        RatingTemplateDirective
    ]
})
export class AuditModule { }
