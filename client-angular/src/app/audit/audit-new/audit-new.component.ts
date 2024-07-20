import { Component, OnInit } from '@angular/core';
import { FormArray, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuditService } from '../audit.service';
import { Area } from '../models/area';
import { lastValueFrom } from 'rxjs';
import { CommonModule } from '@angular/common';
import { RatingComponent } from '../../shared/components/rating/rating.component';
import { RatingTemplateDirective } from '../../shared/components/rating/templates.directive';
import { ApiAnswerForCreationDto, ApiQuestionDto } from '../../api/models';
import { v4 as uuidv4 } from 'uuid';
import { Router } from '@angular/router';

@Component({
  selector: 'app-audit-new',
  templateUrl: './audit-new.component.html',
  styleUrls: ['./audit-new.component.scss'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RatingComponent, RatingTemplateDirective]
})
export class AuditNewComponent implements OnInit {
  areas: Area[] | null = null;
  questions: ApiQuestionDto[] | null = null;

  area = new FormControl<string>('assembly', { validators: [Validators.required], nonNullable: true });
  rating = new FormControl<number>(3, { validators: [Validators.required], nonNullable: true });
  ratings = new FormArray<FormControl<number>>([]);

  form: FormGroup = new FormGroup({
    area: this.area,
    ratings: this.ratings
  });

  private readonly startDate = new Date().toISOString();
  private readonly answers: ApiAnswerForCreationDto[] = [];

  constructor(
    private auditService: AuditService,
    private router: Router
  ) {}

  async ngOnInit() {
    await this.getAreas();
    await this.getQuestions();
    this.addRatingControls();
    this.updateAnswers();
  }

  async onSubmit() {
    // Generate IDs here to ensure each submission has unique IDs.
    // If saving fails and user resubmits, new IDs will prevent errors from duplicate IDs in DB.
    this.answers.forEach((answer) => {
      answer.answerId = uuidv4();
    });

    const audit = {
      auditId: uuidv4(),
      author: 'John',
      area: this.area.value,
      startDate: this.startDate,
      endDate: new Date().toISOString(),
      answers: this.answers,
      actions: []
    };

    this.auditService.saveAudit(audit).subscribe((response) => {
      this.router.navigate(['audits', response.auditId]);
    });
  }

  private async getAreas() {
    this.areas = await lastValueFrom(this.auditService.getAreas());
  }

  private async getQuestions() {
    this.questions = await lastValueFrom(this.auditService.getQuestions());
  }

  private addRatingControls() {
    const defaultAnswer = 3;

    this.questions?.forEach((question) => {
      // Add rating control.
      const ratingControl = new FormControl<number>(defaultAnswer, { nonNullable: true });
      this.ratings.push(ratingControl);

      // Create initial answers to be updated and saved when submitting the form.
      this.answers.push({
        answerId: '<ID_GENERATED_ON_SUBMIT>',
        questionId: question.questionId,
        answerText: defaultAnswer.toString(),
        answerType: 'number'
      });
    });
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
