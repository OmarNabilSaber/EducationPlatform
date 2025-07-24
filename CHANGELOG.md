# Changelog

All notable changes to the Online Education Platform project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [v1.0.0] - 2025-07-22

### Added - Subject Module Implementation
**Merge Summary**: Successfully merged `feature/add-Class-controller-views` branch into master

#### New Controllers
- **BookController**: Full CRUD operations for book management
- **ClassController**: Complete class management functionality 
- **EnrollmentController**: Student enrollment management
- **SubjectController**: Subject creation and management

#### New Models & Data Layer
- **Assignment**: Assignment management model
- **AssignmentSubmission**: Student assignment submissions
- **Book**: Book entity with file upload support
- **Class**: Class/course management
- **ClassSubject**: Many-to-many relationship between classes and subjects
- **Enrollment**: Student-class enrollment tracking
- **Exam**: Examination system foundation
- **ExamSubmission**: Exam submission handling
- **Question**: Question bank for exams
- **Subject**: Subject/course subject definitions
- **Student & Teacher**: User role extensions

#### New Views & UI
- **Book Views**: Create, Edit, Delete, Details, Index pages
- **Class Views**: Complete CRUD interface
- **Subject Views**: Full management interface
- **Enrollment Views**: Student enrollment management
- **Enhanced Layout**: Updated navigation and styling

#### ViewModels & Architecture
- **ClassTeacherViewModel**: Class-teacher relationship management
- **EditEnrollmentViewModel**: Enrollment editing interface
- **SubjectDetailsViewModel & SubjectViewModel**: Subject management
- **TeacherStudentViewModel**: Teacher-student relationships
- Moved all ViewModels to dedicated ViewModels folder for better organization

#### Database & Migrations
- **New Migrations**: Complete schema updates for all new entities
- **Updated DbContext**: Enhanced with new DbSets and relationships
- **Book File Storage**: Implemented file upload system for books and images

#### Technical Improvements
- **GlobalUsing**: Added global using statements for cleaner code
- **Enhanced Configuration**: Updated launch settings and app configuration
- **Asset Management**: Book files and images storage system

### Changed
- **Files Modified**: 68 files changed
- **Code Statistics**: +47,133 insertions, -678 deletions
- **Architecture**: Improved separation of concerns with dedicated ViewModels folder
- **Navigation**: Enhanced site navigation with new module links

### Technical Details
- **Merge Type**: Fast-forward merge (no conflicts)
- **Commit Hash**: 8574969
- **Branch**: feature/add-Class-controller-views → master
- **Repository**: https://github.com/abdulllahhh/OnlineEducationPlatform.git

---

## [Initial Release] - Previous
- Basic user authentication and account management
- Initial project structure and configuration
