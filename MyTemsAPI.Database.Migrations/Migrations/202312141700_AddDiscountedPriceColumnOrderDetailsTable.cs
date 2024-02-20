using FluentMigrator;

namespace MyTemsAPI.Database.Migrations.Migrations;

[Migration(202312141700, "AddDiscountedPriceColumnOrderDetailsTable")]

public class AddDiscountedPriceColumnOrderDetailsTable : Migration
{
    public override void Up()
    {
        Create.Column("DiscountedPrice").OnTable("OrderDetails").AsDecimal().NotNullable();        
    }
    public override void Down() 
    {
        Delete.Column("DiscountPrice").FromTable("OrderDetails");
    }
}
