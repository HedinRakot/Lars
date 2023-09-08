namespace LarsProjekt.Application;

public class ScopedDependent
{
    private Scoped _scoped;
    public ScopedDependent(Scoped scoped)
    {
        _scoped = scoped;
    }

    public int Calculate()
    {
        return _scoped.Calls.Count;
    } 
}