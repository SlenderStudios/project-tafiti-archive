using System;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.UI.WebControls;
using Microsoft.LiveFX.Client;
using Microsoft.LiveFX.ResourceModel;
using WLQuickApps.ContosoBicycleClub.Business;
using WLQuickApps.ContosoBicycleClub.UI;

namespace WLQuickApps.ContosoBicycleClub
{
    public partial class UploadPhotos : System.Web.UI.Page
    {
        private Guid RideID = Guid.Empty;
        public LiveOperatingEnvironment loe = new LiveOperatingEnvironment();

        protected void Page_Load(object sender, EventArgs e)
        {
            HttpRequest req = HttpContext.Current.Request;
            if (WebProfile.Current.PicturesDelToken != null) 
            {
                if (req.QueryString["RideID"] != null) RideID = new Guid(req.QueryString["RideID"]);

                LiveItemAccessOptions liao = new LiveItemAccessOptions(true);
                loe.Connect(WebProfile.Current.PicturesDelToken, AuthenticationTokenType.DelegatedAuthToken, new Uri(Constants.ServiceEndPoint), liao);

                if (!IsPostBack) LoadTree();
            }
        }

        protected void UploadButton_Click(object sender, EventArgs e)
        {
            if (MeshTree.SelectedNode == null) return;

            MeshObject mo = null;
            DataFeed df = null;
            DataEntry de = null;
            RetrievePathObjects(MeshTree.SelectedNode.Value, ref mo, ref df, ref de);
            if (de == null) return;

            // Load the image from mesh into memory.
            byte[] theImage = new byte[de.Resource.MediaResource.Length];
            MemoryStream output = new MemoryStream(theImage);
            de.ReadMediaResource(output);
            output.Flush();
            output.Close();

            // Save the image to the database.
            ImageManager mgr = new ImageManager();
            // Strip the guid from the ID.
            Guid guidImage = new Guid(de.Resource.Id.Substring(9));
            mgr.SaveImage(guidImage, RideID, theImage);
        }

        protected void DoneButton_Click(object sender, EventArgs e)
        {
            HttpRequest req = HttpContext.Current.Request;
            if (req.Cookies[Constants.ReturnCookie] != null)
            {
                Response.Redirect(req.Cookies[Constants.ReturnCookie].Value);
            }
        }

        private void RetrievePathObjects(string path, ref MeshObject mo, ref DataFeed df, ref DataEntry de)
        {
            string moPath = "";
            string dfPath = "";
            string dePath = "";
            SplitSelfLink(MeshTree.SelectedNode.Value, ref moPath, ref dfPath, ref dePath);

            foreach (MeshObject moItem in loe.Mesh.MeshObjects.Entries)
            {
                if (moItem.Resource.SelfLink.ToString() == moPath)
                {
                    mo = moItem;
                    break;
                }
            }
            if (mo == null) return;
            foreach (DataFeed dfItem in mo.DataFeeds.Entries)
            {
                if (dfItem.Resource.SelfLink.ToString() == dfPath)
                {
                    df = dfItem;
                    break;
                }
            }
            if (df == null) return;
            foreach (DataEntry deItem in df.DataEntries.Entries)
            {
                if (deItem.Resource.SelfLink.ToString() == dePath)
                {
                    de = deItem;
                    break;
                }
            }
        }

        private void SplitSelfLink(string path, ref string moPath, ref string dfPath, ref string dePath)
        {
            char[] seps = {'/'};
            string [] parts = path.Split(seps);
            string currentPath = "";
            string lastPart = "";
            foreach (string part in parts)
            {
                currentPath += part;
                if (lastPart == "MeshObjects") moPath = currentPath;
                if (lastPart == "DataFeeds") dfPath = currentPath;
                if (lastPart == "Entries") dePath = currentPath;
                currentPath += '/';
                lastPart = part;
            }
        }

        private void LoadTree()
        {
            if (loe == null) return;

            MeshTree.Nodes.Clear();
            TreeNode tnRoot = new TreeNode("Live Mesh Folders");
            MeshTree.Nodes.Add(tnRoot);

            //TreeImageList.Images.Add(WordMeshAddIn.Properties.Resources.icoFolder);
            //TreeImageList.Images.Add(WordMeshAddIn.Properties.Resources.icoPage);
            //tvFolders.ImageList = TreeImageList;

            var folderObjects = (from mo in loe.Mesh.CreateQuery<MeshObject>()
                                 where mo.Resource.Type == Constants.FOLDER_OBJ_TYPE
                                select mo).OrderBy(mo => mo.Resource.Title);

            foreach (MeshObject mo in folderObjects)
            {
                if (!mo.IsLoaded) mo.Load();
                TreeNode tnFolder = new TreeNode(mo.Resource.Title);
                tnRoot.ChildNodes.Add(tnFolder);
                tnFolder.Value = mo.Resource.SelfLink.ToString();
                LoadFolders(tnFolder, mo);
            }
            MeshTree.ExpandAll();
        }

        private void LoadFolders(TreeNode parentNode, MeshObject mo)
        {
            DataFeed df = (from dataFeed in mo.CreateQuery<DataFeed>()
                           where dataFeed.Resource.Title == Constants.FILE_OBJ_TYPE
                           select dataFeed).FirstOrDefault<DataFeed>();
            if (df == null) return;
            if (!df.IsLoaded) df.Load();
            LoadDataEntries(parentNode, df, null, Constants.ROOT_FOLDER_ID);
        }

        private void LoadDataEntries(TreeNode parentNode, DataFeed df, Uri parentLink, string parentId)
        {
            foreach (DataEntry de in df.DataEntries.Entries)
            {
                if (!de.IsLoaded) de.Load();
                if (de.Resource.ParentId != parentId) continue;
                if (de.Resource.Type == Constants.FOLDER_TYPE)
                {
                    TreeNode tnFolder = new TreeNode(de.Resource.Title);
                    parentNode.ChildNodes.Add(tnFolder);
                    tnFolder.Value = de.Resource.SelfLink.ToString();
                    LoadDataEntries(tnFolder, df, de.Resource.SelfLink, de.Resource.Id);
                }
                else if (de.Resource.Title.ToUpper().EndsWith(".JPG"))
                {
                    TreeNode tnFile = new TreeNode(de.Resource.Title);
                    parentNode.ChildNodes.Add(tnFile);
                    tnFile.Value = de.Resource.SelfLink.ToString();
                }
            }
        }
    }
}
