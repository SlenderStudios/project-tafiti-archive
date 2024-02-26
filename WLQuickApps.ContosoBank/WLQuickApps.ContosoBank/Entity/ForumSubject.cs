namespace WLQuickApps.ContosoBank
{
    public partial class ForumSubject
    {
        public string SubjectImageLocation
        {
            get { return "/images/subjecttype_" + SubjectType + ".png"; }
        }
    }
}