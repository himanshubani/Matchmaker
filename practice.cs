Week-5_Day-6_Session-2_Q-1
===========================
<!DOCTYPE html>
<html>
<head>
   
</head>
<body>
    <h1>Array Operations</h1>

    <label for="num1">Enter Number 1*:</label>
    <input type="number" id="num1"><br>

    <label for="num2">Enter Number 2*:</label>
    <input type="number" id="num2"><br>

    <label for="num3">Enter Number 3*:</label>
    <input type="number" id="num3"><br>

    <label for="num4">Enter Number 4*:</label>
    <input type="number" id="num4"><br>

    <label for="num5">Enter Number 5*:</label>
    <input type="number" id="num5"><br>

    <button id="calculateButton">Calculate</button>

    <div id="errorMessage"></div>
    <div id="sumOfEven"></div>
    <div id="numbersGreaterThan5"></div>

    <script src="script.js"></script> <!-- JavaScript compiled from TypeScript -->

</body>
</html>
------Script.js-------
function calculate(){
    let x1: number = +(<HTMLInputElement>document.getElementById("num1")).value;
    let x2: number = +(<HTMLInputElement>document.getElementById("num2")).value;
    let x3: number = +(<HTMLInputElement>document.getElementById("num3")).value;
    let x4: number = +(<HTMLInputElement>document.getElementById("num4")).value;
    let x5: number = +(<HTMLInputElement>document.getElementById("num5")).value;
    if(!x1||!x2||!x3||!x4||!x5){
        document.getElementById("errorMessage").innerHTML="Enter all the numbers";
        document.getElementById("maximumNo").innerHTML = "";
        document.getElementById("minimumNo").innerHTML = "";
        document.getElementById("sumOfEven").innerHTML = "";
        return;
    }
    let nums:number[]=[];
    nums.push(x1);
    nums.push(x2);
    nums.push(x3);
    nums.push(x4);
    nums.push(x5);
    let maxNumber: number = Math.max(...nums);
    let minNumber: number = Math.min(...nums);
    let SumOfNumbers:number = getSumOfNumbers(nums);
    let numbersGreaterThan5:string = getNumbersGreaterThan5(nums);
    
    function getSumOfNumbers(arr:number[]):number
    {
    let mynums:number=0;
    for(let i:number=0;i<arr.length;i++){
        if(arr[i] % 2 == 0)
        {
            mynums += arr[i];
        }
    }
    return mynums;
    }
    function getNumbersGreaterThan5(arr:number[]):string
    {
    let numbersGreaterThan5:string = '';
    for(let i:number=0;i<arr.length;i++){
        if(arr[i] > 5)
        {
                numbersGreaterThan5 += arr[i] + ', ';
        }

    }
    numbersGreaterThan5 = numbersGreaterThan5.slice(0,-2);

    return numbersGreaterThan5;
    }

    document.getElementById("errorMessage").textContent="";
    document.getElementById("maximumNo").textContent = `Maximum number: ${maxNumber}`;
    document.getElementById("minimumNo").textContent = `Minimum number: ${minNumber}`;
    document.getElementById("sumOfEven").textContent = `Sum of even numbers: ${SumOfNumbers}`;
    document.getElementById("numbersGreaterThan5").textContent = `Numbers greater than 5: ${numbersGreaterThan5}`;
}

