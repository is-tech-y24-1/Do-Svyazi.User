namespace Do_Svyazi.User.Domain.Exceptions;

public class Do_Svyazi_User_InnerLogicException : Exception
{
    public Do_Svyazi_User_InnerLogicException()
    {
    }

    public Do_Svyazi_User_InnerLogicException(string message)
        : base(message)
    {
    }

    public Do_Svyazi_User_InnerLogicException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}