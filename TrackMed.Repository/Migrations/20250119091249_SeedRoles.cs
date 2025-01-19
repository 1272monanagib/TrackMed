using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackMed.Repository.Migrations
{
    /// <inheritdoc />
    public partial class SeedRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                            INSERT INTO AspNetRoles(Id , Name , NormalizedName , ConcurrencyStamp)
                            VALUES (NEWID() , 'Customer' , 'CUSTOMER' , NEWID());

                            INSERT INTO AspNetRoles(Id , Name , NormalizedName , ConcurrencyStamp)
                            VALUES (NEWID() , 'Hospital' , 'HOSPITAL' , NEWID());

                            INSERT INTO AspNetRoles(Id , Name , NormalizedName , ConcurrencyStamp)
                            VALUES (NEWID() , 'Engineer' , 'ENGINEER' , NEWID());

                            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
