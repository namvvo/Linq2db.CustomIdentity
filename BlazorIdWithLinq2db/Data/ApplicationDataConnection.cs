using BlazorIdWithLinq2dbModels;
using Linq2DB.CustomIdentity;

namespace BlazorIdWithLinq2db.Data
{
    public class ApplicationDataConnection : IdentityDataConnection<AspNetUser>
    {
    }
}
