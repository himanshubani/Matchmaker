Week_5_Day_6_Session_3_Q_1
==========================

-----------Index.html-----------
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Library Management System</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            max-width: 1200px;
            margin: 20px auto;
            padding: 0 20px;
        }

        h1 {
            margin-bottom: 30px;
        }

        #bookGrid {
            display: grid;
            grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
            gap: 20px;
        }

        .book {
            padding: 20px;
            border-radius: 8px;
            background-color: #f5f5f5;
            box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
        }

        .book h3 {
            margin-top: 0;
            margin-bottom: 10px;
        }

        .book p {
            margin: 10px 0;
            color: #666;
        }

        .button-group {
            display: flex;
            gap: 10px;
            margin-top: 15px;
        }

        button {
            padding: 8px 16px;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            font-size: 14px;
        }

        button:disabled {
            opacity: 0.5;
            cursor: not-allowed;
        }

        .borrow-button {
            background-color: #007bff;
            color: white;
        }

        .return-button {
            background-color: #6c757d;
            color: white;
        }
    </style>
</head>
<body>
    <h1>Library Management System</h1>
    
    <div id="bookGrid">
        <!-- Books will be dynamically added here -->
    </div>

    <script src="script.js"></script>
</body>
</html>

----------Script.ts----------
interface Book {
    id: number;
    title: string;
    author: string;
    isBorrowed: boolean;
}

class LibraryManager {
    private books: Book[] = [
        {
            id: 0,
            title: "1984",
            author: "George Orwell",
            isBorrowed: false
        },
        {
            id: 1,
            title: "To Kill a Mockingbird",
            author: "Harper Lee",
            isBorrowed: false
        },
        {
            id: 2,
            title: "The Great Gatsby",
            author: "F. Scott Fitzgerald",
            isBorrowed: false
        }
    ];

    constructor() {
        this.loadBooks();
        this.renderBooks();
    }

    private loadBooks(): void {
        const savedBooks = localStorage.getItem('libraryBooks');
        if (savedBooks) {
            this.books = JSON.parse(savedBooks);
        }
    }

    private saveBooks(): void {
        localStorage.setItem('libraryBooks', JSON.stringify(this.books));
    }

    private borrowBook(id: number): void {
        const book = this.books.find(b => b.id === id);
        if (book) {
            book.isBorrowed = true;
            this.saveBooks();
            this.renderBooks();
        }
    }

    private returnBook(id: number): void {
        const book = this.books.find(b => b.id === id);
        if (book) {
            book.isBorrowed = false;
            this.saveBooks();
            this.renderBooks();
        }
    }

    private createBookElement(book: Book): HTMLDivElement {
        const bookElement = document.createElement('div');
        bookElement.className = 'book';

        const title = document.createElement('h3');
        title.textContent = book.title;

        const author = document.createElement('p');
        author.textContent = `by ${book.author}`;

        const status = document.createElement('p');
        status.textContent = `Is borrowed: ${book.isBorrowed}`;

        const buttonGroup = document.createElement('div');
        buttonGroup.className = 'button-group';

        const borrowButton = document.createElement('button');
        borrowButton.className = 'borrow-button';
        borrowButton.id = `borrowButton-${book.id}`;
        borrowButton.textContent = 'Borrow';
        borrowButton.disabled = book.isBorrowed;
        borrowButton.addEventListener('click', () => this.borrowBook(book.id));

        const returnButton = document.createElement('button');
        returnButton.className = 'return-button';
        returnButton.id = `returnButton-${book.id}`;
        returnButton.textContent = 'Return';
        returnButton.disabled = !book.isBorrowed;
        returnButton.addEventListener('click', () => this.returnBook(book.id));

        buttonGroup.appendChild(borrowButton);
        buttonGroup.appendChild(returnButton);

        bookElement.appendChild(title);
        bookElement.appendChild(author);
        bookElement.appendChild(status);
        bookElement.appendChild(buttonGroup);

        return bookElement;
    }

    private renderBooks(): void {
        const bookGrid = document.getElementById('bookGrid');
        if (!bookGrid) return;

        bookGrid.innerHTML = '';
        this.books.forEach(book => {
            const bookElement = this.createBookElement(book);
            bookGrid.appendChild(bookElement);
        });
    }
}

// Initialize the library manager when the DOM is loaded
document.addEventListener('DOMContentLoaded', () => {
    new LibraryManager();
});

==========Commands==========
nvm use 14
npm install
npx tsc
npm start
