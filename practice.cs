<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>User Information</title>
</head>
<body>

    <h2>User Information</h2>

    <label for="nameInput">Enter name:</label>
    <br>
    <input type="text" id="nameInput">
    <br><br>

    <label for="ageInput">Enter age:</label>
    <br>
    <input type="number" id="ageInput">
    <br><br>

    <label for="arrayInput">Enter hobbies (comma-separated):</label>
    <br>
    <input type="text" id="arrayInput">
    <br><br>

    <label for="isStudentSelect">Are you a student? (Select true/false):</label>
    <br>
    <select id="isStudentSelect">
        <option value="true">True</option>
        <option value="false" selected>False</option>
    </select>
    <br><br>

    <button id="showInfoButton">Show Information</button>

    <div id="output"></div>

    <script src="script.js"></script>
</body>
</html>


let name: string;
let age: number;
let hobbies: string[];
let isStudent: boolean;

function showInformation(): void {

    name = (document.getElementById("nameInput") as HTMLInputElement).value;

    age = Number(
        (document.getElementById("ageInput") as HTMLInputElement).value
    );

    const hobbiesInput = (
        document.getElementById("arrayInput") as HTMLInputElement
    ).value;

    hobbies = hobbiesInput
        .split(",")
        .map(hobby => hobby.trim());

    const studentValue = (
        document.getElementById("isStudentSelect") as HTMLSelectElement
    ).value;

    isStudent = studentValue === "true";

    const output = document.getElementById("output") as HTMLDivElement;

    output.innerHTML =
        `Name: ${name}, Type: ${typeof name}<br>` +
        `Age: ${age}, Type: ${typeof age}<br>` +
        `Hobbies: ${hobbies.join(",")}, Type: ${typeof hobbies}<br>` +
        `Student: ${isStudent}, Type: ${typeof isStudent}`;
}

document
    .getElementById("showInfoButton")
    ?.addEventListener("click", showInformation);
