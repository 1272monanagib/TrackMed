using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackMed.Repository.Migrations
{
    /// <inheritdoc />
    public partial class SeedRoleForAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                             INSERT INTO AspNetRoles(Id , Name , NormalizedName , ConcurrencyStamp)
                             VALUES (NEWID() , 'Admin' , 'ADMIN' , NEWID());
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
