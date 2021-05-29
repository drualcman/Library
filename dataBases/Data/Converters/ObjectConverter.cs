using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Data;
using drualcman.Data.Extensions;

namespace drualcman.Data.Converters
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
