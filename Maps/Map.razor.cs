using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using Maps;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Maps
{
    public partial class Map:ComponentBase
    {
        private string MapId = $"map-{Guid.NewGuid().ToString()}";

        [Parameter]
        public List<Marker> Markers { get; set; }

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            await jsRuntime.InvokeVoidAsync("deliveryMap.showOrUpdate", MapId, Markers);
        }
    }
}
