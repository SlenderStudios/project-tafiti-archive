using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using WLQuickApps.ContosoBicycleClub.Data;

namespace WLQuickApps.ContosoBicycleClub.Business
{
    public class ImageManager : BaseDataManager
    {
        public Array GetRideImages(Guid guidRideorEvent)
        {
            using (ContosoBicycleClubDbDataContext context = DataContext)
            {
                try
                {
                    ArrayList images = new ArrayList();

                    context.Connection.Open();
                    SqlCommand cmd = new SqlCommand("Select ImageID from vwImageRide where RideID = @RideID", (SqlConnection)context.Connection);
                    cmd.Parameters.Add("@RideID", SqlDbType.UniqueIdentifier).Value = guidRideorEvent;
                    cmd.Prepare();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        images.Add(dr["ImageID"]);
                    }
                    dr.Close();
                    context.Connection.Close();

                    return images.ToArray();
                }
                catch
                {
                    return null;
                }
                finally
                {
                    context.Connection.Close();
                }
            }
        }

        public void GetImage(Guid guidImage, ref string type, ref byte[] imageBytes)
        { 
            using (ContosoBicycleClubDbDataContext context = DataContext)
			{
                try
                {
                    context.Connection.Open();
                    SqlCommand cmd = new SqlCommand("Select Type, ImageBytes from Image where ImageID = @ImageID", (SqlConnection)context.Connection);
                    cmd.Parameters.Add("@ImageID", SqlDbType.UniqueIdentifier).Value = guidImage; 
                    cmd.Prepare(); 
                    SqlDataReader dr = cmd.ExecuteReader(); 
                    dr.Read();
                    type = dr["Type"].ToString(); 
                    imageBytes = (byte[])dr["ImageBytes"]; 
                    dr.Close();
                    context.Connection.Close(); 
                }
                catch
                {
                }
                finally
                {
                    context.Connection.Close();
                }
            }
        }
        
        public void SaveImage(Guid guidImage, Guid guidRideorEvent, byte[] theImage)
        {
            using (ContosoBicycleClubDbDataContext context = DataContext)
			{
                try
                {
                    // Check if the record exists in the ImageRide table.  If not, add the record later.
                    // If it does, just update the image.
                    SqlCommand cmd = new SqlCommand("Select * from vwImageRide where ImageID = @ImageID and RideID = @RideID", (SqlConnection)context.Connection);
                    cmd.Parameters.Add("@ImageID", SqlDbType.UniqueIdentifier).Value = guidImage;
                    cmd.Parameters.Add("@RideID", SqlDbType.UniqueIdentifier).Value = guidRideorEvent;
                    context.Connection.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    bool hasImageRideRows = rdr.HasRows;
                    rdr.Close();

                    // If the image already exists then do and update.
                    SqlCommand cmd1 = new SqlCommand("Select * from Image where ImageID = @ImageID", (SqlConnection)context.Connection);
                    cmd1.Parameters.Add("@ImageID", SqlDbType.UniqueIdentifier).Value = guidImage;
                    SqlDataReader rdr1 = cmd1.ExecuteReader();
                    bool hasImageRows = rdr1.HasRows;
                    rdr1.Close();

                    SqlCommand cmd2;
                    if (hasImageRows)
                    {
                        cmd2 = new SqlCommand("Update Image Set ImageBytes = @ImageBytes, Size = @Size where ImageID = @ImageID", (SqlConnection)context.Connection);
                        cmd2.Parameters.Add("@ImageBytes", SqlDbType.Image, theImage.Length).Value = theImage;
                        cmd2.Parameters.Add("@Size", SqlDbType.BigInt).Value = theImage.Length;
                        cmd2.Parameters.Add("@ImageID", SqlDbType.UniqueIdentifier).Value = guidImage;
                    }
                    else
                    {
                        cmd2 = new SqlCommand("Insert Into Image (ImageID, ImageBytes, Type, Size) " +
                            " values (@ImageID, @ImageBytes, @Type, @Size)", (SqlConnection)context.Connection);
                        cmd2.Parameters.Add("@ImageID", SqlDbType.UniqueIdentifier).Value = guidImage;
                        cmd2.Parameters.Add("@ImageBytes", SqlDbType.Image, theImage.Length).Value = theImage;
                        cmd2.Parameters.Add("@Type", SqlDbType.VarChar, 4).Value = "jpeg";
                        cmd2.Parameters.Add("@Size", SqlDbType.BigInt).Value = theImage.Length;
                    }
                    cmd2.ExecuteNonQuery();

                    if (!hasImageRideRows)
                    {
                        SqlCommand cmd3 = new SqlCommand("Insert Into ImageRide (ImageID, RideID) " +
                            " values (@ImageID, @RideID)", (SqlConnection)context.Connection);
                        cmd3.Parameters.Add("@ImageID", SqlDbType.UniqueIdentifier).Value = guidImage;
                        cmd3.Parameters.Add("@RideID", SqlDbType.UniqueIdentifier).Value = guidRideorEvent;
                        cmd3.ExecuteNonQuery();
                    }
                }
                catch (Exception err)
                {
                }
                finally
                {
                    context.Connection.Close();
                }
            }
        }
    }
}