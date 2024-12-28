

namespace CsPostApi.Utils.Extensions;

public static class CustomDiManager
{
    public static WebApplicationBuilder InjectDependencies(this WebApplicationBuilder builder)
    {
        return builder;
    }
}