using FluentMigrator;

namespace MyTemsAPI.Database.Migrations.Migrations;

[Migration(202312211500, "AddCountCouponTable")]

public class AddCountCouponTable : AutoReversingMigration
{
    public override void Up()
    {
        
        Create.Column("Count").OnTable("Coupons").AsInt32().NotNullable().WithDefaultValue(0);
        Create.Column("AppliedCount").OnTable("Coupons").AsInt32().NotNullable().WithDefaultValue(0);
        Create.Column("Version").OnTable("Coupons").AsCustom("rowversion").NotNullable();

    }

}
