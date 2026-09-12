using CBS.Core.Accounts.Domain.Exceptions;
using CBS.Core.Exceptions;

namespace CBS.Api.Middlewares;

public class ExceptionMiddleware(RequestDelegate next)
{
  public async Task Invoke(HttpContext httpContext)
  {
    try
    {
      await next(httpContext);
    }
    catch (AccountBlockedException ex)
    {
      LogInConsole(ex);
      await SendHttpWithStatus400(httpContext, new { ex.Message, ex.AccountId });
    }
    catch (InsufficientFundsException ex)
    {
      LogInConsole(ex);
      await SendHttpWithStatus400(httpContext, new { ex.Message, ex.Balance, ex.AccountId });
    }
    catch (AmountIsNegativeException ex)
    {
      LogInConsole(ex);
      await SendHttpWithStatus400(httpContext, new { ex.Message, ex.Amount, ex.AccountId });
    }
    catch (CurrencyMismatchException ex)
    {
      LogInConsole(ex);
      await SendHttpWithStatus400(httpContext, new
      {
        ex.Message, Currency = ex.CurrencyCode, CurrencyOther = ex.CurrencyCodeOther
      });
    }
    catch (ObjectNotFoundException ex)
    {
      LogInConsole(ex);
      httpContext.Response.StatusCode = 404;
      await httpContext.Response.WriteAsJsonAsync(new { ex.Message });
    }
    catch (ArgumentException ex)
    {
      LogInConsole(ex);
      await SendHttpWithStatus400(httpContext, new { ex.Message });
    }
    catch (Exception ex)
    {
      LogInConsole(ex);
      httpContext.Response.StatusCode = 500;
      await httpContext.Response.WriteAsJsonAsync(new { message = "Unhandled exception occured" });
    }
  }

  private static void LogInConsole(Exception ex)
  {
    Console.WriteLine($"[{DateTime.UtcNow}] {ex.GetType()}: {ex.Message}");
  }

  private static async Task SendHttpWithStatus400(HttpContext httpContext, object data)
  {
    httpContext.Response.StatusCode = 400;
    await httpContext.Response.WriteAsJsonAsync(data);
  }
}
