#pragma checksum "/Users/Alexander/Documents/Coding Dojo/C#/Dyno/Views/Home/Profile.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "a7868f2c8f706d6f0aff085b7b17f0c5bdef89ff"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Profile), @"mvc.1.0.view", @"/Views/Home/Profile.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Home/Profile.cshtml", typeof(AspNetCore.Views_Home_Profile))]
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
#line 1 "/Users/Alexander/Documents/Coding Dojo/C#/Dyno/Views/_ViewImports.cshtml"
using Dyno;

#line default
#line hidden
#line 2 "/Users/Alexander/Documents/Coding Dojo/C#/Dyno/Views/_ViewImports.cshtml"
using Dyno.Models;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a7868f2c8f706d6f0aff085b7b17f0c5bdef89ff", @"/Views/Home/Profile.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"d8c5e7d436a3cfd4e3863f0bf0b5a39adf887679", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_Profile : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #line hidden
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.HeadTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper;
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(0, 33, true);
            WriteLiteral("<!DOCTYPE html>\n<html lang=\"en\">\n");
            EndContext();
            BeginContext(33, 1394, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("head", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "a7868f2c8f706d6f0aff085b7b17f0c5bdef89ff3274", async() => {
                BeginContext(39, 1381, true);
                WriteLiteral(@"
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Document</title>
    <style>
        .whiteBG {
            background: white;
        }
        .main {
            border-radius: 10px;
            height: 100vh;
            margin-bottom: -85px;
            overflow: auto;
            text-align: center;
        }
        a {
            color: gray;
        }
        .a1 {
            height: 30px;
            width: 75px;
            text-align: center;
            line-height: 27px;
            float: right;
            border-radius: 8px;
            background: yellow;
            color: black;
            text-decoration: none;
            font-weight: 500;
            border: 2px solid black;
            margin-right: 30px;
            margin-top: 10px;
        }
        .article {
        }
        h3 {
            margin: auto;
        }
        .img123 {
            width: 95%;
            margin-top: 4%;
            margin-bottom: 1");
                WriteLiteral(@"%;
        }
        .leftT {
            margin-left: 2.5%;
            text-align: left !important;
        }

        .a3 {
            float: left;
            margin-top: 4px;
        }
        .divider {
            width: 95%;
            border-bottom: 2px solid black;
            margin: auto;
            margin-top: 40px;
        }
    </style>
");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.HeadTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(1427, 1, true);
            WriteLiteral("\n");
            EndContext();
            BeginContext(1428, 665, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("body", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "a7868f2c8f706d6f0aff085b7b17f0c5bdef89ff5856", async() => {
                BeginContext(1434, 42, true);
                WriteLiteral("\n    <div class=\"container whiteBG main\">\n");
                EndContext();
#line 65 "/Users/Alexander/Documents/Coding Dojo/C#/Dyno/Views/Home/Profile.cshtml"
         foreach(var i in @ViewBag.CurrUser.Interests)
        {

#line default
#line hidden
                BeginContext(1541, 69, true);
                WriteLiteral("            <div class=\"article\">\n                <img class=\"img123\"");
                EndContext();
                BeginWriteAttribute("src", " src=", 1610, "", 1632, 1);
#line 68 "/Users/Alexander/Documents/Coding Dojo/C#/Dyno/Views/Home/Profile.cshtml"
WriteAttributeValue("", 1615, i.Topic.TopicIMG, 1615, 17, false);

#line default
#line hidden
                EndWriteAttribute();
                BeginContext(1632, 36, true);
                WriteLiteral(">\n                <h3 class=\"leftT\">");
                EndContext();
                BeginContext(1669, 18, false);
#line 69 "/Users/Alexander/Documents/Coding Dojo/C#/Dyno/Views/Home/Profile.cshtml"
                             Write(i.Topic.TopicTitle);

#line default
#line hidden
                EndContext();
                BeginContext(1687, 6, true);
                WriteLiteral("</h3>\n");
                EndContext();
                BeginContext(1924, 35, true);
                WriteLiteral("                <a class=\"a3 leftT\"");
                EndContext();
                BeginWriteAttribute("href", " href=", 1959, "", 1983, 1);
#line 71 "/Users/Alexander/Documents/Coding Dojo/C#/Dyno/Views/Home/Profile.cshtml"
WriteAttributeValue("", 1965, i.Topic.TopicLink, 1965, 18, false);

#line default
#line hidden
                EndWriteAttribute();
                BeginContext(1983, 93, true);
                WriteLiteral(" target=\"_blank\">Read More...</a>\n            </div>\n            <div class=\"divider\"></div>\n");
                EndContext();
#line 74 "/Users/Alexander/Documents/Coding Dojo/C#/Dyno/Views/Home/Profile.cshtml"
        }

#line default
#line hidden
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(2093, 8, true);
            WriteLiteral("\n</html>");
            EndContext();
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
