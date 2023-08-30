using System.Net;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IWebHostEnvironment _env;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IWebHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            _logger.LogInformation($"Incoming request: {httpContext.Request.Method} {httpContext.Request.Path}");

            await _next(httpContext);

            _logger.LogInformation($"Outgoing response: {httpContext.Response.StatusCode}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred.");
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        if (_env.IsDevelopment())
        {
            return context.Response.WriteAsync(new
            {
                context.Response.StatusCode,
                exception.Message,
                exception.StackTrace
            }.ToString());
        }

        return context.Response.WriteAsync(new
        {
            context.Response.StatusCode,
            Message = "An unexpected error occurred."
        }.ToString());
    }
}
