using System;
using System.DHTML;
using ScriptFX;
using System.XML;

namespace WLQuickApps.Tafiti.Scripting
{
    public class CommentManager
    {
        static private Dictionary Cache
        {
            get
            {
                if (CommentManager._cache == null)
                {
                    CommentManager._cache = new Dictionary();
                }
                return CommentManager._cache;
            }
        }
        static private Dictionary _cache;

        static private ArrayList OutstandingShelfStackCommentRequests
        {
            get
            {
                if (CommentManager._outstandingShelfStackCommentRequests == null)
                {
                    CommentManager._outstandingShelfStackCommentRequests = new ArrayList();
                }
                return CommentManager._outstandingShelfStackCommentRequests;
            }
        }
        static private ArrayList _outstandingShelfStackCommentRequests;

        static private ArrayList OutstandingCommentRequests
        {
            get
            {
                if (CommentManager._outstandingCommentRequests == null)
                {
                    CommentManager._outstandingCommentRequests = new ArrayList();
                }
                return CommentManager._outstandingCommentRequests;
            }
        }
        static private ArrayList _outstandingCommentRequests;

        static public void BeginGetCommentsForShelfStack(string shelfStackID)
        {
            string body = "shelfStackID=" + shelfStackID;

            XMLHttpRequest request = new XMLHttpRequest();
            request.Open("POST", Utilities.GetSiteUrlRoot() + "/SiteService.asmx/GetConversation", true);
            request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            request.SetRequestHeader("Content-Length", body.Length.ToString());
            request.Onreadystatechange = new Callback(CommentManager.EndGetCommentsForShelfStack);

            CommentManager.OutstandingShelfStackCommentRequests.Add(request);
            request.Send(body);
        }

        static private void EndGetCommentsForShelfStack()
        {
            for (int lcv = 0; lcv < CommentManager.OutstandingShelfStackCommentRequests.Length; lcv++)
            {
                XMLHttpRequest request = (XMLHttpRequest) CommentManager.OutstandingShelfStackCommentRequests[lcv];
                if (request.ReadyState == 4)
                {
                    CommentManager.OutstandingShelfStackCommentRequests.RemoveAt(lcv);
                    lcv--;

                    XMLNodeList commentNodeList = request.ResponseXML.GetElementsByTagName("string");

                    // Make sure that the list isn't null. If it isn't, also make sure there isn't exactly one item with
                    // no contents as this is what results from an empty list (null) being returned from the server.
                    // In other words, the only time we should get inside this block is when there are shelves for the user.
                    if ((commentNodeList == null) || ((commentNodeList.Length == 1) && (commentNodeList[0].Text == string.Empty)))
                    {
                        return;
                    }
                    for (int lcv2 = 0; lcv2 < commentNodeList.Length; lcv2++)
                    {
                        if (!CommentManager.Cache.ContainsKey(commentNodeList[lcv2].Text))
                        {
                            CommentManager.BeginCommentRequest(commentNodeList[lcv2].Text);
                        }
                    }
                }
            }
        }

        static public void BeginCommentRequest(string commentID)
        {
            if (CommentManager.Cache.ContainsKey(commentID))
            {
                return;
            }

            string body = "commentID=" + commentID;

            XMLHttpRequest request = new XMLHttpRequest();
            request.Open("POST", Utilities.GetSiteUrlRoot() + "/SiteService.asmx/GetComment", true);
            request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            request.SetRequestHeader("Content-Length", body.Length.ToString());
            request.Onreadystatechange = new Callback(CommentManager.EndCommentRequest);

            CommentManager.OutstandingCommentRequests.Add(request);
            request.Send(body);
        }

        static private void EndCommentRequest()
        {
            for (int lcv = 0; lcv < CommentManager.OutstandingCommentRequests.Length; lcv++)
            {
                XMLHttpRequest request = (XMLHttpRequest)CommentManager.OutstandingCommentRequests[lcv];
                if (request.ReadyState == 4)
                {
                    CommentManager.OutstandingCommentRequests.RemoveAt(lcv);
                    lcv--;

                    Comment comment = Comment.CreateFromXmlNode(request.ResponseXML.LastChild);
                    CommentManager.Cache[comment.CommentID] = comment;
                    return;
                }
            }
        }

        static public Comment[] GetCommentsForShelfStack(string shelfStackID)
        {
            string body = "shelfStackID=" + shelfStackID;

            XMLHttpRequest request = new XMLHttpRequest();
            request.Open("POST", Utilities.GetSiteUrlRoot() + "/SiteService.asmx/GetConversation", false);
            request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            request.SetRequestHeader("Content-Length", body.Length.ToString());
            request.Send(body);

            XMLNodeList commentNodeList = request.ResponseXML.GetElementsByTagName("string");

            // Make sure that the list isn't null. If it isn't, also make sure there isn't exactly one item with
            // no contents as this is what results from an empty list (null) being returned from the server.
            // In other words, the only time we should get inside this block is when there are shelves for the user.
            if ((commentNodeList == null) || ((commentNodeList.Length == 1) && (commentNodeList[0].Text == string.Empty)))
            {
                return new Comment[0];
            }

            Comment[] comments = new Comment[commentNodeList.Length];

            for (int lcv = 0; lcv < commentNodeList.Length; lcv++)
            {
                comments[lcv] = CommentManager.GetComment(commentNodeList[lcv].Text);
            }
            return comments;
        }

        static public void AddComment(string shelfStackID, string text)
        {
            ShelfStack shelfStack = ShelfStackManager.GetShelfStack(shelfStackID);

            string body = "shelfStackID=" + shelfStackID + "&text=" + text.Escape();

            XMLHttpRequest request = new XMLHttpRequest();
            request.Open("POST", Utilities.GetSiteUrlRoot() + "/SiteService.asmx/AddComment", false);
            request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            request.SetRequestHeader("Content-Length", body.Length.ToString());
            request.Send(body);

            ShelfStackManager.SendShelfStackUpdate(shelfStack);
        }

        static public Comment GetComment(string commentID)
        {
            if (CommentManager.Cache.ContainsKey(commentID))
            {
                return (Comment)CommentManager.Cache[commentID];
            }

            string body = "commentID=" + commentID;

            XMLHttpRequest request = new XMLHttpRequest();
            request.Open("POST", Utilities.GetSiteUrlRoot() + "/SiteService.asmx/GetComment", false);
            request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            request.SetRequestHeader("Content-Length", body.Length.ToString());
            request.Send(body);

            Comment comment = Comment.CreateFromXmlNode(request.ResponseXML.LastChild);
            CommentManager.Cache[commentID] = comment;
            return comment;
        }

        static public void CacheComment(Comment comment)
        {
            if (!CommentManager.Cache.ContainsKey(comment.CommentID))
            {
                CommentManager.Cache[comment.CommentID] = comment;
            }
        }
    }
}
