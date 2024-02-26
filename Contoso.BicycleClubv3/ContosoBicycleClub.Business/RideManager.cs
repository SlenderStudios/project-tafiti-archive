using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WLQuickApps.ContosoBicycleClub.Data;
using WLQuickApps.ContosoBicycleClub.Business;
using WLQuickApps.ContosoBicycleClub.Business.MetaWeblogApi;

namespace WLQuickApps.ContosoBicycleClub.Business
{
	public class RideManager : BaseDataManager
	{
		#region Ride
		public void SaveRide(Ride ride)
		{
			SaveInternal(ride, RecordType.Ride, PostCategory.Rides);
		}

		public Ride GetRide(Guid id)
		{
			using (ContosoBicycleClubDbDataContext context = DataContext)
			{
				var rides = (from ride in context.Rides
							 where ride.RideId == id
							 select ride);

				if (rides.Count<Ride>() > 0)
				{
					return rides.First();
				}
				else
				{
					return null;
				}
			}
		}

		public IList<Ride> GetAllRides()
		{
			using (ContosoBicycleClubDbDataContext context = DataContext)
			{
				return (from ride in context.Rides
						where ride.RecordType == RecordType.Ride
						orderby ride.EventDate descending
						select ride
				 ).ToList<Ride>();
			}
		}

		public IList<Ride> GetRidesByPage(int startRow, int pageSize)
		{
			using (ContosoBicycleClubDbDataContext context = DataContext)
			{
				return (from ride in context.Rides
						where ride.RecordType == RecordType.Ride
						orderby ride.EventDate descending
						select ride

				 ).Skip(startRow).Take(pageSize).ToList<Ride>();
			}
		}

		public IList<Ride> GetMyRides(string ownerId)
		{
			using (ContosoBicycleClubDbDataContext context = DataContext)
			{
				return (from ride in context.Rides
						where ride.RecordType == RecordType.Ride
						&& ride.OwnerId == ownerId
						orderby ride.EventDate descending
						select ride
				 ).ToList<Ride>();
			}

		}
		public IList<Ride> GetUpcomingRides()
		{
			using (ContosoBicycleClubDbDataContext context = DataContext)
			{
				return (from ride in context.Rides
						where ride.EventDate >= DateTime.Today && ride.RecordType == RecordType.Ride
						orderby ride.EventDate descending
						select ride
				 ).ToList<Ride>();
			}
		}

		public IList<Ride> GetUpcomingRidesByPage(int startRow, int pageSize)
		{
			using (ContosoBicycleClubDbDataContext context = DataContext)
			{
				return (from ride in context.Rides
						where ride.EventDate >= DateTime.Today && ride.RecordType == RecordType.Ride
						orderby ride.EventDate descending
						select ride

				 ).Skip(startRow).Take(pageSize).ToList<Ride>();
			}
		}

		public IList<Ride> GetPastRides()
		{
			using (ContosoBicycleClubDbDataContext context = DataContext)
			{
				return (from ride in context.Rides
						where ride.EventDate < DateTime.Today && ride.RecordType == RecordType.Ride
						orderby ride.EventDate descending
						select ride
				 ).ToList<Ride>();
			}
		}

		public IList<Ride> GetPastRidesByPage(int startRow, int pageSize)
		{
			using (ContosoBicycleClubDbDataContext context = DataContext)
			{
				return (from ride in context.Rides
						where ride.EventDate < DateTime.Today && ride.RecordType == RecordType.Ride
						orderby ride.EventDate descending
						select ride 

				 ).Skip(startRow).Take(pageSize).ToList<Ride>();
			}
		}
		#endregion

		#region Event

		private void SaveInternal(Ride ride, string recordType, string postCategory)
		{
			MsnSpacesMetaWeblogService blogSvc = new MsnSpacesMetaWeblogService();

			Post post = new Post();
			post.categories = new string[] { postCategory };
			post.title = ride.Title;
			post.description = ride.Description;
			post.dateCreated = DateTime.Now;

			using (ContosoBicycleClubDbDataContext context = DataContext)
			{
				var item = context.Rides.SingleOrDefault(c => c.RideId == ride.RideId);

				if (string.IsNullOrEmpty(ride.BlogPostId))
				{
					ride.BlogPostId = blogSvc.CreatePost(post);
				}
				else
				{
					blogSvc.UpdatePost(ride.BlogPostId, post);
				}

				//let MetaWeblogAPI filter the description HTML
				//XSS issue should be addressed by MetaWeblogAPI
				ride.Description = blogSvc.GetPost(ride.BlogPostId).description;

				if (item == null)
				{
					//Add                     
					ride.RideId = Guid.NewGuid();
					ride.RecordType = recordType;
					context.Rides.InsertOnSubmit(ride);
				}
				else
				{
					// item.RideId = ride.RideId;
					item.CopyFrom(ride);
					item.RecordType = recordType;
				}

				context.SubmitChanges();
			}
		}

		public void SaveEvent(Ride ride)
		{
			SaveInternal(ride, RecordType.Event, PostCategory.Events);
		}

		public Ride GetEvent(Guid id)
		{
			using (ContosoBicycleClubDbDataContext context = DataContext)
			{
				var rides = (from ride in context.Rides
							 where ride.RideId == id
							 select ride);

				if (rides.Count<Ride>() > 0)
				{
					return rides.First();
				}
				else
				{
					return null;
				}
			}
		}


		public IList<Ride> GetEventsByPage(int startRow, int pageSize)
		{
			using (ContosoBicycleClubDbDataContext context = DataContext)
			{
				return (from ride in context.Rides
						where ride.RecordType == RecordType.Event
						orderby ride.EventDate descending
						select ride

				 ).Skip(startRow).Take(pageSize).ToList<Ride>(); ;
			}
		}

		public IList<Ride> GetMyEvents(string ownerId)
		{
			using (ContosoBicycleClubDbDataContext context = DataContext)
			{
				return (from ride in context.Rides
						where ride.RecordType == RecordType.Event
						&& ride.OwnerId == ownerId
						orderby ride.EventDate 
						select ride
				 ).ToList<Ride>();
			}
		}

		public IList<Ride> GetAllEvents()
		{
			using (ContosoBicycleClubDbDataContext context = DataContext)
			{
				return (from ride in context.Rides
						where ride.RecordType == RecordType.Event
						orderby ride.EventDate descending
						select ride
				 ).ToList<Ride>();
			}
		}

		public IList<Ride> GetUpcomingEvents()
		{
			using (ContosoBicycleClubDbDataContext context = DataContext)
			{
				return (from ride in context.Rides
						where ride.EventDate >= DateTime.Today && ride.RecordType == RecordType.Event
						orderby ride.EventDate descending
						select ride
				 ).ToList<Ride>();
			}
		}

		public IList<Ride> GetUpcomingEventsByPage(int startRow, int pageSize)
		{
			using (ContosoBicycleClubDbDataContext context = DataContext)
			{
				return (from ride in context.Rides
						where ride.EventDate >= DateTime.Today && ride.RecordType == RecordType.Event
						orderby ride.EventDate descending
						select ride

				 ).Skip(startRow).Take(pageSize).ToList<Ride>(); ;
			}
		}

		public IList<Ride> GetPastEvents()
		{
			using (ContosoBicycleClubDbDataContext context = DataContext)
			{
				return (from ride in context.Rides
						where ride.EventDate < DateTime.Today && ride.RecordType == RecordType.Event
						orderby ride.EventDate descending
						select ride
				 ).ToList<Ride>();
			}
		}

		public IList<Ride> GetPastEventsByPage(int startRow, int pageSize)
		{
			using (ContosoBicycleClubDbDataContext context = DataContext)
			{
				return (from ride in context.Rides
						where ride.EventDate < DateTime.Today && ride.RecordType == RecordType.Event
						orderby ride.EventDate descending
						select ride

				 ).Skip(startRow).Take(pageSize).ToList<Ride>(); ;
			}
		}


		#endregion

	}
}
