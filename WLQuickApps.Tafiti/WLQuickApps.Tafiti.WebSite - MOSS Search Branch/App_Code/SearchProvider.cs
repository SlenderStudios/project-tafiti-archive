using System;
using System.Data;
using System.Configuration;
using System.Configuration.Provider;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using LiveSearch;

public abstract class SearchProvider : ProviderBase
{
    public abstract LiveXmlSearchResults ExecuteSearch();
}

public class SearchProviderCollection : ProviderCollection
{
    public new SearchProvider this[string name]
    {
        get { return (SearchProvider)base[name]; }
    }

    public override void Add(ProviderBase provider)
    {
        if (provider == null)
            throw new ArgumentNullException("provider");

        if (!(provider is SearchProvider))
            throw new ArgumentException
                ("Invalid provider type", "provider");

        base.Add(provider);
    }
}

public class SearchProviderSection : ConfigurationSection
{
    [ConfigurationProperty("providers")]
    public ProviderSettingsCollection Providers
    {
        get { return 
              (ProviderSettingsCollection)base["providers"];}
    }

    [StringValidator(MinLength = 1)]
    [ConfigurationProperty("defaultProvider", 
        DefaultValue = "SearchProviderSharePoint")]
    public string DefaultProvider
    {
        get { return (string)base["defaultProvider"]; }
        set { base["defaultProvider"] = value; }
    }
}