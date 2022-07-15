

namespace eShop.BuildingBlocks.Common.ErrorHandler;

 class ErrorHandler : IErrorHandler
{
    private JsonErrorResponse jsonErrorResponse;
    private readonly IWebHostEnvironment _env;
    public ErrorHandler(IWebHostEnvironment env)
    {
        _env = env;
        this.jsonErrorResponse = new JsonErrorResponse();
    }

    public JsonErrorResponse GetError(Exception ex)
    {
        string ErrorMessage = "Error - Please contact the manager ";
        if (_env.IsDevelopment())
        {
            jsonErrorResponse.DeveloperMessage = ex.ToString();
        }
        
        if (ex.GetType() == typeof(DivideByZeroException))
        {
            ErrorMessage = "Divide By Zero";
            jsonErrorResponse.StatusCode = 521;
        }
        if (ex.GetType() == typeof(System.Security.Cryptography.CryptographicException))
        {
            ErrorMessage = "Error In Decryption";
            jsonErrorResponse.StatusCode = 522;
        }
        if (ex.GetType() == typeof(FormatException))
        {
            ErrorMessage = "FormatError";
            jsonErrorResponse.StatusCode = 523;
        }
        if (ex.GetType() == typeof(SqlException))
        {
            SqlException? exsql = ex as SqlException;
            ErrorMessage = GetSqlServerError(exsql);
        }
        if (ex.GetType() == typeof(DbUpdateConcurrencyException))
        {
            ErrorMessage = "The desired information has been changed by another user. Try again";
        }
        if (ex.GetType() == typeof(DbUpdateException))
        {
            SqlException? exsql = ex.InnerException as SqlException;
            ErrorMessage = GetSqlServerError(exsql);
        }
        jsonErrorResponse.Messages =new [] { ErrorMessage };

        return jsonErrorResponse;
    }

    private string GetSqlServerError(SqlException exsql)
    {
        string ErrorMessage = "Data Base Error";
        
        if (exsql.Number == 2627)
        {
            ErrorMessage = "Duplicated Data";
            jsonErrorResponse.StatusCode = 531;
        }
        if (exsql.Number == 547)
        {
            ErrorMessage = " Because the information is dependent on other parts, it cannot be changed ";
            jsonErrorResponse.StatusCode = 532;
        }
        if (exsql.Number == 0 || exsql.Number == 2 || exsql.Number == -2)
        {
            ErrorMessage = " The database cannot be accessed ";
            jsonErrorResponse.StatusCode = 533;
        }
        return ErrorMessage;
    }
}



