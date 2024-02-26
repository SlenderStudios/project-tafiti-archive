using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;

[assembly: System.Web.UI.WebResource("Controls.Calendar.Calendar.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("Controls.Calendar.Calendar.css", "text/css", PerformSubstitution = true)]
[assembly: System.Web.UI.WebResource("Controls.Calendar.arrow-left.gif", "image/gif")]
[assembly: System.Web.UI.WebResource("Controls.Calendar.arrow-right.gif", "image/gif")]

namespace Controls
{
    [Designer(typeof(CalendarDesigner))]
    [RequiredScript(typeof(CommonToolkitScripts))]
    [RequiredScript(typeof(DateTimeScripts))]
    [RequiredScript(typeof(ThreadingScripts))]
    [RequiredScript(typeof(AnimationScripts))]
    [ClientCssResource("Controls.Calendar.Calendar.css")]
    [ClientScriptResource("Controls.Calendar", "Controls.Calendar.Calendar.js")]
    [ToolboxItem("System.Web.UI.Design.WebControlToolboxItem, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
    [ToolboxBitmap(typeof(Calendar), "Calendar.Calendar.ico")]
    public class Calendar : ScriptControlBase
    {
        public Calendar() :
            base(false, HtmlTextWriterTag.Div)
        {
        }

        [DefaultValue("")]
        [ExtenderControlProperty]
        [ClientPropertyName("cssClass")]
        public override string CssClass
        {
            get { return base.CssClass; }
            set { base.CssClass = value; }
        }

        [DefaultValue("d")]
        [ExtenderControlProperty]
        [ClientPropertyName("format")]
        public virtual string Format
        {
            get { return (string)(ViewState["Format"] ?? "d"); }
            set { ViewState["Format"] = value; }
        }

        [DefaultValue(true)]
        [ExtenderControlProperty]
        [ClientPropertyName("animated")]
        public virtual bool Animated
        {
            get { return (bool)(ViewState["Animated"] ?? true); }
            set { ViewState["Animated"] = value; }
        }

        [DefaultValue(FirstDayOfWeek.Default)]
        [ExtenderControlProperty]
        [ClientPropertyName("firstDayOfWeek")]
        public virtual FirstDayOfWeek FirstDayOfWeek
        {
            get { return (FirstDayOfWeek)(ViewState["FirstDayOfWeek"] ?? FirstDayOfWeek.Default); }
            set { ViewState["FirstDayOfWeek"] = value; }
        }

        [DefaultValue("")]
        [ExtenderControlProperty]
        [ClientPropertyName("selectedDate")]
        public DateTime? SelectedDate
        {
            get { return (DateTime)(ViewState["SelectedDate"] ?? DateTime.Today); }
            set { ViewState["SelectedDate"] = value; }
        }

        [DefaultValue("")]
        [ExtenderControlEvent]
        [ClientPropertyName("dateSelectionChanged")]
        public virtual string OnClientDateSelectionChanged
        {
            get { return (string)(ViewState["OnClientDateSelectionChanged"] ?? string.Empty); }
            set { ViewState["OnClientDateSelectionChanged"] = value; }
        }

        [DefaultValue("")]
        [ExtenderControlEvent]
        [ClientPropertyName("visibleDateChanged")]
        public virtual string OnClientVisibleDateChanged
        {
            get { return (string)(ViewState["OnClientVisibleDateChanged"] ?? string.Empty); }
            set { ViewState["OnClientVisibleDateChanged"] = value; }
        }

        /*
         * These overrides are to prevent Master Pages
         * from renaming the control id
         */
        
        /// <summary>
        /// Override to force simple IDs all around
        /// </summary>
        public override string UniqueID
        {
            get
            {
                return this.ID;
            }
        }

        /// <summary>
        /// Override to force simple IDs all around
        /// </summary>
        public override string ClientID
        {
            get
            {
                return this.ID;
            }

        }
    }
}
