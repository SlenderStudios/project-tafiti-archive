using System;
using System.Data;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using WLQuickApps.Tafiti.Business;

namespace WLQuickApps.Tafiti.WebSite
{
    public class MasterData
    {
        public ShelfStackData[] ShelfStackDatas;
        public ShelfStackItemData[] ShelfStackItemDatas;
        public CommentData[] CommentDatas;
        public UserData[] UserDatas;

        public MasterData() { }

        public MasterData(ReadOnlyCollection<ShelfStack> shelfStacks)
        {
            List<ShelfStackItemData> shelfStackItemDatas = new List<ShelfStackItemData>();
            List<CommentData> commentDatas = new List<CommentData>();
            Dictionary<string, UserData> userDatas = new Dictionary<string, UserData>();

            this.ShelfStackDatas = new ShelfStackData[shelfStacks.Count];
            for (int shelfStackIndex = 0; shelfStackIndex < shelfStacks.Count; shelfStackIndex++)
            {
                this.ShelfStackDatas[shelfStackIndex] = new ShelfStackData(shelfStacks[shelfStackIndex]);

                foreach (ShelfStackItem shelfStackItem in shelfStacks[shelfStackIndex].ShelfStackItems)
                {
                    shelfStackItemDatas.Add(new ShelfStackItemData(shelfStackItem));
                }

                foreach (Comment comment in shelfStacks[shelfStackIndex].Conversation)
                {
                    commentDatas.Add(new CommentData(comment));
                }

                foreach (User user in shelfStacks[shelfStackIndex].Owners)
                {
                    if (!userDatas.ContainsKey(user.UserID))
                    {
                        userDatas.Add(user.UserID, new UserData(user));
                    }
                }
            }

            this.ShelfStackItemDatas = shelfStackItemDatas.ToArray();
            this.CommentDatas = commentDatas.ToArray();
            this.UserDatas = userDatas.Values.ToArray();
        }
    }
}