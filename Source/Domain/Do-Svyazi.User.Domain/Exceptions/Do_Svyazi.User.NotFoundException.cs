namespace Do_Svyazi.User.Domain.Exceptions;

public class Do_Svyazi_User_NotFoundException : Exception
{
    public Do_Svyazi_User_NotFoundException()
    {
    }

    public Do_Svyazi_User_NotFoundException(string message)
        : base(message)
    {
    }

    public Do_Svyazi_User_NotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}