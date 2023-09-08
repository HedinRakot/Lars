namespace LarsProjekt.Application;

public class Singleton
{
    //public Singleton()
    //{
    //    CalledAt = new List<DateTime>();
    //    CalledAtWithOffset = new List<DateTimeOffset>();
    //}

    //public int Count { get; set; } 

    public List<DateTime> CalledAt { get; set; } = new List<DateTime>();

    public List<DateTimeOffset> CalledAtWithOffset { get; set; } = new List<DateTimeOffset>();

    public List<string> CalledAtAsString { get; set; } = new List<string>();
}