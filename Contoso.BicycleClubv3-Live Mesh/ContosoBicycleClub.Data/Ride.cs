using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WLQuickApps.ContosoBicycleClub.Data
{
	public partial class Ride
	{
		public void CopyFrom(Ride from)
		{
			this.Title = from.Title;
			this.Description = from.Description;
			this.BlogPostId = from.BlogPostId;
			this.VECollectionId = from.VECollectionId;
			this.PhotoAlbumLink = from.PhotoAlbumLink;
			this.PhotoLink = from.PhotoLink;
			this.VideoLink = from.VideoLink;
			this.EventDate = from.EventDate;
			this.OwnerId = from.OwnerId;
			this.OwnerName = from.OwnerName;
			this.RecordType = from.RecordType;
		}
	}
}
