namespace Do_Svyazi.User.Domain.Exceptions;

public class Do_Svyazi_User_BusinessLogicException : Exception
{
    public Do_Svyazi_User_BusinessLogicException()
    {
    }

    public Do_Svyazi_User_BusinessLogicException(string message)
        : base(message)
    {
    }

    public Do_Svyazi_User_BusinessLogicException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}