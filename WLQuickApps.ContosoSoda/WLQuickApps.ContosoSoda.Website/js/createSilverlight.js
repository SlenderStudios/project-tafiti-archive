function createSilverlightHome()
{  
    Silverlight.createObject(
        "xaml/home.xaml", // Source property value
		parentElement, // DOM reference to hosting DIV tag
		"sl1", // Unique plug-in ID value
        { width:'1000', height:'700',
			inplaceInstallPrompt:false,
            background:'#FFFFFF', isWindowless:'true',
            framerate:'24', version:'1.0' },
        { onError:null, onLoad:null },
        null);
}