using Microsoft.EntityFrameworkCore.Migrations;

namespace MedicalInformationSystem.Migrations
{
    public partial class SeedAdminUserAndRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"INSERT INTO [dbo].[AspNetUsers] ([Id], [Discriminator], [PatientSSN], [FullName], [City], [Gender], [DateOfBirth], [RelativeOneName], [RelativeOnePhoneNumber], [RelativeTwoName], [RelativeTwoPhoneNumber], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'0f0cf8db-c7b0-49ce-a104-6f28a381667e', N'ApplicationUser', N'123456789123456', N'admin', N'string', N'male', N'2021-06-25 13:15:49', N'string', N'string', N'string', N'string', N'123456789123456', N'123456789123456', NULL, NULL, 0, N'AQAAAAEAACcQAAAAEGsycuzBgkiwObmWNCL50C2Xf9mVTqyhAXFsognJyHD/RobvQa4I/7A+DkC10UgO0A==', N'QPRQUON4OFRG6VJAQ6CVJMVDGZ6U7DW6', N'138018bc-af83-4bd0-9044-8dca3c5a4628', N'123456789123456', 0, 0, NULL, 1, 0)
INSERT INTO [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'0dc54a34-cfe3-4eb8-9840-091a7e0d3a47', N'hospital', N'HOSPITAL', N'991c94d1-2119-4acc-b3ab-6f8712240281')
INSERT INTO [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'c50553ea-1e37-45fa-ae71-e2e3c56098b2', N'Patient', N'PATIENT', N'20a7218e-57ec-4859-a237-d94d0d12de99')
INSERT INTO [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'f1e48500-23ff-4c39-bf09-db47510bc787', N'Admin', N'ADMIN', N'eeea5fa6-f3c2-4677-b5ab-5d917dc83119')
INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'0f0cf8db-c7b0-49ce-a104-6f28a381667e', N'f1e48500-23ff-4c39-bf09-db47510bc787')
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
