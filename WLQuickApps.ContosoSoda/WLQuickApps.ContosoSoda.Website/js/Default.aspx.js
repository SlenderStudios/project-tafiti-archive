function SendMessage() {
    //send message
    AlertService.SendGroupRequest(currentCulture);
}


function onSendRequest(result)
{
    //Localize appointmentSentText in resource file, defined in Default.aspx
    //popup.show(appointmentSentText);
}

function onFailed(error)
{      
    alert("Error:"+error.get_message());
}
