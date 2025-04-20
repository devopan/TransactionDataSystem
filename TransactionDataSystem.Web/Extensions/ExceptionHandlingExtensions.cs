using Microsoft.AspNetCore.Builder;
using TransactionDataSystem.Web.Middleware;

namespace TransactionDataSystem.Web.Extensions
{
    public static class ExceptionHandlingExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}