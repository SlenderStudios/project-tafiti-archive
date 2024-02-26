// WLQuickApps.Tafiti.Scripting.js
//


Type.createNamespace('WLQuickApps.Tafiti.Scripting');

////////////////////////////////////////////////////////////////////////////////
// WLQuickApps.Tafiti.Scripting.Constants

WLQuickApps.Tafiti.Scripting.Constants = function WLQuickApps_Tafiti_Scripting_Constants() {
}


////////////////////////////////////////////////////////////////////////////////
// WLQuickApps.Tafiti.Scripting.CryptographyManager

WLQuickApps.Tafiti.Scripting.CryptographyManager = function WLQuickApps_Tafiti_Scripting_CryptographyManager() {
}
WLQuickApps.Tafiti.Scripting.CryptographyManager.get__hashCache = function WLQuickApps_Tafiti_Scripting_CryptographyManager$get__hashCache() {
    if (!WLQuickApps.Tafiti.Scripting.CryptographyManager._hashCache) {
        WLQuickApps.Tafiti.Scripting.CryptographyManager._hashCache = {};
    }
    return WLQuickApps.Tafiti.Scripting.CryptographyManager._hashCache;
}
WLQuickApps.Tafiti.Scripting.CryptographyManager.getMD5Hash = function WLQuickApps_Tafiti_Scripting_CryptographyManager$getMD5Hash(input) {
    if (Object.keyExists(WLQuickApps.Tafiti.Scripting.CryptographyManager.get__hashCache(), input)) {
        return WLQuickApps.Tafiti.Scripting.CryptographyManager.get__hashCache()[input];
    }
    var a = 1732584193;
    var b = 4023233417;
    var c = 2562383102;
    var d = 271733878;
    var s11 = 7;
    var s12 = 12;
    var s13 = 17;
    var s14 = 22;
    var s21 = 5;
    var s22 = 9;
    var s23 = 14;
    var s24 = 20;
    var s31 = 4;
    var s32 = 11;
    var s33 = 16;
    var s34 = 23;
    var s41 = 6;
    var s42 = 10;
    var s43 = 15;
    var s44 = 21;
    var words = WLQuickApps.Tafiti.Scripting.CryptographyManager._convertToWordArray(input);
    for (var lcv = 0; lcv < words.length; lcv += 16) {
        var lastA = a;
        var lastB = b;
        var lastC = c;
        var lastD = d;
        a = WLQuickApps.Tafiti.Scripting.CryptographyManager._FF(a, b, c, d, words[lcv + 0], s11, 3614090360);
        d = WLQuickApps.Tafiti.Scripting.CryptographyManager._FF(d, a, b, c, words[lcv + 1], s12, 3905402710);
        c = WLQuickApps.Tafiti.Scripting.CryptographyManager._FF(c, d, a, b, words[lcv + 2], s13, 606105819);
        b = WLQuickApps.Tafiti.Scripting.CryptographyManager._FF(b, c, d, a, words[lcv + 3], s14, 3250441966);
        a = WLQuickApps.Tafiti.Scripting.CryptographyManager._FF(a, b, c, d, words[lcv + 4], s11, 4118548399);
        d = WLQuickApps.Tafiti.Scripting.CryptographyManager._FF(d, a, b, c, words[lcv + 5], s12, 1200080426);
        c = WLQuickApps.Tafiti.Scripting.CryptographyManager._FF(c, d, a, b, words[lcv + 6], s13, 2821735955);
        b = WLQuickApps.Tafiti.Scripting.CryptographyManager._FF(b, c, d, a, words[lcv + 7], s14, 4249261313);
        a = WLQuickApps.Tafiti.Scripting.CryptographyManager._FF(a, b, c, d, words[lcv + 8], s11, 1770035416);
        d = WLQuickApps.Tafiti.Scripting.CryptographyManager._FF(d, a, b, c, words[lcv + 9], s12, 2336552879);
        c = WLQuickApps.Tafiti.Scripting.CryptographyManager._FF(c, d, a, b, words[lcv + 10], s13, 4294925233);
        b = WLQuickApps.Tafiti.Scripting.CryptographyManager._FF(b, c, d, a, words[lcv + 11], s14, 2304563134);
        a = WLQuickApps.Tafiti.Scripting.CryptographyManager._FF(a, b, c, d, words[lcv + 12], s11, 1804603682);
        d = WLQuickApps.Tafiti.Scripting.CryptographyManager._FF(d, a, b, c, words[lcv + 13], s12, 4254626195);
        c = WLQuickApps.Tafiti.Scripting.CryptographyManager._FF(c, d, a, b, words[lcv + 14], s13, 2792965006);
        b = WLQuickApps.Tafiti.Scripting.CryptographyManager._FF(b, c, d, a, words[lcv + 15], s14, 1236535329);
        a = WLQuickApps.Tafiti.Scripting.CryptographyManager._GG(a, b, c, d, words[lcv + 1], s21, 4129170786);
        d = WLQuickApps.Tafiti.Scripting.CryptographyManager._GG(d, a, b, c, words[lcv + 6], s22, 3225465664);
        c = WLQuickApps.Tafiti.Scripting.CryptographyManager._GG(c, d, a, b, words[lcv + 11], s23, 643717713);
        b = WLQuickApps.Tafiti.Scripting.CryptographyManager._GG(b, c, d, a, words[lcv + 0], s24, 3921069994);
        a = WLQuickApps.Tafiti.Scripting.CryptographyManager._GG(a, b, c, d, words[lcv + 5], s21, 3593408605);
        d = WLQuickApps.Tafiti.Scripting.CryptographyManager._GG(d, a, b, c, words[lcv + 10], s22, 38016083);
        c = WLQuickApps.Tafiti.Scripting.CryptographyManager._GG(c, d, a, b, words[lcv + 15], s23, 3634488961);
        b = WLQuickApps.Tafiti.Scripting.CryptographyManager._GG(b, c, d, a, words[lcv + 4], s24, 3889429448);
        a = WLQuickApps.Tafiti.Scripting.CryptographyManager._GG(a, b, c, d, words[lcv + 9], s21, 568446438);
        d = WLQuickApps.Tafiti.Scripting.CryptographyManager._GG(d, a, b, c, words[lcv + 14], s22, 3275163606);
        c = WLQuickApps.Tafiti.Scripting.CryptographyManager._GG(c, d, a, b, words[lcv + 3], s23, 4107603335);
        b = WLQuickApps.Tafiti.Scripting.CryptographyManager._GG(b, c, d, a, words[lcv + 8], s24, 1163531501);
        a = WLQuickApps.Tafiti.Scripting.CryptographyManager._GG(a, b, c, d, words[lcv + 13], s21, 2850285829);
        d = WLQuickApps.Tafiti.Scripting.CryptographyManager._GG(d, a, b, c, words[lcv + 2], s22, 4243563512);
        c = WLQuickApps.Tafiti.Scripting.CryptographyManager._GG(c, d, a, b, words[lcv + 7], s23, 1735328473);
        b = WLQuickApps.Tafiti.Scripting.CryptographyManager._GG(b, c, d, a, words[lcv + 12], s24, 2368359562);
        a = WLQuickApps.Tafiti.Scripting.CryptographyManager._HH(a, b, c, d, words[lcv + 5], s31, 4294588738);
        d = WLQuickApps.Tafiti.Scripting.CryptographyManager._HH(d, a, b, c, words[lcv + 8], s32, 2272392833);
        c = WLQuickApps.Tafiti.Scripting.CryptographyManager._HH(c, d, a, b, words[lcv + 11], s33, 1839030562);
        b = WLQuickApps.Tafiti.Scripting.CryptographyManager._HH(b, c, d, a, words[lcv + 14], s34, 4259657740);
        a = WLQuickApps.Tafiti.Scripting.CryptographyManager._HH(a, b, c, d, words[lcv + 1], s31, 2763975236);
        d = WLQuickApps.Tafiti.Scripting.CryptographyManager._HH(d, a, b, c, words[lcv + 4], s32, 1272893353);
        c = WLQuickApps.Tafiti.Scripting.CryptographyManager._HH(c, d, a, b, words[lcv + 7], s33, 4139469664);
        b = WLQuickApps.Tafiti.Scripting.CryptographyManager._HH(b, c, d, a, words[lcv + 10], s34, 3200236656);
        a = WLQuickApps.Tafiti.Scripting.CryptographyManager._HH(a, b, c, d, words[lcv + 13], s31, 681279174);
        d = WLQuickApps.Tafiti.Scripting.CryptographyManager._HH(d, a, b, c, words[lcv + 0], s32, 3936430074);
        c = WLQuickApps.Tafiti.Scripting.CryptographyManager._HH(c, d, a, b, words[lcv + 3], s33, 3572445317);
        b = WLQuickApps.Tafiti.Scripting.CryptographyManager._HH(b, c, d, a, words[lcv + 6], s34, 76029189);
        a = WLQuickApps.Tafiti.Scripting.CryptographyManager._HH(a, b, c, d, words[lcv + 9], s31, 3654602809);
        d = WLQuickApps.Tafiti.Scripting.CryptographyManager._HH(d, a, b, c, words[lcv + 12], s32, 3873151461);
        c = WLQuickApps.Tafiti.Scripting.CryptographyManager._HH(c, d, a, b, words[lcv + 15], s33, 530742520);
        b = WLQuickApps.Tafiti.Scripting.CryptographyManager._HH(b, c, d, a, words[lcv + 2], s34, 3299628645);
        a = WLQuickApps.Tafiti.Scripting.CryptographyManager._II(a, b, c, d, words[lcv + 0], s41, 4096336452);
        d = WLQuickApps.Tafiti.Scripting.CryptographyManager._II(d, a, b, c, words[lcv + 7], s42, 1126891415);
        c = WLQuickApps.Tafiti.Scripting.CryptographyManager._II(c, d, a, b, words[lcv + 14], s43, 2878612391);
        b = WLQuickApps.Tafiti.Scripting.CryptographyManager._II(b, c, d, a, words[lcv + 5], s44, 4237533241);
        a = WLQuickApps.Tafiti.Scripting.CryptographyManager._II(a, b, c, d, words[lcv + 12], s41, 1700485571);
        d = WLQuickApps.Tafiti.Scripting.CryptographyManager._II(d, a, b, c, words[lcv + 3], s42, 2399980690);
        c = WLQuickApps.Tafiti.Scripting.CryptographyManager._II(c, d, a, b, words[lcv + 10], s43, 4293915773);
        b = WLQuickApps.Tafiti.Scripting.CryptographyManager._II(b, c, d, a, words[lcv + 1], s44, 2240044497);
        a = WLQuickApps.Tafiti.Scripting.CryptographyManager._II(a, b, c, d, words[lcv + 8], s41, 1873313359);
        d = WLQuickApps.Tafiti.Scripting.CryptographyManager._II(d, a, b, c, words[lcv + 15], s42, 4264355552);
        c = WLQuickApps.Tafiti.Scripting.CryptographyManager._II(c, d, a, b, words[lcv + 6], s43, 2734768916);
        b = WLQuickApps.Tafiti.Scripting.CryptographyManager._II(b, c, d, a, words[lcv + 13], s44, 1309151649);
        a = WLQuickApps.Tafiti.Scripting.CryptographyManager._II(a, b, c, d, words[lcv + 4], s41, 4149444226);
        d = WLQuickApps.Tafiti.Scripting.CryptographyManager._II(d, a, b, c, words[lcv + 11], s42, 3174756917);
        c = WLQuickApps.Tafiti.Scripting.CryptographyManager._II(c, d, a, b, words[lcv + 2], s43, 718787259);
        b = WLQuickApps.Tafiti.Scripting.CryptographyManager._II(b, c, d, a, words[lcv + 9], s44, 3951481745);
        a = WLQuickApps.Tafiti.Scripting.CryptographyManager._safeAdd(a, lastA);
        b = WLQuickApps.Tafiti.Scripting.CryptographyManager._safeAdd(b, lastB);
        c = WLQuickApps.Tafiti.Scripting.CryptographyManager._safeAdd(c, lastC);
        d = WLQuickApps.Tafiti.Scripting.CryptographyManager._safeAdd(d, lastD);
    }
    var hash = WLQuickApps.Tafiti.Scripting.CryptographyManager._wordToHex(a) + WLQuickApps.Tafiti.Scripting.CryptographyManager._wordToHex(b) + WLQuickApps.Tafiti.Scripting.CryptographyManager._wordToHex(c) + WLQuickApps.Tafiti.Scripting.CryptographyManager._wordToHex(d);
    WLQuickApps.Tafiti.Scripting.CryptographyManager.get__hashCache()[input] = hash;
    return hash;
}
WLQuickApps.Tafiti.Scripting.CryptographyManager._rotateLeft = function WLQuickApps_Tafiti_Scripting_CryptographyManager$_rotateLeft(value, bitsToShift) {
    return (value << bitsToShift) | (value >>> (32 - bitsToShift));
}
WLQuickApps.Tafiti.Scripting.CryptographyManager._safeAdd = function WLQuickApps_Tafiti_Scripting_CryptographyManager$_safeAdd(first, second) {
    var x4 = first & 1073741824;
    var y4 = second & 1073741824;
    var x8 = first & 2147483648;
    var y8 = second & 2147483648;
    var result = (first & 1073741823) + (second & 1073741823);
    if ((x4 & y4)) {
        return (result ^ 2147483648 ^ x8 ^ y8);
    }
    if ((x4 | y4)) {
        if ((result & 1073741824)) {
            return (result ^ 3221225472 ^ x8 ^ y8);
        }
        else {
            return (result ^ 1073741824 ^ x8 ^ y8);
        }
    }
    else {
        return (result ^ x8 ^ y8);
    }
}
WLQuickApps.Tafiti.Scripting.CryptographyManager._f = function WLQuickApps_Tafiti_Scripting_CryptographyManager$_f(x, y, z) {
    return (x & y) | ((~x) & z);
}
WLQuickApps.Tafiti.Scripting.CryptographyManager._g = function WLQuickApps_Tafiti_Scripting_CryptographyManager$_g(x, y, z) {
    return (x & z) | (y & (~z));
}
WLQuickApps.Tafiti.Scripting.CryptographyManager._h = function WLQuickApps_Tafiti_Scripting_CryptographyManager$_h(x, y, z) {
    return (x ^ y ^ z);
}
WLQuickApps.Tafiti.Scripting.CryptographyManager._i = function WLQuickApps_Tafiti_Scripting_CryptographyManager$_i(x, y, z) {
    return (y ^ (x | (~z)));
}
WLQuickApps.Tafiti.Scripting.CryptographyManager._FF = function WLQuickApps_Tafiti_Scripting_CryptographyManager$_FF(a, b, c, d, x, s, ac) {
    a = WLQuickApps.Tafiti.Scripting.CryptographyManager._safeAdd(a, WLQuickApps.Tafiti.Scripting.CryptographyManager._safeAdd(WLQuickApps.Tafiti.Scripting.CryptographyManager._safeAdd(WLQuickApps.Tafiti.Scripting.CryptographyManager._f(b, c, d), x), ac));
    return WLQuickApps.Tafiti.Scripting.CryptographyManager._safeAdd(WLQuickApps.Tafiti.Scripting.CryptographyManager._rotateLeft(a, s), b);
}
WLQuickApps.Tafiti.Scripting.CryptographyManager._GG = function WLQuickApps_Tafiti_Scripting_CryptographyManager$_GG(a, b, c, d, x, s, ac) {
    a = WLQuickApps.Tafiti.Scripting.CryptographyManager._safeAdd(a, WLQuickApps.Tafiti.Scripting.CryptographyManager._safeAdd(WLQuickApps.Tafiti.Scripting.CryptographyManager._safeAdd(WLQuickApps.Tafiti.Scripting.CryptographyManager._g(b, c, d), x), ac));
    return WLQuickApps.Tafiti.Scripting.CryptographyManager._safeAdd(WLQuickApps.Tafiti.Scripting.CryptographyManager._rotateLeft(a, s), b);
}
WLQuickApps.Tafiti.Scripting.CryptographyManager._HH = function WLQuickApps_Tafiti_Scripting_CryptographyManager$_HH(a, b, c, d, x, s, ac) {
    a = WLQuickApps.Tafiti.Scripting.CryptographyManager._safeAdd(a, WLQuickApps.Tafiti.Scripting.CryptographyManager._safeAdd(WLQuickApps.Tafiti.Scripting.CryptographyManager._safeAdd(WLQuickApps.Tafiti.Scripting.CryptographyManager._h(b, c, d), x), ac));
    return WLQuickApps.Tafiti.Scripting.CryptographyManager._safeAdd(WLQuickApps.Tafiti.Scripting.CryptographyManager._rotateLeft(a, s), b);
}
WLQuickApps.Tafiti.Scripting.CryptographyManager._II = function WLQuickApps_Tafiti_Scripting_CryptographyManager$_II(a, b, c, d, x, s, ac) {
    a = WLQuickApps.Tafiti.Scripting.CryptographyManager._safeAdd(a, WLQuickApps.Tafiti.Scripting.CryptographyManager._safeAdd(WLQuickApps.Tafiti.Scripting.CryptographyManager._safeAdd(WLQuickApps.Tafiti.Scripting.CryptographyManager._i(b, c, d), x), ac));
    return WLQuickApps.Tafiti.Scripting.CryptographyManager._safeAdd(WLQuickApps.Tafiti.Scripting.CryptographyManager._rotateLeft(a, s), b);
}
WLQuickApps.Tafiti.Scripting.CryptographyManager._convertToWordArray = function WLQuickApps_Tafiti_Scripting_CryptographyManager$_convertToWordArray(text) {
    var wordCount;
    var messageLength = text.length;
    var numberOfWords_temp1 = messageLength + 8;
    var numberOfWords_temp2 = (numberOfWords_temp1 - (numberOfWords_temp1 % 64)) / 64;
    var numberOfWords = (numberOfWords_temp2 + 1) * 16;
    var wordArray = new Array(numberOfWords - 1);
    var bytePosition = 0;
    var byteCount = 0;
    while (byteCount < messageLength) {
        wordCount = (byteCount - (byteCount % 4)) / 4;
        bytePosition = (byteCount % 4) * 8;
        wordArray[wordCount] = (wordArray[wordCount] | (text.charCodeAt(byteCount) << bytePosition));
        byteCount++;
    }
    wordCount = (byteCount - (byteCount % 4)) / 4;
    bytePosition = (byteCount % 4) * 8;
    wordArray[wordCount] = (wordArray[wordCount] | (128 << bytePosition));
    wordArray[numberOfWords - 2] = (messageLength << 3);
    wordArray[numberOfWords - 1] = (messageLength >> 29);
    return wordArray;
}
WLQuickApps.Tafiti.Scripting.CryptographyManager._wordToHex = function WLQuickApps_Tafiti_Scripting_CryptographyManager$_wordToHex(word) {
    var stringBuilder = new StringBuilder();
    var byteValue;
    for (var lcv = 0; lcv <= 3; lcv++) {
        byteValue = ((word >>> (lcv * 8)) & 255);
        var temp = '0' + byteValue.toString(16);
        stringBuilder.append(temp.substr(temp.length - 2, 2));
    }
    return stringBuilder.toLocaleString();
}


////////////////////////////////////////////////////////////////////////////////
// WLQuickApps.Tafiti.Scripting.MessengerManager

WLQuickApps.Tafiti.Scripting.MessengerManager = function WLQuickApps_Tafiti_Scripting_MessengerManager() {
}
WLQuickApps.Tafiti.Scripting.MessengerManager.get_myMessengerController = function WLQuickApps_Tafiti_Scripting_MessengerManager$get_myMessengerController() {
    return WLQuickApps.Tafiti.Scripting.MessengerManager._myMessengerController;
}
WLQuickApps.Tafiti.Scripting.MessengerManager.get_isSignedIn = function WLQuickApps_Tafiti_Scripting_MessengerManager$get_isSignedIn() {
    return ((WLQuickApps.Tafiti.Scripting.MessengerManager._myMessengerController) && WLQuickApps.Tafiti.Scripting.MessengerManager._myMessengerController.get_isSignedIn());
}
WLQuickApps.Tafiti.Scripting.MessengerManager.startMessenger = function WLQuickApps_Tafiti_Scripting_MessengerManager$startMessenger() {
    if (!WLQuickApps.Tafiti.Scripting.MessengerManager._myMessengerController) {
        WLQuickApps.Tafiti.Scripting.MessengerManager._myMessengerController = new WLQuickApps.Tafiti.Scripting.MessengerController();
    }
}
WLQuickApps.Tafiti.Scripting.MessengerManager.getMessengerContactByEmailHash = function WLQuickApps_Tafiti_Scripting_MessengerManager$getMessengerContactByEmailHash(emailHash) {
    WLQuickApps.Tafiti.Scripting.MessengerManager._verifyUserIsLoggedInToMessenger();
    var $enum1 = WLQuickApps.Tafiti.Scripting.MessengerManager.get_myMessengerController().get_loggedInUser().get_contacts().getEnumerator();
    while ($enum1.moveNext()) {
        var contact = $enum1.get_current();
        if (WLQuickApps.Tafiti.Scripting.Utilities.hash(contact.get_currentAddress().get_address()) === emailHash) {
            return contact;
        }
    }
    return null;
}
WLQuickApps.Tafiti.Scripting.MessengerManager.supportsTafitiMessages = function WLQuickApps_Tafiti_Scripting_MessengerManager$supportsTafitiMessages(emailHash) {
    var contact = WLQuickApps.Tafiti.Scripting.MessengerManager.getMessengerContactByEmailHash(emailHash);
    if ((!contact) || !contact.get_currentAddress().get_isOnline()) {
        return false;
    }
    if (!contact.get_currentAddress().get_capabilities().supportsMessageType(Microsoft.Live.Messenger.MessageType.applicationMessage)) {
        return false;
    }
    var conversation = WLQuickApps.Tafiti.Scripting.MessengerManager.get_myMessengerController().get_loggedInUser().get_conversations().create(contact.get_currentAddress());
    return conversation.supportsApplicationMessageType(WLQuickApps.Tafiti.Scripting.Constants.tafitiUpdateMessageID);
}
WLQuickApps.Tafiti.Scripting.MessengerManager.sendUpdateMessage = function WLQuickApps_Tafiti_Scripting_MessengerManager$sendUpdateMessage(emailHash, shelfStack) {
    WLQuickApps.Tafiti.Scripting.MessengerManager._verifyUserIsLoggedInToMessenger();
    if (WLQuickApps.Tafiti.Scripting.MessengerManager.supportsTafitiMessages(emailHash)) {
        var contact = WLQuickApps.Tafiti.Scripting.MessengerManager.getMessengerContactByEmailHash(emailHash);
        var conversation = WLQuickApps.Tafiti.Scripting.MessengerManager.get_myMessengerController().get_loggedInUser().get_conversations().create(contact.get_currentAddress());
        conversation.sendMessage(new WLQuickApps.Tafiti.Scripting.TafitiUpdateMessage(shelfStack.shelfStackID), null);
    }
    else {
        var message = 'I have updated the shelf stack \"' + shelfStack.label + '\" at ' + WLQuickApps.Tafiti.Scripting.Utilities.getSiteUrlRoot();
        WLQuickApps.Tafiti.Scripting.MessengerManager.sendTextMessage(emailHash, message);
    }
}
WLQuickApps.Tafiti.Scripting.MessengerManager.sendTextMessage = function WLQuickApps_Tafiti_Scripting_MessengerManager$sendTextMessage(emailHash, message) {
    WLQuickApps.Tafiti.Scripting.MessengerManager._verifyUserIsLoggedInToMessenger();
    var contact = WLQuickApps.Tafiti.Scripting.MessengerManager.getMessengerContactByEmailHash(emailHash);
    if ((!contact) || !contact.get_currentAddress().get_isOnline()) {
        return;
    }
    var conversation = WLQuickApps.Tafiti.Scripting.MessengerManager.get_myMessengerController().get_loggedInUser().get_conversations().create(contact.get_currentAddress());
    conversation.sendMessage(new Microsoft.Live.Messenger.TextMessage(message, Microsoft.Live.Messenger.TextMessageFormat.get_defaultFormat()), null);
}
WLQuickApps.Tafiti.Scripting.MessengerManager.sendShelfStackInvite = function WLQuickApps_Tafiti_Scripting_MessengerManager$sendShelfStackInvite(emailHash, shelfStack) {
    WLQuickApps.Tafiti.Scripting.MessengerManager._verifyUserIsLoggedInToMessenger();
    var message = 'I have invited you to join the shelf stack \"' + shelfStack.label + '\" at ' + WLQuickApps.Tafiti.Scripting.Utilities.getSiteUrlRoot();
    var found = false;
    var $enum1 = shelfStack.get_owners().getEnumerator();
    while ($enum1.moveNext()) {
        var tafitiUser = $enum1.get_current();
        if (!tafitiUser.get_isOnline() || tafitiUser.get_isLoggedInUser()) {
            continue;
        }
        WLQuickApps.Tafiti.Scripting.MessengerManager.sendUpdateMessage(emailHash, shelfStack);
    }
    if (WLQuickApps.Tafiti.Scripting.MessengerManager.supportsTafitiMessages(emailHash)) {
        WLQuickApps.Tafiti.Scripting.MessengerManager.sendUpdateMessage(emailHash, shelfStack);
    }
    else {
        WLQuickApps.Tafiti.Scripting.MessengerManager.sendTextMessage(emailHash, message);
    }
}
WLQuickApps.Tafiti.Scripting.MessengerManager.isContact = function WLQuickApps_Tafiti_Scripting_MessengerManager$isContact(emailHash) {
    var $enum1 = WLQuickApps.Tafiti.Scripting.MessengerManager.get_myMessengerController().get_loggedInUser().get_contacts().getEnumerator();
    while ($enum1.moveNext()) {
        var contact = $enum1.get_current();
        if (WLQuickApps.Tafiti.Scripting.Utilities.hash(contact.get_currentAddress().get_address()) === emailHash) {
            return true;
        }
    }
    return false;
}
WLQuickApps.Tafiti.Scripting.MessengerManager.getDisplayName = function WLQuickApps_Tafiti_Scripting_MessengerManager$getDisplayName(emailHash) {
    var user = WLQuickApps.Tafiti.Scripting.TafitiUserManager.getUserByEmailHash(emailHash);
    if ((user) && (user.get_messengerAddress())) {
        return user.get_displayName();
    }
    return 'Unknown';
}
WLQuickApps.Tafiti.Scripting.MessengerManager.addContact = function WLQuickApps_Tafiti_Scripting_MessengerManager$addContact(emailHash) {
    var $enum1 = WLQuickApps.Tafiti.Scripting.MessengerManager.get_myMessengerController().get_loggedInUser().get_pendingContacts().getEnumerator();
    while ($enum1.moveNext()) {
        var pendingContact = $enum1.get_current();
        if (WLQuickApps.Tafiti.Scripting.Utilities.hash(pendingContact.get_imAddress().get_address()) === emailHash) {
            WLQuickApps.Tafiti.Scripting.MessengerManager.get_myMessengerController().addContact(pendingContact.get_imAddress().get_address());
            return;
        }
    }
    var $enum2 = WLQuickApps.Tafiti.Scripting.MessengerManager.get_myMessengerController().get_loggedInUser().get_conversations().getEnumerator();
    while ($enum2.moveNext()) {
        var conversation = $enum2.get_current();
        var $enum3 = conversation.get_roster().getEnumerator();
        while ($enum3.moveNext()) {
            var imAddress = $enum3.get_current();
            if (WLQuickApps.Tafiti.Scripting.Utilities.hash(imAddress.get_address()) === emailHash) {
                WLQuickApps.Tafiti.Scripting.MessengerManager.get_myMessengerController().addContact(imAddress.get_address());
                return;
            }
        }
    }
    var user = WLQuickApps.Tafiti.Scripting.TafitiUserManager.getUserByEmailHash(emailHash);
    if ((user) && (user.get_messengerAddress())) {
        WLQuickApps.Tafiti.Scripting.MessengerManager.get_myMessengerController().addContact(user.get_messengerAddress().get_address());
    }
}
WLQuickApps.Tafiti.Scripting.MessengerManager._verifyUserIsLoggedInToMessenger = function WLQuickApps_Tafiti_Scripting_MessengerManager$_verifyUserIsLoggedInToMessenger() {
    if (!WLQuickApps.Tafiti.Scripting.MessengerManager.get_myMessengerController().get_isSignedIn()) {
        throw new Error('You must be logged in to messenger to do this');
    }
}


////////////////////////////////////////////////////////////////////////////////
// WLQuickApps.Tafiti.Scripting.InteropManager

WLQuickApps.Tafiti.Scripting.InteropManager = function WLQuickApps_Tafiti_Scripting_InteropManager() {
}
WLQuickApps.Tafiti.Scripting.InteropManager.get_interop = function WLQuickApps_Tafiti_Scripting_InteropManager$get_interop() {
    if (!WLQuickApps.Tafiti.Scripting.InteropManager._updater) {
        WLQuickApps.Tafiti.Scripting.InteropManager._updater = new SJ.Interop();
    }
    return WLQuickApps.Tafiti.Scripting.InteropManager._updater;
}
WLQuickApps.Tafiti.Scripting.InteropManager.updateShelfStack = function WLQuickApps_Tafiti_Scripting_InteropManager$updateShelfStack(shelfStack) {
    WLQuickApps.Tafiti.Scripting.InteropManager.get_interop().updateShelfStack(shelfStack);
}
WLQuickApps.Tafiti.Scripting.InteropManager.popIMConsentDialog = function WLQuickApps_Tafiti_Scripting_InteropManager$popIMConsentDialog(pendingShelfStack) {
    WLQuickApps.Tafiti.Scripting.InteropManager.get_interop().popIMConsentDialog(pendingShelfStack);
}
WLQuickApps.Tafiti.Scripting.InteropManager.popAcceptContactDialog = function WLQuickApps_Tafiti_Scripting_InteropManager$popAcceptContactDialog(displayName, emailAddress, inviteMessage) {
    WLQuickApps.Tafiti.Scripting.InteropManager.get_interop().popAcceptContactDialog(displayName, emailAddress, inviteMessage);
}
WLQuickApps.Tafiti.Scripting.InteropManager.messengerStatusChanged = function WLQuickApps_Tafiti_Scripting_InteropManager$messengerStatusChanged(isSignedIn) {
    WLQuickApps.Tafiti.Scripting.InteropManager.get_interop().messengerStatusChanged(isSignedIn);
}
WLQuickApps.Tafiti.Scripting.InteropManager.onIncomingTextMessage = function WLQuickApps_Tafiti_Scripting_InteropManager$onIncomingTextMessage(emailhash, displayName, messageText) {
    WLQuickApps.Tafiti.Scripting.InteropManager.get_interop().onIncomingTextMessage(emailhash, displayName, messageText);
}


////////////////////////////////////////////////////////////////////////////////
// WLQuickApps.Tafiti.Scripting.ContactElement

WLQuickApps.Tafiti.Scripting.ContactElement = function WLQuickApps_Tafiti_Scripting_ContactElement(contact) {
    this._contact = contact;
    var tafitiUser = WLQuickApps.Tafiti.Scripting.TafitiUserManager.getUserByEmailHash(WLQuickApps.Tafiti.Scripting.Utilities.hash(contact.get_currentAddress().get_address()));
    this._visual = new SJ.ContactVisual(tafitiUser);
    this._contact.get_currentAddress().get_presence().add_propertyChanged(Delegate.create(this, this._propertyChanged));
}
WLQuickApps.Tafiti.Scripting.ContactElement.prototype = {
    _contact: null,
    _visual: null,
    
    get_emailHash: function WLQuickApps_Tafiti_Scripting_ContactElement$get_emailHash() {
        return WLQuickApps.Tafiti.Scripting.Utilities.hash(this._contact.get_currentAddress().get_address());
    },
    
    remove: function WLQuickApps_Tafiti_Scripting_ContactElement$remove() {
        this._visual.remove();
    },
    
    _propertyChanged: function WLQuickApps_Tafiti_Scripting_ContactElement$_propertyChanged(sender, e) {
        if (!String.isNullOrEmpty(this._contact.get_currentAddress().get_presence().get_displayName())) {
            this._visual.displayName = this._contact.get_currentAddress().get_presence().get_displayName();
        }
        else {
            this._visual.displayName = this._contact.get_currentAddress().get_presence().get_imAddress().get_address();
        }
        this._visual.isOnline = (this._contact.get_currentAddress().get_presence().get_status() !== Microsoft.Live.Messenger.PresenceStatus.offline);
        this._visual.updateUI();
    }
}


////////////////////////////////////////////////////////////////////////////////
// WLQuickApps.Tafiti.Scripting.TafitiUserManager

WLQuickApps.Tafiti.Scripting.TafitiUserManager = function WLQuickApps_Tafiti_Scripting_TafitiUserManager() {
}
WLQuickApps.Tafiti.Scripting.TafitiUserManager.get__usersCache = function WLQuickApps_Tafiti_Scripting_TafitiUserManager$get__usersCache() {
    if (!WLQuickApps.Tafiti.Scripting.TafitiUserManager._usersCache) {
        WLQuickApps.Tafiti.Scripting.TafitiUserManager._usersCache = {};
    }
    return WLQuickApps.Tafiti.Scripting.TafitiUserManager._usersCache;
}
WLQuickApps.Tafiti.Scripting.TafitiUserManager.get__emailHashCache = function WLQuickApps_Tafiti_Scripting_TafitiUserManager$get__emailHashCache() {
    if (!WLQuickApps.Tafiti.Scripting.TafitiUserManager._emailHashCache) {
        WLQuickApps.Tafiti.Scripting.TafitiUserManager._emailHashCache = {};
    }
    return WLQuickApps.Tafiti.Scripting.TafitiUserManager._emailHashCache;
}
WLQuickApps.Tafiti.Scripting.TafitiUserManager.get__outstandingUserRequests = function WLQuickApps_Tafiti_Scripting_TafitiUserManager$get__outstandingUserRequests() {
    if (!WLQuickApps.Tafiti.Scripting.TafitiUserManager._outstandingUserRequests) {
        WLQuickApps.Tafiti.Scripting.TafitiUserManager._outstandingUserRequests = [];
    }
    return WLQuickApps.Tafiti.Scripting.TafitiUserManager._outstandingUserRequests;
}
WLQuickApps.Tafiti.Scripting.TafitiUserManager.get__outstandingUserListRequests = function WLQuickApps_Tafiti_Scripting_TafitiUserManager$get__outstandingUserListRequests() {
    if (!WLQuickApps.Tafiti.Scripting.TafitiUserManager._outstandingUserListRequests) {
        WLQuickApps.Tafiti.Scripting.TafitiUserManager._outstandingUserListRequests = [];
    }
    return WLQuickApps.Tafiti.Scripting.TafitiUserManager._outstandingUserListRequests;
}
WLQuickApps.Tafiti.Scripting.TafitiUserManager.get_alwaysSendMessages = function WLQuickApps_Tafiti_Scripting_TafitiUserManager$get_alwaysSendMessages() {
    if (WLQuickApps.Tafiti.Scripting.TafitiUserManager._alwaysSendMessages) {
        return true;
    }
    var body = '';
    var request = new XMLHttpRequest();
    request.open('POST', WLQuickApps.Tafiti.Scripting.Utilities.getSiteUrlRoot() + '/SiteService.asmx/GetAlwaysSendMessages', false);
    request.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    request.setRequestHeader('Content-Length', body.length.toString());
    request.send(body);
    return Boolean.parse(request.responseXML.lastChild.text);
}
WLQuickApps.Tafiti.Scripting.TafitiUserManager.set_alwaysSendMessages = function WLQuickApps_Tafiti_Scripting_TafitiUserManager$set_alwaysSendMessages(value) {
    WLQuickApps.Tafiti.Scripting.TafitiUserManager._alwaysSendMessages = value;
    var body = 'alwaysSendMessages=' + value.toString();
    var request = new XMLHttpRequest();
    request.open('POST', WLQuickApps.Tafiti.Scripting.Utilities.getSiteUrlRoot() + '/SiteService.asmx/SetAlwaysSendMessages', false);
    request.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    request.setRequestHeader('Content-Length', body.length.toString());
    request.send(body);
    return value;
}
WLQuickApps.Tafiti.Scripting.TafitiUserManager.get_loggedInUserID = function WLQuickApps_Tafiti_Scripting_TafitiUserManager$get_loggedInUserID() {
    return WLQuickApps.Tafiti.Scripting.TafitiUserManager._loggedInUserID;
}
WLQuickApps.Tafiti.Scripting.TafitiUserManager.set_loggedInUserID = function WLQuickApps_Tafiti_Scripting_TafitiUserManager$set_loggedInUserID(value) {
    WLQuickApps.Tafiti.Scripting.TafitiUserManager._loggedInUserID = value;
    return value;
}
WLQuickApps.Tafiti.Scripting.TafitiUserManager.get_loggedInUser = function WLQuickApps_Tafiti_Scripting_TafitiUserManager$get_loggedInUser() {
    if (!WLQuickApps.Tafiti.Scripting.TafitiUserManager._loggedInUserID) {
        return null;
    }
    return WLQuickApps.Tafiti.Scripting.TafitiUserManager.getUser(WLQuickApps.Tafiti.Scripting.TafitiUserManager._loggedInUserID);
}
WLQuickApps.Tafiti.Scripting.TafitiUserManager.beginUserRequestByUserID = function WLQuickApps_Tafiti_Scripting_TafitiUserManager$beginUserRequestByUserID(userID) {
    if (Object.keyExists(WLQuickApps.Tafiti.Scripting.TafitiUserManager.get__usersCache(), userID)) {
        return;
    }
    var body = 'userID=' + userID;
    var request = new XMLHttpRequest();
    request.open('POST', WLQuickApps.Tafiti.Scripting.Utilities.getSiteUrlRoot() + '/SiteService.asmx/GetUser', true);
    request.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    request.setRequestHeader('Content-Length', body.length.toString());
    request.onreadystatechange = Delegate.create(null, WLQuickApps.Tafiti.Scripting.TafitiUserManager._endUserRequest);
    WLQuickApps.Tafiti.Scripting.TafitiUserManager.get__outstandingUserRequests().add(request);
    request.send(body);
}
WLQuickApps.Tafiti.Scripting.TafitiUserManager.beginUserRequestByEmailHash = function WLQuickApps_Tafiti_Scripting_TafitiUserManager$beginUserRequestByEmailHash(emailHash) {
    if (Object.keyExists(WLQuickApps.Tafiti.Scripting.TafitiUserManager.get__emailHashCache(), emailHash)) {
        return;
    }
    var body = 'emailHash=' + emailHash;
    var request = new XMLHttpRequest();
    request.open('POST', WLQuickApps.Tafiti.Scripting.Utilities.getSiteUrlRoot() + '/SiteService.asmx/GetUserByEmailHash', true);
    request.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    request.setRequestHeader('Content-Length', body.length.toString());
    request.onreadystatechange = Delegate.create(null, WLQuickApps.Tafiti.Scripting.TafitiUserManager._endUserRequest);
    WLQuickApps.Tafiti.Scripting.TafitiUserManager.get__outstandingUserRequests().add(request);
    request.send(body);
}
WLQuickApps.Tafiti.Scripting.TafitiUserManager._endUserRequest = function WLQuickApps_Tafiti_Scripting_TafitiUserManager$_endUserRequest() {
    for (var lcv = 0; lcv < WLQuickApps.Tafiti.Scripting.TafitiUserManager.get__outstandingUserRequests().length; lcv++) {
        var request = WLQuickApps.Tafiti.Scripting.TafitiUserManager.get__outstandingUserRequests()[lcv];
        if (request.readyState === 4) {
            WLQuickApps.Tafiti.Scripting.TafitiUserManager.get__outstandingUserRequests().removeAt(lcv);
            lcv--;
            var user = WLQuickApps.Tafiti.Scripting.TafitiUser.createFromXmlNode(request.responseXML.lastChild);
            WLQuickApps.Tafiti.Scripting.TafitiUserManager.cacheUser(user);
            return;
        }
    }
}
WLQuickApps.Tafiti.Scripting.TafitiUserManager.beginUserListRequestByEmailHash = function WLQuickApps_Tafiti_Scripting_TafitiUserManager$beginUserListRequestByEmailHash(emailHashes) {
    var body = 'emailHashes=' + emailHashes.join(',');
    var request = new XMLHttpRequest();
    request.open('POST', WLQuickApps.Tafiti.Scripting.Utilities.getSiteUrlRoot() + '/SiteService.asmx/GetUserListByEmailHash', true);
    request.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    request.setRequestHeader('Content-Length', body.length.toString());
    request.onreadystatechange = Delegate.create(null, WLQuickApps.Tafiti.Scripting.TafitiUserManager._endUserListRequest);
    WLQuickApps.Tafiti.Scripting.TafitiUserManager.get__outstandingUserRequests().add(request);
    request.send(body);
}
WLQuickApps.Tafiti.Scripting.TafitiUserManager._endUserListRequest = function WLQuickApps_Tafiti_Scripting_TafitiUserManager$_endUserListRequest() {
    for (var lcv = 0; lcv < WLQuickApps.Tafiti.Scripting.TafitiUserManager.get__outstandingUserRequests().length; lcv++) {
        var request = WLQuickApps.Tafiti.Scripting.TafitiUserManager.get__outstandingUserRequests()[lcv];
        if (request.readyState === 4) {
            WLQuickApps.Tafiti.Scripting.TafitiUserManager.get__outstandingUserRequests().removeAt(lcv);
            lcv--;
            var shelfStackNodeList = request.responseXML.getElementsByTagName('UserData');
            for (var index = 0; index < shelfStackNodeList.length; index++) {
                var tafitiUser = WLQuickApps.Tafiti.Scripting.TafitiUser.createFromXmlNode(shelfStackNodeList[index]);
                WLQuickApps.Tafiti.Scripting.TafitiUserManager.cacheUser(tafitiUser);
            }
        }
    }
}
WLQuickApps.Tafiti.Scripting.TafitiUserManager.getUserListRequestByEmailHash = function WLQuickApps_Tafiti_Scripting_TafitiUserManager$getUserListRequestByEmailHash(emailHashes) {
    var missingUsers = [];
    var $enum1 = emailHashes.getEnumerator();
    while ($enum1.moveNext()) {
        var emailHash = $enum1.get_current();
        if (!Object.keyExists(WLQuickApps.Tafiti.Scripting.TafitiUserManager.get__emailHashCache(), emailHash)) {
            missingUsers.add(emailHash);
        }
    }
    if (!missingUsers.length) {
        return;
    }
    var body = 'emailHashes=' + missingUsers.join(',');
    var request = new XMLHttpRequest();
    request.open('POST', WLQuickApps.Tafiti.Scripting.Utilities.getSiteUrlRoot() + '/SiteService.asmx/GetUserListByEmailHash', false);
    request.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    request.setRequestHeader('Content-Length', body.length.toString());
    request.send(body);
    var shelfStackNodeList = request.responseXML.getElementsByTagName('UserData');
    for (var index = 0; index < shelfStackNodeList.length; index++) {
        var tafitiUser = WLQuickApps.Tafiti.Scripting.TafitiUser.createFromXmlNode(shelfStackNodeList[index]);
        WLQuickApps.Tafiti.Scripting.TafitiUserManager.cacheUser(tafitiUser);
    }
}
WLQuickApps.Tafiti.Scripting.TafitiUserManager.updateUserDetails = function WLQuickApps_Tafiti_Scripting_TafitiUserManager$updateUserDetails(displayName, email) {
    var body = 'displayName=' + escape(displayName) + '&emailHash=' + escape(WLQuickApps.Tafiti.Scripting.Utilities.hash(email));
    var request = new XMLHttpRequest();
    request.open('POST', WLQuickApps.Tafiti.Scripting.Utilities.getSiteUrlRoot() + '/SiteService.asmx/UpdateUserDetails', false);
    request.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    request.setRequestHeader('Content-Length', body.length.toString());
    request.send(body);
}
WLQuickApps.Tafiti.Scripting.TafitiUserManager.getUserByEmailHash = function WLQuickApps_Tafiti_Scripting_TafitiUserManager$getUserByEmailHash(emailHash) {
    if (Object.keyExists(WLQuickApps.Tafiti.Scripting.TafitiUserManager.get__emailHashCache(), emailHash)) {
        return WLQuickApps.Tafiti.Scripting.TafitiUserManager.get__emailHashCache()[emailHash];
    }
    var body = 'emailHash=' + emailHash;
    var request = new XMLHttpRequest();
    request.open('POST', WLQuickApps.Tafiti.Scripting.Utilities.getSiteUrlRoot() + '/SiteService.asmx/GetUserByEmailHash', false);
    request.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    request.setRequestHeader('Content-Length', body.length.toString());
    request.send(body);
    var user;
    if (request.responseXML.lastChild.text !== '') {
        user = WLQuickApps.Tafiti.Scripting.TafitiUser.createFromXmlNode(request.responseXML.lastChild);
    }
    else {
        user = new WLQuickApps.Tafiti.Scripting.TafitiUser();
        user.emailHash = emailHash;
        user.userID = emailHash;
    }
    WLQuickApps.Tafiti.Scripting.TafitiUserManager.cacheUser(user);
    return user;
}
WLQuickApps.Tafiti.Scripting.TafitiUserManager.getUser = function WLQuickApps_Tafiti_Scripting_TafitiUserManager$getUser(userID) {
    if (Object.keyExists(WLQuickApps.Tafiti.Scripting.TafitiUserManager.get__usersCache(), userID)) {
        return WLQuickApps.Tafiti.Scripting.TafitiUserManager.get__usersCache()[userID];
    }
    var body = 'userID=' + userID;
    var request = new XMLHttpRequest();
    request.open('POST', WLQuickApps.Tafiti.Scripting.Utilities.getSiteUrlRoot() + '/SiteService.asmx/GetUser', false);
    request.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    request.setRequestHeader('Content-Length', body.length.toString());
    request.send(body);
    var user = WLQuickApps.Tafiti.Scripting.TafitiUser.createFromXmlNode(request.responseXML.lastChild);
    WLQuickApps.Tafiti.Scripting.TafitiUserManager.cacheUser(user);
    return user;
}
WLQuickApps.Tafiti.Scripting.TafitiUserManager.cacheUser = function WLQuickApps_Tafiti_Scripting_TafitiUserManager$cacheUser(tafitiUser) {
    if (!Object.keyExists(WLQuickApps.Tafiti.Scripting.TafitiUserManager.get__usersCache(), tafitiUser.userID)) {
        WLQuickApps.Tafiti.Scripting.TafitiUserManager.get__usersCache()[tafitiUser.userID] = tafitiUser;
        WLQuickApps.Tafiti.Scripting.TafitiUserManager.get__emailHashCache()[tafitiUser.emailHash] = tafitiUser;
    }
}


////////////////////////////////////////////////////////////////////////////////
// WLQuickApps.Tafiti.Scripting.TafitiUpdateMessageFactory

WLQuickApps.Tafiti.Scripting.TafitiUpdateMessageFactory = function WLQuickApps_Tafiti_Scripting_TafitiUpdateMessageFactory() {
    WLQuickApps.Tafiti.Scripting.TafitiUpdateMessageFactory.constructBase(this, [ WLQuickApps.Tafiti.Scripting.MessengerManager.get_myMessengerController().get_loggedInUser() ]);
}
WLQuickApps.Tafiti.Scripting.TafitiUpdateMessageFactory.prototype = {
    
    deserialize: function WLQuickApps_Tafiti_Scripting_TafitiUpdateMessageFactory$deserialize(sender, id, content) {
        return new WLQuickApps.Tafiti.Scripting.TafitiUpdateMessage(content);
    },
    
    serialize: function WLQuickApps_Tafiti_Scripting_TafitiUpdateMessageFactory$serialize(message) {
        return (message).shelfStackID;
    }
}


////////////////////////////////////////////////////////////////////////////////
// WLQuickApps.Tafiti.Scripting.TafitiUpdateMessage

WLQuickApps.Tafiti.Scripting.TafitiUpdateMessage = function WLQuickApps_Tafiti_Scripting_TafitiUpdateMessage(shelfStackID) {
    WLQuickApps.Tafiti.Scripting.TafitiUpdateMessage.constructBase(this);
    this.shelfStackID = shelfStackID;
}
WLQuickApps.Tafiti.Scripting.TafitiUpdateMessage.prototype = {
    shelfStackID: null,
    
    get_id: function WLQuickApps_Tafiti_Scripting_TafitiUpdateMessage$get_id() {
        return WLQuickApps.Tafiti.Scripting.Constants.tafitiUpdateMessageID;
    }
}


////////////////////////////////////////////////////////////////////////////////
// WLQuickApps.Tafiti.Scripting.MessengerController

WLQuickApps.Tafiti.Scripting.MessengerController = function WLQuickApps_Tafiti_Scripting_MessengerController() {
    if (WLQuickApps.Tafiti.Scripting.Utilities.userIsLoggedIn()) {
        this._contacts = [];
        this._contactLookup = {};
        window.attachEvent('onbeforeunload', Delegate.create(this, this.document_Unload));
        this._signInControl = new Microsoft.Live.Messenger.UI.SignInControl(WLQuickApps.Tafiti.Scripting.Constants.signInFrameID, WLQuickApps.Tafiti.Scripting.Utilities.getSiteUrlRoot() + '/privacy.html', WLQuickApps.Tafiti.Scripting.Utilities.getSiteUrlRoot() + '/Channel.html', '');
        this._signInControl.add_authenticationCompleted(Delegate.create(this, this.authenticationCompleted));
    }
}
WLQuickApps.Tafiti.Scripting.MessengerController.prototype = {
    _localUser: null,
    _signInControl: null,
    _contacts: null,
    _contactLookup: null,
    
    get_isSignedIn: function WLQuickApps_Tafiti_Scripting_MessengerController$get_isSignedIn() {
        return ((this._localUser) && (this._localUser.get_address().get_presence().get_status() === Microsoft.Live.Messenger.PresenceStatus.online));
    },
    
    get_loggedInUser: function WLQuickApps_Tafiti_Scripting_MessengerController$get_loggedInUser() {
        return this._localUser;
    },
    
    addContact: function WLQuickApps_Tafiti_Scripting_MessengerController$addContact(address) {
        this._localUser.addContact(address, 'I have added you as a contact through Tafiti.', null);
    },
    
    authenticationCompleted: function WLQuickApps_Tafiti_Scripting_MessengerController$authenticationCompleted(sender, e) {
        this._localUser = new Microsoft.Live.Messenger.User(e.get_identity());
        this._localUser.add_signInCompleted(Delegate.create(this, this.signInCompleted));
        this._localUser.signIn(null);
    },
    
    signInCompleted: function WLQuickApps_Tafiti_Scripting_MessengerController$signInCompleted(sender, e) {
        if (e.get_resultCode() === Microsoft.Live.Messenger.SignInResultCode.success) {
            this._localUser.get_address().get_presence().set_status(Microsoft.Live.Messenger.PresenceStatus.online);
            this._localUser.get_presence().add_propertyChanged(Delegate.create(this, this.presencePropertyChanged));
            var emailHashes = [];
            var $enum1 = this._localUser.get_contacts().getEnumerator();
            while ($enum1.moveNext()) {
                var contact = $enum1.get_current();
                emailHashes.add(WLQuickApps.Tafiti.Scripting.Utilities.hash(contact.get_currentAddress().get_address()));
            }
            WLQuickApps.Tafiti.Scripting.TafitiUserManager.getUserListRequestByEmailHash(emailHashes.extract(0));
            var $enum2 = this._localUser.get_contacts().getEnumerator();
            while ($enum2.moveNext()) {
                var contact = $enum2.get_current();
                var contactHash = WLQuickApps.Tafiti.Scripting.Utilities.hash(contact.get_currentAddress().get_address());
                if (!Object.keyExists(this._contactLookup, contactHash)) {
                    var contactElement = new WLQuickApps.Tafiti.Scripting.ContactElement(contact);
                    this._contacts.add(contactElement);
                    this._contactLookup[contactHash] = contactElement;
                }
            }
            if (!this._localUser.get_messageFactory()) {
                this._localUser.set_messageFactory(new WLQuickApps.Tafiti.Scripting.TafitiUpdateMessageFactory());
                if (!this._localUser.get_messageFactory().isRegistered(WLQuickApps.Tafiti.Scripting.Constants.tafitiUpdateMessageID)) {
                    this._localUser.get_messageFactory().register(WLQuickApps.Tafiti.Scripting.Constants.tafitiUpdateMessageID);
                }
            }
            this._localUser.get_contacts().add_collectionChanged(Delegate.create(this, this.contacts_CollectionChanged));
            this._localUser.add_signOutCompleted(Delegate.create(this, this._localUser_SignOutCompleted));
            this._localUser.get_pendingContacts().add_collectionChanged(Delegate.create(this, this.pendingContacts_CollectionChanged));
            this._localUser.get_conversations().add_collectionChanged(Delegate.create(this, this.conversations_CollectionChanged));
            WLQuickApps.Tafiti.Scripting.InteropManager.messengerStatusChanged(true);
        }
        else {
        }
    },
    
    pendingContacts_CollectionChanged: function WLQuickApps_Tafiti_Scripting_MessengerController$pendingContacts_CollectionChanged(sender, e) {
        for (var lcv = e.get_newStartingIndex(); lcv < e.get_newItems().length; lcv++) {
            var pendingContact = e.get_newItems()[lcv];
            WLQuickApps.Tafiti.Scripting.InteropManager.popAcceptContactDialog(pendingContact.get_imAddress().get_presence().get_displayName(), pendingContact.get_imAddress().get_address(), pendingContact.get_inviteMessage());
        }
    },
    
    document_Unload: function WLQuickApps_Tafiti_Scripting_MessengerController$document_Unload() {
        if (this.get_isSignedIn()) {
            this._localUser.signOut(Microsoft.Live.Messenger.SignOutLocations.local, false);
        }
    },
    
    conversations_CollectionChanged: function WLQuickApps_Tafiti_Scripting_MessengerController$conversations_CollectionChanged(sender, e) {
        for (var lcv = 0; lcv < e.get_newItems().length; lcv++) {
            var conversation = e.get_newItems()[lcv];
            conversation.add_messageReceived(Delegate.create(this, this.conversation_MessageReceived));
        }
    },
    
    conversation_MessageReceived: function WLQuickApps_Tafiti_Scripting_MessengerController$conversation_MessageReceived(sender, e) {
        switch (e.get_message().get_type()) {
            case Microsoft.Live.Messenger.MessageType.applicationMessage:
                var updateMessage = e.get_message();
                if (updateMessage) {
                    WLQuickApps.Tafiti.Scripting.ShelfStackManager.updateShelfStack(updateMessage.shelfStackID);
                }
                break;
            case Microsoft.Live.Messenger.MessageType.nudgeMessage:
                break;
            case Microsoft.Live.Messenger.MessageType.textMessage:
                var textMessage = e.get_message();
                var displayName = textMessage.get_sender().get_presence().get_displayName();
                if (String.isNullOrEmpty(displayName)) {
                    displayName = textMessage.get_sender().get_presence().get_imAddress().get_address();
                }
                WLQuickApps.Tafiti.Scripting.InteropManager.onIncomingTextMessage(WLQuickApps.Tafiti.Scripting.Utilities.hash(textMessage.get_sender().get_address()), displayName, textMessage.get_text());
                break;
            default:
                throw new Error('Unknown message type');
        }
    },
    
    contacts_CollectionChanged: function WLQuickApps_Tafiti_Scripting_MessengerController$contacts_CollectionChanged(sender, e) {
        for (var lcv = 0; lcv < e.get_newItems().length; lcv++) {
            var contact = e.get_newItems()[lcv];
            var contactHash = WLQuickApps.Tafiti.Scripting.Utilities.hash(contact.get_currentAddress().get_address());
            var contactElement = this._contactLookup[contactHash];
            if (!contactElement) {
                contactElement = new WLQuickApps.Tafiti.Scripting.ContactElement(contact);
                this._contacts.add(contactElement);
                this._contactLookup[contactHash] = contactElement;
            }
        }
    },
    
    _localUser_SignOutCompleted: function WLQuickApps_Tafiti_Scripting_MessengerController$_localUser_SignOutCompleted(sender, e) {
        if (this._localUser) {
            this._localUser.dispose();
            this._localUser = null;
        }
        WLQuickApps.Tafiti.Scripting.InteropManager.messengerStatusChanged(false);
        window.navigate(window.location.href);
    },
    
    presencePropertyChanged: function WLQuickApps_Tafiti_Scripting_MessengerController$presencePropertyChanged(sender, e) {
        var presence = sender;
        if ((e.get_propertyName() === 'Status') && (presence.get_imAddress().get_address() === this._localUser.get_address().get_address())) {
            WLQuickApps.Tafiti.Scripting.InteropManager.messengerStatusChanged(false);
        }
        else {
            WLQuickApps.Tafiti.Scripting.TafitiUserManager.updateUserDetails(presence.get_displayName(), presence.get_imAddress().get_address());
            WLQuickApps.Tafiti.Scripting.ShelfStackManager.beginGetMasterData();
        }
    }
}


////////////////////////////////////////////////////////////////////////////////
// WLQuickApps.Tafiti.Scripting.Comment

WLQuickApps.Tafiti.Scripting.Comment = function WLQuickApps_Tafiti_Scripting_Comment() {
}
WLQuickApps.Tafiti.Scripting.Comment.createFromXmlNode = function WLQuickApps_Tafiti_Scripting_Comment$createFromXmlNode(node) {
    var comment = new WLQuickApps.Tafiti.Scripting.Comment();
    for (var lcv = 0; lcv < node.childNodes.length; lcv++) {
        var childNode = node.childNodes[lcv];
        if (String.isNullOrEmpty(childNode.baseName)) {
            continue;
        }
        switch (childNode.baseName.toLowerCase()) {
            case 'commentid':
                comment.commentID = childNode.text;
                break;
            case 'shelfstackid':
                comment.shelfStackID = childNode.text;
                break;
            case 'timestamp':
                comment.timestamp = new Date(childNode.text);
                break;
            case 'text':
                comment.text = childNode.text;
                break;
            case 'userid':
                comment._ownerID = childNode.text;
                WLQuickApps.Tafiti.Scripting.TafitiUserManager.beginUserRequestByUserID(comment._ownerID);
                break;
            default:
                break;
        }
    }
    return comment;
}
WLQuickApps.Tafiti.Scripting.Comment.prototype = {
    commentID: null,
    shelfStackID: null,
    timestamp: null,
    text: null,
    
    get_owner: function WLQuickApps_Tafiti_Scripting_Comment$get_owner() {
        return WLQuickApps.Tafiti.Scripting.TafitiUserManager.getUser(this._ownerID);
    },
    
    _ownerID: null
}


////////////////////////////////////////////////////////////////////////////////
// WLQuickApps.Tafiti.Scripting.CommentManager

WLQuickApps.Tafiti.Scripting.CommentManager = function WLQuickApps_Tafiti_Scripting_CommentManager() {
}
WLQuickApps.Tafiti.Scripting.CommentManager.get__cache = function WLQuickApps_Tafiti_Scripting_CommentManager$get__cache() {
    if (!WLQuickApps.Tafiti.Scripting.CommentManager._cache) {
        WLQuickApps.Tafiti.Scripting.CommentManager._cache = {};
    }
    return WLQuickApps.Tafiti.Scripting.CommentManager._cache;
}
WLQuickApps.Tafiti.Scripting.CommentManager.get__outstandingShelfStackCommentRequests = function WLQuickApps_Tafiti_Scripting_CommentManager$get__outstandingShelfStackCommentRequests() {
    if (!WLQuickApps.Tafiti.Scripting.CommentManager._outstandingShelfStackCommentRequests) {
        WLQuickApps.Tafiti.Scripting.CommentManager._outstandingShelfStackCommentRequests = [];
    }
    return WLQuickApps.Tafiti.Scripting.CommentManager._outstandingShelfStackCommentRequests;
}
WLQuickApps.Tafiti.Scripting.CommentManager.get__outstandingCommentRequests = function WLQuickApps_Tafiti_Scripting_CommentManager$get__outstandingCommentRequests() {
    if (!WLQuickApps.Tafiti.Scripting.CommentManager._outstandingCommentRequests) {
        WLQuickApps.Tafiti.Scripting.CommentManager._outstandingCommentRequests = [];
    }
    return WLQuickApps.Tafiti.Scripting.CommentManager._outstandingCommentRequests;
}
WLQuickApps.Tafiti.Scripting.CommentManager.beginGetCommentsForShelfStack = function WLQuickApps_Tafiti_Scripting_CommentManager$beginGetCommentsForShelfStack(shelfStackID) {
    var body = 'shelfStackID=' + shelfStackID;
    var request = new XMLHttpRequest();
    request.open('POST', WLQuickApps.Tafiti.Scripting.Utilities.getSiteUrlRoot() + '/SiteService.asmx/GetConversation', true);
    request.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    request.setRequestHeader('Content-Length', body.length.toString());
    request.onreadystatechange = Delegate.create(null, WLQuickApps.Tafiti.Scripting.CommentManager._endGetCommentsForShelfStack);
    WLQuickApps.Tafiti.Scripting.CommentManager.get__outstandingShelfStackCommentRequests().add(request);
    request.send(body);
}
WLQuickApps.Tafiti.Scripting.CommentManager._endGetCommentsForShelfStack = function WLQuickApps_Tafiti_Scripting_CommentManager$_endGetCommentsForShelfStack() {
    for (var lcv = 0; lcv < WLQuickApps.Tafiti.Scripting.CommentManager.get__outstandingShelfStackCommentRequests().length; lcv++) {
        var request = WLQuickApps.Tafiti.Scripting.CommentManager.get__outstandingShelfStackCommentRequests()[lcv];
        if (request.readyState === 4) {
            WLQuickApps.Tafiti.Scripting.CommentManager.get__outstandingShelfStackCommentRequests().removeAt(lcv);
            lcv--;
            var commentNodeList = request.responseXML.getElementsByTagName('string');
            if ((!commentNodeList) || ((commentNodeList.length === 1) && (commentNodeList[0].text === String.Empty))) {
                return;
            }
            for (var lcv2 = 0; lcv2 < commentNodeList.length; lcv2++) {
                if (!Object.keyExists(WLQuickApps.Tafiti.Scripting.CommentManager.get__cache(), commentNodeList[lcv2].text)) {
                    WLQuickApps.Tafiti.Scripting.CommentManager.beginCommentRequest(commentNodeList[lcv2].text);
                }
            }
        }
    }
}
WLQuickApps.Tafiti.Scripting.CommentManager.beginCommentRequest = function WLQuickApps_Tafiti_Scripting_CommentManager$beginCommentRequest(commentID) {
    if (Object.keyExists(WLQuickApps.Tafiti.Scripting.CommentManager.get__cache(), commentID)) {
        return;
    }
    var body = 'commentID=' + commentID;
    var request = new XMLHttpRequest();
    request.open('POST', WLQuickApps.Tafiti.Scripting.Utilities.getSiteUrlRoot() + '/SiteService.asmx/GetComment', true);
    request.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    request.setRequestHeader('Content-Length', body.length.toString());
    request.onreadystatechange = Delegate.create(null, WLQuickApps.Tafiti.Scripting.CommentManager._endCommentRequest);
    WLQuickApps.Tafiti.Scripting.CommentManager.get__outstandingCommentRequests().add(request);
    request.send(body);
}
WLQuickApps.Tafiti.Scripting.CommentManager._endCommentRequest = function WLQuickApps_Tafiti_Scripting_CommentManager$_endCommentRequest() {
    for (var lcv = 0; lcv < WLQuickApps.Tafiti.Scripting.CommentManager.get__outstandingCommentRequests().length; lcv++) {
        var request = WLQuickApps.Tafiti.Scripting.CommentManager.get__outstandingCommentRequests()[lcv];
        if (request.readyState === 4) {
            WLQuickApps.Tafiti.Scripting.CommentManager.get__outstandingCommentRequests().removeAt(lcv);
            lcv--;
            var comment = WLQuickApps.Tafiti.Scripting.Comment.createFromXmlNode(request.responseXML.lastChild);
            WLQuickApps.Tafiti.Scripting.CommentManager.get__cache()[comment.commentID] = comment;
            return;
        }
    }
}
WLQuickApps.Tafiti.Scripting.CommentManager.getCommentsForShelfStack = function WLQuickApps_Tafiti_Scripting_CommentManager$getCommentsForShelfStack(shelfStackID) {
    var body = 'shelfStackID=' + shelfStackID;
    var request = new XMLHttpRequest();
    request.open('POST', WLQuickApps.Tafiti.Scripting.Utilities.getSiteUrlRoot() + '/SiteService.asmx/GetConversation', false);
    request.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    request.setRequestHeader('Content-Length', body.length.toString());
    request.send(body);
    var commentNodeList = request.responseXML.getElementsByTagName('string');
    if ((!commentNodeList) || ((commentNodeList.length === 1) && (commentNodeList[0].text === String.Empty))) {
        return new Array(0);
    }
    var comments = new Array(commentNodeList.length);
    for (var lcv = 0; lcv < commentNodeList.length; lcv++) {
        comments[lcv] = WLQuickApps.Tafiti.Scripting.CommentManager.getComment(commentNodeList[lcv].text);
    }
    return comments;
}
WLQuickApps.Tafiti.Scripting.CommentManager.addComment = function WLQuickApps_Tafiti_Scripting_CommentManager$addComment(shelfStackID, text) {
    var shelfStack = WLQuickApps.Tafiti.Scripting.ShelfStackManager.getShelfStack(shelfStackID);
    var body = 'shelfStackID=' + shelfStackID + '&text=' + escape(text);
    var request = new XMLHttpRequest();
    request.open('POST', WLQuickApps.Tafiti.Scripting.Utilities.getSiteUrlRoot() + '/SiteService.asmx/AddComment', false);
    request.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    request.setRequestHeader('Content-Length', body.length.toString());
    request.send(body);
    WLQuickApps.Tafiti.Scripting.ShelfStackManager.sendShelfStackUpdate(shelfStack);
}
WLQuickApps.Tafiti.Scripting.CommentManager.getComment = function WLQuickApps_Tafiti_Scripting_CommentManager$getComment(commentID) {
    if (Object.keyExists(WLQuickApps.Tafiti.Scripting.CommentManager.get__cache(), commentID)) {
        return WLQuickApps.Tafiti.Scripting.CommentManager.get__cache()[commentID];
    }
    var body = 'commentID=' + commentID;
    var request = new XMLHttpRequest();
    request.open('POST', WLQuickApps.Tafiti.Scripting.Utilities.getSiteUrlRoot() + '/SiteService.asmx/GetComment', false);
    request.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    request.setRequestHeader('Content-Length', body.length.toString());
    request.send(body);
    var comment = WLQuickApps.Tafiti.Scripting.Comment.createFromXmlNode(request.responseXML.lastChild);
    WLQuickApps.Tafiti.Scripting.CommentManager.get__cache()[commentID] = comment;
    return comment;
}
WLQuickApps.Tafiti.Scripting.CommentManager.cacheComment = function WLQuickApps_Tafiti_Scripting_CommentManager$cacheComment(comment) {
    if (!Object.keyExists(WLQuickApps.Tafiti.Scripting.CommentManager.get__cache(), comment.commentID)) {
        WLQuickApps.Tafiti.Scripting.CommentManager.get__cache()[comment.commentID] = comment;
    }
}


////////////////////////////////////////////////////////////////////////////////
// WLQuickApps.Tafiti.Scripting.TafitiUser

WLQuickApps.Tafiti.Scripting.TafitiUser = function WLQuickApps_Tafiti_Scripting_TafitiUser() {
}
WLQuickApps.Tafiti.Scripting.TafitiUser.createFromXmlNode = function WLQuickApps_Tafiti_Scripting_TafitiUser$createFromXmlNode(node) {
    var user = new WLQuickApps.Tafiti.Scripting.TafitiUser();
    for (var lcv = 0; lcv < node.childNodes.length; lcv++) {
        var childNode = node.childNodes[lcv];
        if (String.isNullOrEmpty(childNode.baseName)) {
            continue;
        }
        switch (childNode.baseName.toLowerCase()) {
            case 'userid':
                user.userID = childNode.text;
                break;
            case 'displayname':
                user._displayName = childNode.text;
                break;
            case 'emailhash':
                user.emailHash = childNode.text;
                break;
            case 'messengerpresenceid':
                user.messengerPresenceID = childNode.text;
                break;
            default:
                break;
        }
    }
    return user;
}
WLQuickApps.Tafiti.Scripting.TafitiUser.prototype = {
    userID: null,
    emailHash: null,
    messengerPresenceID: null,
    
    get_displayName: function WLQuickApps_Tafiti_Scripting_TafitiUser$get_displayName() {
        if ((this.get_messengerAddress()) && !String.isNullOrEmpty(this.get_messengerAddress().get_presence().get_displayName())) {
            if (!String.isNullOrEmpty(this.get_messengerAddress().get_presence().get_displayName())) {
                return this.get_messengerAddress().get_presence().get_displayName();
            }
            return this.get_messengerAddress().get_presence().get_imAddress().get_address();
        }
        return this._displayName;
    },
    
    _displayName: null,
    
    get_messengerAddress: function WLQuickApps_Tafiti_Scripting_TafitiUser$get_messengerAddress() {
        if (!WLQuickApps.Tafiti.Scripting.MessengerManager.get_myMessengerController().get_isSignedIn()) {
            this._messengerAddress = null;
        }
        if ((!this._messengerAddress) && WLQuickApps.Tafiti.Scripting.MessengerManager.get_myMessengerController().get_isSignedIn()) {
            if (!String.compare(this.emailHash, WLQuickApps.Tafiti.Scripting.Utilities.hash(WLQuickApps.Tafiti.Scripting.MessengerManager.get_myMessengerController().get_loggedInUser().get_address().get_address()), true)) {
                this._messengerAddress = WLQuickApps.Tafiti.Scripting.MessengerManager.get_myMessengerController().get_loggedInUser().get_address();
            }
            else {
                var $enum1 = WLQuickApps.Tafiti.Scripting.MessengerManager.get_myMessengerController().get_loggedInUser().get_contacts().getEnumerator();
                while ($enum1.moveNext()) {
                    var contact = $enum1.get_current();
                    if (!String.compare(this.emailHash, WLQuickApps.Tafiti.Scripting.Utilities.hash(contact.get_currentAddress().get_address()), true)) {
                        this._messengerAddress = contact.get_currentAddress();
                        break;
                    }
                }
            }
        }
        return this._messengerAddress;
    },
    
    _messengerAddress: null,
    
    get_isLoggedInUser: function WLQuickApps_Tafiti_Scripting_TafitiUser$get_isLoggedInUser() {
        return ((WLQuickApps.Tafiti.Scripting.TafitiUserManager.get_loggedInUser()) && (this.emailHash === WLQuickApps.Tafiti.Scripting.TafitiUserManager.get_loggedInUser().emailHash));
    },
    
    get_isOnline: function WLQuickApps_Tafiti_Scripting_TafitiUser$get_isOnline() {
        return ((this.get_messengerAddress()) && this.get_messengerAddress().get_isOnline());
    }
}


////////////////////////////////////////////////////////////////////////////////
// WLQuickApps.Tafiti.Scripting.EntryPoint

WLQuickApps.Tafiti.Scripting.EntryPoint = function WLQuickApps_Tafiti_Scripting_EntryPoint() {
}
WLQuickApps.Tafiti.Scripting.EntryPoint.main = function WLQuickApps_Tafiti_Scripting_EntryPoint$main(arguments) {
    if (WLQuickApps.Tafiti.Scripting.Utilities.userIsLoggedIn()) {
        WLQuickApps.Tafiti.Scripting.ShelfStackManager.beginGetMasterData();
    }
}
WLQuickApps.Tafiti.Scripting.EntryPoint.start = function WLQuickApps_Tafiti_Scripting_EntryPoint$start() {
    if (WLQuickApps.Tafiti.Scripting.Utilities.userIsLoggedIn()) {
        WLQuickApps.Tafiti.Scripting.MessengerManager.startMessenger();
    }
}


////////////////////////////////////////////////////////////////////////////////
// WLQuickApps.Tafiti.Scripting.ShelfStack

WLQuickApps.Tafiti.Scripting.ShelfStack = function WLQuickApps_Tafiti_Scripting_ShelfStack() {
}
WLQuickApps.Tafiti.Scripting.ShelfStack.createFromXmlNode = function WLQuickApps_Tafiti_Scripting_ShelfStack$createFromXmlNode(shelfStackNode) {
    var shelfStack = new WLQuickApps.Tafiti.Scripting.ShelfStack();
    for (var lcv = 0; lcv < shelfStackNode.childNodes.length; lcv++) {
        var childNode = shelfStackNode.childNodes[lcv];
        if (String.isNullOrEmpty(childNode.baseName)) {
            continue;
        }
        var list;
        switch (childNode.baseName.toLowerCase()) {
            case 'label':
                shelfStack.label = childNode.text;
                break;
            case 'shelfstackid':
                shelfStack.shelfStackID = childNode.text;
                break;
            case 'lastmodifiedtimestamp':
                shelfStack.lastModifiedTimestamp = new Date(childNode.text);
                break;
            case 'shelfstackitemids':
                list = [];
                for (var id = 0; id < childNode.childNodes.length; id++) {
                    if (String.isNullOrEmpty(childNode.childNodes[id].baseName)) {
                        continue;
                    }
                    list.add(WLQuickApps.Tafiti.Scripting.ShelfStackManager.getShelfStackItem(childNode.childNodes[id].text));
                }
                shelfStack.shelfStackItems = list.extract(0, list.length);
                break;
            case 'owneremailhashes':
                list = [];
                for (var id = 0; id < childNode.childNodes.length; id++) {
                    if (String.isNullOrEmpty(childNode.childNodes[id].baseName)) {
                        continue;
                    }
                    list.add(childNode.childNodes[id].text);
                    WLQuickApps.Tafiti.Scripting.TafitiUserManager.beginUserRequestByEmailHash(childNode.childNodes[id].text);
                }
                shelfStack._ownerEmailHashes = list.extract(0, list.length);
                break;
            default:
                break;
        }
    }
    return shelfStack;
}
WLQuickApps.Tafiti.Scripting.ShelfStack.prototype = {
    shelfStackID: null,
    label: null,
    shelfStackItems: null,
    
    get_owners: function WLQuickApps_Tafiti_Scripting_ShelfStack$get_owners() {
        if (!this._owners) {
            this._owners = new Array(this._ownerEmailHashes.length);
            for (var lcv = 0; lcv < this._ownerEmailHashes.length; lcv++) {
                this._owners[lcv] = WLQuickApps.Tafiti.Scripting.TafitiUserManager.getUserByEmailHash(this._ownerEmailHashes[lcv]);
            }
        }
        return this._owners;
    },
    
    _owners: null,
    _ownerEmailHashes: null,
    lastModifiedTimestamp: null,
    
    addOwner: function WLQuickApps_Tafiti_Scripting_ShelfStack$addOwner(user) {
        this._owners = this.get_owners().concat([ user ]);
    }
}


////////////////////////////////////////////////////////////////////////////////
// WLQuickApps.Tafiti.Scripting.ShelfStackItem

WLQuickApps.Tafiti.Scripting.ShelfStackItem = function WLQuickApps_Tafiti_Scripting_ShelfStackItem() {
}
WLQuickApps.Tafiti.Scripting.ShelfStackItem.createFromXmlNode = function WLQuickApps_Tafiti_Scripting_ShelfStackItem$createFromXmlNode(shelfStackItemNode) {
    var shelfStackItem = new WLQuickApps.Tafiti.Scripting.ShelfStackItem();
    for (var lcv = 0; lcv < shelfStackItemNode.childNodes.length; lcv++) {
        var childNode = shelfStackItemNode.childNodes[lcv];
        if (String.isNullOrEmpty(childNode.baseName)) {
            continue;
        }
        switch (childNode.baseName.toLowerCase()) {
            case 'shelfstackitemid':
                shelfStackItem.shelfStackItemID = childNode.text;
                break;
            case 'shelfstackid':
                shelfStackItem.shelfStackID = childNode.text;
                break;
            case 'domain':
                shelfStackItem.domain = childNode.text;
                break;
            case 'title':
                shelfStackItem.title = childNode.text;
                break;
            case 'description':
                shelfStackItem.description = childNode.text;
                break;
            case 'url':
                shelfStackItem.url = childNode.text;
                break;
            case 'userid':
                shelfStackItem.owner = WLQuickApps.Tafiti.Scripting.TafitiUserManager.getUser(childNode.text);
                break;
            case 'timestamp':
                shelfStackItem.lastModifiedTimestamp = new Date(childNode.text);
                break;
            case 'imageurl':
                shelfStackItem.imageUrl = childNode.text;
                break;
            case 'source':
                shelfStackItem.source = childNode.text;
                break;
            case 'height':
                shelfStackItem.height = childNode.text;
                break;
            case 'width':
                shelfStackItem.width = childNode.text;
                break;
            default:
                break;
        }
    }
    return shelfStackItem;
}
WLQuickApps.Tafiti.Scripting.ShelfStackItem.prototype = {
    shelfStackItemID: null,
    shelfStackID: null,
    domain: null,
    title: null,
    description: null,
    source: null,
    imageUrl: null,
    height: null,
    width: null,
    url: null,
    owner: null,
    lastModifiedTimestamp: null
}


////////////////////////////////////////////////////////////////////////////////
// WLQuickApps.Tafiti.Scripting.ShelfStackManager

WLQuickApps.Tafiti.Scripting.ShelfStackManager = function WLQuickApps_Tafiti_Scripting_ShelfStackManager() {
}
WLQuickApps.Tafiti.Scripting.ShelfStackManager.get_myShelfStacks = function WLQuickApps_Tafiti_Scripting_ShelfStackManager$get_myShelfStacks() {
    if (!WLQuickApps.Tafiti.Scripting.ShelfStackManager._myShelfStacks) {
        WLQuickApps.Tafiti.Scripting.ShelfStackManager._myShelfStacks = [];
    }
    return WLQuickApps.Tafiti.Scripting.ShelfStackManager._myShelfStacks;
}
WLQuickApps.Tafiti.Scripting.ShelfStackManager.get__shelfStackItemCache = function WLQuickApps_Tafiti_Scripting_ShelfStackManager$get__shelfStackItemCache() {
    if (!WLQuickApps.Tafiti.Scripting.ShelfStackManager._shelfStackItemCache) {
        WLQuickApps.Tafiti.Scripting.ShelfStackManager._shelfStackItemCache = {};
    }
    return WLQuickApps.Tafiti.Scripting.ShelfStackManager._shelfStackItemCache;
}
WLQuickApps.Tafiti.Scripting.ShelfStackManager.get__shelfStackCache = function WLQuickApps_Tafiti_Scripting_ShelfStackManager$get__shelfStackCache() {
    if (!WLQuickApps.Tafiti.Scripting.ShelfStackManager._shelfStackCache) {
        WLQuickApps.Tafiti.Scripting.ShelfStackManager._shelfStackCache = {};
    }
    return WLQuickApps.Tafiti.Scripting.ShelfStackManager._shelfStackCache;
}
WLQuickApps.Tafiti.Scripting.ShelfStackManager.get__outstandingMasterDataRequests = function WLQuickApps_Tafiti_Scripting_ShelfStackManager$get__outstandingMasterDataRequests() {
    if (!WLQuickApps.Tafiti.Scripting.ShelfStackManager._outstandingMasterDataRequests) {
        WLQuickApps.Tafiti.Scripting.ShelfStackManager._outstandingMasterDataRequests = [];
    }
    return WLQuickApps.Tafiti.Scripting.ShelfStackManager._outstandingMasterDataRequests;
}
WLQuickApps.Tafiti.Scripting.ShelfStackManager.get__outstandingShelfStackRequests = function WLQuickApps_Tafiti_Scripting_ShelfStackManager$get__outstandingShelfStackRequests() {
    if (!WLQuickApps.Tafiti.Scripting.ShelfStackManager._outstandingShelfStackRequests) {
        WLQuickApps.Tafiti.Scripting.ShelfStackManager._outstandingShelfStackRequests = [];
    }
    return WLQuickApps.Tafiti.Scripting.ShelfStackManager._outstandingShelfStackRequests;
}
WLQuickApps.Tafiti.Scripting.ShelfStackManager.get__outstandingShelfStackItemRequests = function WLQuickApps_Tafiti_Scripting_ShelfStackManager$get__outstandingShelfStackItemRequests() {
    if (!WLQuickApps.Tafiti.Scripting.ShelfStackManager._outstandingShelfStackItemRequests) {
        WLQuickApps.Tafiti.Scripting.ShelfStackManager._outstandingShelfStackItemRequests = [];
    }
    return WLQuickApps.Tafiti.Scripting.ShelfStackManager._outstandingShelfStackItemRequests;
}
WLQuickApps.Tafiti.Scripting.ShelfStackManager.beginGetMasterData = function WLQuickApps_Tafiti_Scripting_ShelfStackManager$beginGetMasterData() {
    var body = '';
    var request = new XMLHttpRequest();
    request.open('POST', WLQuickApps.Tafiti.Scripting.Utilities.getSiteUrlRoot() + '/SiteService.asmx/GetMasterData', true);
    request.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    request.setRequestHeader('Content-Length', body.length.toString());
    request.onreadystatechange = Delegate.create(null, WLQuickApps.Tafiti.Scripting.ShelfStackManager._endMasterDataRequest);
    WLQuickApps.Tafiti.Scripting.ShelfStackManager.get__outstandingMasterDataRequests().add(request);
    request.send(body);
}
WLQuickApps.Tafiti.Scripting.ShelfStackManager._endMasterDataRequest = function WLQuickApps_Tafiti_Scripting_ShelfStackManager$_endMasterDataRequest() {
    for (var lcv = 0; lcv < WLQuickApps.Tafiti.Scripting.ShelfStackManager.get__outstandingMasterDataRequests().length; lcv++) {
        var request = WLQuickApps.Tafiti.Scripting.ShelfStackManager.get__outstandingMasterDataRequests()[lcv];
        if (request.readyState === 4) {
            WLQuickApps.Tafiti.Scripting.ShelfStackManager.get__outstandingMasterDataRequests().removeAt(lcv);
            lcv--;
            var shelfStackNodeList = request.responseXML.getElementsByTagName('UserData');
            for (var index = 0; index < shelfStackNodeList.length; index++) {
                var tafitiUser = WLQuickApps.Tafiti.Scripting.TafitiUser.createFromXmlNode(shelfStackNodeList[index]);
                WLQuickApps.Tafiti.Scripting.TafitiUserManager.cacheUser(tafitiUser);
            }
            shelfStackNodeList = request.responseXML.getElementsByTagName('CommentData');
            for (var index = 0; index < shelfStackNodeList.length; index++) {
                var comment = WLQuickApps.Tafiti.Scripting.Comment.createFromXmlNode(shelfStackNodeList[index]);
                WLQuickApps.Tafiti.Scripting.CommentManager.cacheComment(comment);
            }
            shelfStackNodeList = request.responseXML.getElementsByTagName('ShelfStackItemData');
            for (var index = 0; index < shelfStackNodeList.length; index++) {
                var shelfStackItem = WLQuickApps.Tafiti.Scripting.ShelfStackItem.createFromXmlNode(shelfStackNodeList[index]);
                WLQuickApps.Tafiti.Scripting.ShelfStackManager.get__shelfStackItemCache()[shelfStackItem.shelfStackItemID] = shelfStackItem;
            }
            shelfStackNodeList = request.responseXML.getElementsByTagName('ShelfStackData');
            for (var index = 0; index < shelfStackNodeList.length; index++) {
                var shelfStack = WLQuickApps.Tafiti.Scripting.ShelfStack.createFromXmlNode(shelfStackNodeList[index]);
                var found = false;
                for (var item = 0; item < WLQuickApps.Tafiti.Scripting.ShelfStackManager.get_myShelfStacks().length; item++) {
                    if (shelfStack.shelfStackID === (WLQuickApps.Tafiti.Scripting.ShelfStackManager.get_myShelfStacks()[item]).shelfStackID) {
                        WLQuickApps.Tafiti.Scripting.ShelfStackManager.get_myShelfStacks()[item] = shelfStack;
                        found = true;
                        break;
                    }
                }
                if (!found) {
                    WLQuickApps.Tafiti.Scripting.ShelfStackManager.get_myShelfStacks().add(shelfStack);
                }
                WLQuickApps.Tafiti.Scripting.ShelfStackManager.get__shelfStackCache()[shelfStack.shelfStackID] = shelfStack;
                WLQuickApps.Tafiti.Scripting.InteropManager.updateShelfStack(shelfStack);
            }
        }
    }
}
WLQuickApps.Tafiti.Scripting.ShelfStackManager.beginAddShelfStack = function WLQuickApps_Tafiti_Scripting_ShelfStackManager$beginAddShelfStack(label) {
    var body = 'label=' + escape(label);
    var request = new XMLHttpRequest();
    request.open('POST', WLQuickApps.Tafiti.Scripting.Utilities.getSiteUrlRoot() + '/SiteService.asmx/AddShelfStack', true);
    request.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    request.setRequestHeader('Content-Length', body.length.toString());
    request.onreadystatechange = Delegate.create(null, WLQuickApps.Tafiti.Scripting.ShelfStackManager._endShelfStackRequest);
    WLQuickApps.Tafiti.Scripting.ShelfStackManager.get__outstandingShelfStackRequests().add(request);
    request.send(body);
}
WLQuickApps.Tafiti.Scripting.ShelfStackManager._endShelfStackRequest = function WLQuickApps_Tafiti_Scripting_ShelfStackManager$_endShelfStackRequest() {
    for (var lcv = 0; lcv < WLQuickApps.Tafiti.Scripting.ShelfStackManager.get__outstandingShelfStackRequests().length; lcv++) {
        var request = WLQuickApps.Tafiti.Scripting.ShelfStackManager.get__outstandingShelfStackRequests()[lcv];
        if (request.readyState === 4) {
            WLQuickApps.Tafiti.Scripting.ShelfStackManager.get__outstandingShelfStackRequests().removeAt(lcv);
            lcv--;
            var shelfStack = WLQuickApps.Tafiti.Scripting.ShelfStack.createFromXmlNode(request.responseXML.lastChild);
            var found = false;
            for (var index = 0; index < WLQuickApps.Tafiti.Scripting.ShelfStackManager.get_myShelfStacks().length; index++) {
                if (shelfStack.shelfStackID === (WLQuickApps.Tafiti.Scripting.ShelfStackManager.get_myShelfStacks()[index]).shelfStackID) {
                    WLQuickApps.Tafiti.Scripting.ShelfStackManager.get_myShelfStacks()[index] = shelfStack;
                    found = true;
                    break;
                }
            }
            if (!found) {
                WLQuickApps.Tafiti.Scripting.ShelfStackManager.get_myShelfStacks().add(shelfStack);
            }
            WLQuickApps.Tafiti.Scripting.ShelfStackManager.get__shelfStackCache()[shelfStack.shelfStackID] = shelfStack;
            WLQuickApps.Tafiti.Scripting.ShelfStackManager.sendShelfStackUpdate(shelfStack);
            WLQuickApps.Tafiti.Scripting.InteropManager.updateShelfStack(shelfStack);
        }
    }
}
WLQuickApps.Tafiti.Scripting.ShelfStackManager.addShelfStack = function WLQuickApps_Tafiti_Scripting_ShelfStackManager$addShelfStack(label) {
    var body = 'label=' + escape(label);
    var request = new XMLHttpRequest();
    request.open('POST', WLQuickApps.Tafiti.Scripting.Utilities.getSiteUrlRoot() + '/SiteService.asmx/AddShelfStack', false);
    request.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    request.setRequestHeader('Content-Length', body.length.toString());
    request.send(body);
    var shelfStack = WLQuickApps.Tafiti.Scripting.ShelfStack.createFromXmlNode(request.responseXML.lastChild);
    WLQuickApps.Tafiti.Scripting.ShelfStackManager.get_myShelfStacks().add(shelfStack);
    WLQuickApps.Tafiti.Scripting.ShelfStackManager.get__shelfStackCache()[shelfStack.shelfStackID] = shelfStack;
    return shelfStack;
}
WLQuickApps.Tafiti.Scripting.ShelfStackManager.leaveShelfStack = function WLQuickApps_Tafiti_Scripting_ShelfStackManager$leaveShelfStack(shelfStackID) {
    if (!Object.keyExists(WLQuickApps.Tafiti.Scripting.ShelfStackManager.get__shelfStackCache(), shelfStackID)) {
        return;
    }
    var shelfStack = WLQuickApps.Tafiti.Scripting.ShelfStackManager.getShelfStack(shelfStackID);
    delete WLQuickApps.Tafiti.Scripting.ShelfStackManager.get__shelfStackCache()[shelfStackID];
    var body = 'shelfStackID=' + escape(shelfStackID);
    var request = new XMLHttpRequest();
    request.open('POST', WLQuickApps.Tafiti.Scripting.Utilities.getSiteUrlRoot() + '/SiteService.asmx/LeaveShelfStack', true);
    request.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    request.setRequestHeader('Content-Length', body.length.toString());
    request.send(body);
    WLQuickApps.Tafiti.Scripting.ShelfStackManager.sendShelfStackUpdate(shelfStack);
}
WLQuickApps.Tafiti.Scripting.ShelfStackManager.beginAddShelfStackItem = function WLQuickApps_Tafiti_Scripting_ShelfStackManager$beginAddShelfStackItem(shelfStackID, domain, title, description, url, imageUrl, width, height, source) {
    var body = 'shelfStackID=' + escape(shelfStackID) + '&domain=' + escape(domain) + '&title=' + escape(title) + '&description=' + escape(description) + '&url=' + escape(url) + '&imageUrl=' + escape(imageUrl) + '&width=' + width.toString() + '&height=' + height.toString() + '&source=' + escape(source);
    var request = new XMLHttpRequest();
    request.open('POST', WLQuickApps.Tafiti.Scripting.Utilities.getSiteUrlRoot() + '/SiteService.asmx/AddShelfStackItem', true);
    request.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    request.setRequestHeader('Content-Length', body.length.toString());
    request.onreadystatechange = Delegate.create(null, WLQuickApps.Tafiti.Scripting.ShelfStackManager._endShelfStackItemRequest);
    WLQuickApps.Tafiti.Scripting.ShelfStackManager.get__outstandingShelfStackItemRequests().add(request);
    request.send(body);
}
WLQuickApps.Tafiti.Scripting.ShelfStackManager._endShelfStackItemRequest = function WLQuickApps_Tafiti_Scripting_ShelfStackManager$_endShelfStackItemRequest() {
    for (var lcv = 0; lcv < WLQuickApps.Tafiti.Scripting.ShelfStackManager.get__outstandingShelfStackItemRequests().length; lcv++) {
        var request = WLQuickApps.Tafiti.Scripting.ShelfStackManager.get__outstandingShelfStackItemRequests()[lcv];
        if (request.readyState === 4) {
            WLQuickApps.Tafiti.Scripting.ShelfStackManager.get__outstandingShelfStackItemRequests().removeAt(lcv);
            lcv--;
            var shelfStackItem = WLQuickApps.Tafiti.Scripting.ShelfStackItem.createFromXmlNode(request.responseXML.lastChild);
            var shelfStack = WLQuickApps.Tafiti.Scripting.ShelfStackManager.getShelfStack(shelfStackItem.shelfStackID);
            WLQuickApps.Tafiti.Scripting.ShelfStackManager.get__shelfStackItemCache()[shelfStackItem.shelfStackItemID] = shelfStackItem;
            var found = false;
            for (var index = 0; index < shelfStack.shelfStackItems.length; index++) {
                if (shelfStackItem.shelfStackItemID === shelfStack.shelfStackItems[index].shelfStackItemID) {
                    shelfStack.shelfStackItems[index] = shelfStackItem;
                    found = true;
                    break;
                }
            }
            if (!found) {
                shelfStack.shelfStackItems = shelfStack.shelfStackItems.concat([ shelfStackItem ]);
            }
            WLQuickApps.Tafiti.Scripting.InteropManager.updateShelfStack(shelfStack);
            WLQuickApps.Tafiti.Scripting.ShelfStackManager.sendShelfStackUpdate(shelfStack);
        }
    }
}
WLQuickApps.Tafiti.Scripting.ShelfStackManager.addShelfStackItem = function WLQuickApps_Tafiti_Scripting_ShelfStackManager$addShelfStackItem(shelfStackID, domain, title, description, url, imageUrl, width, height, source) {
    var body = 'shelfStackID=' + escape(shelfStackID) + '&domain=' + escape(domain) + '&title=' + escape(title) + '&description=' + escape(description) + '&url=' + escape(url) + '&imageUrl=' + escape(imageUrl) + '&width=' + width.toString() + '&height=' + height.toString() + '&source=' + escape(source);
    var request = new XMLHttpRequest();
    request.open('POST', WLQuickApps.Tafiti.Scripting.Utilities.getSiteUrlRoot() + '/SiteService.asmx/AddShelfStackItem', false);
    request.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    request.setRequestHeader('Content-Length', body.length.toString());
    request.send(body);
    WLQuickApps.Tafiti.Scripting.ShelfStackManager.updateShelfStack(shelfStackID);
    WLQuickApps.Tafiti.Scripting.ShelfStackManager.sendShelfStackUpdate(WLQuickApps.Tafiti.Scripting.ShelfStackManager.getShelfStack(shelfStackID));
    return request.responseXML.lastChild.text;
}
WLQuickApps.Tafiti.Scripting.ShelfStackManager.checkForUpdates = function WLQuickApps_Tafiti_Scripting_ShelfStackManager$checkForUpdates() {
    var body = 'lastUpdate=' + escape(WLQuickApps.Tafiti.Scripting.ShelfStackManager._lastUpdate.toLocaleString());
    var request = new XMLHttpRequest();
    request.open('POST', WLQuickApps.Tafiti.Scripting.Utilities.getSiteUrlRoot() + '/SiteService.asmx/CheckForUpdates', false);
    request.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    request.setRequestHeader('Content-Length', body.length.toString());
    request.send(body);
    WLQuickApps.Tafiti.Scripting.ShelfStackManager._lastUpdate = Date.get_now();
    return Boolean.parse(request.responseXML.lastChild.text);
}
WLQuickApps.Tafiti.Scripting.ShelfStackManager.beginUpdateShelfStack = function WLQuickApps_Tafiti_Scripting_ShelfStackManager$beginUpdateShelfStack(shelfStackID) {
    var body = 'shelfStackID=' + shelfStackID;
    var request = new XMLHttpRequest();
    request.open('POST', WLQuickApps.Tafiti.Scripting.Utilities.getSiteUrlRoot() + '/SiteService.asmx/GetShelfStack', true);
    request.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    request.setRequestHeader('Content-Length', body.length.toString());
    request.onreadystatechange = Delegate.create(null, WLQuickApps.Tafiti.Scripting.ShelfStackManager._endShelfStackRequest);
    WLQuickApps.Tafiti.Scripting.ShelfStackManager.get__outstandingShelfStackRequests().add(request);
    request.send(body);
}
WLQuickApps.Tafiti.Scripting.ShelfStackManager.updateShelfStack = function WLQuickApps_Tafiti_Scripting_ShelfStackManager$updateShelfStack(shelfStackID) {
    var body = 'shelfStackID=' + shelfStackID;
    var request = new XMLHttpRequest();
    request.open('POST', WLQuickApps.Tafiti.Scripting.Utilities.getSiteUrlRoot() + '/SiteService.asmx/GetShelfStack', false);
    request.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    request.setRequestHeader('Content-Length', body.length.toString());
    request.send(body);
    var shelfStack = WLQuickApps.Tafiti.Scripting.ShelfStack.createFromXmlNode(request.responseXML.lastChild);
    if (Object.keyExists(WLQuickApps.Tafiti.Scripting.ShelfStackManager.get__shelfStackCache(), shelfStackID)) {
        for (var lcv = 0; lcv < WLQuickApps.Tafiti.Scripting.ShelfStackManager.get_myShelfStacks().length; lcv++) {
            var existingShelfStack = WLQuickApps.Tafiti.Scripting.ShelfStackManager.get_myShelfStacks()[lcv];
            if (existingShelfStack.shelfStackID === shelfStackID) {
                WLQuickApps.Tafiti.Scripting.ShelfStackManager._myShelfStacks[lcv] = shelfStack;
                break;
            }
        }
    }
    else {
        WLQuickApps.Tafiti.Scripting.ShelfStackManager._myShelfStacks.add(shelfStack);
    }
    WLQuickApps.Tafiti.Scripting.ShelfStackManager.get__shelfStackCache()[shelfStackID] = shelfStack;
    WLQuickApps.Tafiti.Scripting.InteropManager.updateShelfStack(shelfStack);
}
WLQuickApps.Tafiti.Scripting.ShelfStackManager.getShelfStack = function WLQuickApps_Tafiti_Scripting_ShelfStackManager$getShelfStack(shelfStackID) {
    if (Object.keyExists(WLQuickApps.Tafiti.Scripting.ShelfStackManager.get__shelfStackCache(), shelfStackID)) {
        return WLQuickApps.Tafiti.Scripting.ShelfStackManager.get__shelfStackCache()[shelfStackID];
    }
    var body = 'shelfStackID=' + shelfStackID;
    var request = new XMLHttpRequest();
    request.open('POST', WLQuickApps.Tafiti.Scripting.Utilities.getSiteUrlRoot() + '/SiteService.asmx/GetShelfStack', false);
    request.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    request.setRequestHeader('Content-Length', body.length.toString());
    request.send(body);
    var shelfStack = WLQuickApps.Tafiti.Scripting.ShelfStack.createFromXmlNode(request.responseXML.lastChild);
    WLQuickApps.Tafiti.Scripting.ShelfStackManager.get_myShelfStacks().add(shelfStack);
    WLQuickApps.Tafiti.Scripting.ShelfStackManager.get__shelfStackCache()[shelfStackID] = shelfStack;
    return shelfStack;
}
WLQuickApps.Tafiti.Scripting.ShelfStackManager.getShelfStackItem = function WLQuickApps_Tafiti_Scripting_ShelfStackManager$getShelfStackItem(shelfStackItemID) {
    shelfStackItemID = shelfStackItemID.toLowerCase();
    if (Object.keyExists(WLQuickApps.Tafiti.Scripting.ShelfStackManager.get__shelfStackItemCache(), shelfStackItemID)) {
        return WLQuickApps.Tafiti.Scripting.ShelfStackManager.get__shelfStackItemCache()[shelfStackItemID];
    }
    var body = 'shelfStackItemID=' + shelfStackItemID;
    var request = new XMLHttpRequest();
    request.open('POST', WLQuickApps.Tafiti.Scripting.Utilities.getSiteUrlRoot() + '/SiteService.asmx/GetShelfStackItem', false);
    request.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    request.setRequestHeader('Content-Length', body.length.toString());
    request.send(body);
    var shelfItem = WLQuickApps.Tafiti.Scripting.ShelfStackItem.createFromXmlNode(request.responseXML.lastChild);
    WLQuickApps.Tafiti.Scripting.ShelfStackManager.get__shelfStackItemCache()[shelfStackItemID] = shelfItem;
    return shelfItem;
}
WLQuickApps.Tafiti.Scripting.ShelfStackManager.addUserToShelfStack = function WLQuickApps_Tafiti_Scripting_ShelfStackManager$addUserToShelfStack(shelfStackID, emailHash) {
    var shelfStack = WLQuickApps.Tafiti.Scripting.ShelfStackManager.getShelfStack(shelfStackID);
    var $enum1 = shelfStack.get_owners().getEnumerator();
    while ($enum1.moveNext()) {
        var owner = $enum1.get_current();
        if (owner.emailHash === emailHash) {
            return;
        }
    }
    var user = WLQuickApps.Tafiti.Scripting.TafitiUserManager.getUserByEmailHash(emailHash);
    var body = 'shelfStackID=' + shelfStackID + '&emailHash=' + emailHash;
    var request = new XMLHttpRequest();
    request.open('POST', WLQuickApps.Tafiti.Scripting.Utilities.getSiteUrlRoot() + '/SiteService.asmx/AddUserToShelfStack', false);
    request.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    request.setRequestHeader('Content-Length', body.length.toString());
    request.send(body);
    WLQuickApps.Tafiti.Scripting.ShelfStackManager.beginUpdateShelfStack(shelfStack.shelfStackID);
    WLQuickApps.Tafiti.Scripting.MessengerManager.sendShelfStackInvite(emailHash, shelfStack);
}
WLQuickApps.Tafiti.Scripting.ShelfStackManager.setShelfStackLabel = function WLQuickApps_Tafiti_Scripting_ShelfStackManager$setShelfStackLabel(shelfStackID, label) {
    var body = 'shelfStackID=' + shelfStackID + '&label=' + label;
    var request = new XMLHttpRequest();
    request.open('POST', WLQuickApps.Tafiti.Scripting.Utilities.getSiteUrlRoot() + '/SiteService.asmx/SetShelfStackLabel', true);
    request.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    request.setRequestHeader('Content-Length', body.length.toString());
    request.send(body);
    var shelfStack = WLQuickApps.Tafiti.Scripting.ShelfStackManager.getShelfStack(shelfStackID);
    WLQuickApps.Tafiti.Scripting.ShelfStackManager.beginUpdateShelfStack(shelfStackID);
    WLQuickApps.Tafiti.Scripting.ShelfStackManager.sendShelfStackUpdate(shelfStack);
}
WLQuickApps.Tafiti.Scripting.ShelfStackManager.removeShelfStackItem = function WLQuickApps_Tafiti_Scripting_ShelfStackManager$removeShelfStackItem(shelfStackItemID) {
    if (!Object.keyExists(WLQuickApps.Tafiti.Scripting.ShelfStackManager.get__shelfStackItemCache(), shelfStackItemID)) {
        return;
    }
    var shelfStackItem = WLQuickApps.Tafiti.Scripting.ShelfStackManager.getShelfStackItem(shelfStackItemID);
    var shelfStack = WLQuickApps.Tafiti.Scripting.ShelfStackManager.getShelfStack(shelfStackItem.shelfStackID);
    var body = 'shelfStackItemID=' + shelfStackItemID;
    if (!Object.keyExists(WLQuickApps.Tafiti.Scripting.ShelfStackManager.get__shelfStackItemCache(), shelfStackItemID)) {
        return;
    }
    var request = new XMLHttpRequest();
    request.open('POST', WLQuickApps.Tafiti.Scripting.Utilities.getSiteUrlRoot() + '/SiteService.asmx/RemoveShelfStackItem', true);
    request.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    request.setRequestHeader('Content-Length', body.length.toString());
    request.send(body);
    WLQuickApps.Tafiti.Scripting.ShelfStackManager.beginUpdateShelfStack(shelfStack.shelfStackID);
    WLQuickApps.Tafiti.Scripting.ShelfStackManager.sendShelfStackUpdate(shelfStack);
}
WLQuickApps.Tafiti.Scripting.ShelfStackManager.sendShelfStackUpdate = function WLQuickApps_Tafiti_Scripting_ShelfStackManager$sendShelfStackUpdate(shelfStack) {
    var sendMessage = false;
    var $enum1 = shelfStack.get_owners().getEnumerator();
    while ($enum1.moveNext()) {
        var user = $enum1.get_current();
        if (user.get_isOnline() && !user.get_isLoggedInUser()) {
            sendMessage = true;
            break;
        }
    }
    if (!sendMessage) {
        return;
    }
    if (WLQuickApps.Tafiti.Scripting.TafitiUserManager.get_alwaysSendMessages()) {
        WLQuickApps.Tafiti.Scripting.ShelfStackManager.sendShelfStackUpdateApproved(shelfStack);
    }
    else {
        WLQuickApps.Tafiti.Scripting.InteropManager.popIMConsentDialog(shelfStack);
    }
}
WLQuickApps.Tafiti.Scripting.ShelfStackManager.sendShelfStackUpdateApproved = function WLQuickApps_Tafiti_Scripting_ShelfStackManager$sendShelfStackUpdateApproved(shelfStack) {
    var $enum1 = shelfStack.get_owners().getEnumerator();
    while ($enum1.moveNext()) {
        var user = $enum1.get_current();
        if (user.get_isOnline() && !user.get_isLoggedInUser()) {
            WLQuickApps.Tafiti.Scripting.MessengerManager.sendUpdateMessage(user.emailHash, shelfStack);
        }
    }
}


////////////////////////////////////////////////////////////////////////////////
// WLQuickApps.Tafiti.Scripting.Utilities

WLQuickApps.Tafiti.Scripting.Utilities = function WLQuickApps_Tafiti_Scripting_Utilities() {
}
WLQuickApps.Tafiti.Scripting.Utilities.createSpan = function WLQuickApps_Tafiti_Scripting_Utilities$createSpan(innerText, cssClass) {
    return WLQuickApps.Tafiti.Scripting.Utilities.createNode('span', innerText, cssClass);
}
WLQuickApps.Tafiti.Scripting.Utilities.createListItem = function WLQuickApps_Tafiti_Scripting_Utilities$createListItem(innerText) {
    return WLQuickApps.Tafiti.Scripting.Utilities.createNode('li', innerText, String.Empty);
}
WLQuickApps.Tafiti.Scripting.Utilities.createTertiaryHeader = function WLQuickApps_Tafiti_Scripting_Utilities$createTertiaryHeader(innerText) {
    return WLQuickApps.Tafiti.Scripting.Utilities.createNode('h3', innerText, String.Empty);
}
WLQuickApps.Tafiti.Scripting.Utilities.createParagraph = function WLQuickApps_Tafiti_Scripting_Utilities$createParagraph(innerText) {
    return WLQuickApps.Tafiti.Scripting.Utilities.createNode('p', innerText, String.Empty);
}
WLQuickApps.Tafiti.Scripting.Utilities.createNode = function WLQuickApps_Tafiti_Scripting_Utilities$createNode(elementType, innerText, cssClass) {
    var node = document.createElement(elementType);
    node.innerText = innerText;
    node.className = cssClass;
    return node;
}
WLQuickApps.Tafiti.Scripting.Utilities.getSiteUrlRoot = function WLQuickApps_Tafiti_Scripting_Utilities$getSiteUrlRoot() {
    var url = window.location.protocol + '//' + window.location.hostname;
    if (window.location.port.toString() !== String.Empty) {
        url += ':' + window.location.port;
    }
    return url;
}
WLQuickApps.Tafiti.Scripting.Utilities.userIsLoggedIn = function WLQuickApps_Tafiti_Scripting_Utilities$userIsLoggedIn() {
    return ($(WLQuickApps.Tafiti.Scripting.Constants.signInFrameID));
}
WLQuickApps.Tafiti.Scripting.Utilities.hash = function WLQuickApps_Tafiti_Scripting_Utilities$hash(clearText) {
    return WLQuickApps.Tafiti.Scripting.CryptographyManager.getMD5Hash(clearText.toLowerCase());
}


WLQuickApps.Tafiti.Scripting.Constants.createClass('WLQuickApps.Tafiti.Scripting.Constants');
WLQuickApps.Tafiti.Scripting.CryptographyManager.createClass('WLQuickApps.Tafiti.Scripting.CryptographyManager');
WLQuickApps.Tafiti.Scripting.MessengerManager.createClass('WLQuickApps.Tafiti.Scripting.MessengerManager');
WLQuickApps.Tafiti.Scripting.InteropManager.createClass('WLQuickApps.Tafiti.Scripting.InteropManager');
WLQuickApps.Tafiti.Scripting.ContactElement.createClass('WLQuickApps.Tafiti.Scripting.ContactElement');
WLQuickApps.Tafiti.Scripting.TafitiUserManager.createClass('WLQuickApps.Tafiti.Scripting.TafitiUserManager');
WLQuickApps.Tafiti.Scripting.TafitiUpdateMessageFactory.createClass('WLQuickApps.Tafiti.Scripting.TafitiUpdateMessageFactory', Microsoft.Live.Messenger.Messaging.ApplicationMessageFactory);
WLQuickApps.Tafiti.Scripting.TafitiUpdateMessage.createClass('WLQuickApps.Tafiti.Scripting.TafitiUpdateMessage', Microsoft.Live.Messenger.Messaging.ApplicationMessage);
WLQuickApps.Tafiti.Scripting.MessengerController.createClass('WLQuickApps.Tafiti.Scripting.MessengerController');
WLQuickApps.Tafiti.Scripting.Comment.createClass('WLQuickApps.Tafiti.Scripting.Comment');
WLQuickApps.Tafiti.Scripting.CommentManager.createClass('WLQuickApps.Tafiti.Scripting.CommentManager');
WLQuickApps.Tafiti.Scripting.TafitiUser.createClass('WLQuickApps.Tafiti.Scripting.TafitiUser');
WLQuickApps.Tafiti.Scripting.EntryPoint.createClass('WLQuickApps.Tafiti.Scripting.EntryPoint');
WLQuickApps.Tafiti.Scripting.ShelfStack.createClass('WLQuickApps.Tafiti.Scripting.ShelfStack');
WLQuickApps.Tafiti.Scripting.ShelfStackItem.createClass('WLQuickApps.Tafiti.Scripting.ShelfStackItem');
WLQuickApps.Tafiti.Scripting.ShelfStackManager.createClass('WLQuickApps.Tafiti.Scripting.ShelfStackManager');
WLQuickApps.Tafiti.Scripting.Utilities.createClass('WLQuickApps.Tafiti.Scripting.Utilities');
WLQuickApps.Tafiti.Scripting.Constants.signInFrameID = 'signInFrame';
WLQuickApps.Tafiti.Scripting.Constants.displayPictureUrl = 'DisplayPictureUrl';
WLQuickApps.Tafiti.Scripting.Constants.displayName = 'DisplayName';
WLQuickApps.Tafiti.Scripting.Constants.personalMessage = 'PersonalMessage';
WLQuickApps.Tafiti.Scripting.Constants.status = 'Status';
WLQuickApps.Tafiti.Scripting.Constants.onlineImageUrl = 'images/online.png';
WLQuickApps.Tafiti.Scripting.Constants.offlineImageUrl = 'images/offline.png';
WLQuickApps.Tafiti.Scripting.Constants.cssClassStrong = 'strong';
WLQuickApps.Tafiti.Scripting.Constants.cssClassSubText = 'subtext';
WLQuickApps.Tafiti.Scripting.Constants.cssClassCollapsed = 'collapsed';
WLQuickApps.Tafiti.Scripting.Constants.cssClassExpanded = 'expanded';
WLQuickApps.Tafiti.Scripting.Constants.cssClassEmpty = 'empty';
WLQuickApps.Tafiti.Scripting.Constants.cssPropertyBlock = 'block';
WLQuickApps.Tafiti.Scripting.Constants.cssPropertyInline = 'inline';
WLQuickApps.Tafiti.Scripting.Constants.cssPropertyNone = 'none';
WLQuickApps.Tafiti.Scripting.Constants.tafitiUpdateMessageID = 'tafitiupdate';
WLQuickApps.Tafiti.Scripting.CryptographyManager._hashCache = null;
WLQuickApps.Tafiti.Scripting.MessengerManager._myMessengerController = null;
WLQuickApps.Tafiti.Scripting.InteropManager._updater = null;
WLQuickApps.Tafiti.Scripting.TafitiUserManager._usersCache = null;
WLQuickApps.Tafiti.Scripting.TafitiUserManager._emailHashCache = null;
WLQuickApps.Tafiti.Scripting.TafitiUserManager._outstandingUserRequests = null;
WLQuickApps.Tafiti.Scripting.TafitiUserManager._outstandingUserListRequests = null;
WLQuickApps.Tafiti.Scripting.TafitiUserManager._alwaysSendMessages = false;
WLQuickApps.Tafiti.Scripting.TafitiUserManager._loggedInUserID = null;
WLQuickApps.Tafiti.Scripting.CommentManager._cache = null;
WLQuickApps.Tafiti.Scripting.CommentManager._outstandingShelfStackCommentRequests = null;
WLQuickApps.Tafiti.Scripting.CommentManager._outstandingCommentRequests = null;
WLQuickApps.Tafiti.Scripting.ShelfStackManager._myShelfStacks = null;
WLQuickApps.Tafiti.Scripting.ShelfStackManager._shelfStackItemCache = null;
WLQuickApps.Tafiti.Scripting.ShelfStackManager._shelfStackCache = null;
WLQuickApps.Tafiti.Scripting.ShelfStackManager._outstandingMasterDataRequests = null;
WLQuickApps.Tafiti.Scripting.ShelfStackManager._outstandingShelfStackRequests = null;
WLQuickApps.Tafiti.Scripting.ShelfStackManager._outstandingShelfStackItemRequests = null;
WLQuickApps.Tafiti.Scripting.ShelfStackManager._lastUpdate = Date.get_now();

// ---- Do not remove this footer ----
// Generated using Script# v0.4.5.0 (http://projects.nikhilk.net)
// -----------------------------------
