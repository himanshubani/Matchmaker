<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Array Operations</title>
</head>
<body>

    <h1>Array Operations</h1>

    <label>Enter Number 1*:</label>
    <input type="number" id="num1">
    <br><br>

    <label>Enter Number 2*:</label>
    <input type="number" id="num2">
    <br><br>

    <label>Enter Number 3*:</label>
    <input type="number" id="num3">
    <br><br>

    <label>Enter Number 4*:</label>
    <input type="number" id="num4">
    <br><br>

    <label>Enter Number 5*:</label>
    <input type="number" id="num5">
    <br><br>

    <button id="calculateButton">Calculate</button>

    <p id="errorMessage" style="color:red;"></p>

    <p id="maximumNo"></p>
    <p id="minimumNo"></p>
    <p id="sumOfAllNumbers"></p>

    <script src="script.js"></script>

</body>
</html>



function calculate(): void {

    const n1 = (document.getElementById("num1") as HTMLInputElement).value;
    const n2 = (document.getElementById("num2") as HTMLInputElement).value;
    const n3 = (document.getElementById("num3") as HTMLInputElement).value;
    const n4 = (document.getElementById("num4") as HTMLInputElement).value;
    const n5 = (document.getElementById("num5") as HTMLInputElement).value;

    const errorMessage = document.getElementById("errorMessage") as HTMLElement;
    const maximumNo = document.getElementById("maximumNo") as HTMLElement;
    const minimumNo = document.getElementById("minimumNo") as HTMLElement;
    const sumOfAllNumbers = document.getElementById("sumOfAllNumbers") as HTMLElement;

    if (
        n1 === "" ||
        n2 === "" ||
        n3 === "" ||
        n4 === "" ||
        n5 === ""
    ) {
        errorMessage.textContent = "Enter all the numbers";

        maximumNo.textContent = "";
        minimumNo.textContent = "";
        sumOfAllNumbers.textContent = "";

        return;
    }

    errorMessage.textContent = "";

    const numbers: number[] = [
        Number(n1),
        Number(n2),
        Number(n3),
        Number(n4),
        Number(n5)
    ];

    const maximum = Math.max(...numbers);
    const minimum = Math.min(...numbers);

    const sum = numbers.reduce(function (total, current) {
        return total + current;
    }, 0);

    maximumNo.textContent = "Maximum number: " + maximum;
    minimumNo.textContent = "Minimum number: " + minimum;
    sumOfAllNumbers.textContent = "Sum of all numbers: " + sum;
}

document
    .getElementById("calculateButton")
    ?.addEventListener("click", calculate);
