import { Component, OnInit, signal } from '@angular/core';
import { FormArray, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuditService } from '../audit.service';
import { Area } from '../models/area';
import { firstValueFrom } from 'rxjs';
import { RatingComponent } from '../../shared/components/rating/rating.component';
import { ApiAnswerForCreationDto, ApiQuestionDto, ApiSaveAuditActionRequest } from '../../api/models';
import { v4 as uuidv4 } from 'uuid';
import { Router } from '@angular/router';
import { AuditActionComponent } from '../audit-action/audit-action.component';

@Component({
  selector: 'app-audit-new',
  templateUrl: './audit-new.component.html',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    RatingComponent,
    AuditActionComponent
  ]
})
export class AuditNewComponent implements OnInit {
  readonly auditId = uuidv4();
  private readonly startDate = new Date().toISOString();

  areas: Area[] | null = null;
  questions: ApiQuestionDto[] | null = null;
  private answers: ApiAnswerForCreationDto[] = [];
  auditActions = signal<ApiSaveAuditActionRequest[]>([]);

  // Form
  area = new FormControl<string>('assembly', { validators: [Validators.required], nonNullable: true });
  rating = new FormControl<number>(3, { validators: [Validators.required], nonNullable: true });
  ratings = new FormArray<FormControl<number>>([]);

  form: FormGroup = new FormGroup({
    area: this.area,
    ratings: this.ratings
  });

  // Configuration
  private readonly defaultAnswer = 3;

  constructor(
    private auditService: AuditService,
    private router: Router
  ) { }

  async ngOnInit() {
    await this.getAreas();
    await this.getQuestions();
    this.addRatingControls();
    this.createInitialAnswers();
    this.updateAnswers();
  }

  async onSave() {

    // Generate IDs here to ensure each submission has unique IDs.
    // If saving fails and user resubmits, new IDs will prevent errors from duplicate IDs in DB.
    this.answers.forEach((answer) => {
      answer.answerId = uuidv4();
    });

    const audit = {
      auditId: this.auditId,
      author: 'John',
      area: this.area.value,
      startDate: this.startDate,
      endDate: new Date().toISOString(),
      answers: this.answers,
      actions: this.auditActions()
    };

    this.auditService.saveAudit(audit).subscribe((response) => {
      this.router.navigate(['audits', response.auditId]);
    });
  }

  private async getAreas() {
    this.areas = await firstValueFrom(this.auditService.getAreas());
  }

  private async getQuestions() {
    this.questions = await firstValueFrom(this.auditService.getQuestions());
  }

  private addRatingControls() {
    this.questions?.forEach(() => {
      const ratingControl = new FormControl<number>(this.defaultAnswer, { nonNullable: true });
      this.ratings.push(ratingControl);
    });
  }

  private createInitialAnswers() {
    const answers: ApiAnswerForCreationDto[] = [];

    // Create initial answers to be updated and saved when submitting the form.
    this.questions?.forEach((question) => {
      answers.push({
        answerId: '<ID_GENERATED_LATER>',
        questionId: question.questionId,
        answerText: this.defaultAnswer.toString(),
        answerType: 'number'
      });
    });

    this.answers = answers;
  }

  private updateAnswers() {

    // Ratings and answers are based on questions.
    // They share the same index.
    this.ratings.valueChanges.subscribe((ratings) => {
      this.answers.forEach((answer, i) => {
        answer.answerText = ratings[i].toString();
      });
    });
  }
}
