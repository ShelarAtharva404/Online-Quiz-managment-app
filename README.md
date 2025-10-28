Library Management System (ASP.NET Core MVC)
A simple Library Management System built using ASP.NET Core MVC, Tailwind CSS, and JSON files as the database (no SQL required).

This project allows users to register, login, browse books, borrow and return books, and for the librarian (Admin) to manage books.

Features
User registration and login
First registered user is Admin (Librarian)
Admin can add new books
Users can browse, borrow, and return books
Search books by title or author
JSON-based data storage (no database required)
Clean TailwindCSS UI
Installation Steps
Clone the repository
git clone <YOUR_GITHUB_URL>
cd LibraryApp

1. **Run the Project**
dotnet run



# How to Use

1. First Registration
    The first user to register becomes the Admin (Librarian)
    Admin can add books and manage the library

2. Subsequent Users
    Any other user will have User role
    Users can browse books, borrow, and return books
    Users cannot add books

3. Login
    Navigate to /Account/Login
    Admin and Users use the same login page

4. Borrow Books
    Click on View on a book
    If logged in, click Borrow to borrow the book
    Borrowed books are tracked in borrow.json

5. Return Books
    Go to My Books (in navbar)
    Click Return to submit the book back to the library
About
A Library Management System, where the system store which user had borrowed which book with its Timing and Date

Resources
 Readme
 Activity
Stars
 0 stars
Watchers
 0 watching
Forks
 0 forks
Report repository
Releases
No releases published
Packages
No packages published
Contributors
2
@Aditya-1725
Aditya-1725 Vawhal Aditya Dipak
@AdityaVawhal
AdityaVawhal Vawhal Aditya Dipak
Languages
C#
47.1%
 
HTML
35.8%
 
CSS
16.3%
 
JavaScript
0.8%
Footer
© 2025 GitHub, Inc.
Footer navigation
Terms
Privacy

