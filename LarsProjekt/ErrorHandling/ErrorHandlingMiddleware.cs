namespace LarsProjekt.ErrorHandling;

public class ErrorHandlingMiddleware
{
    private RequestDelegate _next;
    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch
        {            
            context.Response.Redirect("/Error/UnexpectedError");
        }
    }
}
