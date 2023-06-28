namespace FrogExhibitionBLL.Interfaces.IProvider
{
    public interface IUserProvider
    {
        string GetUserEmail();
        Task<string> GetUserIdAsync();
    }
}