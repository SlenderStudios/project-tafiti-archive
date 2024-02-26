/*****************************************************************************
 * ContosoCup.aspx
 * Notes: Page used to demonstrate what the ContosoCup gadget would look like
 * **************************************************************************/

using System;
using System.IO;
using System.Text;
using System.Web.UI;

using WLQuickApps.ContosoBank.Entity;
using WLQuickApps.ContosoBank.Logic;

namespace WLQuickApps.ContosoBank
{
    public partial class ContosoCupPage : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Response.Write(renderLeaderBoard());
            }
        }

        private static string renderLeaderBoard()
        {
            ContosoCup leaderBoard = ContosoCupLogic.GetLeaderBoard();
            var sb = new StringBuilder();

            var writer = new HtmlTextWriter(new StringWriter(sb));
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "CupLeader");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            writer.RenderBeginTag(HtmlTextWriterTag.Table);

            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            writer.RenderBeginTag(HtmlTextWriterTag.Th);
            writer.RenderEndTag();

            writer.RenderBeginTag(HtmlTextWriterTag.Th);
            writer.Write("P");
            writer.RenderEndTag();

            writer.RenderBeginTag(HtmlTextWriterTag.Th);
            writer.Write("W");
            writer.RenderEndTag();

            writer.RenderBeginTag(HtmlTextWriterTag.Th);
            writer.Write("L");
            writer.RenderEndTag();

            writer.RenderBeginTag(HtmlTextWriterTag.Th);
            writer.Write("D");
            writer.RenderEndTag();
            writer.RenderEndTag();
            renderResults(leaderBoard, writer);
            writer.RenderEndTag();
            writer.RenderEndTag();

            writer.AddAttribute(HtmlTextWriterAttribute.Class, "CupPlayer");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            writer.Write(String.Format("Player of the week - {0}", leaderBoard.PlayerOfWeek));
            writer.RenderEndTag();

            return sb.ToString();
        }

        private static void renderResults(ContosoCup leaderBoard, HtmlTextWriter writer)
        {
            foreach (TeamResult result in leaderBoard.Ladder)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                writer.RenderBeginTag(HtmlTextWriterTag.Td);
                writer.Write(result.Name);
                writer.RenderEndTag();

                writer.AddAttribute(HtmlTextWriterAttribute.Class, "Score");
                writer.RenderBeginTag(HtmlTextWriterTag.Td);
                writer.Write(result.Played);
                writer.RenderEndTag();

                writer.AddAttribute(HtmlTextWriterAttribute.Class, "Score");
                writer.RenderBeginTag(HtmlTextWriterTag.Td);
                writer.Write(result.Won);
                writer.RenderEndTag();

                writer.AddAttribute(HtmlTextWriterAttribute.Class, "Score");
                writer.RenderBeginTag(HtmlTextWriterTag.Td);
                writer.Write(result.Lost);
                writer.RenderEndTag();

                writer.AddAttribute(HtmlTextWriterAttribute.Class, "Score");
                writer.RenderBeginTag(HtmlTextWriterTag.Td);
                writer.Write(result.Draw);
                writer.RenderEndTag();
                writer.RenderEndTag();
            }
        }
    }
}