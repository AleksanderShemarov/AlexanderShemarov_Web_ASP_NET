using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text.Encodings.Web;


namespace AlexanderShemarov.UI.TagHelpers
{
    public class Pager(LinkGenerator linkGenerator, IHttpContextAccessor httpContextAccessor) : TagHelper
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string? TrainType { get; set; }
        public bool Admin { get; set; } = false;
        int Prev
        {
            get => CurrentPage == 1 ? 1 : CurrentPage - 1;
        }
        int Next
        {
            get => CurrentPage == TotalPages ? TotalPages : CurrentPage + 1;
        }


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.AddClass("row", HtmlEncoder.Default);

            var nav = new TagBuilder("nav");
            nav.Attributes.Add("aria-label", "pagination");

            var ul = new TagBuilder("ul");
            ul.AddCssClass("pagination");

            #region Previous Page Button
            ul.InnerHtml.AppendHtml(
                CreateListItem(TrainType, Prev, "<span aria-hidden=\"true\">&laquo;</span>")
            );
            #endregion Previous Page Button

            #region Markup for switching between pages
            for (var index = 1; index <= TotalPages; index++)
            {
                ul.InnerHtml.AppendHtml(
                    CreateListItem(TrainType, index, String.Empty)
                );
            }
            #endregion Markup for switching between pages

            #region Previous Page Button
            ul.InnerHtml.AppendHtml(
                CreateListItem(TrainType, Next, "<span aria-hidden =\"true\">&raquo;</span>")
            );
            # endregion Previous Page Button

            nav.InnerHtml.AppendHtml(ul);
            output.Content.AppendHtml(ul);
        }

        /// <summary>
        /// A Page Button Markup of Pager
        /// </summary>
        /// <param name="trainType"></param>
        /// <param name="pageNo"></param>
        /// <param name="innerText"></param>
        /// <returns></returns>
        TagBuilder CreateListItem(string? trainType, int pageNo, string? innerText)
        {
            var li = new TagBuilder("li");
            li.AddCssClass("page-item");
            if (pageNo == CurrentPage && String.IsNullOrEmpty(innerText))
            {
                li.AddCssClass("active");
            }

            var a = new TagBuilder("a");
            a.AddCssClass("page-link");

            var routeData = new
            {
                pageno = pageNo,
                trainType
            };

            string url;
            // Razor for Admin's Page instead of MVC
            if (Admin)
            {
                url = linkGenerator.GetPathByPage(
                    httpContextAccessor.HttpContext,
                    page: "./Index",
                    values: routeData
                );
            }
            else
            {
                url = linkGenerator.GetPathByAction(
                    "index", "product", routeData
                );
            }

            a.Attributes.Add("href", url);

            var text = String.IsNullOrEmpty(innerText) ? pageNo.ToString() : innerText;
            a.InnerHtml.AppendHtml(text);
            li.InnerHtml.AppendHtml(a);

            return li;
        }
    }
}
