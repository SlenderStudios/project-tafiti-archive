/// Authors:   Bronwen Zande - bronwen.zande@aptovita.com
///            John O'Brien - john.obrien@aptovita.com

Type.registerNamespace("Contoso");    
    
Contoso.LiveContacts = function()
{
}

Contoso.LiveContacts.prototype = 
{
    processErrorDescription: function(description)
    {
        var s = "";
        if (description.length > 0)
        {
            for (var i = 0; i < description.length; i++) {
                s += "";
                for (var j in description[i]) {
                    s += j + ": " + description[i][j] + "";
                }
                s += "";
            } 
            alert(s);
        }
         
    },

    _hescq: function(p_str) 
    {
        return p_str.replace(/&/g,"&amp;").replace(/</g, "&lt;").replace(/>/g,"&gt;").replace(/\"/g,"&quot;").replace(/\'/g,"&apos;"); 
    },
    
    receiveData: function(p_contacts) 
    {
       this.sendToContacts(p_contacts);
    },
    
    sendToContacts: function(p_contacts)
    {
        // insert your email logic here...
        var s;
        if (p_contacts.length > 1)
        {
            s = FavoritesMsg;
        }
        else
        {
            s = FavoriteMsg;
        }
            
        for (var i = 0; i < p_contacts.length; i++) 
        {
            for (var j in p_contacts[i]) 
            {
                if (this._hescq(j) == 'name')
                {
                    s += this._hescq(p_contacts[i][j]) + ",";
                }
            }
        }
        s = s.substr(0, s.length-1);
        alert(s);   
    }
 }   
 
Contoso.LiveContacts.registerClass('Contoso.LiveContacts', null);

 if (typeof(Sys) !== "undefined") Sys.Application.notifyScriptLoaded();