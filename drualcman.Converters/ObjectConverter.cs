using drualcman.Converters.Extensions;
using System.Threading.Tasks;

namespace drualcman.Converters
{
    public static class ObjectConverter
    {
        #region Methods
        #region TO
        public static string ToJson(object o)
            => o.ToJson();
        #endregion
        #endregion

        #region async
        #region TO
        public static async Task<string> ToJsonAsync(object o)
            => await o.ToJsonAsync();
        #endregion
        #endregion

    }
}
