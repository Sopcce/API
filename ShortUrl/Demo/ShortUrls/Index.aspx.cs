
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Net;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.DynamicData.ModelProviders;

namespace ShortUrls
{
    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            String url = "http://blog.csdn.net/it_man/article/details/8973156";


            string uuuu = new ShortUrlHelper().GetApiShortUrl(url);
            uuuu = new ShortUrlHelper().GetApiShortUrl(url,ShortApiUrlType.t_cn);
            Response.Write(uuuu);



            //GroupCollection groups = Regex.Match(json, "\"tinyurl\":\"(?<tinyurl>[^\"]*)\"").Groups;
            //url = groups["tinyurl"].Value;
        }




    }
}