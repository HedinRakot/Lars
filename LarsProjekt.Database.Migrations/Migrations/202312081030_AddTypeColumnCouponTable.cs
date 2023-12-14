using FluentMigrator;

namespace LarsProjekt.Database.Migrations.Migrations;

[Migration(202312081030, "AddTypeColumnCouponTable")]

public class AddTypeColumnCouponTable : Migration
{
    public override void Up()
    {
        Create.Column("Type").OnTable("Coupons").AsString(int.MaxValue).NotNullable().WithDefaultValue("Percent");        
    }
    public override void Down() 
    {
        Delete.Column("Type").FromTable("Coupons");
    }
}
