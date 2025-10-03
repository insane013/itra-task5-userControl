namespace Task5.Models.User;

public class UserFilter : BaseFilter
{
    public UserFilter()
    {
        this.PageNumber = 1;
        this.PageSize = 10;
    }
}
