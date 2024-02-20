using FluentMigrator;
using FluentMigrator.Expressions;

namespace MyTemsAPI.Database.Migrations.Migrations;

[Migration(202311161030, "Initial")]
public class Initial : AutoReversingMigration
{
    public override void Up()
    {
        Create.Table("Address")
            .WithColumn("Id").AsInt64().NotNullable().Identity().PrimaryKey()
            .WithColumn("FirstName").AsString(50).NotNullable()
            .WithColumn("LastName").AsString(50).NotNullable()
            .WithColumn("Street").AsString(50).NotNullable()
            .WithColumn("HouseNumber").AsString(10).NotNullable()
            .WithColumn("City").AsString(50).NotNullable()
            .WithColumn("State").AsString(50).Nullable()
            .WithColumn("PostalCode").AsString(50).NotNullable()
            .WithColumn("Country").AsString(50).Nullable()
            .WithColumn("Phone").AsString(50).Nullable();

        Create.Table("Users")
            .WithColumn("Id").AsInt64().NotNullable().Identity().PrimaryKey()
            .WithColumn("Username").AsString(50).NotNullable()
            .WithColumn("Email").AsString(50).NotNullable()
            .WithColumn("Password").AsString(50).NotNullable()
            .WithColumn("AddressId").AsInt64().NotNullable().ForeignKey("FK_Users_Address", "Address", "Id");

        Create.Table("Coupons")
            .WithColumn("Id").AsInt64().NotNullable().Identity().PrimaryKey()
            .WithColumn("Code").AsString(int.MaxValue).NotNullable()
            .WithColumn("Discount").AsString(int.MaxValue).Nullable();

        Create.Table("Orders")
            .WithColumn("Id").AsInt64().NotNullable().Identity().PrimaryKey()
            .WithColumn("Total").AsDecimal().NotNullable()
            .WithColumn("Date").AsDateTimeOffset().NotNullable()
            .WithColumn("AddressId").AsInt64().NotNullable().ForeignKey("FK_Orders_Address", "Address", "Id")
            .WithColumn("UserId").AsInt64().NotNullable().ForeignKey("FK_Orders_Users", "Users", "Id");

        Create.Table("Products")
            .WithColumn("Id").AsInt64().NotNullable().Identity().PrimaryKey()
            .WithColumn("Name").AsString(50).NotNullable()
            .WithColumn("Category").AsString(50).NotNullable()
            .WithColumn("Description").AsString(int.MaxValue).NotNullable()
            .WithColumn("Price").AsDecimal().NotNullable()
            .WithColumn("PriceOffer").AsDecimal().NotNullable()
            .WithColumn("Image").AsString(int.MaxValue).Nullable();

        Create.Table("OrderDetails")
            .WithColumn("Id").AsInt64().NotNullable().Identity().PrimaryKey()
            .WithColumn("Quantity").AsInt32().NotNullable()
            .WithColumn("UnitPrice").AsDecimal().NotNullable()
            .WithColumn("OrderId").AsInt64().NotNullable().ForeignKey("FK_OrderDetails_Orders", "Orders", "Id")
            .WithColumn("ProductId").AsInt64().NotNullable().ForeignKey("FK_OrderDetails_Products", "Products", "Id");

    }
}
