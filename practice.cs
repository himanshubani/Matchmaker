import { Component } from '@angular/core';

@Component({
  selector: 'app-text-transformation',
  templateUrl: './text-transformation.component.html',
  styleUrls: ['./text-transformation.component.css']
})
export class TextTransformationComponent {
  // Properties to hold transformed text and calculated statistics
  transformedText: string = '';
  textLength: number = 0;
  lowercaseCount: number = 0;
  uppercaseCount: number = 0;
  numberCount: number = 0;
  specialCharCount: number = 0;

  constructor() {}

  // Method to process the user input
  transformText(inputText: string): void {
    // 1. Convert to uppercase and store it
    this.transformedText = inputText.toUpperCase();

    // 2. Calculate and store total length (including spaces)
    this.textLength = inputText.length;

    // 3. Reset all counts before iterating
    this.lowercaseCount = 0;
    this.uppercaseCount = 0;
    this.numberCount = 0;
    this.specialCharCount = 0;

    // 4. Loop through each character to update statistics
    for (let i = 0; i < inputText.length; i++) {
      const char = inputText[i];

      if (char >= 'a' && char <= 'z') {
        this.lowercaseCount++;
      } else if (char >= 'A' && char <= 'Z') {
        this.uppercaseCount++;
      } else if (char >= '0' && char <= '9') {
        this.numberCount++;
      } else {
        // Any character that is not a letter or number is treated as special (including spaces)
        this.specialCharCount++;
      }
    }
  }
}


<!-- Input field to accept user data on every keystroke -->
<input 
  type="text" 
  placeholder="Type your text here..." 
  (input)="transformText($any($event.target).value)" 
/>

<!-- Statistical Output Blocks -->
<p>Transformed Text: {{ transformedText }}</p>
<p>Length of Text: {{ textLength }}</p>
<p>Lowercase Count: {{ lowercaseCount }}</p>
<p>Uppercase Count: {{ uppercaseCount }}</p>
<p>Number Count: {{ numberCount }}</p>
<p>Special Character Count: {{ specialCharCount }}</p>
