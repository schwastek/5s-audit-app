import { Component, inject, OnInit, signal } from '@angular/core';
import { FormArray, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuditService } from '../audit.service';
import { Area } from '../models/area';
import { firstValueFrom } from 'rxjs';
import { RatingComponent } from '../../shared/components/rating/rating.component';
import { ApiAnswerForCreationDto, ApiQuestionDto } from '../../api/models';
import { v4 as uuidv4 } from 'uuid';
import { Router } from '@angular/router';
import { AuditActionComponent, SaveOrUpdateAuditAction } from '../audit-action/audit-action.component';
import { LoadingButtonDirective } from '../../shared/components/loading-button/loading-button.directive';

@Component({
  selector: 'app-audit-new',
  templateUrl: './audit-new.component.html',
  imports: [
    ReactiveFormsModule,
    RatingComponent,
    AuditActionComponent,
    LoadingButtonDirective
  ]
})
export class AuditNewComponent implements OnInit {
  private auditService = inject(AuditService);
  private router = inject(Router);

  readonly auditId = uuidv4();
  private readonly startDate = new Date().toISOString();

  // Content
  areas= signal<Area[] | null>(null);
  auditActions = signal<SaveOrUpdateAuditAction[]>([]);
  questions: ApiQuestionDto[] | null = null;
  private answers: ApiAnswerForCreationDto[] = [];

  // Form
  area = new FormControl<string>('assembly', { validators: [Validators.required], nonNullable: true });
  ratings = new FormArray<FormControl<number>>([]);

  form: FormGroup = new FormGroup({
    area: this.area,
    ratings: this.ratings
  });

  // Form status
  isQuestionsReady = signal(false);
  isFormSaving = signal(false);

  // Configuration
  private readonly defaultAnswer = 3;

  ngOnInit() {
    this.getAreas();
    this.getQuestions()
      .then(() => this.createInitialAnswers())
      .then(() => this.addRatings())
      .then(() => this.updateAnswersWhenRatingChanges())
      .then(() => this.isQuestionsReady.set(true));
  }

  async onSave() {
    if (this.form.invalid) return;

    this.isFormSaving.set(true);

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
      this.isFormSaving.set(false);
      this.router.navigate(['audits', response.auditId]);
    });
  }

  private async getAreas() {
    const areas = await firstValueFrom(this.auditService.getAreas());
    this.areas.set(areas);
  }

  private async getQuestions() {
    this.questions = await firstValueFrom(this.auditService.getQuestions());
  }

  private createInitialAnswers() {
    const answers: ApiAnswerForCreationDto[] = [];

    // Create initial answers to be updated and saved when submitting the form.
    this.questions!.forEach((question) => {
      answers.push({
        answerId: '<ID_GENERATED_LATER>',
        questionId: question.questionId,
        answerText: this.defaultAnswer.toString(),
        answerType: 'number'
      });
    });

    this.answers = answers;
  }

  private addRatings() {
    this.questions!.forEach(() => {
      const ratingControl = new FormControl<number>(this.defaultAnswer, { nonNullable: true });
      this.ratings.push(ratingControl);
    });
  }

  private updateAnswersWhenRatingChanges() {
    // Ratings and answers are based on questions. They share the same index.
    this.ratings.valueChanges.subscribe((ratings) => {
      this.answers.forEach((answer, i) => {
        answer.answerText = ratings[i].toString();
      });
    });
  }
}
