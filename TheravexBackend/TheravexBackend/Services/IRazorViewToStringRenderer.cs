namespace TheravexBackend.Services
{
    public interface IRazorViewToStringRenderer
    {
        Task<string> RenderViewToStringAsync(string viewName, object model);
    }

}
