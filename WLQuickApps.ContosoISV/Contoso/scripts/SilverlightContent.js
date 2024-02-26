function pageIsBig()
{
    // if the page has a _big.aspx in it there is a different background to be used.
    return (window.location.toString().toLowerCase().indexOf("_big.aspx") > -1);
}

function onLoadRTW(plugin, uc, root)
{
    // if we are in big screen (one of the ASPXs)
    if(pageIsBig())
    {
        // set a certain background
        root.findName("imported_ribbon").Source = "css/ribbon.png";
    }
    else
    {
        // set a certain background
        root.findName("imported_ribbon").Source = "css/ribbon2.png";
    }
     
    // split the init params
    var params = plugin.initParams.split (',');
    
    // enumerate the init params
    for (var i=0; i<params.length; i++)
    {
        var index = i+1;
        
        // was the object found?
        if(root.findName("vid"+index) != null)
        {
            // Set the media element
            root.findName("vid"+index).Source = params[i];
            
            // start the playing
            root.findName("vid"+index).Play();
        }
    }
}

function createSilverlightControl()
{  
    //Localize playVideos in resource file, defined in Default.aspx and Default_BIG.aspx
    var videos = playVideos.split(',');
    var params = "";
    for(var video in videos)
    {
        params += "streaming:/" + videos[video] + ",";
    }
    
    // Create the hosted silverlight app - this JS is externally referenced.
    Silverlight.createHostedObjectEx( 
        {   
            source: "XAML/VideoGoRound_big_RTW.xaml", 
            parentElement: $get("SilverlightControlHost"), 
            id: "SilverlightControl", 
            properties:
            { 
                width:'549', 
                height:'243', 
                version:'1.0', 
                background:document.body.style.backgroundColor, 
                isWindowless:'false' 
            }, 
            events:
            { 
                    onError:null, 
                    onLoad:onLoadRTW 
            },
            initParams:params
        }
    );
}
