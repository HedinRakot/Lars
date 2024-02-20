using FluentMigrator;

namespace MyTemsAPI.Database.Migrations.Migrations;

[Migration(202312211220, "AddExpiryDateCouponTable")]

public class AddExpiryDateCouponTable : Migration
{
    public override void Up()
    {
        
        Create.Column("ExpiryDate").OnTable("Coupons").AsDateTimeOffset().NotNullable().WithDefaultValue(DateTimeOffset.Now);
    }
    public override void Down() 
    {
        Delete.Column("ExpiryDate").FromTable("Coupons");
    }
}
