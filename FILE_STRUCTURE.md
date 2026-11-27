# 🏛️ Lumora Learning Platform — Team Engineering & File Structure Guide
**دليل المعمارية وهيكلة الملفات لفريق العمل (Code Raiders — DEPI Round 5)**

---

## 📌 نظرة عامة على المعمارية (N-Tier Enterprise Architecture)
تم تقسيم المشروع إلى **4 مشاريع مستقلة (Class Libraries + MVC Web App)** داخل نفس الـ Solution لتطبيق مبدأ **فصل المسؤوليات (Separation of Concerns)**، وتسهيل عمل الـ 6 أفراد في الفريق بدون حدوث أي Git Conflicts.

```text
Lumora project/
│
├── Lumora.sln                          # ملف الـ Solution الكلاسيكي (Visual Studio)
├── Lumora.slnx                         # ملف الـ Solution الحديث (.NET 10 / VS 2022 v17.10+)
│
├── Lumora.Core/                        # [Layer 1] قلب النظام والموديلز والواجهات (Domain Entities & Interfaces)
│   ├── Common/                         # الـ Custom Exceptions والـ Constants المشتركة
│   ├── DTOs/                           # كائنات نقل البيانات بين الطبقات (Courses, Orders, Certificates)
│   ├── Models/                         # جداول قاعدة البيانات الـ 30 (Course, Student, Instructor, Section, Lesson...)
│   │   └── Enums/                      # التصنيفات (CourseStatus, Difficulty, PaymentStatus, AccountStatus...)
│   ├── Repositories/
│   │   └── Interfaces/                 # عقود التعامل مع الداتابيز (IGenericRepository, ICourseRepository, IUnitOfWork...)
│   ├── Services/
│   │   └── Interfaces/                 # عقود البيزنس والخدمات (ICourseCatalogService, ICheckoutService...)
│   └── GlobalUsings.cs
│
├── Lumora.Data/                        # [Layer 2] طبقة البيانات والاتصال بقاعدة البيانات (EF Core & SQL Server)
│   ├── Data/
│   │   ├── AppDbContext.cs             # كلاس الـ DbContext الرسمي ومجموعات الـ DbSets
│   │   ├── DbSeeder.cs                 # تغذية الداتابيز المبدئية بالبيانات للتجربة
│   │   └── Configurations/             # ضبط العلاقات والمفاتيح عبر Fluent API لكل جدول منفصلاً
│   ├── Migrations/                     # ملفات مايجريشنز الـ Entity Framework Core
│   ├── Repositories/
│   │   └── Implementations/            # الكود الفعلي لتنفيذ الـ Repositories والـ UnitOfWork
│   └── GlobalUsings.cs
│
├── Lumora.Services/                    # [Layer 3] طبقة العمليات والمنطق والخدمات الخارجية (Business Logic Layer)
│   ├── Infrastructure/                 # محولات الخدمات الخارجية (Azure Blob Storage, Stripe, iTextSharp PDF)
│   ├── Services/
│   │   ├── Implementations/            # الكود الفعلي لخدمات المنصة (CourseCatalogService, CheckoutService...)
│   │   ├── Strategies/                 # استراتيجيات الخصومات والكوبونات (Strategy Pattern)
│   │   └── Observers/                  # نظام مراقبة استكمال الكورس وإصدار الشهادات فوراً (Observer Pattern)
│   └── GlobalUsings.cs
│
└── Lumora.Web/                         # [Layer 4] طبقة واجهة المستخدم والـ MVC (Presentation Layer)
    ├── Controllers/                    # وحدات التحكم (HomeController, AccountController, CatalogController...)
    ├── ViewModels/                     # كائنات التحقق من صحة الفورمات والـ DataAnnotations
    ├── Views/                          # صفحات الـ Razor Views (.cshtml)
    │   ├── Account/                    # تسجيل الدخول، إنشاء حساب جديد، استعادة كلمة المرور
    │   ├── Catalog/                    # استعراض الكورسات، الفلاتر والبحث، وتفاصيل الكورس والمنهج
    │   ├── Checkout/                   # السلة، الدفع، وتأكيد الطلب
    │   ├── Home/                       # الصفحة الرئيسية (Landing Page)
    │   ├── Verification/               # فحص وتوثيق صحة الشهادات الصادرة برقم الكود
    │   ├── Shared/                     # القوالب المشتركة (_Layout, _AuthLayout, Navbar, Footer)
    │   ├── _ViewImports.cshtml         # تضمين الـ Namespaces والـ TagHelpers لكافة الصفحات
    │   └── _ViewStart.cshtml           # الربط التلقائي لكل الصفحات بالقالب الرئيسي _Layout
    ├── wwwroot/                        # الملفات الثابتة (css, js, images, logos)
    ├── appsettings.json                # إعدادات السيرفر وقاعدة البيانات (ConnectionStrings)
    ├── appsettings.Development.json
    └── Program.cs                      # نقطة انطلاق التطبيق وإعداد الـ Dependency Injection (DI)
```

---

## 🎯 مهام الفريق وتوزيع الملفات (Team Domain Ownership)

| اسم الزميل | الدور والمسؤولية | الملفات والمشاريع التي يعمل بها |
| :--- | :--- | :--- |
| **محمد مصطفى** *(Team Leader)* | **Public Website & Auth Layer** | • `Lumora.Web/Controllers/AccountController.cs` و `HomeController.cs`<br>• `Lumora.Web/Views/Account/` و `Home/` و `Shared/`<br>• إدارة المعمارية، التكامل العام، وتسجيل الخدمات في `Program.cs`. |
| **أحمد محمد** | **Database & Data Access Layer** | • **مشروع `Lumora.Data` بالكامل** (`AppDbContext`, `Configurations/`, `Migrations/`, `Repositories/Implementations/`)<br>• الـ Entities في `Lumora.Core/Models/`. |
| **أحمد هاني** | **Admin Layer** | • `Lumora.Web/Controllers/AdminController.cs`<br>• `Lumora.Web/Views/Admin/`<br>• تطوير عمليات الاعتماد والإشراف في `Lumora.Services/` (`AdminApprovalService`). |
| **حبيبة خالد** | **Instructor Layer** | • `Lumora.Web/Controllers/InstructorController.cs`<br>• `Lumora.Web/Views/Instructor/`<br>• تطوير منطق بناء الكورسات والمناهج والامتحانات في `Lumora.Services/`. |
| **مريم بدوي** | **Student Layer** | • `Lumora.Web/Controllers/StudentController.cs`<br>• `Lumora.Web/Views/Student/`<br>• تشغيل الفيديوهات، متابعة تقدم الطالب، وحل الامتحانات في `Lumora.Services/`. |
| **أسامة ممدوح** | **Dashboards & Frontend Polish** | • بناء وتطوير الـ 3 Dashboards (Student, Instructor, Admin)<br>• ضبط وتنسيق ملفات الـ CSS والـ JS في `Lumora.Web/wwwroot/` ومخططات `Chart.js`. |

---

## ⚙️ قواعد ذهبية للعمل على المشروع (Engineering Guidelines)

### 1. إعداد الاتصال بقاعدة البيانات (Connection String)
- **ممنوع نهائياً** كتابة الـ Connection String داخل كود C# (`Program.cs` أو `AppDbContext.cs`).
- كل عضو في الفريق يقوم بضبط اسم السيرفر الخاص بجهازه داخل ملف `appsettings.json` فقط تحت المفتاح:
  ```json
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=LumoraDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True;"
  }
  ```
  *(إذا كان السيرفر لديك باسم `SQLEXPRESS`، قم بتغيير `Server=.` إلى `Server=.\\SQLEXPRESS`).*

### 2. إدارة المايجريشنز (Database Migrations)
- **أحمد محمد** هو الشخص الوحيد المسؤول عن إصدار المايجريشنز (`Add-Migration` / `Update-Database`).
- في حال احتاج أي عضو إضافة جدول أو عمود جديد، يقوم بإخبار أحمد محمد ليقوم هو بإضافتها ورفعها، وذلك لتجنب تعارض تواريخ ملفات المايجريشنز (Migration Conflicts).

### 3. إنشاء خدمات جديدة (Adding New Services)
عند رغبتك في بناء ميزة جديدة، اتبع الخطوات الثلاث التالية:
- قم بتعريف الـ Interface أولاً داخل مشروع:  
  `Lumora.Core/Services/Interfaces/`
- قم بكتابة كود التنفيذ (Implementation) داخل مشروع:  
  `Lumora.Services/Services/Implementations/`
- اطلب من التيم ليدر (محمد مصطفى) تسجيلها في الـ Dependency Injection داخل `Program.cs` بسطر واحد:
  `builder.Services.AddScoped<IMyService, MyService>();`

### 4. التعامل مع الـ Git
- قبل بدء كتابة أي كود جديد، نفذ دائماً:
  ```bash
  git pull origin main
  ```
- لا تقم بتعديل ملفات تخص زميل آخر خارج نطاق مسؤوليتك لتجنب الـ Merge Conflicts.
- تأكد قبل عمل Commit أن الـ Solution تبني بنجاح عبر:
  ```bash
  dotnet build "Lumora project/Lumora.sln"
  ```

---

## 🚀 كيفية تشغيل المشروع محلياً (How to Run)
- افتح ملف السوليوشن [Lumora.sln](file:///D:/Lumora%20%E2%80%94%20Learn%20Beyond%20Limits/Lumora%20project/Lumora.sln) في Visual Studio.
- تأكد أن مشروع **`Lumora.Web`** هو الـ Startup Project.
- اضغط **`Ctrl + F5`** أو اكتب في الـ Terminal:
  ```bash
  dotnet run --project "Lumora project\Lumora.Web\Lumora.Web.csproj"
  ```
- افتح المتصفح على الرابط: **`http://localhost:5000`**

---
*Lumora Learning Platform — Learn Beyond Limits*  
*DEPI Round 5 | Digital Egypt Pioneers Initiative*
