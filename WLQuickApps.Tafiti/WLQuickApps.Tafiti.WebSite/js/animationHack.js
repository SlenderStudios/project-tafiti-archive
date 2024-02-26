// Copyright 2007 Microsoft Corp. All Rights Reserved.

OffscreenAnimationHack = function () {
    this.panel = null;
    this.timer = null;
    this.counter = 0;
}

OffscreenAnimationHack.prototype.run = function () {
    var chars = SJ.xmlEscape("ABCDEFGHIJKLMNOPQRSTUVWXYZ abcdefghijklmnopqrstuvwxyz1234567890 !@#$%^&*()`~-_=+[]\{}|;':\",./<>?");
    var s = chars;
    s += "<Run FontWeight='Bold'>" + chars + "</Run>";
    s += "<Run FontSize='16'>" + chars + "</Run>";
    s += "<Run FontSize='16' FontWeight='Bold'>" + chars + "</Run>";
    
    this.panel = new SJ.ResultsPage(10000,100,500,1000);
    this.panel.setParent(canvas);
    
    var offscreenTextBlock = new SJ.TextBlock(0,0,500,200,s,'TextWrapping="WrapWithOverflow"');
    this.panel.setContent(offscreenTextBlock);

    this.timer = setTimeout(SJ.methodCaller(this,'animateOut'),100);
}

OffscreenAnimationHack.prototype.animateOut = function () {
    if (this.timer) {
      clearTimeout(this.timer);
      this.timer = null;
    }
    this.panel.animate('animateOutBack');
    this.timer = setTimeout(SJ.methodCaller(this,'animateIn'), 750);
}

OffscreenAnimationHack.prototype.animateIn = function () {
    if (this.timer) {
      clearTimeout(this.timer);
      this.timer = null;
    }
    this.counter++;
    if (this.counter < 5) {        
        this.panel.animate('animateInFront');
        this.timer = setTimeout(SJ.methodCaller(this,'animateOut'), 750);
    } else {
        this.panel.dispose();
    }
}
