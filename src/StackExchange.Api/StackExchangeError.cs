namespace StackExchange.Api
{
    public enum StackExchangeError
    {
        BadParameter = 400,
        AccessTokenRequired = 401,
        InvalidAccessToken = 402,
        AccessDenied = 403,
        NoMethod = 404,
        KeyRequired = 405,
        AccessTokenCompromised = 406,
        InternalError = 500,
        ThrottleViolation = 502,
        TemporarilyUnavailable = 503
    }
}