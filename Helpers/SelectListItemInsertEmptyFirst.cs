using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.Helpers
{
    public static class SelectListItemInsertEmptyFirst
    {
        public static IEnumerable<SelectListItem> InsertEmptyFirst(this IEnumerable<SelectListItem> list, string emptyText = "", string emptyValue = "")
        {
            return new[] { new SelectListItem { Text = emptyText, Value = emptyValue } }.Concat(list);
        }
    }
}