﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.1433
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WLQuickApps.ContosoBicycleClub.Data
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[System.Data.Linq.Mapping.DatabaseAttribute(Name="contosobicycleclubdb")]
	public partial class ContosoBicycleClubDbDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertRide(Ride instance);
    partial void UpdateRide(Ride instance);
    partial void DeleteRide(Ride instance);
    #endregion
		
		public ContosoBicycleClubDbDataContext() : 
				base(global::WLQuickApps.ContosoBicycleClub.Data.Properties.Settings.Default.contosobicycleclubdbConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public ContosoBicycleClubDbDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public ContosoBicycleClubDbDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public ContosoBicycleClubDbDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public ContosoBicycleClubDbDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<Ride> Rides
		{
			get
			{
				return this.GetTable<Ride>();
			}
		}
	}
	
	[Table(Name="dbo.Ride")]
	public partial class Ride : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private System.Guid _RideId;
		
		private string _Title;
		
		private string _BlogPostId;
		
		private string _VECollectionId;
		
		private string _PhotoAlbumLink;
		
		private string _PhotoLink;
		
		private string _VideoLink;
		
		private System.Nullable<System.DateTime> _EventDate;
		
		private string _RecordType;
		
		private string _Description;
		
		private string _OwnerId;
		
		private string _OwnerName;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnRideIdChanging(System.Guid value);
    partial void OnRideIdChanged();
    partial void OnTitleChanging(string value);
    partial void OnTitleChanged();
    partial void OnBlogPostIdChanging(string value);
    partial void OnBlogPostIdChanged();
    partial void OnVECollectionIdChanging(string value);
    partial void OnVECollectionIdChanged();
    partial void OnPhotoAlbumLinkChanging(string value);
    partial void OnPhotoAlbumLinkChanged();
    partial void OnPhotoLinkChanging(string value);
    partial void OnPhotoLinkChanged();
    partial void OnVideoLinkChanging(string value);
    partial void OnVideoLinkChanged();
    partial void OnEventDateChanging(System.Nullable<System.DateTime> value);
    partial void OnEventDateChanged();
    partial void OnRecordTypeChanging(string value);
    partial void OnRecordTypeChanged();
    partial void OnDescriptionChanging(string value);
    partial void OnDescriptionChanged();
    partial void OnOwnerIdChanging(string value);
    partial void OnOwnerIdChanged();
    partial void OnOwnerNameChanging(string value);
    partial void OnOwnerNameChanged();
    #endregion
		
		public Ride()
		{
			OnCreated();
		}
		
		[Column(Storage="_RideId", DbType="UniqueIdentifier NOT NULL", IsPrimaryKey=true)]
		public System.Guid RideId
		{
			get
			{
				return this._RideId;
			}
			set
			{
				if ((this._RideId != value))
				{
					this.OnRideIdChanging(value);
					this.SendPropertyChanging();
					this._RideId = value;
					this.SendPropertyChanged("RideId");
					this.OnRideIdChanged();
				}
			}
		}
		
		[Column(Storage="_Title", DbType="VarChar(255)")]
		public string Title
		{
			get
			{
				return this._Title;
			}
			set
			{
				if ((this._Title != value))
				{
					this.OnTitleChanging(value);
					this.SendPropertyChanging();
					this._Title = value;
					this.SendPropertyChanged("Title");
					this.OnTitleChanged();
				}
			}
		}
		
		[Column(Storage="_BlogPostId", DbType="VarChar(255)")]
		public string BlogPostId
		{
			get
			{
				return this._BlogPostId;
			}
			set
			{
				if ((this._BlogPostId != value))
				{
					this.OnBlogPostIdChanging(value);
					this.SendPropertyChanging();
					this._BlogPostId = value;
					this.SendPropertyChanged("BlogPostId");
					this.OnBlogPostIdChanged();
				}
			}
		}
		
		[Column(Storage="_VECollectionId", DbType="VarChar(50)")]
		public string VECollectionId
		{
			get
			{
				return this._VECollectionId;
			}
			set
			{
				if ((this._VECollectionId != value))
				{
					this.OnVECollectionIdChanging(value);
					this.SendPropertyChanging();
					this._VECollectionId = value;
					this.SendPropertyChanged("VECollectionId");
					this.OnVECollectionIdChanged();
				}
			}
		}
		
		[Column(Storage="_PhotoAlbumLink", DbType="VarChar(2000)")]
		public string PhotoAlbumLink
		{
			get
			{
				return this._PhotoAlbumLink;
			}
			set
			{
				if ((this._PhotoAlbumLink != value))
				{
					this.OnPhotoAlbumLinkChanging(value);
					this.SendPropertyChanging();
					this._PhotoAlbumLink = value;
					this.SendPropertyChanged("PhotoAlbumLink");
					this.OnPhotoAlbumLinkChanged();
				}
			}
		}
		
		[Column(Storage="_PhotoLink", DbType="VarChar(2000)")]
		public string PhotoLink
		{
			get
			{
				return this._PhotoLink;
			}
			set
			{
				if ((this._PhotoLink != value))
				{
					this.OnPhotoLinkChanging(value);
					this.SendPropertyChanging();
					this._PhotoLink = value;
					this.SendPropertyChanged("PhotoLink");
					this.OnPhotoLinkChanged();
				}
			}
		}
		
		[Column(Storage="_VideoLink", DbType="VarChar(2000)")]
		public string VideoLink
		{
			get
			{
				return this._VideoLink;
			}
			set
			{
				if ((this._VideoLink != value))
				{
					this.OnVideoLinkChanging(value);
					this.SendPropertyChanging();
					this._VideoLink = value;
					this.SendPropertyChanged("VideoLink");
					this.OnVideoLinkChanged();
				}
			}
		}
		
		[Column(Storage="_EventDate", DbType="DateTime")]
		public System.Nullable<System.DateTime> EventDate
		{
			get
			{
				return this._EventDate;
			}
			set
			{
				if ((this._EventDate != value))
				{
					this.OnEventDateChanging(value);
					this.SendPropertyChanging();
					this._EventDate = value;
					this.SendPropertyChanged("EventDate");
					this.OnEventDateChanged();
				}
			}
		}
		
		[Column(Storage="_RecordType", DbType="VarChar(50)")]
		public string RecordType
		{
			get
			{
				return this._RecordType;
			}
			set
			{
				if ((this._RecordType != value))
				{
					this.OnRecordTypeChanging(value);
					this.SendPropertyChanging();
					this._RecordType = value;
					this.SendPropertyChanged("RecordType");
					this.OnRecordTypeChanged();
				}
			}
		}
		
		[Column(Storage="_Description", DbType="NText", UpdateCheck=UpdateCheck.Never)]
		public string Description
		{
			get
			{
				return this._Description;
			}
			set
			{
				if ((this._Description != value))
				{
					this.OnDescriptionChanging(value);
					this.SendPropertyChanging();
					this._Description = value;
					this.SendPropertyChanged("Description");
					this.OnDescriptionChanged();
				}
			}
		}
		
		[Column(Storage="_OwnerId", DbType="VarChar(50)")]
		public string OwnerId
		{
			get
			{
				return this._OwnerId;
			}
			set
			{
				if ((this._OwnerId != value))
				{
					this.OnOwnerIdChanging(value);
					this.SendPropertyChanging();
					this._OwnerId = value;
					this.SendPropertyChanged("OwnerId");
					this.OnOwnerIdChanged();
				}
			}
		}
		
		[Column(Storage="_OwnerName", DbType="VarChar(255)")]
		public string OwnerName
		{
			get
			{
				return this._OwnerName;
			}
			set
			{
				if ((this._OwnerName != value))
				{
					this.OnOwnerNameChanging(value);
					this.SendPropertyChanging();
					this._OwnerName = value;
					this.SendPropertyChanged("OwnerName");
					this.OnOwnerNameChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
#pragma warning restore 1591