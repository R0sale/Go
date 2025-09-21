using RazorLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class RazorRender
    {
        private readonly RazorLightEngine _lightEngine;

        public RazorRender()
        {
            _lightEngine = new RazorLightEngineBuilder()
                .UseFileSystemProject("/app/ViewsModels")
                .UseMemoryCachingProvider()
                .Build();
        }

        public async Task<string> RenderAsync<T>(string viewPath, T model)
        {
            return await _lightEngine.CompileRenderAsync(viewPath, model);
        }
    }
}
