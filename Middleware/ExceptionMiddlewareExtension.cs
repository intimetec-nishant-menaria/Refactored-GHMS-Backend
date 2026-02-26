namespace guest_house_management_backend.Middleware
{
    public static class ExceptionMiddlewareExtension
    {
        public static IApplicationBuilder UseGlobalExceptionMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<GlobalExceptionMiddleware>();
        }
    }
}
