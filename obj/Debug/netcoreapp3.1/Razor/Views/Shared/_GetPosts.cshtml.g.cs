#pragma checksum "C:\Users\максимилиан\source\repos\TrueStoryMVC\Views\Shared\_GetPosts.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "ecd4a080082ae419d39c64dc481c03eaf791b2d0"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared__GetPosts), @"mvc.1.0.view", @"/Views/Shared/_GetPosts.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\максимилиан\source\repos\TrueStoryMVC\Views\_ViewImports.cshtml"
using TrueStoryMVC;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\максимилиан\source\repos\TrueStoryMVC\Views\_ViewImports.cshtml"
using TrueStoryMVC.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"ecd4a080082ae419d39c64dc481c03eaf791b2d0", @"/Views/Shared/_GetPosts.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"6111c0b417022c5043cf18d1056a81b31b0d1a68", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared__GetPosts : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<TrueStoryMVC.Models.Post>>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "C:\Users\максимилиан\source\repos\TrueStoryMVC\Views\Shared\_GetPosts.cshtml"
 if (Model != null)
{

#line default
#line hidden
#nullable disable
            WriteLiteral("<div class=\"row\">\r\n");
#nullable restore
#line 6 "C:\Users\максимилиан\source\repos\TrueStoryMVC\Views\Shared\_GetPosts.cshtml"
     foreach (Post item in Model)
    {
        await Html.RenderPartialAsync("_OnePost", item);
    }

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\r\n");
#nullable restore
#line 11 "C:\Users\максимилиан\source\repos\TrueStoryMVC\Views\Shared\_GetPosts.cshtml"
}

#line default
#line hidden
#nullable disable
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<TrueStoryMVC.Models.Post>> Html { get; private set; }
    }
}
#pragma warning restore 1591