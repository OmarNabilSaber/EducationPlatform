# Education Platform - Database ER Diagram

```mermaid
erDiagram
    %% User Management
    ApplicationUser {
        string Id PK
        string UserName
        string Email
        string FullName
        string Role
    }

    Student {
        string Id PK, FK
    }

    Teacher {
        string Id PK, FK
    }

    %% Academic Structure
    Class {
        int ClassId PK
        string ClassName
        string TeacherId FK
    }

    Subject {
        int SubjectId PK
        string Name
        string Description
    }

    ClassSubject {
        int ClassId PK, FK
        int SubjectId PK, FK
    }

    %% Learning Content
    Assignment {
        int AssignmentId PK
        string Title
        string Description
        string FilePath
        datetime DueDate
        int TotalScore
        int SubjectId FK
        int ClassId FK
    }

    Book {
        int BookId PK
        string Name
        string Description
        string ImageUrl
        string BookUrl
    }

    Exam {
        int ExamId PK
        string Title
        string Instructions
        datetime AvailableFrom
        datetime AvailableTo
        int TimeLimitMinutes
        int PassingScore
        int ClassId FK
        int SubjectId FK
    }

    Question {
        int QuestionId PK
        string Text
        decimal Points
        json Options
        string CorrectAnswer
        int ExamId FK
    }

    %% Student Activities
    Enrollment {
        int ClassId PK, FK
        string StudentId PK, FK
    }

    AssignmentSubmission {
        int AssignmentId PK, FK
        string StudentId PK, FK
        string FilePath
        datetime SubmissionDate
        decimal Score
    }

    ExamSubmission {
        int ExamId PK, FK
        string StudentId PK, FK
        json Answers
        datetime SubmissionDate
        decimal Score
    }

    %% System Features
    Notification {
        int Id PK
        string Title
        string Message
        string UserId FK
        boolean IsRead
        datetime CreatedAt
    }

    %% Relationships
    ApplicationUser ||--o{ Student : "inherits"
    ApplicationUser ||--o{ Teacher : "inherits"
    
    Teacher ||--o{ Class : "teaches"
    Class ||--o{ Enrollment : "has students"
    Class ||--o{ ClassSubject : "has subjects"
    Class ||--o{ Assignment : "has assignments"
    Class ||--o{ Exam : "has exams"
    
    Subject ||--o{ ClassSubject : "taught in classes"
    Subject ||--o{ Assignment : "has assignments"
    Subject ||--o{ Exam : "has exams"
    
    Assignment ||--o{ AssignmentSubmission : "has submissions"
    Exam ||--o{ Question : "has questions"
    Exam ||--o{ ExamSubmission : "has submissions"
    
    Student ||--o{ Enrollment : "enrolls in"
    Student ||--o{ AssignmentSubmission : "submits"
    Student ||--o{ ExamSubmission : "takes"
    
    ApplicationUser ||--o{ Notification : "receives"
```

## Database Schema Summary

### **Primary Keys:**
- **Simple PKs**: ApplicationUser.Id, Class.ClassId, Subject.SubjectId, Assignment.AssignmentId, Exam.ExamId, Question.QuestionId, Book.BookId, Notification.Id
- **Composite PKs**: 
  - ClassSubject (ClassId + SubjectId)
  - Enrollment (ClassId + StudentId)
  - AssignmentSubmission (AssignmentId + StudentId)
  - ExamSubmission (ExamId + StudentId)

### **Foreign Key Relationships:**
- **One-to-Many**: Teacher → Classes, Class → Assignments/Exams, Subject → Assignments/Exams
- **Many-to-Many**: 
  - Students ↔ Classes (via Enrollment)
  - Classes ↔ Subjects (via ClassSubject)
- **One-to-Many**: Assignment → Submissions, Exam → Questions/Submissions

### **Special Data Types:**
- **JSON Fields**: Question.Options, ExamSubmission.Answers
- **Nullable Fields**: AssignmentSubmission.Score, ExamSubmission.Score
- **Precision Decimals**: Scores (18,2 precision)
- **File Paths**: Assignment.FilePath, AssignmentSubmission.FilePath, Book.ImageUrl/BookUrl

### **Cascade Delete Rules:**
- Class deletion → cascades to Assignments, Exams, Enrollments, ClassSubjects
- Subject deletion → cascades to Assignments, Exams, ClassSubjects
- Assignment deletion → cascades to AssignmentSubmissions
- Exam deletion → cascades to Questions, ExamSubmissions
- User deletion → cascades to Notifications, AssignmentSubmissions, ExamSubmissions 