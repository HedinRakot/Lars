using FluentMigrator;

namespace LarsProjekt.Database.Migrations.Migrations;

[Migration(202312111800, "AddDiscountColumnOrderDetailsTable")]

public class AddDiscountColumnOrderDetailsTable : Migration
{
    public override void Up()
    {
        Create.Column("Discount").OnTable("OrderDetails").AsDecimal().NotNullable();        
    }
    public override void Down() 
    {
        Delete.Column("Discount").FromTable("OrderDetails");
    }
}
