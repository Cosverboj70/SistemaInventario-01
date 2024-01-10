using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaInventario.AccesoDatos.Migrations
{
    /// <inheritdoc />
    public partial class OrdenDetalle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrdenDetalle_Ordenes_OrdenId",
                table: "OrdenDetalle");

            migrationBuilder.DropForeignKey(
                name: "FK_OrdenDetalle_Productos_ProductoId",
                table: "OrdenDetalle");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrdenDetalle",
                table: "OrdenDetalle");

            migrationBuilder.RenameTable(
                name: "OrdenDetalle",
                newName: "OrdenDetalles");

            migrationBuilder.RenameIndex(
                name: "IX_OrdenDetalle_ProductoId",
                table: "OrdenDetalles",
                newName: "IX_OrdenDetalles_ProductoId");

            migrationBuilder.RenameIndex(
                name: "IX_OrdenDetalle_OrdenId",
                table: "OrdenDetalles",
                newName: "IX_OrdenDetalles_OrdenId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrdenDetalles",
                table: "OrdenDetalles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrdenDetalles_Ordenes_OrdenId",
                table: "OrdenDetalles",
                column: "OrdenId",
                principalTable: "Ordenes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrdenDetalles_Productos_ProductoId",
                table: "OrdenDetalles",
                column: "ProductoId",
                principalTable: "Productos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrdenDetalles_Ordenes_OrdenId",
                table: "OrdenDetalles");

            migrationBuilder.DropForeignKey(
                name: "FK_OrdenDetalles_Productos_ProductoId",
                table: "OrdenDetalles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrdenDetalles",
                table: "OrdenDetalles");

            migrationBuilder.RenameTable(
                name: "OrdenDetalles",
                newName: "OrdenDetalle");

            migrationBuilder.RenameIndex(
                name: "IX_OrdenDetalles_ProductoId",
                table: "OrdenDetalle",
                newName: "IX_OrdenDetalle_ProductoId");

            migrationBuilder.RenameIndex(
                name: "IX_OrdenDetalles_OrdenId",
                table: "OrdenDetalle",
                newName: "IX_OrdenDetalle_OrdenId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrdenDetalle",
                table: "OrdenDetalle",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrdenDetalle_Ordenes_OrdenId",
                table: "OrdenDetalle",
                column: "OrdenId",
                principalTable: "Ordenes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrdenDetalle_Productos_ProductoId",
                table: "OrdenDetalle",
                column: "ProductoId",
                principalTable: "Productos",
                principalColumn: "Id");
        }
    }
}
