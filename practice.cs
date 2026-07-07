quiz.component.ts
import { Component } from '@angular/core';
 
import { quizQuestions } from '../../quiz';
@Component({
 
  selector: 'app-quiz',
 
  templateUrl: './quiz.component.html',
 
  styleUrls: ['./quiz.component.css']
 
})
 
export class QuizComponent {
 
  quizQuestions = quizQuestions;
 
  currentQuestionIndex: number = 0;
 
  showFeedback: boolean = false;
 
  feedback: string = '';
 
  score: number = 0;
 
  selectedOptionIndex: number | null = null;
 
  quizEnded: boolean = false;
 
  checkAnswer(optionIndex: number): void {
 
    if (this.showFeedback) {
 
    return;
 
    }
 
    this.selectedOptionIndex = optionIndex;
 
    const currentQuestion = this.quizQuestions[this.currentQuestionIndex];
 
    if (currentQuestion.options[optionIndex]=== currentQuestion.correctAnswer) {
 
    this.feedback = 'Correct Answer!';
 
    this.score++;
 
    } else {
 
    this.feedback = 'Incorrect Answer!';
 
    }
 
    this.showFeedback = true;
 
  }
 
  nextQuestion(): void {
 
    this.currentQuestionIndex++;
 
    this.showFeedback = false;
 
    this.feedback = '';
 
    this.selectedOptionIndex = null;
 
    if (this.currentQuestionIndex >= this.quizQuestions.length) {
 
    this.endQuiz();
 
    }
 
  }
 
  endQuiz(): void {
 
    this.quizEnded = true;
 
  }
 
  restartQuiz(): void {
 
    this.currentQuestionIndex = 0;
 
    this.showFeedback = false;
 
    this.feedback = '';
 
    this.score = 0;
 
    this.selectedOptionIndex = null;
 
    this.quizEnded = false;
 
  }
 
}=======
quiz.component.html
<div class="quiz-container">
 
    <h1>Welcome to the Interactive Quiz Application</h1>
 
    <div *ngIf="!quizEnded">
 
        <h2>
 
            Question {{ currentQuestionIndex + 1 }} of {{ quizQuestions.length }}
 
        </h2>
 
        <p class="question">
 
            {{ quizQuestions[currentQuestionIndex].question }}
 
        </p>
 
        <ul>
<li *ngFor="let option of quizQuestions[currentQuestionIndex].options; let i = index"(click)="checkAnswer(i)" [ngClass]="{  'correct': showFeedback && i === quizQuestions[currentQuestionIndex].correctAnswer,     'incorrect': showFeedback && i === selectedOptionIndex && i !== quizQuestions[currentQuestionIndex].correctAnswer}">
 
                {{ option }}
 
            </li>
 
        </ul>
 
        <p *ngIf="showFeedback" class="feedback">
 
            {{ feedback }}
 
        </p>
 
        <button *ngIf="showFeedback" (click)="nextQuestion()">
 
            Next Question
 
        </button>
 
    </div>
 
    <div *ngIf="quizEnded">
 
        <h2>Quiz Completed!</h2>
 
        <p class="score">
 
            Your Final Score: {{ score }} / {{ quizQuestions.length }}
 
        </p>
 
        <button (click)="restartQuiz()">
 
            Restart Quiz
 
        </button>
 
    </div>
 
</div>
