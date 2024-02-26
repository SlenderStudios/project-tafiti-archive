
// Create the Silverlight Application (appName & accountID)
function CreateSilverlight(name, accountid)
{

    var MediaWrapper = document.getElementById("MediaWrapper");

    Silverlight.createHostedObjectEx({source: "streaming:/" + accountid + "/" + name, parentElement: MediaWrapper});

}