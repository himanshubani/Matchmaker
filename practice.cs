import { Component } from '@angular/core';

@Component({
  selector: 'app-text-transformation',
  templateUrl: './text-transformation.component.html',
  styleUrls: ['./text-transformation.component.css']
})
export class TextTransformationComponent {

  transformedText: string = '';
  textLength: number = 0;
  lowercaseCount: number = 0;
  uppercaseCount: number = 0;
  numberCount: number = 0;
  specialCharCount: number = 0;

  transformText(inputText: string): void {

    // Convert text to uppercase
    this.transformedText = inputText.toUpperCase();

    // Length including spaces
    this.textLength = inputText.length;

    // Reset counts
    this.lowercaseCount = 0;
    this.uppercaseCount = 0;
    this.numberCount = 0;
    this.specialCharCount = 0;

    // Count character types
    for (let ch of inputText) {

      if (ch >= 'a' && ch <= 'z') {
        this.lowercaseCount++;
      }
      else if (ch >= 'A' && ch <= 'Z') {
        this.uppercaseCount++;
      }
      else if (ch >= '0' && ch <= '9') {
        this.numberCount++;
      }
      else {
        this.specialCharCount++;
      }
    }
  }
}


<h2>Text Transformation</h2>

<input
  type="text"
  (input)="transformText($any($event.target).value)"
  placeholder="Enter text here"
/>

<p>Transformed Text: {{ transformedText }}</p>
<p>Length of Text: {{ textLength }}</p>
<p>Lowercase Count: {{ lowercaseCount }}</p>
<p>Uppercase Count: {{ uppercaseCount }}</p>
<p>Number Count: {{ numberCount }}</p>
<p>Special Character Count: {{ specialCharCount }}</p>
