using FluentMigrator;

namespace MyTemsAPI.Database.Migrations.Migrations;

[Migration(202312142100, "AddExpiredCloumnCouponTable")]

public class AddExpiredCloumnCouponTable : Migration
{
    public override void Up()
    {
        Create.Column("Expired").OnTable("Coupons").AsBoolean().NotNullable().WithDefaultValue(false);
    }
    public override void Down() 
    {
        Delete.Column("Expired").FromTable("Coupons");
    }
}
