﻿using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.IO;
using WLQuickApps.ContosoBicycleClub.UI;
using WLQuickApps.ContosoBicycleClub.Data;
using WLQuickApps.ContosoBicycleClub.Business;
using WLQuickApps.ContosoBicycleClub.Business.MetaWeblogApi;

namespace WLQuickApps.ContosoBicycleClub
{
	/// <summary>
	/// ValidateRequest is turned off on this page since we want to accept html input.
	/// </summary>
	public partial class Edit : System.Web.UI.Page
	{
		string eventType;
		public string EventLabel { get; set; }

		protected void Page_Load(object sender, EventArgs e)
		{
			SaveButton.Attributes.Add("onclick", string.Format("showProgress('{0}');", StatusMessage.ClientID));

			this.eventType = Page.Request.QueryString["type"];

			if (string.IsNullOrEmpty(this.eventType)) this.eventType = "";

			switch (this.eventType.ToLower()) {
				case "ride":
					EventLabel = Resources.ContosoBicycleClubWeb.RideCapitalizedLabel;
					break;
				default:
					EventLabel = Resources.ContosoBicycleClubWeb.EventCapitalizedLabel;
					break;
			}

			if (!Page.IsPostBack)
			{
				LoadRideDropdown(string.Empty);
			}
		}

		protected void LoadRideDropdown(string rideId)
		{
			RideDropDownList.DataTextField = "Title";
			RideDropDownList.DataValueField = "RideId";
			RideDropDownList.DataSource = GetList();
			RideDropDownList.DataBind();
			RideDropDownList.Items.Insert(0, new ListItem("Create new", ""));

			if (!string.IsNullOrEmpty(rideId))
				RideDropDownList.SelectedValue = rideId;
		}

		protected void RideDropDownList_SelectedIndexChanged(object sender, EventArgs e)
		{
			StatusMessage.InnerText = string.Empty;
			StatusMessage.Visible = false;

			if (!string.IsNullOrEmpty(RideDropDownList.SelectedValue))
			{
				Ride ride = LoadRecord(new Guid(RideDropDownList.SelectedValue));
				Bind(ride);
			}
			else
			{
				Bind(null);
			}
		}

		protected void SaveButton_Click(object sender, EventArgs e)
		{
			try
			{
				Ride ride;

				if (!string.IsNullOrEmpty(RideIdTextBox.Text))
				{
					Guid id = new Guid(RideIdTextBox.Text);
					ride = LoadRecord(id);
				}
				else
				{
					ride = new Ride();
				}

				Unbind(ride);

				if (FileUploadTextbox.HasFile)
				{
					FileInfo meidaFileInfo = new FileInfo(FileUploadTextbox.PostedFile.FileName);

					string filename; 
					//170714 - Unique filename is generated by creating a new Guid.
					filename = Guid.NewGuid().ToString().Replace("-", "") + meidaFileInfo.Extension;

					string tempfolder = Path.Combine(Server.MapPath("."), @"_temp_upload");
					string localFilename = Path.Combine(tempfolder, filename);

					FileUploadTextbox.PostedFile.SaveAs(localFilename);

					meidaFileInfo = new FileInfo(localFilename);
					
					string filesetName = meidaFileInfo.Name.Replace(meidaFileInfo.Extension, "");
					string accountID = ConfigurationManager.AppSettings["sls_id"];

					ride.VideoLink = string.Format("/{0}/{1}/{2}", accountID, filesetName, meidaFileInfo.Name); ;

					SilverlightStreamingService uploadService = new SilverlightStreamingService();

					SSResult result = uploadService.Upload(localFilename, tempfolder);
				}

				SaveRecord(ride);

				Bind(ride);

				switch (this.eventType.ToLower())
				{
					case "ride":
						StatusMessage.InnerText = Resources.ContosoBicycleClubWeb.RideUpdateSuccessLabel;
						break;
					default:
						StatusMessage.InnerText = Resources.ContosoBicycleClubWeb.EventUpdateSuccessLabel;
						break;
				}
				StatusMessage.Attributes["class"] = "success";
				StatusMessage.Visible = true;

				LoadRideDropdown(ride.RideId.ToString());

			}
			catch (Exception ex)
			{
				#if DEBUG
				StatusMessage.InnerText = ex.Message;
				#else
				StatusMessage.InnerText = "Failed to save this ride. Please try again later.";
				#endif
				StatusMessage.Attributes["class"] = "error";
				StatusMessage.Visible = true;
			}
		}

		protected void CancelButton_Click(object sender, EventArgs e)
		{
			Response.Redirect("~/default.aspx");
		}

		#region Private Helper Methods
		private void Bind(Ride ride)
		{
			if (ride != null)
			{
				//170714
				RideIdTextBox.Text = ride.RideId.ToString();
				RideNameTextBox.Text = Server.HtmlEncode(ride.Title);
				RideDateTextBox.Text = Server.HtmlEncode(ride.EventDate.ToString());
				
				//In a real application, we would use a HTML Editor to allow user to edit the description
				//and the Editor should be XSS safe. Here we will encode everything and unencode the allowedTags.
				RideDescTextBox.Text = AntiXssHelper.HtmlEncode(ride.Description, AntiXssHelper.AllowedTags); 

				VECollectionIdTextBox.Text = Server.HtmlEncode(ride.VECollectionId);
				PhotoAlbumLinkTextBox.Text = Server.HtmlEncode(ride.PhotoAlbumLink);
				PhotoLinkTextBox.Text = Server.HtmlEncode(ride.PhotoLink);
				VideoLinkTextBox.Text = Server.HtmlEncode(ride.VideoLink);
			}
			else
			{
				RideIdTextBox.Text = string.Empty;
				RideNameTextBox.Text = string.Empty;
				RideDateTextBox.Text = string.Empty;
				RideDescTextBox.Text = string.Empty;
				VECollectionIdTextBox.Text = string.Empty;
				PhotoAlbumLinkTextBox.Text = string.Empty;
				PhotoLinkTextBox.Text = string.Empty;
				VideoLinkTextBox.Text = string.Empty;
			}
		}

		private void Unbind(Ride ride)
		{
			ride.Title = RideNameTextBox.Text;
			ride.EventDate = DateTime.Parse(RideDateTextBox.Text);

			//HtmlEcode at the input time.
			//We don't want to html encode when display.
			ride.Description = AntiXssHelper.HtmlEncode(RideDescTextBox.Text, AntiXssHelper.AllowedTags);

			ride.VECollectionId = VECollectionIdTextBox.Text;
			ride.PhotoAlbumLink = PhotoAlbumLinkTextBox.Text;
			ride.PhotoLink = PhotoLinkTextBox.Text;
			ride.VideoLink = VideoLinkTextBox.Text;
		}

		private IEnumerable GetList()
		{
			RideManager mgr = new RideManager();
			if (eventType.Equals(RecordType.Event))
			{
				return mgr.GetMyEvents(Page.User.Identity.Name);
			}
			else
			{
				return mgr.GetMyRides(Page.User.Identity.Name);
			}
		}

		private Ride LoadRecord(Guid id)
		{
			RideManager mgr = new RideManager();
			Ride item;

			if (eventType.Equals(RecordType.Event))
			{
				item = mgr.GetEvent(id);
			}
			else
			{
				item = mgr.GetRide(id);
			}

			//get the description from spaces
			MsnSpacesMetaWeblogService blogSvc = new MsnSpacesMetaWeblogService();
			Post post = blogSvc.GetPost(item.BlogPostId);
			item.Description = post.description;

			return item;
		}

		private void SaveRecord(Ride ride)
		{
			string appPath = Request.ApplicationPath;

			ride.OwnerName = WebProfile.Current.DisplayName;
			ride.OwnerId = Page.User.Identity.Name;
			if (string.IsNullOrEmpty(ride.PhotoLink)) ride.PhotoLink = (appPath == "/" ? "" : appPath) + "/assets/images/default_thumbnail.jpg";

			RideManager mgr = new RideManager();
			if (eventType.Equals(RecordType.Event))
			{
				mgr.SaveEvent(ride);
			}
			else
			{
				mgr.SaveRide(ride);
			}
		}
		#endregion

	}
}