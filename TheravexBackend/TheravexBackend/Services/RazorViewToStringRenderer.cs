//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Abstractions;
//using Microsoft.AspNetCore.Mvc.ModelBinding;
//using Microsoft.AspNetCore.Mvc.Razor;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.AspNetCore.Mvc.ViewFeatures;

//namespace TheravexBackend.Services
//{
//    public class RazorViewToStringRenderer : IRazorViewToStringRenderer
//    {
//        private readonly IRazorViewEngine _viewEngine;
//        private readonly ITempDataProvider _tempDataProvider;
//        private readonly IServiceProvider _serviceProvider;

//        public RazorViewToStringRenderer(
//            IRazorViewEngine viewEngine,
//            ITempDataProvider tempDataProvider,
//            IServiceProvider serviceProvider)
//        {
//            _viewEngine = viewEngine;
//            _tempDataProvider = tempDataProvider;
//            _serviceProvider = serviceProvider;
//        }

//        public async Task<string> RenderViewToStringAsync(string viewName, object model)
//        {
//            var httpContext = new DefaultHttpContext { RequestServices = _serviceProvider };
//            var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());

//            using var sw = new StringWriter();

//            var viewResult = _viewEngine.FindView(actionContext, viewName, false);
//            if (!viewResult.Success)
//                throw new Exception($"Vue {viewName} introuvable");

//            var viewDictionary = new ViewDataDictionary(
//                new EmptyModelMetadataProvider(),
//                new ModelStateDictionary())
//            {
//                Model = model
//            };

//            var viewContext = new ViewContext(
//                actionContext,
//                viewResult.View,
//                viewDictionary,
//                new TempDataDictionary(actionContext.HttpContext, _tempDataProvider),
//                sw,
//                new HtmlHelperOptions()
//            );

//            await viewResult.View.RenderAsync(viewContext);
//            return sw.ToString();
//        }
//    }
//}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;

namespace TheravexBackend.Services
{
    public class RazorViewToStringRenderer : IRazorViewToStringRenderer
    {
        private readonly IRazorViewEngine _viewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IServiceProvider _serviceProvider;

        public RazorViewToStringRenderer(
            IRazorViewEngine viewEngine,
            ITempDataProvider tempDataProvider,
            IServiceProvider serviceProvider)
        {
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
        }

        public async Task<string> RenderViewToStringAsync(string viewName, object model)
        {
            var httpContext = new DefaultHttpContext { RequestServices = _serviceProvider };
            var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());

            using var sw = new StringWriter();

            // Try multiple ways to locate the view:
            // 1) FindView (uses ActionContext/controller/action)
            // 2) GetView with the provided name
            // 3) Common absolute locations (~/Views/{View}/{View}.cshtml and ~/Views/Shared/{View}.cshtml)
            ViewEngineResult viewResult = _viewEngine.FindView(actionContext, viewName, isMainPage: false);

            if (!viewResult.Success)
                viewResult = _viewEngine.GetView(executingFilePath: null, viewPath: viewName, isMainPage: false);

            if (!viewResult.Success)
            {
                var candidates = new[]
                {
                    $"~/Views/{viewName}/{viewName}.cshtml",
                    $"~/Views/Shared/{viewName}.cshtml",
                    $"/Views/{viewName}/{viewName}.cshtml",
                    $"/Views/Shared/{viewName}.cshtml"
                };

                foreach (var path in candidates)
                {
                    viewResult = _viewEngine.GetView(executingFilePath: null, viewPath: path, isMainPage: false);
                    if (viewResult.Success) break;
                }
            }

            //if (!viewResult.Success)
            //{
            //    var searched = viewResult?.SearchedLocations.Count > 0/* is { Length: > 0 }*/ ? string.Join(Environment.NewLine, viewResult.SearchedLocations) : "No locations reported by view engine.";
            //    throw new Exception($"Vue \"{viewName}\" introuvable. Emplacements cherchés:{Environment.NewLine}{searched}");
            //}

            var viewDictionary = new ViewDataDictionary(
                new EmptyModelMetadataProvider(),
                new ModelStateDictionary())
            {
                Model = model
            };

            var viewContext = new ViewContext(
                actionContext,
                viewResult.View,
                viewDictionary,
                new TempDataDictionary(actionContext.HttpContext, _tempDataProvider),
                sw,
                new HtmlHelperOptions()
            );

            await viewResult.View.RenderAsync(viewContext);
            return sw.ToString();
        }
    }
}
