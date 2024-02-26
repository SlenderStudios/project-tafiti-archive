
function escapeHTML(str)
{
   var div = document.createElement('div');
   var text = document.createTextNode(str);
   div.appendChild(text);
   return div.innerHTML;
}

function getDescription(field)
{
    var description = escapeHTML(field.Description);
    var address = escapeHTML(field.Address);
    var phoneNumber = escapeHTML(field.PhoneNumber);
    var fieldID = escapeHTML(field.FieldID);
    
    if (field.Address) { description += "<br />" + address; }
    if (field.PhoneNumber) { description += "<br />Phone: " + phoneNumber; }
    description += "<br /><a href='/Field/ViewField.aspx?fieldID=" + fieldID + "'>View Field</a>";
    if (userIsLoggedIn && 
        (typeof(myFields[fieldID]) == "undefined") && 
        (typeof(addToMyFields) != "undefined"))
    {
        description += "<br /><a href='javascript://Add to my fields' onclick='addToMyFields(" + fieldID + ")'>Add To My Fields</a>";
    }
    
    if (typeof(viewDirections) == "undefined")
    {
        description += "<br /><a href='/Directions.aspx?endAddress=" + encodeURIComponent(address) + "'>Directions</a>";
    }
    else
    {
        description += "<br /><a href='javascript:viewDirections(\"" + address + "\")'>Directions</a>";
    }
    

    return description;
}