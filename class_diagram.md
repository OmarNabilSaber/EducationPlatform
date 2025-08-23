# Education Platform - Class Diagram

```mermaid
classDiagram
    %% User Management
    class ApplicationUser {
        +string Id
        +string UserName
        +string Email
        +string FullName
        +string Role
        +List~Notification~ Notifications
    }

    class Student {
        +ICollection~Enrollment~ Enrollments
        +ICollection~ExamSubmission~ ExamSubmissions
        +ICollection~AssignmentSubmission~ AssignmentSubmission
    }

    class Teacher {
        +ICollection~Class~ Classes
    }

    %% Academic Structure
    class Class {
        +int ClassId
        +string ClassName
        +string TeacherId
        +ApplicationUser Teacher
        +ICollection~Enrollment~ Enrollments
        +ICollection~ClassSubject~ Subjects
        +ICollection~Assignment~ Assignments
        +ICollection~Exam~ Exams
    }

    class Subject {
        +int SubjectId
        +string Name
        +string Description
        +ICollection~ClassSubject~ Classes
        +ICollection~Assignment~ Assignments
        +ICollection~Exam~ Exams
    }

    class ClassSubject {
        +int ClassId
        +int SubjectId
        +Class Class
        +Subject Subject
    }

    %% Learning Content
    class Assignment {
        +int AssignmentId
        +string Title
        +string Description
        +string FilePath
        +DateTime DueDate
        +int TotalScore
        +int SubjectId
        +Subject Subject
        +int ClassId
        +Class Class
        +ICollection~AssignmentSubmission~ Submissions
    }

    class Book {
        +int BookId
        +string Name
        +string Description
        +string ImageUrl
        +string BookUrl
    }

    class Exam {
        +int ExamId
        +string Title
        +string Instructions
        +DateTime AvailableFrom
        +DateTime AvailableTo
        +int TimeLimitMinutes
        +int PassingScore
        +bool IsAvailable
        +int ClassId
        +Class Class
        +int SubjectId
        +Subject Subject
        +ICollection~Question~ Questions
        +ICollection~ExamSubmission~ Submissions
    }

    class Question {
        +int QuestionId
        +string Text
        +decimal Points
        +List~string~ Options
        +string CorrectAnswer
        +int ExamId
        +Exam Exam
    }

    %% Student Activities
    class Enrollment {
        +int ClassId
        +string StudentId
        +Class Class
        +Student Student
    }

    class AssignmentSubmission {
        +int AssignmentId
        +string StudentId
        +string FilePath
        +DateTime SubmissionDate
        +decimal? Score
        +Assignment Assignment
        +Student Student
    }

    class ExamSubmission {
        +int ExamId
        +string StudentId
        +Dictionary~int,string~ Answers
        +DateTime SubmissionDate
        +decimal? Score
        +Exam Exam
        +Student Student
    }

    %% System Features
    class Notification {
        +int Id
        +string Title
        +string Message
        +string UserId
        +ApplicationUser User
        +bool IsRead
        +DateTime CreatedAt
        +static SendNotification()
    }

    %% Relationships
    ApplicationUser <|-- Student
    ApplicationUser <|-- Teacher

    %% Class Relationships
    Class ||--o{ Enrollment : "has"
    Class ||--o{ ClassSubject : "has"
    Class ||--o{ Assignment : "has"
    Class ||--o{ Exam : "has"
    Class }o--|| Teacher : "taught by"

    %% Subject Relationships
    Subject ||--o{ ClassSubject : "has"
    Subject ||--o{ Assignment : "has"
    Subject ||--o{ Exam : "has"

    %% Assignment Relationships
    Assignment }o--|| Subject : "belongs to"
    Assignment }o--|| Class : "belongs to"
    Assignment ||--o{ AssignmentSubmission : "has"

    %% Exam Relationships
    Exam }o--|| Class : "belongs to"
    Exam }o--|| Subject : "belongs to"
    Exam ||--o{ Question : "has"
    Exam ||--o{ ExamSubmission : "has"

    %% Question Relationships
    Question }o--|| Exam : "belongs to"

    %% Student Activity Relationships
    Student ||--o{ Enrollment : "has"
    Student ||--o{ AssignmentSubmission : "has"
    Student ||--o{ ExamSubmission : "has"

    %% Enrollment Relationships
    Enrollment }o--|| Class : "enrolls in"
    Enrollment }o--|| Student : "enrolled by"

    %% Submission Relationships
    AssignmentSubmission }o--|| Assignment : "submits to"
    AssignmentSubmission }o--|| Student : "submitted by"
    ExamSubmission }o--|| Exam : "submits to"
    ExamSubmission }o--|| Student : "submitted by"

    %% Notification Relationships
    Notification }o--|| ApplicationUser : "notifies"

    %% ClassSubject Junction Table
    ClassSubject }o--|| Class : "links"
    ClassSubject }o--|| Subject : "links"

    %% Teacher Relationships
    Teacher ||--o{ Class : "teaches"
```

## Database Schema Overview

### **Core Entities:**

1. **User Management**
   - `ApplicationUser` (Base user with Identity)
   - `Student` (Extends ApplicationUser)
   - `Teacher` (Extends ApplicationUser)

2. **Academic Structure**
   - `Class` (Courses with teachers)
   - `Subject` (Course subjects)
   - `ClassSubject` (Many-to-many relationship)

3. **Learning Content**
   - `Assignment` (Homework with files)
   - `Book` (Digital library)
   - `Exam` (Timed assessments)
   - `Question` (Exam questions)

4. **Student Activities**
   - `Enrollment` (Student-Class relationship)
   - `AssignmentSubmission` (Student submissions)
   - `ExamSubmission` (Exam answers)

5. **System Features**
   - `Notification` (User notifications)

### **Key Relationships:**

- **One-to-Many**: Teacher → Classes, Class → Assignments/Exams
- **Many-to-Many**: Students ↔ Classes (via Enrollment), Classes ↔ Subjects (via ClassSubject)
- **Composite Keys**: AssignmentSubmission (AssignmentId + StudentId), ExamSubmission (ExamId + StudentId)
- **Inheritance**: Student and Teacher inherit from ApplicationUser

### **Data Types:**
- **JSON Storage**: Question options, Exam answers
- **Precision Decimals**: Scores (18,2)
- **File Paths**: Assignment files, Book files, Submission files
- **Timestamps**: Due dates, submission dates, availability windows 