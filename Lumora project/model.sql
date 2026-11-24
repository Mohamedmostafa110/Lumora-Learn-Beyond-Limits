CREATE TABLE [Admins] (
    [AdminId] nvarchar(36) NOT NULL DEFAULT (LOWER(CONVERT(nvarchar(36), NEWID()))),
    [FullName] nvarchar(200) NOT NULL,
    [Email] nvarchar(256) NOT NULL,
    [AccountStatus] nvarchar(30) NOT NULL,
    [EmailVerifiedAt] datetime2 NULL,
    CONSTRAINT [PK_Admins] PRIMARY KEY ([AdminId])
);
GO


CREATE TABLE [Categories] (
    [CategoryId] nvarchar(36) NOT NULL DEFAULT (LOWER(CONVERT(nvarchar(36), NEWID()))),
    [Name] nvarchar(100) NOT NULL,
    CONSTRAINT [PK_Categories] PRIMARY KEY ([CategoryId])
);
GO


CREATE TABLE [Coupons] (
    [CouponId] nvarchar(36) NOT NULL DEFAULT (LOWER(CONVERT(nvarchar(36), NEWID()))),
    [Code] nvarchar(50) NOT NULL,
    [DiscountType] nvarchar(30) NOT NULL,
    [DiscountValue] decimal(18,2) NOT NULL,
    [ValidFrom] datetime2 NOT NULL,
    [ValidTo] datetime2 NOT NULL,
    [Status] nvarchar(30) NOT NULL,
    CONSTRAINT [PK_Coupons] PRIMARY KEY ([CouponId])
);
GO


CREATE TABLE [Instructors] (
    [InstructorId] nvarchar(36) NOT NULL DEFAULT (LOWER(CONVERT(nvarchar(36), NEWID()))),
    [FullName] nvarchar(200) NOT NULL,
    [Email] nvarchar(256) NOT NULL,
    [AccountStatus] nvarchar(30) NOT NULL,
    [EmailVerifiedAt] datetime2 NULL,
    CONSTRAINT [PK_Instructors] PRIMARY KEY ([InstructorId])
);
GO


CREATE TABLE [Students] (
    [StudentId] nvarchar(36) NOT NULL DEFAULT (LOWER(CONVERT(nvarchar(36), NEWID()))),
    [FullName] nvarchar(200) NOT NULL,
    [Email] nvarchar(256) NOT NULL,
    [AccountStatus] nvarchar(30) NOT NULL,
    [EmailVerifiedAt] datetime2 NULL,
    [CreatedAt] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
    CONSTRAINT [PK_Students] PRIMARY KEY ([StudentId])
);
GO


CREATE TABLE [Tags] (
    [TagId] nvarchar(36) NOT NULL DEFAULT (LOWER(CONVERT(nvarchar(36), NEWID()))),
    [Name] nvarchar(100) NOT NULL,
    CONSTRAINT [PK_Tags] PRIMARY KEY ([TagId])
);
GO


CREATE TABLE [Courses] (
    [CourseId] nvarchar(36) NOT NULL DEFAULT (LOWER(CONVERT(nvarchar(36), NEWID()))),
    [Title] nvarchar(250) NOT NULL,
    [Description] nvarchar(max) NULL,
    [Difficulty] nvarchar(30) NOT NULL,
    [Price] decimal(18,2) NOT NULL,
    [Discount] decimal(18,2) NOT NULL,
    [Status] nvarchar(30) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [PublishedAt] datetime2 NULL,
    [InstructorId] nvarchar(36) NOT NULL,
    CONSTRAINT [PK_Courses] PRIMARY KEY ([CourseId]),
    CONSTRAINT [FK_Courses_Instructors_InstructorId] FOREIGN KEY ([InstructorId]) REFERENCES [Instructors] ([InstructorId]) ON DELETE NO ACTION
);
GO


CREATE TABLE [Orders] (
    [OrderId] nvarchar(36) NOT NULL DEFAULT (LOWER(CONVERT(nvarchar(36), NEWID()))),
    [TotalAmount] decimal(18,2) NOT NULL,
    [Currency] nvarchar(3) NOT NULL,
    [Status] nvarchar(30) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [StudentId] nvarchar(36) NOT NULL,
    CONSTRAINT [PK_Orders] PRIMARY KEY ([OrderId]),
    CONSTRAINT [FK_Orders_Students_StudentId] FOREIGN KEY ([StudentId]) REFERENCES [Students] ([StudentId]) ON DELETE NO ACTION
);
GO


CREATE TABLE [Announcements] (
    [AnnouncementId] nvarchar(36) NOT NULL DEFAULT (LOWER(CONVERT(nvarchar(36), NEWID()))),
    [Title] nvarchar(250) NOT NULL,
    [Body] nvarchar(max) NOT NULL,
    [PublishedAt] datetime2 NOT NULL,
    [CourseId] nvarchar(36) NOT NULL,
    [InstructorId] nvarchar(36) NOT NULL,
    CONSTRAINT [PK_Announcements] PRIMARY KEY ([AnnouncementId]),
    CONSTRAINT [FK_Announcements_Courses_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([CourseId]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Announcements_Instructors_InstructorId] FOREIGN KEY ([InstructorId]) REFERENCES [Instructors] ([InstructorId]) ON DELETE NO ACTION
);
GO


CREATE TABLE [ApprovalEvents] (
    [ApprovalEventId] nvarchar(36) NOT NULL DEFAULT (LOWER(CONVERT(nvarchar(36), NEWID()))),
    [Decision] nvarchar(30) NOT NULL,
    [ReviewedAt] datetime2 NOT NULL,
    [AdminId] nvarchar(36) NOT NULL,
    [CourseId] nvarchar(36) NOT NULL,
    CONSTRAINT [PK_ApprovalEvents] PRIMARY KEY ([ApprovalEventId]),
    CONSTRAINT [FK_ApprovalEvents_Admins_AdminId] FOREIGN KEY ([AdminId]) REFERENCES [Admins] ([AdminId]) ON DELETE NO ACTION,
    CONSTRAINT [FK_ApprovalEvents_Courses_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([CourseId]) ON DELETE NO ACTION
);
GO


CREATE TABLE [Certificates] (
    [CertificateId] nvarchar(36) NOT NULL DEFAULT (LOWER(CONVERT(nvarchar(36), NEWID()))),
    [VerificationId] nvarchar(100) NOT NULL,
    [PdfReference] nvarchar(500) NOT NULL,
    [IssuedAt] datetime2 NOT NULL,
    [StudentId] nvarchar(36) NOT NULL,
    [CourseId] nvarchar(36) NOT NULL,
    CONSTRAINT [PK_Certificates] PRIMARY KEY ([CertificateId]),
    CONSTRAINT [FK_Certificates_Courses_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([CourseId]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Certificates_Students_StudentId] FOREIGN KEY ([StudentId]) REFERENCES [Students] ([StudentId]) ON DELETE NO ACTION
);
GO


CREATE TABLE [Course_Category] (
    [CourseId] nvarchar(36) NOT NULL,
    [CategoryId] nvarchar(36) NOT NULL,
    CONSTRAINT [PK_Course_Category] PRIMARY KEY ([CourseId], [CategoryId]),
    CONSTRAINT [FK_Course_Category_Categories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [Categories] ([CategoryId]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Course_Category_Courses_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([CourseId]) ON DELETE CASCADE
);
GO


CREATE TABLE [Course_Tag] (
    [CourseId] nvarchar(36) NOT NULL,
    [TagId] nvarchar(36) NOT NULL,
    CONSTRAINT [PK_Course_Tag] PRIMARY KEY ([CourseId], [TagId]),
    CONSTRAINT [FK_Course_Tag_Courses_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([CourseId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Course_Tag_Tags_TagId] FOREIGN KEY ([TagId]) REFERENCES [Tags] ([TagId]) ON DELETE NO ACTION
);
GO


CREATE TABLE [Enrollments] (
    [EnrollmentId] nvarchar(36) NOT NULL DEFAULT (LOWER(CONVERT(nvarchar(36), NEWID()))),
    [Status] nvarchar(30) NOT NULL,
    [EnrolledAt] datetime2 NOT NULL,
    [CompletedAt] datetime2 NULL,
    [StudentId] nvarchar(36) NOT NULL,
    [CourseId] nvarchar(36) NOT NULL,
    CONSTRAINT [PK_Enrollments] PRIMARY KEY ([EnrollmentId]),
    CONSTRAINT [FK_Enrollments_Courses_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([CourseId]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Enrollments_Students_StudentId] FOREIGN KEY ([StudentId]) REFERENCES [Students] ([StudentId]) ON DELETE NO ACTION
);
GO


CREATE TABLE [QuestionThreads] (
    [QuestionThreadId] nvarchar(36) NOT NULL DEFAULT (LOWER(CONVERT(nvarchar(36), NEWID()))),
    [Body] nvarchar(max) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [StudentId] nvarchar(36) NOT NULL,
    [CourseId] nvarchar(36) NOT NULL,
    CONSTRAINT [PK_QuestionThreads] PRIMARY KEY ([QuestionThreadId]),
    CONSTRAINT [FK_QuestionThreads_Courses_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([CourseId]) ON DELETE NO ACTION,
    CONSTRAINT [FK_QuestionThreads_Students_StudentId] FOREIGN KEY ([StudentId]) REFERENCES [Students] ([StudentId]) ON DELETE NO ACTION
);
GO


CREATE TABLE [Reviews] (
    [ReviewId] nvarchar(36) NOT NULL DEFAULT (LOWER(CONVERT(nvarchar(36), NEWID()))),
    [Rating] int NOT NULL,
    [Body] nvarchar(max) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [StudentId] nvarchar(36) NOT NULL,
    [CourseId] nvarchar(36) NOT NULL,
    CONSTRAINT [PK_Reviews] PRIMARY KEY ([ReviewId]),
    CONSTRAINT [CK_Review_Rating] CHECK ([Rating] BETWEEN 1 AND 5),
    CONSTRAINT [FK_Reviews_Courses_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([CourseId]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Reviews_Students_StudentId] FOREIGN KEY ([StudentId]) REFERENCES [Students] ([StudentId]) ON DELETE NO ACTION
);
GO


CREATE TABLE [Sections] (
    [SectionId] nvarchar(36) NOT NULL DEFAULT (LOWER(CONVERT(nvarchar(36), NEWID()))),
    [Title] nvarchar(250) NOT NULL,
    [SortOrder] int NOT NULL,
    [CourseId] nvarchar(36) NOT NULL,
    CONSTRAINT [PK_Sections] PRIMARY KEY ([SectionId]),
    CONSTRAINT [FK_Sections_Courses_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([CourseId]) ON DELETE NO ACTION
);
GO


CREATE TABLE [OrderCoupons] (
    [CouponId] nvarchar(36) NOT NULL,
    [OrderId] nvarchar(36) NOT NULL,
    [AppliedAmount] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_OrderCoupons] PRIMARY KEY ([CouponId], [OrderId]),
    CONSTRAINT [FK_OrderCoupons_Coupons_CouponId] FOREIGN KEY ([CouponId]) REFERENCES [Coupons] ([CouponId]) ON DELETE NO ACTION,
    CONSTRAINT [FK_OrderCoupons_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Orders] ([OrderId]) ON DELETE CASCADE
);
GO


CREATE TABLE [OrderItems] (
    [ItemId] nvarchar(36) NOT NULL DEFAULT (LOWER(CONVERT(nvarchar(36), NEWID()))),
    [PriceAtPurchase] decimal(18,2) NOT NULL,
    [OrderId] nvarchar(36) NOT NULL,
    [CourseId] nvarchar(36) NOT NULL,
    CONSTRAINT [PK_OrderItems] PRIMARY KEY ([ItemId]),
    CONSTRAINT [FK_OrderItems_Courses_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([CourseId]) ON DELETE NO ACTION,
    CONSTRAINT [FK_OrderItems_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Orders] ([OrderId]) ON DELETE CASCADE
);
GO


CREATE TABLE [Payments] (
    [PaymentId] nvarchar(36) NOT NULL DEFAULT (LOWER(CONVERT(nvarchar(36), NEWID()))),
    [ExternalId] nvarchar(200) NOT NULL,
    [Amount] decimal(18,2) NOT NULL,
    [Currency] nvarchar(3) NOT NULL,
    [Status] nvarchar(30) NOT NULL,
    [PaidAt] datetime2 NULL,
    [Provider] nvarchar(50) NOT NULL,
    [OrderId] nvarchar(36) NOT NULL,
    CONSTRAINT [PK_Payments] PRIMARY KEY ([PaymentId]),
    CONSTRAINT [FK_Payments_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Orders] ([OrderId]) ON DELETE NO ACTION
);
GO


CREATE TABLE [Answers] (
    [AnswerId] nvarchar(36) NOT NULL DEFAULT (LOWER(CONVERT(nvarchar(36), NEWID()))),
    [Body] nvarchar(max) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [QuestionThreadId] nvarchar(36) NOT NULL,
    [InstructorId] nvarchar(36) NOT NULL,
    CONSTRAINT [PK_Answers] PRIMARY KEY ([AnswerId]),
    CONSTRAINT [FK_Answers_Instructors_InstructorId] FOREIGN KEY ([InstructorId]) REFERENCES [Instructors] ([InstructorId]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Answers_QuestionThreads_QuestionThreadId] FOREIGN KEY ([QuestionThreadId]) REFERENCES [QuestionThreads] ([QuestionThreadId]) ON DELETE CASCADE
);
GO


CREATE TABLE [ReviewVotes] (
    [ReviewVoteId] nvarchar(36) NOT NULL DEFAULT (LOWER(CONVERT(nvarchar(36), NEWID()))),
    [Helpful] bit NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [ReviewId] nvarchar(36) NOT NULL,
    [StudentId] nvarchar(36) NOT NULL,
    CONSTRAINT [PK_ReviewVotes] PRIMARY KEY ([ReviewVoteId]),
    CONSTRAINT [FK_ReviewVotes_Reviews_ReviewId] FOREIGN KEY ([ReviewId]) REFERENCES [Reviews] ([ReviewId]) ON DELETE CASCADE,
    CONSTRAINT [FK_ReviewVotes_Students_StudentId] FOREIGN KEY ([StudentId]) REFERENCES [Students] ([StudentId]) ON DELETE NO ACTION
);
GO


CREATE TABLE [Lessons] (
    [LessonId] nvarchar(36) NOT NULL DEFAULT (LOWER(CONVERT(nvarchar(36), NEWID()))),
    [Title] nvarchar(250) NOT NULL,
    [SortOrder] int NOT NULL,
    [CourseId] nvarchar(36) NOT NULL,
    [SectionId] nvarchar(36) NOT NULL,
    CONSTRAINT [PK_Lessons] PRIMARY KEY ([LessonId]),
    CONSTRAINT [FK_Lessons_Courses_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([CourseId]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Lessons_Sections_SectionId] FOREIGN KEY ([SectionId]) REFERENCES [Sections] ([SectionId]) ON DELETE NO ACTION
);
GO


CREATE TABLE [Quizzes] (
    [QuizId] nvarchar(36) NOT NULL DEFAULT (LOWER(CONVERT(nvarchar(36), NEWID()))),
    [Title] nvarchar(250) NOT NULL,
    [PassingScore] decimal(5,2) NOT NULL,
    [CourseId] nvarchar(36) NOT NULL,
    [SectionId] nvarchar(36) NOT NULL,
    CONSTRAINT [PK_Quizzes] PRIMARY KEY ([QuizId]),
    CONSTRAINT [FK_Quizzes_Courses_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([CourseId]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Quizzes_Sections_SectionId] FOREIGN KEY ([SectionId]) REFERENCES [Sections] ([SectionId]) ON DELETE NO ACTION
);
GO


CREATE TABLE [Bookmarks] (
    [BookmarkId] nvarchar(36) NOT NULL DEFAULT (LOWER(CONVERT(nvarchar(36), NEWID()))),
    [CreatedAt] datetime2 NOT NULL,
    [Position] int NOT NULL,
    [StudentId] nvarchar(36) NOT NULL,
    [LessonId] nvarchar(36) NOT NULL,
    CONSTRAINT [PK_Bookmarks] PRIMARY KEY ([BookmarkId]),
    CONSTRAINT [FK_Bookmarks_Lessons_LessonId] FOREIGN KEY ([LessonId]) REFERENCES [Lessons] ([LessonId]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Bookmarks_Students_StudentId] FOREIGN KEY ([StudentId]) REFERENCES [Students] ([StudentId]) ON DELETE NO ACTION
);
GO


CREATE TABLE [LessonProgresses] (
    [LessonProgressId] nvarchar(36) NOT NULL DEFAULT (LOWER(CONVERT(nvarchar(36), NEWID()))),
    [IsCompleted] bit NOT NULL,
    [CompletedAt] datetime2 NULL,
    [EnrollmentId] nvarchar(36) NOT NULL,
    [LessonId] nvarchar(36) NOT NULL,
    CONSTRAINT [PK_LessonProgresses] PRIMARY KEY ([LessonProgressId]),
    CONSTRAINT [FK_LessonProgresses_Enrollments_EnrollmentId] FOREIGN KEY ([EnrollmentId]) REFERENCES [Enrollments] ([EnrollmentId]) ON DELETE NO ACTION,
    CONSTRAINT [FK_LessonProgresses_Lessons_LessonId] FOREIGN KEY ([LessonId]) REFERENCES [Lessons] ([LessonId]) ON DELETE NO ACTION
);
GO


CREATE TABLE [MediaAssets] (
    [MediaAssetId] nvarchar(36) NOT NULL DEFAULT (LOWER(CONVERT(nvarchar(36), NEWID()))),
    [StorageKey] nvarchar(500) NOT NULL,
    [ThumbnailKey] nvarchar(500) NULL,
    [ProcessingStatus] nvarchar(30) NOT NULL,
    [LessonId] nvarchar(36) NOT NULL,
    CONSTRAINT [PK_MediaAssets] PRIMARY KEY ([MediaAssetId]),
    CONSTRAINT [FK_MediaAssets_Lessons_LessonId] FOREIGN KEY ([LessonId]) REFERENCES [Lessons] ([LessonId]) ON DELETE CASCADE
);
GO


CREATE TABLE [Notes] (
    [NoteId] nvarchar(36) NOT NULL DEFAULT (LOWER(CONVERT(nvarchar(36), NEWID()))),
    [Content] nvarchar(max) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [StudentId] nvarchar(36) NOT NULL,
    [LessonId] nvarchar(36) NOT NULL,
    CONSTRAINT [PK_Notes] PRIMARY KEY ([NoteId]),
    CONSTRAINT [FK_Notes_Lessons_LessonId] FOREIGN KEY ([LessonId]) REFERENCES [Lessons] ([LessonId]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Notes_Students_StudentId] FOREIGN KEY ([StudentId]) REFERENCES [Students] ([StudentId]) ON DELETE NO ACTION
);
GO


CREATE TABLE [Questions] (
    [QuestionId] nvarchar(36) NOT NULL DEFAULT (LOWER(CONVERT(nvarchar(36), NEWID()))),
    [Prompt] nvarchar(max) NOT NULL,
    [Type] nvarchar(30) NOT NULL,
    [Explanation] nvarchar(max) NULL,
    [SortOrder] int NOT NULL,
    [QuizId] nvarchar(36) NOT NULL,
    CONSTRAINT [PK_Questions] PRIMARY KEY ([QuestionId]),
    CONSTRAINT [FK_Questions_Quizzes_QuizId] FOREIGN KEY ([QuizId]) REFERENCES [Quizzes] ([QuizId]) ON DELETE CASCADE
);
GO


CREATE TABLE [QuizAttempts] (
    [QuizAttemptId] nvarchar(36) NOT NULL DEFAULT (LOWER(CONVERT(nvarchar(36), NEWID()))),
    [Score] decimal(5,2) NOT NULL,
    [IsPassed] bit NOT NULL,
    [AttemptedAt] datetime2 NOT NULL,
    [QuizId] nvarchar(36) NOT NULL,
    [StudentId] nvarchar(36) NOT NULL,
    CONSTRAINT [PK_QuizAttempts] PRIMARY KEY ([QuizAttemptId]),
    CONSTRAINT [FK_QuizAttempts_Quizzes_QuizId] FOREIGN KEY ([QuizId]) REFERENCES [Quizzes] ([QuizId]) ON DELETE NO ACTION,
    CONSTRAINT [FK_QuizAttempts_Students_StudentId] FOREIGN KEY ([StudentId]) REFERENCES [Students] ([StudentId]) ON DELETE NO ACTION
);
GO


CREATE TABLE [AttemptAnswers] (
    [AttemptAnswerId] nvarchar(36) NOT NULL DEFAULT (LOWER(CONVERT(nvarchar(36), NEWID()))),
    [Response] nvarchar(max) NOT NULL,
    [IsCorrect] bit NOT NULL,
    [QuestionId] nvarchar(36) NOT NULL,
    [QuizAttemptId] nvarchar(36) NOT NULL,
    CONSTRAINT [PK_AttemptAnswers] PRIMARY KEY ([AttemptAnswerId]),
    CONSTRAINT [FK_AttemptAnswers_Questions_QuestionId] FOREIGN KEY ([QuestionId]) REFERENCES [Questions] ([QuestionId]) ON DELETE NO ACTION,
    CONSTRAINT [FK_AttemptAnswers_QuizAttempts_QuizAttemptId] FOREIGN KEY ([QuizAttemptId]) REFERENCES [QuizAttempts] ([QuizAttemptId]) ON DELETE CASCADE
);
GO


CREATE UNIQUE INDEX [IX_Admins_Email] ON [Admins] ([Email]);
GO


CREATE INDEX [IX_Announcements_CourseId] ON [Announcements] ([CourseId]);
GO


CREATE INDEX [IX_Announcements_InstructorId] ON [Announcements] ([InstructorId]);
GO


CREATE INDEX [IX_Answers_InstructorId] ON [Answers] ([InstructorId]);
GO


CREATE INDEX [IX_Answers_QuestionThreadId] ON [Answers] ([QuestionThreadId]);
GO


CREATE INDEX [IX_ApprovalEvents_AdminId] ON [ApprovalEvents] ([AdminId]);
GO


CREATE INDEX [IX_ApprovalEvents_CourseId] ON [ApprovalEvents] ([CourseId]);
GO


CREATE INDEX [IX_AttemptAnswers_QuestionId] ON [AttemptAnswers] ([QuestionId]);
GO


CREATE INDEX [IX_AttemptAnswers_QuizAttemptId] ON [AttemptAnswers] ([QuizAttemptId]);
GO


CREATE INDEX [IX_Bookmarks_LessonId] ON [Bookmarks] ([LessonId]);
GO


CREATE INDEX [IX_Bookmarks_StudentId] ON [Bookmarks] ([StudentId]);
GO


CREATE INDEX [IX_Certificates_CourseId] ON [Certificates] ([CourseId]);
GO


CREATE INDEX [IX_Certificates_StudentId] ON [Certificates] ([StudentId]);
GO


CREATE UNIQUE INDEX [IX_Certificates_VerificationId] ON [Certificates] ([VerificationId]);
GO


CREATE UNIQUE INDEX [IX_Coupons_Code] ON [Coupons] ([Code]);
GO


CREATE INDEX [IX_Course_Category_CategoryId] ON [Course_Category] ([CategoryId]);
GO


CREATE INDEX [IX_Course_Tag_TagId] ON [Course_Tag] ([TagId]);
GO


CREATE INDEX [IX_Courses_InstructorId] ON [Courses] ([InstructorId]);
GO


CREATE INDEX [IX_Enrollments_CourseId] ON [Enrollments] ([CourseId]);
GO


CREATE UNIQUE INDEX [IX_Enrollments_StudentId_CourseId] ON [Enrollments] ([StudentId], [CourseId]);
GO


CREATE UNIQUE INDEX [IX_Instructors_Email] ON [Instructors] ([Email]);
GO


CREATE UNIQUE INDEX [IX_LessonProgresses_EnrollmentId_LessonId] ON [LessonProgresses] ([EnrollmentId], [LessonId]);
GO


CREATE INDEX [IX_LessonProgresses_LessonId] ON [LessonProgresses] ([LessonId]);
GO


CREATE INDEX [IX_Lessons_CourseId] ON [Lessons] ([CourseId]);
GO


CREATE INDEX [IX_Lessons_SectionId] ON [Lessons] ([SectionId]);
GO


CREATE INDEX [IX_MediaAssets_LessonId] ON [MediaAssets] ([LessonId]);
GO


CREATE INDEX [IX_Notes_LessonId] ON [Notes] ([LessonId]);
GO


CREATE INDEX [IX_Notes_StudentId] ON [Notes] ([StudentId]);
GO


CREATE INDEX [IX_OrderCoupons_OrderId] ON [OrderCoupons] ([OrderId]);
GO


CREATE INDEX [IX_OrderItems_CourseId] ON [OrderItems] ([CourseId]);
GO


CREATE INDEX [IX_OrderItems_OrderId] ON [OrderItems] ([OrderId]);
GO


CREATE INDEX [IX_Orders_StudentId] ON [Orders] ([StudentId]);
GO


CREATE UNIQUE INDEX [IX_Payments_ExternalId] ON [Payments] ([ExternalId]);
GO


CREATE INDEX [IX_Payments_OrderId] ON [Payments] ([OrderId]);
GO


CREATE INDEX [IX_Questions_QuizId] ON [Questions] ([QuizId]);
GO


CREATE INDEX [IX_QuestionThreads_CourseId] ON [QuestionThreads] ([CourseId]);
GO


CREATE INDEX [IX_QuestionThreads_StudentId] ON [QuestionThreads] ([StudentId]);
GO


CREATE INDEX [IX_QuizAttempts_QuizId] ON [QuizAttempts] ([QuizId]);
GO


CREATE INDEX [IX_QuizAttempts_StudentId] ON [QuizAttempts] ([StudentId]);
GO


CREATE INDEX [IX_Quizzes_CourseId] ON [Quizzes] ([CourseId]);
GO


CREATE INDEX [IX_Quizzes_SectionId] ON [Quizzes] ([SectionId]);
GO


CREATE INDEX [IX_Reviews_CourseId] ON [Reviews] ([CourseId]);
GO


CREATE UNIQUE INDEX [IX_Reviews_StudentId_CourseId] ON [Reviews] ([StudentId], [CourseId]);
GO


CREATE INDEX [IX_ReviewVotes_ReviewId] ON [ReviewVotes] ([ReviewId]);
GO


CREATE UNIQUE INDEX [IX_ReviewVotes_StudentId_ReviewId] ON [ReviewVotes] ([StudentId], [ReviewId]);
GO


CREATE INDEX [IX_Sections_CourseId] ON [Sections] ([CourseId]);
GO


CREATE UNIQUE INDEX [IX_Students_Email] ON [Students] ([Email]);
GO


