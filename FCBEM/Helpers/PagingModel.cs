using Microsoft.AspNetCore.Mvc.Rendering;

namespace FCBEM24.Helpers
{
    public class PagingModel
    {
        public int PageNumber { get; set; }
        public int CountPages { get; set; }
        public Func<int?, string>? GenerateUrl { get; set; }

        public SelectList PageSelectList
        {
            get
            {
                List<PageItem> lst = [];
                for (int i = 1; i <= CountPages; ++i)
                {
                    lst.Add(new PageItem { Value = GenerateUrl(i), Text = i.ToString() });
                }
                return new SelectList(lst, "Value", "Text", GenerateUrl(PageNumber));
            }
        }

        public class PageItem
        {
            public string Value { get; set; }
            public string Text { get; set; }
        }
    }
}
