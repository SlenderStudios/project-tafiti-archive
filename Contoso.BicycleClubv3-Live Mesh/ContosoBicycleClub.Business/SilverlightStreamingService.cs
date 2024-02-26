using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Net;
using System.Text;
using System.Configuration;
using System.Resources;

namespace WLQuickApps.ContosoBicycleClub.Business
{
	public class SSResult
	{
		/// <summary>
		/// Gets or sets the video URL.
		/// </summary>
		/// <value>The video URL.</value>
		public string VideoUrl { get; set; }

		/// <summary>
		/// Gets or sets the preview URL.
		/// </summary>
		/// <value>The preview URL.</value>
		public string PreviewUrl { get; set; }

	}

	public class SilverlightStreamingService
	{
		private static string ACCOUNTID
		{ 
			get { return ConfigurationManager.AppSettings["sls_id"].Trim(); } 
		}

        private static string KEY
		{ 
			get { return ConfigurationManager.AppSettings["sls_secret"].Trim(); } 
		}


		const long BUFFER_SIZE = 8192;

		public SSResult Upload(string mediaFilePath, string tempfolder)
		{

			FileInfo meidaFileInfo = new FileInfo(mediaFilePath);

			string videoFileName = meidaFileInfo.Name;

			string zipFilename = CreateSilverlightPackage(tempfolder, mediaFilePath);

			//string filesetName = Guid.NewGuid().ToString().Replace("-", string.Empty);
			string filesetName = videoFileName.Replace(meidaFileInfo.Extension, "");

			Uri silverlightStreamingUri = new Uri(string.Format("http://silverlight.services.live.com/{0}/{1}", ACCOUNTID, filesetName));

			// Upload to Silverlight Streaming
			System.Net.HttpWebRequest request = System.Net.WebRequest.Create(silverlightStreamingUri) as HttpWebRequest;
			request.Timeout = 20 * 60 * 1000;
			request.ReadWriteTimeout = 20 * 60 * 1000;

			//Note: Can't set authorization header with NetworkCredential or larger files time out
			//request.Credentials = new NetworkCredential(ACCOUNTID, KEY);
			byte[] userPass = System.Text.Encoding.Default.GetBytes(ACCOUNTID + ":" + KEY);
			string basic = "Basic " + Convert.ToBase64String(userPass);
			request.Headers["Authorization"] = basic;

			request.PreAuthenticate = true;
			request.ContentType = System.Net.Mime.MediaTypeNames.Application.Zip;
			request.Method = WebRequestMethods.Http.Post;



			using (System.IO.FileStream inputStream = new System.IO.FileStream(zipFilename, FileMode.Open))
			{
				using (System.IO.Stream requestStream = request.GetRequestStream())
				{
					CopyStream(inputStream, requestStream);
					requestStream.Close();
				}
			}			

#if !DEBUG
			//Delete temp files in release mode.
			{
				if (System.IO.File.Exists(mediaFilePath))
					System.IO.File.Delete(mediaFilePath);
				if (System.IO.File.Exists(zipFilename))
					System.IO.File.Delete(zipFilename);
			}
#endif

			try
			{
				System.Net.HttpWebResponse response = request.GetResponse() as HttpWebResponse;

				if (response.StatusCode == HttpStatusCode.OK)
				{
					return new SSResult()
					{
						VideoUrl = string.Format("streaming:/{0}/{1}/{2}", ACCOUNTID, filesetName, videoFileName),
						PreviewUrl = string.Format("streaming:/{0}/{1}/{2}", ACCOUNTID, filesetName, videoFileName)
					};
				}
			}
			catch //(Exception ex)
			{
				throw;
			}

			return null;
		}

		/// <summary>
		/// Copies the stream.
		/// </summary>
		/// <param name="inputStream">The input stream.</param>
		/// <param name="requestStream">The request stream.</param>
		private static void CopyStream(System.IO.FileStream inputStream, System.IO.Stream requestStream)
		{
			long bufferSize = inputStream.Length < BUFFER_SIZE ? inputStream.Length : BUFFER_SIZE;
			byte[] buffer = new byte[bufferSize];
			int bytesRead = 0;
			long bytesWritten = 0;
			while ((bytesRead = inputStream.Read(buffer, 0, buffer.Length)) != 0)
			{
				requestStream.Write(buffer, 0, bytesRead);
				bytesWritten += bufferSize;
			}
		}

		/// <summary>
		/// Creates a package for Silverlight Streaming.
		/// </summary>
		/// <param name="videoFileName">Name of the video file.</param>
		/// <param name="workingDirectory">The working directory.</param>
		/// <returns>A string representing the .zip path.</returns>
		private static string CreateSilverlightPackage(string tempfolder, string videoFileName)
		{
			//string zipFilename = Path.GetTempFileName();
			string zipFilename = Path.Combine(tempfolder, Path.GetRandomFileName());

			//deletes tmp file
			if (File.Exists(zipFilename))
				File.Delete(zipFilename);
			//rename it to zip
			zipFilename = Path.ChangeExtension(zipFilename, "zip");
			if (File.Exists(zipFilename))
				File.Delete(zipFilename);
			using (Package zip = System.IO.Packaging.Package.Open(zipFilename, FileMode.Create))
			{
				AddToZip(zip, videoFileName);
				//Adds silverlight manifest file
				AddToZip(zip, GetManifest());
			}

			return zipFilename;
		}

		/// <summary>
		/// Gets the manifest.
		/// </summary>
		/// <returns>Returns a string representing the manifest.</returns>
		private static string GetManifest()
		{
			string manifestPath = Path.Combine(Path.GetTempPath(), "manifest.xml");
			if (!File.Exists(manifestPath))
			{
				StreamWriter writer = File.CreateText(manifestPath);
				writer.Write("<SilverlightApp><source>dummy.xaml</source></SilverlightApp>");
				writer.Close();
			}

			return manifestPath;
		}

		/// <summary>
		/// Adds a file to a zip archive using System.IO.Packaging.
		/// </summary>
		/// <param name="zip">The zip archive.</param>
		/// <param name="localFileName">Name of the local file.</param>
		private static void AddToZip(Package zip, string localFileName)
		{
			//Gets only filename, its will be the name in zip file
			string destFilename = ".\\" + Path.GetFileName(localFileName);
			Uri uri = PackUriHelper.CreatePartUri(new Uri(destFilename, UriKind.Relative));
			PackagePart part = zip.CreatePart(uri, "");
			// Copy the data to the Document Part
			using (FileStream fileStream = new FileStream(localFileName, FileMode.Open, FileAccess.Read))
			{
				using (Stream dest = part.GetStream())
				{
					CopyStream(fileStream, dest);
				}
			}
		}

	}
}
