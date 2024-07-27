using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Talabat.Repository.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAddressEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShippingAddress_Street",
                table: "Orders",
                newName: "Street");

            migrationBuilder.RenameColumn(
                name: "ShippingAddress_Country",
                table: "Orders",
                newName: "Country");

            migrationBuilder.RenameColumn(
                name: "ShippingAddress_City",
                table: "Orders",
                newName: "City");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Street",
                table: "Orders",
                newName: "ShippingAddress_Street");

            migrationBuilder.RenameColumn(
                name: "Country",
                table: "Orders",
                newName: "ShippingAddress_Country");

            migrationBuilder.RenameColumn(
                name: "City",
                table: "Orders",
                newName: "ShippingAddress_City");
        }
    }
}
