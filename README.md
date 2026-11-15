# 🌟 Lumora — Learn Beyond Limits
> Enterprise Interactive E-Learning Platform with Verifiable Credentials & Secured Cloud Video Streaming.

---

## 📌 Architecture Overview
The platform is built following **N-Tier Layered Architecture** with strict Separation of Concerns (SoC) and industry design patterns:

```
[ Presentation Tier ]  ➔ Controllers, ViewModels, Razor Views / REST Endpoints
        ⬇
[ Business Tier ]      ➔ Application Services, Strategy Pattern (Discounts), Observer Pattern (Certification)
        ⬇
[ Data Access Tier ]   ➔ Unit of Work & Repositories (ICourseRepository, IOrderRepository...)
        ⬇
[ Database Tier ]      ➔ Microsoft SQL Server (LumoraDb) via Entity Framework Core 10
```

---

## 🏛️ Project Structure

```
Lumora/
├── 📁 Common/                   # Cross-cutting concerns & domain exceptions
│   ├── 📁 Constants/            # Roles, status codes, currencies
│   └── 📁 Exceptions/           # DuplicatePurchaseException, InvalidCouponException
│
├── 📁 Controllers/              # Presentation Tier (MVC & API Controllers)
│   ├── HomeController.cs        # Landing page, counters, featured courses
│   ├── CatalogController.cs     # Catalog browsing & filtering
│   ├── VerificationController.cs# Public anti-fraud certificate verification
│   ├── StudentController.cs     # Student dashboard & learning portal
│   ├── InstructorController.cs  # Instructor course builder & analytics
│   ├── AdminController.cs       # Operational dashboard & course approval queue
│   └── CheckoutController.cs    # Cart, coupon discount calculation & payments
│
├── 📁 Data/                     # Data Access Layer (DAL)
│   ├── AppDbContext.cs          # EF Core context (31 DbSets, Cascade rules)
│   ├── DbSeeder.cs              # Initial seed data for rapid local development
│   └── 📁 Configurations/       # Fluent API Entity configurations (31 classes)
│
├── 📁 DTOs/                     # Data Transfer Objects
│   ├── 📁 Courses/              # CourseCardDto, CourseDetailDto
│   ├── 📁 Orders/               # CheckoutRequestDto, OrderSummaryDto
│   ├── 📁 Certificates/         # CertificateVerificationResultDto
│   └── 📁 Quizzes/              # QuizSubmissionDto, QuizEvaluationResultDto
│
├── 📁 Infrastructure/           # Cloud & External Service Providers
│   ├── 📁 Storage/              # Azure Blob Storage & SAS token generator
│   ├── 📁 Payments/             # Stripe Payment Gateway
│   └── 📁 Pdf/                  # Verifiable PDF Certificate generator
│
├── 📁 Migrations/               # EF Core database migrations
│
├── 📁 Models/                   # Domain Entities
│   ├── 📁 Enums/                # 11 Domain enums (CourseStatus, PaymentStatus, etc.)
│   └── ... (31 entity classes)
│
├── 📁 Repositories/             # Repository Pattern
│   ├── 📁 Interfaces/           # IGenericRepository, ICourseRepository, IUnitOfWork
│   └── 📁 Implementations/      # GenericRepository, CourseRepository, UnitOfWork
│
├── 📁 Services/                 # Business Logic Layer (BLL)
│   ├── 📁 Interfaces/           # ICourseCatalogService, ICheckoutService, etc.
│   ├── 📁 Implementations/      # CourseCatalogService, CheckoutService, etc.
│   ├── 📁 Strategies/           # Strategy Pattern for Coupon Discounts
│   └── 📁 Observers/            # Observer Pattern for Course Completion & Certificates
│
└── 📁 Views/                    # Presentation Layer Views (Bootstrap 5)
    ├── 📁 Shared/               # Master _Layout.cshtml and theme switcher
    ├── 📁 Home/                 # Landing page with counters and catalog
    └── 📁 Verification/         # Certificate authenticity verification portal
```

---

## 🚀 Getting Started

### 1. Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/)
- [Microsoft SQL Server](https://www.microsoft.com/sql-server) (LocalDB, MSSQLSERVER, or SQLEXPRESS)
- Visual Studio 2026 / Visual Studio Code

### 2. Database Setup & Migrations
Ensure SQL Server is running, then apply migrations:
```powershell
dotnet ef database update --project "Lumora project/Lumora/Lumora.csproj" --startup-project "Lumora project/Lumora/Lumora.csproj"
```

### 3. Run Application
```powershell
dotnet run --project "Lumora project/Lumora/Lumora.csproj"
```
The application will launch on `http://localhost:5000`.

---

## 👥 Team Module Assignments (Based on User Scenarios)

| Layer / Feature | Key Controllers & Services | Team Responsibilities |
| :--- | :--- | :--- |
| **Guest & Catalog** | `HomeController`, `CatalogController`, `ICourseCatalogService` | Landing hero, live counters, search & dynamic filter |
| **Certification & Anti-Fraud** | `VerificationController`, `ICertificateVerificationService`, `PdfCertificateGenerator` | Public verification bar, unforgeable QR/Code, PDF generation |
| **Student Layer & Player** | `StudentController`, `IStudentLearningService`, `AzureBlobStorageService` | My learning dashboard, secure video streaming (SAS tokens), notes & bookmarks |
| **Checkout & Monetization** | `CheckoutController`, `ICheckoutService`, `IDiscountStrategy`, `StripePaymentGateway` | Prevent duplicate purchases, Strategy pattern discounts, atomic Unit of Work |
| **Instructor Studio** | `InstructorController`, `IInstructorService` | Course builder, curriculum sorting, quiz bank, submit for review |
| **Admin Operations** | `AdminController`, `IAdminApprovalService` | Course approval queue (Approve/Reject), user moderation, audit trail |

---
&copy; 2026 Lumora Team. All Rights Reserved.
