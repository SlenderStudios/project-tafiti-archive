// Copyright 2007 Microsoft Corp. All Rights Reserved.

SJ.GetDragVisual = function (domain) {
    var imageNames = {
        web: "images/shlf_web.png",
        images: "images/shlf_image.png",
        news: "images/shlf_news.png",
        feeds: "images/shlf_feed.png",
        phonebook: "images/shlf_phonebook.png",
        query: "images/dragQuery.png",
        contact: "images/dragContact.png"
    };
    
    var imagePath = imageNames[domain];

    var xaml =
            '<Image IsHitTestVisible="false" Source="' + imagePath + '"> \
                <Image.Triggers /> \
                <Image.RenderTransform> \
                    <ScaleTransform ScaleX="0.5" ScaleY="0.5" /> \
                </Image.RenderTransform> \
             </Image>';
    return SJ.createFromXaml(xaml);
}

// Called on each item in a ResultView to mixin a remove button.

SJ.mixinRemoveButton = function (obj, searchResult, left, top) {
    var images = {idle: "images/removeResult.png", hover: "images/removeResult_hover.png",
                  activeDown: "images/removeResult.png", activeUp: "removeResult_hover.png"};
    SJ.mixinHoverButton(left, top, 65, 22, obj, images, SJ.methodCaller(obj, 'removeResult'));
    obj.removeResult = function (sender,eventArgs) {
        if (obj.onRemoveResult)
            obj.onRemoveResult(obj, searchResult);
    }
}

SJ.mixinHoverButton = function (left, top, width, height, obj, images, onClick) {
    obj.hoverBtn = new SJ.Button(left, top, width, height, '', images);
    obj.hoverBtn.onClick = onClick;

    obj.hookUpEvent("MouseEnter");
    obj.MouseEnter = SJ.mixinHoverButtonMouseEnter;
    
    obj.hookUpEvent("MouseLeave");
    obj.MouseLeave = SJ.mixinHoverButtonMouseLeave;
}

SJ.mixinHoverButtonMouseEnter = function (sender, eventArgs) {
    if (!SJ.dragDrop.dragging) {
        this.hoverBtn.setParent(this);
        this.hoverBtn.doLayout();
    }
}

SJ.mixinHoverButtonMouseLeave = function (sender, eventArgs) {
    this.hoverBtn.setParent(null);
}

// Search card

SJ.SearchCard = function (top, left, onSearch, onCloseClicked) {
    SJ.Control.call(this);

    var xaml = 
        "<Canvas> \
            <Image Source='images/blank_index_card.png' /> \
            <Image Source='images/searchbox_for_card.png' Canvas.Top='100' Canvas.Left='75' /> \
         </Canvas>";
    this.visual = SJ.createFromXaml(xaml);
    
    this.onSearch = onSearch;
    
    this.searchQuery = new SJ.LabelEdit(83, 107, 170, 20, "Search...");
    this.searchQuery.onEnterKeyPressed = onSearch;
    this.searchQuery.setParent(this.visual);

    this.goBtn = new SJ.Button(230, 130, 35, 35, "",
        {idle: "images/Go_default.png", hover: "images/Go_hvr.png",
         activeDown: "images/Go_hvr.png", activeUp: "images/Go_hvr.png",
         disabled: "images/progress_light.png", rotateOnDisabled: true});
    this.goBtn.onClick = onSearch;
    this.goBtn.setParent(this.visual);
    
    this.closeBtn = new SJ.Button(20, 20, 11, 10, '', 
        {idle: "images/indexcard_close.png", hover: "images/indexcard_close_hover.png",
         activeDown: "images/indexcard_close.png", activeUp: "images/indexcard_close_hover.png"});
    this.closeBtn.onClick = onCloseClicked;
    this.closeBtn.setParent(this.visual);

    this.rotation = 0;
}

SJ.SearchCard.prototype = new SJ.Control;

SJ.SearchCard.prototype.toString = function () {
    return "SJ.SearchCard";
}

SJ.SearchCard.prototype.getQueryText = function () {
    return this.searchQuery.getText();
}

SJ.SearchCard.prototype.setQueryText = function (text) {
    this.searchQuery.setText(text);
}

SJ.SearchCard.prototype.setSearchEnabled = function (enabled) {
    if (enabled) {
        this.searchQuery.onEnterKeyPressed = this.onSearch;
        this.goBtn.setEnabled(true);
        this.closeBtn.setEnabled(true);
    }
    else {
        this.searchQuery.onEnterKeyPressed = null;
        this.goBtn.setEnabled(false);
        this.closeBtn.setEnabled(false);
    }
}

// Search card stack

SJ.SearchCardStack = function (top, left, onSearch) {
    SJ.Control.call(this);

    var xaml = 
        "<Canvas \
            xmlns='http://schemas.microsoft.com/client/2007' \
            xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml' \
            x:Name='%name%' Opacity='0' > \
            <Canvas.RenderTransform> \
                <TransformGroup> \
                    <TranslateTransform x:Name='%translate%' X='0' Y='0' /> \
                </TransformGroup> \
            </Canvas.RenderTransform> \
            <Canvas.Resources> \
                <Storyboard x:Name='%fadeIn%'> \
                    <DoubleAnimation \
                         Storyboard.TargetName='%name%' \
                         Storyboard.TargetProperty='Opacity' \
                         BeginTime='0:0:0' From='0' To='1' Duration='0:0:1' /> \
                </Storyboard> \
            </Canvas.Resources> \
         </Canvas>";
    
    this.names = {};
    xaml = SJ.generateUniqueNames(xaml, this.names);
    this.visual = SJ.createFromXaml(xaml);
    
    SJ.placeElement(this.visual, top, left);

    // create a single handler that we can use on the Storyboard.Completed event
    this.storyboardCompletedHandlerName = "SJ_" + SJ_uniqueID++;
    SJ_tagHandler(this.storyboardCompletedHandlerName, "SearchCardStack.StoryboardCompleted");
    window[this.storyboardCompletedHandlerName] = SJ.methodCaller(this, "storyboardCompleted");

    this.cards = [];
    this.onSearch = onSearch;
    this.onPopped = null;
    
    this.push('');    
}

SJ.SearchCardStack.prototype = new SJ.Control;

SJ.SearchCardStack.prototype.toString = function () {
    return "SJ.SearchCardStack";
}

SJ.SearchCardStack.prototype.getQueryText = function () {
    var card = this.cards[this.cards.length-1];
    return card.getQueryText();
}

SJ.SearchCardStack.prototype.setQueryText = function (text) {
    var card = this.cards[this.cards.length-1];
    return card.setQueryText(text);
}

SJ.SearchCardStack.prototype.setSearchEnabled = function (enabled) {
    var card = this.cards[this.cards.length-1];
    card.setSearchEnabled(enabled);
}

SJ.SearchCardStack.prototype.onCloseButton = function () {
    this.pop( function () {
            if (this.onPopped)
                this.onPopped();
        } );
}

SJ.SearchCardStack.prototype.push = function (query, onStoryboardCompleted) {
    // adds a card just behind the top-most card

    var card = new SJ.SearchCard(0, 0, this.onSearch, SJ.methodCaller(this, "onCloseButton"));
    card.setQueryText(query);

    var index = Math.max(this.cards.length-1, 0);
    this.cards.splice(index, 0, card);
    this.visual.Children.Insert(index, card.visual);
    
    this.fixStack(onStoryboardCompleted);
}

SJ.SearchCardStack.prototype.pop = function (onStoryboardCompleted) {
    if (this.visual.Children.Count > 1) {
        this.visual.Children.RemoveAt(this.visual.Children.Count-1);
        this.cards.pop();
        this.fixStack(onStoryboardCompleted);
    }
}

SJ.SearchCardStack.prototype.fixStack = function (onStoryboardCompleted) {
    // Unfortunately we may do this before the images have loaded so we have to know the image sizes.
    
    var angle = 0;
    var centerX = 373 / 2;
    var centerY = 234 / 2;
    
    var iTopItem = this.cards.length - 1;
    var iEndItem = Math.max(iTopItem-2,0);
    for (var i = iTopItem; i >= iEndItem ; i--) {
        var child = this.cards[i].visual;
        
        var names = {};
        var xaml =
            "<TransformGroup xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'> \
                <RotateTransform x:Name='%rotate%' Angle='" + this.cards[i].rotation + "' \
                    CenterX='" + centerX + "' CenterY='" + centerY + "' /> \
            </TransformGroup>";
        child.RenderTransform = SJ.createFromXaml(xaml, names);
        
        if (this.cards[i].rotation != angle) {
            var storyboardNames = {};
            var aniXaml =
                "<Storyboard xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml' x:Name='%name%'> \
                    <DoubleAnimation Storyboard.TargetName='" + names["rotate"] + "' Storyboard.TargetProperty='Angle' \
                        Duration='0:0:0.5' From='" + this.cards[i].rotation + "' To='" + angle + "' /> \
                </Storyboard>";
            var animation = SJ.createFromXaml(aniXaml, storyboardNames);
            child.Resources.Add(animation);
            if (onStoryboardCompleted) {
                // pass on the responsibility of calling onStoryboardCompleted() to the storyboard
                this.onStoryboardCompleted = onStoryboardCompleted;
                animation.addEventListener("completed", this.storyboardCompletedHandlerName);
                onStoryboardCompleted = null;
            }
            animation.Begin();
        }
        
        this.cards[i].rotation = angle;
        
        angle -= 6;
    }
    
    // hide close box if there's only one card left
    if (this.cards.length == 1)
        this.cards[0].closeBtn.setParent(null);
    else
        this.cards[this.cards.length-1].closeBtn.setParent(this.cards[this.cards.length-1].visual);

    if (onStoryboardCompleted) {
        // no storyboard was begun that would ultimately call onCompleted(), so we call it here
        onStoryboardCompleted();
    }
}

SJ.SearchCardStack.prototype.storyboardCompleted = function () {
    if (this.onStoryboardCompleted) {
        this.onStoryboardCompleted();
        this.onStoryboardCompleted = null;
    }
}

// todo: narrower is a hack to make room for the ImageBorder -- layout should just be
// variable-width (use DockPanel)

SJ.WebResultView = function (result, narrower) {
    SJ.StackPanel.call(this, 0, 0);
    
    this.result = result;
    
    var title = result.title || "(untitled)";
    var desc = result.description || "";
    var url = result.url;
    this.url = url;

    var linkStack = new SJ.StackPanel(0, 0, true);
    this.linkStack = linkStack;
    linkStack.margin = {left: 15, right: 12};
    linkStack.setParent(this);
    this.names["link"] = this.linkStack.names["top"];
    
    var titleBlock = new SJ.Hyperlink(0, 0, 168, 0, SJ.xmlEscape(title),
                                      "TextWrapping='NoWrap' FontSize='11' FontWeight='Bold' \
                                      Foreground='#0067a6' x:Name='%title%'",
                                      url);
    titleBlock.hAlign = "left";
    titleBlock.ellipsis = true;
    titleBlock.setParent(linkStack);
    
    var matches = url.match(/:\/\/([^/]+)\//);
    if (matches) {
        var host = matches[1];
        var linkBlock = new SJ.Hyperlink(0, 0, 168, 0, SJ.xmlEscape(host),
                                         "FontSize='11' Foreground='#37a100'",
                                         url);
        linkBlock.hAlign = "left";
        linkBlock.ellipsis = true;
        linkBlock.setParent(linkStack);
    }
    
    this.descBlock = new SJ.TextBlock(0, 0, narrower ? 280 : 316, 0, SJ.xmlEscape(desc),
                                     "TextWrapping='Wrap' FontSize='11' x:Name='%description%'");
    this.descBlock.hAlign = "left";
    this.descBlock.setParent(this);
    this.names["desc"] = this.descBlock.names["top"];
    
    this.visual.Cursor = "Hand";
    this.searchResult = { domain: "web", result: result };
    
    this.updateLayout();
    
    this.hookUpEvent("MouseLeftButtonDown");
}

SJ.WebResultView.prototype = new SJ.StackPanel;

SJ.WebResultView.prototype.toString = function () {
    return "SJ.WebResultView";
}

SJ.WebResultView.prototype.sizeChanged = function (width, height) {
    this.linkStack.setWidth((width * 0.3) - this.linkStack.marginWidth());
    this.descBlock.setWidth((width * 0.7) - this.descBlock.marginHeight());
    SJ.StackPanel.prototype.sizeChanged.call(this, width, height);
}

SJ.WebResultView.prototype.MouseLeftButtonDown = function(sender, eventArgs) {
    // ignore SJ.cancelBubble    
    var dragImage = SJ.GetDragVisual('web');
    SJ.beginDragDrop(this.searchResult.result, dragImage, eventArgs);
}

// ImageResultView

SJ.ImageResultView = function (result, queryText) {
    SJ.Control.call(this);
    
    this.thumbnailUrl = result.imageUrl;
    var url = result.url;
    var thumbnailWidth = parseInt(result.width);
    var thumbnailHeight = parseInt(result.height);
    var shadow = 6;
    var xaml = "<Canvas Width='" + (thumbnailWidth + shadow) + "' Height='" + (thumbnailHeight + shadow) + "'> \
        <Rectangle Canvas.Top='" + shadow + "' \
                Width='" + thumbnailWidth + "' Height='" + thumbnailHeight + "' Fill='#80000000'> \
            <Rectangle.RenderTransform> \
             <TransformGroup> \
              <RotateTransform Angle='-1' /> \
             </TransformGroup> \
            </Rectangle.RenderTransform> \
        </Rectangle> \
        <TextBlock /> \
        </Canvas>";
    this.visual = SJ.createFromXaml(xaml);
    this.shadow = this.visual.Children.GetItem(0);
    this.status = this.visual.Children.GetItem(1);
    this.status.Text = "loading...";
    
    this.visual.Cursor = "Hand";    
    this.searchResult = { domain: "images", result: result };    

    this.clickUrl = result.url;
    
    // todo: use WPF/E downloader?
    this.img = new Image;
    this.img.onload = SJ.methodCaller(this, "imageLoaded");
    this.img.src = this.thumbnailUrl;
    
    this.hookUpEvent("MouseLeftButtonDown");
}

SJ.ImageResultView.prototype = new SJ.Control;

SJ.ImageResultView.prototype.toString = function () {
    return "SJ.ImageResultView";
}

SJ.ImageResultView.prototype.measure = function (availWidth, availHeight) {
    this.desiredWidth = this.imageBtn ? this.imageBtn.getWidth() : parseInt(this.searchResult.result.width);
    this.desiredHeight = this.imageBtn ? this.imageBtn.getHeight() : parseInt(this.searchResult.result.height);
    SJ_logCall("measure", this, this.desiredWidth, this.desiredHeight);
}

SJ.ImageResultView.prototype.MouseLeftButtonDown = function (sender, eventArgs) {
    // ignore SJ.cancelBubble
    var dragImage = SJ.GetDragVisual('images');
    SJ.beginDragDrop(this.searchResult.result, dragImage, eventArgs);
}

SJ.ImageResultView.prototype.imageLoaded = function () {
    this.status.Text = "";    
    var thumbnailWidth = parseInt(this.searchResult.result.width);
    var thumbnailHeight = parseInt(this.searchResult.result.height);
    var shadow = 6;
    this.imageBtn = new SJ.Button(shadow, 0, thumbnailWidth, thumbnailHeight, '',
        {idle: this.thumbnailUrl, hover: this.thumbnailUrl,
         activeDown: this.thumbnailUrl, activeUp: this.thumbnailUrl});
    this.imageBtn.onClick = SJ.methodCaller(this, "imageClicked");
    this.imageBtn.setParent(this);
}

SJ.ImageResultView.prototype.imageClicked = function () {
    SJ.openWindow(this.clickUrl);
}

// NewsResultView

SJ.NewsResultView = function (width, result) {
    SJ.StackPanel.call(this, 0, 0, width, 0, true);
    
    this.visual.Background = "#00000000";
    
    var title = result.title || "(untitled)";
    var desc = result.description || "";
    var source = result.source || "";
    var url = result.url;

    var titleBlock = new SJ.Hyperlink(0, 0, width, 0, SJ.xmlEscape(title),
        "TextWrapping='Wrap' FontSize='30' FontFamily='Times New Roman' Foreground='#111111'", url);
    titleBlock.margin = {top: 11};
    titleBlock.setParent(this);
    
    var sourceBlock = new SJ.TextBlock(0, 0, width, 0, SJ.xmlEscape(source),
        "TextWrapping='NoWrap' FontSize='10' FontFamily='Arial' FontStyle='Italic' Foreground='#111111'");
    sourceBlock.setParent(this);

    var descBlock = new SJ.TextBlock(0, 0, width, 0, SJ.xmlEscape(desc),
                                     "TextWrapping='Wrap' FontSize='11'");
    descBlock.margin = {top: 11};
    descBlock.setParent(this);
    
    this.visual.Cursor = "Hand";
    this.searchResult = { domain: "news", result: result };
    
    this.hookUpEvent("MouseLeftButtonDown");
}

SJ.NewsResultView.prototype = new SJ.StackPanel;

SJ.NewsResultView.prototype.toString = function () {
    return "SJ.NewsResultView";
}

SJ.NewsResultView.prototype.MouseLeftButtonDown = function(sender, eventArgs) {
    // ignore SJ.cancelBubble
    var dragImage = SJ.GetDragVisual('news');
    SJ.beginDragDrop(this.searchResult.result, dragImage, eventArgs);
}

// NewsListView
// The view consists of vertically-stacked “pages” based on a constant
// target height. Each page has multiple columns. To place an article we add
// it to the shortest column. When all columns are over the target height, 
// we start a new page. There are rotating “page styles” that are just sets of column
// widths.
//
// Note: Call addResult instead of addItem so automatic column selection can take place.

SJ.NewsListView = function (left, top, width, height) {
    SJ.ListView.call(this, left, top, width, height);
    
    this.pageHeight = height;
    this.masterColumnWidths = [[150, 300, 150, 300, 150, 300, 150, 300], [300, 150, 300, 150, 300, 150, 300, 150]];
    this.curStyle = 0;  // style == which column widths
    this.newsResultViewMargin = {left: 11, bottom: 11, right: 11};
    
    this.results = [];
}

SJ.NewsListView.prototype = new SJ.ListView;

SJ.NewsListView.prototype.toString = function () {
    return "SJ.NewsListView";
}

SJ.NewsListView.prototype.sizeChanged = function (width, height) {
    var results = this.results;
    this.clearItems();
    SJ.ListView.prototype.sizeChanged.call(this, width, height);
    this.pageHeight = height;
    if (results) {
        for (var i = 0; i < results.length; i++) {
            this.addResult(results[i]);
        }
    }
    SJ.ListView.prototype.sizeChanged.call(this,width,height);
}

SJ.NewsListView.prototype.clearItems = function () {
    this.results = [];
    this.curStyle = 0;
    this.curPage = null;
    this.columns = null;
    this.borders = null;
    SJ.ListView.prototype.clearItems.call(this);    
}

SJ.NewsListView.prototype.addResult = function (result) {
    this.results.push(result);

    if (!this.curPage)
        this.newPage();
    
    var column = this.pickColumn();
    var columnWidth = this.columnWidths[column];

    if (this.columns[column].children.length > 0) {
        var border = new SJ.Control();
        border.visual = SJ.createFromXaml("<Rectangle Width='" + ((columnWidth + 22) * 0.50) + "' Height='3' Fill='#F0EDE5' />");
        if (column == 0) {
            border.hAlign = "right";
            border.margin = {right: 10};
        }
        else {
            border.margin = {left: 10};
        }
        border.setParent(this.columns[column]);
    }
    
    var view = new SJ.NewsResultView(columnWidth, result);
    view.margin = this.newsResultViewMargin;
    view.setParent(this.columns[column]);
    
    for (var j = 0; j < this.borders.length; j++) {
        this.borders[j].visual.Height = Math.max(this.columns[j].getHeight(), this.columns[j + 1].getHeight());
    }

    this.stack.invalidateLayout();
    this.listChanged();            
}

SJ.NewsListView.prototype.pickColumn = function () {
    var column = -1;
    var shortest = this.pageHeight;
    for (var i = 0; i < this.columns.length; i++) {
        var colHeight = this.columns[i].getHeight();
        if (colHeight < shortest) {
            column = i;
            shortest = colHeight;
        }
    }
    
    if (column == -1) {
        this.newPage();
        column = 0;
    }
    
    return column;
}
    

SJ.NewsListView.prototype.newPage = function () {
    this.curPage = new SJ.StackPanel(0, 0);
    this.columns = [];
    this.borders = [];
    this.columnWidths = [];
    this.curStyle = (this.curStyle + 1) % this.masterColumnWidths.length;
    var availWidth = this.getWidth();
    
    var masterColumnWidths = this.masterColumnWidths[this.curStyle];
    var colWidth = 0;
    for (var i = 0; i < masterColumnWidths.length; i++) {
        // verify that we have enough space
        var nextWidth = masterColumnWidths[i] 
                        + this.newsResultViewMargin.left // article's left/right margin
                        + this.newsResultViewMargin.right
                        + (i == 0 ? 10 : 3); // 1st column width or border width
        if (colWidth + nextWidth >= availWidth)
            break;
    
        this.columnWidths.push(masterColumnWidths[i]);
        colWidth += nextWidth;
        
        var column = new SJ.StackPanel(0, 0, true);
        if (i == 0) {
            column.margin = {left: 10};
        } else {
            var border = new SJ.Control();
            border.visual = SJ.createFromXaml("<Rectangle Width='3' Fill='#F0EDE5' />");
            border.setParent(this.curPage);
            this.borders.push(border);        
        }
        column.setParent(this.curPage);
        this.columns.push(column);
    }
    
    // expand last column to fill available space
    this.columnWidths[i-1] += Math.max(0, availWidth - colWidth - 25);
    
    if (this.itemCount() == 0) {
        var xaml = "<Canvas Height='20'> \
                     <Line Width='" + availWidth + "' Height='20' X1='5' Y1='10' X2='" + (availWidth-55) + "' Y2='10' Stroke='#212121' StrokeThickness='8' StrokeStartLineCap='Round' StrokeEndLineCap='Round' /> \
                     <Line Width='" + availWidth + "' Height='20' X1='2' Y1='17' X2='" + (availWidth-53) + "' Y2='17' Stroke='#212121' StrokeThickness='1' StrokeStartLineCap='Round' StrokeEndLineCap='Round' /> \
                    </Canvas>";
        var headerLines = new SJ.Control();
        headerLines.visual = SJ.createFromXaml(xaml);
        headerLines.margin = {left: 20};
        this.addItem(headerLines);
    }
    else {
        var border = new SJ.Control();
        border.visual = SJ.createFromXaml("<Rectangle Width='" + this.getWidth() + "' Height='3' Fill='#F0EDE5' />");
        this.addItem(border);
    }
    
    this.addItem(this.curPage);
}

// FeedResultView

SJ.FeedResultView = function (result) {
    SJ.StackPanel.call(this, 0, 0);
    
    this.visual.Background = "#00000000";
    
    var title = result.title || "(untitled)";
    var desc = result.description || "";
    var url = result.url;

    var width = 453; // todo: use dockpanel
    
    // 3 columns
    
    // column #1: title + subscribe link
    
    var leftStack = new SJ.StackPanel(0, 0, true);
    leftStack.setParent(this);
    
    var titleBlock = new SJ.TextBlock(0, 0, 140, 0, SJ.xmlEscape(title),
                                      "TextWrapping='Wrap' FontFamily='Arial' FontSize='20' FontWeight='Bold' \
                                      Foreground='#132241'");
    titleBlock.ellipsis = true;
    titleBlock.margin = {top: 10, right: 5};
    titleBlock.hAlign = "right";
    titleBlock.setParent(leftStack);
    
    var linkBlock = new SJ.Hyperlink(0, 0, 60, 0, "View Feed",
                                         "FontSize='12' Foreground='#d57d4b'", url);
    linkBlock.margin = {top: 10, right: 5};
    linkBlock.hAlign = "right";
    linkBlock.setParent(leftStack);
    
    // column #2: border
    
    var border = new SJ.Control();
    border.visual = SJ.createFromXaml("<Line Width='15' Height='120' X1='5' Y1='5' X2='5' Y2='115' Stroke='#d3cfc6' StrokeThickness='5' StrokeStartLineCap='Round' StrokeEndLineCap='Round' />");
    border.setParent(this);
    
    // column #3: description
     
    var descBlock = new SJ.TextBlock(0, 0, 250, 0, SJ.xmlEscape(desc),
                                     "TextWrapping='Wrap' FontSize='11' Foreground='#676662'");
    descBlock.margin = {top: 10};
    descBlock.setParent(this);
    
    this.updateLayout();

    this.visual.Cursor = "Hand";
    this.searchResult = { domain: "feeds", result: result };
    
    this.hookUpEvent("MouseLeftButtonDown");
}

SJ.FeedResultView.prototype = new SJ.StackPanel;

SJ.FeedResultView.prototype.toString = function () {
    return "SJ.FeedResultView";
}

SJ.FeedResultView.prototype.MouseLeftButtonDown = function (sender, eventArgs) {
    // ignore SJ.cancelBubble
    var dragImage = SJ.GetDragVisual('feeds');
    SJ.beginDragDrop(this.searchResult.result, dragImage, eventArgs);
}

// QueryResultView

SJ.QueryResultView = function (query) {
    SJ.StackPanel.call(this, 0, 0);
    
    this.query = query;

    this.visual.Background = "#00000000";
    
    this.queryStack = new SJ.StackPanel(0, 0, true);
    this.queryStack.margin = {left: 12, right: 12};
    this.queryStack.setParent(this);
    
    this.labelBlock = new SJ.TextBlock(0, 0, 550, 0, 'search', 'FontSize="11" TextWrapping="NoWrap"');
    this.labelBlock.setParent(this.queryStack);

    this.linkBlock = new SJ.Hyperlink(0, 0, 550, 0, SJ.xmlEscape(this.query),
                                     "FontSize='11' Foreground='#37a100'", this.query);
    this.linkBlock.hAlign = "left";
    this.linkBlock.setParent(this.queryStack);
    this.linkBlock.onNavigate = SJ.methodCaller(this, 'onNavigateHandler');

    this.hookUpEvent("MouseLeftButtonDown");
}

SJ.QueryResultView.prototype = new SJ.StackPanel;

SJ.QueryResultView.prototype.toString = function () {
    return "SJ.QueryResultView";
}

SJ.QueryResultView.prototype.MouseLeftButtonDown = function (sender, eventArgs) {
    if (!SJ.cancelBubble) {
        SJ.cancelBubble = true;
        var dragImage = SJ.GetDragVisual('query');
        SJ.beginDragDrop( { query: this.query }, dragImage, eventArgs );
    }
}

SJ.QueryResultView.prototype.onNavigateHandler = function (sender, eventArgs) {
    if (this.onNavigate)
        this.onNavigate(this, this.query);
}

// ResultsHeader

SJ.ResultsHeader = function (domain, width) {
    SJ.Control.call(this, 0, 0, true);
    
    this.domain = domain;
    this.capDomain = domain.substr(0, 1).toUpperCase() + domain.substr(1);
    
    this.visual = SJ.createFromXaml("<Canvas />");
    
    this.vStack = new SJ.StackPanel(0, 0, true);
    this.vStack.setParent(this);
        
    this.wood = new SJ.Image(0, 0, "images/wood_hdr.jpg");
    this.wood.setImageSize(2048, 50);
    this.wood.setParent(this.vStack);
    
    this.handle = new SJ.Image(0, 0, "images/slvr_handle.png");
    this.handle.setImageSize(236, 74);
    this.handle.hAlign = "center";
    this.handle.margin = {top: -50};
    this.handle.visible(false);
    this.handle.setParent(this.vStack);
    
    this.queryLabel = new SJ.TextBlock(0, 0, 192, 20, "",
        "TextWrapping='NoWrap' FontFamily='Courier New' FontSize='12'");
    this.queryLabel.ellipsis = true;
    this.queryLabel.hAlign = "center";
    this.queryLabel.margin = {top: -60};
    this.queryLabel.visible(false);
    this.queryLabel.setParent(this.vStack);
    
    this.wordWheel = {};
    this.wordWheel.filter = "";
    
    this.wordWheel.layer = new SJ.Layer(0,0);
    this.wordWheel.layer.setParent(this);
    
    this.wordWheel.background = new SJ.Image(0, 0, "images/wrd_wheel.png");
    this.wordWheel.background.setImageSize(190, 23);
    this.wordWheel.background.setParent(this.wordWheel.layer);
    
    this.wordWheel.labelEdit = new SJ.LabelEdit(5, 5, 180, 20, "filter these results");
    this.wordWheel.labelEdit.onKeyPressed = SJ.methodCaller(this, "onWordWheelKeyPressed");
    this.wordWheel.labelEdit.setParent(this.wordWheel.layer);

    var hStack = new SJ.StackPanel(0, 0);
    hStack.setWidth(width);
    hStack.margin = {top:30, bottom: 15};
    hStack.setParent(this.vStack);
    
    var icon = new SJ.Image(0, 0, "images/16_" + domain + ".png");
    icon.setImageSize(16, 16);
    icon.vAlign = "center";
    icon.margin = {left: 15, right: 5};
    icon.setParent(hStack);
    
    this.domainBlock = new SJ.TextBlock(0, 0, 60, 0, "", "FontSize='16' TextWrapping='NoWrap'");
    this.domainBlock.margin = {left: 5, top: 5};
    this.domainBlock.setParent(hStack);
    
    this.infoBlock = new SJ.TextBlock(0, 0, 165, 0, "", "FontSize='11' Foreground='#333333' TextWrapping='NoWrap'");
    this.infoBlock.margin = {left: 30, top: 10};
    this.infoBlock.setParent(hStack);

    if (domain == "web") {
        this.fullScreenLink = new SJ.FullScreenButton(0, 0, true);
        this.fullScreenLink.onClick = SJ.methodCaller(this, "onFullScreen");
        this.fullScreenLink.visible(false);
        this.fullScreenLink.setParent(hStack);
    }
    
    this.updateInfo(null, null);
    
    this.hookUpEvent("MouseLeftButtonDown");
}

SJ.ResultsHeader.prototype = new SJ.Control;

SJ.ResultsHeader.prototype.toString = function () {
    return "SJ.ResultsHeader";
}

SJ.ResultsHeader.prototype.sizeChanged = function (width, height) {
    this.vStack.setWidth(width);
    this.queryLabel.setWidth(192);
    this.wordWheel.layer.move(width - 200, 65);
    if (this.fullScreenLink) {
        this.fullScreenLink.showLabel(width > 575);
        var left = (width > 575) ? 50 + (width-575)/2 : 0;
        this.fullScreenLink.margin = {left: left, top: 3};
    }
    SJ.Control.prototype.sizeChanged.call(this,width,height);
}

SJ.ResultsHeader.prototype.invalidateLayout = function () {
    this.vStack.invalidateLayout();
}

SJ.ResultsHeader.prototype.measure = function (availWidth, availHeight) {
    this.vStack.measure(availWidth, availHeight);
    this.desiredWidth = this.vStack.desiredWidth;
    this.desiredHeight = this.vStack.desiredHeight;
    SJ_logCall("measure", this, this.desiredWidth, this.desiredHeight);
}

SJ.ResultsHeader.prototype.arrange = function (left, top, width, height) {
    SJ.Control.prototype.arrange.call(this, left, top, width, height);
    
    this.vStack.arrange(0, 0, this.vStack.desiredWidth, this.vStack.desiredHeight);
}

SJ.ResultsHeader.prototype.showWordWheel = function () {
    this.wordWheel.layer.visible(true);
}

SJ.ResultsHeader.prototype.hideWordWheel = function () {
    this.wordWheel.layer.visible(false);
}

SJ.ResultsHeader.prototype.resetWordWheel = function () {
    if (this.wordWheel.timer)
        clearTimeout(this.wordWheel.timer);
    this.wordWheel.labelEdit.setText("");
    this.wordWheel.filter = "";
}

SJ.ResultsHeader.prototype.setWordWheelFilter = function (text) {
    this.wordWheel.filter = text;
    this.wordWheel.labelEdit.setText(text);
}

SJ.ResultsHeader.prototype.onWordWheelKeyPressed = function () {
    if (this.wordWheel.timer)
        clearTimeout(this.wordWheel.timer);
    this.wordWheel.timer = setTimeout(SJ.methodCaller(this, "onWordWheelCheckText"), 500);
}
    
SJ.ResultsHeader.prototype.onWordWheelCheckText = function () {
    var wwText = this.wordWheel.labelEdit.getText();
    if (wwText != this.wordWheel.filter)
        if (this.onWordWheelFilterChanged)
            this.onWordWheelFilterChanged(this, wwText);
}

SJ.ResultsHeader.prototype.onFullScreen = function () {
    if (this.onFullScreenClicked)
        this.onFullScreenClicked(this, null);
}
    
SJ.ResultsHeader.prototype.updateInfo = function (query, stats) {
    this.query = query;
    this.stats = stats;
    
    if (query) {
        this.queryLabel.setText(SJ.xmlEscape(query));
        this.handle.visible(true);
        this.queryLabel.visible(true);
        if (this.fullScreenLink)
            this.fullScreenLink.visible(true);
    }
    
    this.domainBlock.setText(this.capDomain);

    if (stats && stats != "") {
        this.infoBlock.setText(SJ.xmlEscape(stats));
        this.showWordWheel();
    }
    else {
        this.infoBlock.setText("");
        this.hideWordWheel();
    }
}

SJ.ResultsHeader.prototype.MouseLeftButtonDown = function (sender, eventArgs) {
    if (!SJ.cancelBubble) {
        SJ.cancelBubble = true;
        if (this.query) {
            var dragImage = SJ.GetDragVisual('query');
            SJ.beginDragDrop( { query: this.query } , dragImage, eventArgs);
        }
    }
}

SJ.ResultsPage = function (left, top, width, height, domain) {
    SJ.AnimatePanel.call(this, left, top, width, height);
    
    if (SJ.wpfeControl) {
        this.visual.Background = "#faf7f1";        
        this.setAnimations([
            '<Storyboard x:Name="%animateOutBack%"> \
                <DoubleAnimation \
                     Storyboard.TargetName="%scale%" \
                     Storyboard.TargetProperty="ScaleX" \
                     BeginTime="0:0:0" From="1" To="0.85" Duration="0:0:0.3" /> \
                <DoubleAnimation \
                     Storyboard.TargetName="%scale%" \
                     Storyboard.TargetProperty="ScaleY" \
                     BeginTime="0:0:0" From="1" To="0.85" Duration="0:0:0.3" /> \
                <DoubleAnimation \
                     Storyboard.TargetName="%translate%" \
                     Storyboard.TargetProperty="X" \
                     BeginTime="0:0:0.3" From="0" By="2300"   \
                     Duration="0:0:0.3" /> \
            </Storyboard>',
            '<Storyboard x:Name="%animateOutFront%"> \
                <DoubleAnimation \
                     Storyboard.TargetName="%scale%" \
                     Storyboard.TargetProperty="ScaleX" \
                     BeginTime="0:0:0" From="1" To="1.15" Duration="0:0:0.3" /> \
                <DoubleAnimation \
                     Storyboard.TargetName="%scale%" \
                     Storyboard.TargetProperty="ScaleY" \
                     BeginTime="0:0:0" From="1" To="1.15" Duration="0:0:0.3" /> \
                <DoubleAnimation \
                     Storyboard.TargetName="%translate%" \
                     Storyboard.TargetProperty="X" \
                     BeginTime="0:0:0.3" From="0" By="2300"   \
                     Duration="0:0:0.3" /> \
            </Storyboard>',
            '<Storyboard x:Name="%animateInFront%"> \
                <DoubleAnimation \
                     Storyboard.TargetName="%translate%" \
                     Storyboard.TargetProperty="X" \
                     BeginTime="0:0:0" From="2300" To="0" Duration="0:0:0.3" /> \
                <DoubleAnimation \
                     Storyboard.TargetName="%scale%" \
                     Storyboard.TargetProperty="ScaleX" \
                     BeginTime="0:0:0.3" From="1.15" To="1" \
                     Duration="0:0:0.3" /> \
                 <DoubleAnimation \
                     Storyboard.TargetName="%scale%" \
                     Storyboard.TargetProperty="ScaleY" \
                     BeginTime="0:0:0.3" From="1.15" To="1" \
                     Duration="0:0:0.3" /> \
            </Storyboard>',
            '<Storyboard x:Name="%animateInBack%"> \
                  <DoubleAnimation \
                     Storyboard.TargetName="%translate%" \
                     Storyboard.TargetProperty="X" \
                     BeginTime="0:0:0" From="2300" To="0" Duration="0:0:0.3" /> \
                  <DoubleAnimation \
                     Storyboard.TargetName="%scale%" \
                     Storyboard.TargetProperty="ScaleX" \
                     BeginTime="0:0:0.3" From="0.85" To="1" \
                     Duration="0:0:0.3" /> \
                  <DoubleAnimation \
                     Storyboard.TargetName="%scale%" \
                     Storyboard.TargetProperty="ScaleY" \
                     BeginTime="0:0:0.3" From="0.85" To="1" \
                     Duration="0:0:0.3" /> \
                </Storyboard>'
        ]);
    }
}

SJ.ResultsPage.prototype = new SJ.AnimatePanel;

SJ.ResultsPage.prototype.toString = function () {
    return "SJ.ResultsPage";
}

SJ.ResultsPage.prototype.situate = function (place) {
    var scaler = SJ.findElement(this.names["scale"]);
    var translater = SJ.findElement(this.names["translate"]);
    switch (place) {
        case "in":
            scaler.ScaleX = 1;
            scaler.ScaleY = 1;
            translater.X = 0;
            break;
        case "outBack":
            scaler.ScaleX = 0.85;
            scaler.ScaleY = 0.85;
            translater.X = 2300;
            break;
        case "outFront":
            scaler.ScaleX = 1.15;
            scaler.ScaleY = 1.15;
            translater.X = 2300;
            break;
    }
}

// SavedSearchHeader

SJ.SavedSearchHeader = function (width, savedResultsView) {
    SJ.Control.call(this, 0, 0, true);

    this.savedResultsView = savedResultsView;
    
    this.visual = SJ.createFromXaml("<Canvas />");
    
    this.vStack = new SJ.StackPanel(0, 0, true);
    this.vStack.setParent(this);
        
    window.headerBackground = new SJ.Image(0, 0, "images/silver_hdr.jpg");
    headerBackground.setImageSize(2048, 50);
    headerBackground.setParent(this.vStack);
    
    var hStack = new SJ.StackPanel(0, 0);
    hStack.margin = {top: -50, bottom: 25};
    hStack.setParent(this.vStack);
    
    var postBtn = new SJ.Hyperlink(0, 0, 50, 0, "Blog it",
                                      "FontSize='13' Foreground='#eeeeee'", "post");
    postBtn.onNavigate = SJ.methodCaller(this, "postToSpace");
    postBtn.vAlign = "bottom";
    postBtn.margin = {left: 15, top: 15};
    postBtn.setParent(hStack);

    var emailBtn = new SJ.Hyperlink(0, 0, 50, 0, "E-mail it",
                                      "FontSize='13' Foreground='#eeeeee'", "email");
    emailBtn.onNavigate = SJ.methodCaller(this, "emailSnapshot");
    emailBtn.vAlign = "bottom";
    emailBtn.margin = {left: 5};
    emailBtn.setParent(hStack);

    this.closeBtn = new SJ.Button(0, 0, 15, 14, "Close", 
                {idle: "images/CloseX.png", hover: "images/CloseX.png",
                 activeDown: "images/CloseX.png", activeUp: "images/CloseX.png"});
    this.closeBtn.labelAlign = "right";
    this.closeBtn.label.FontSize = '13';
    this.closeBtn.label.Foreground = '#eeeeee';
    this.closeBtn.onClick = SJ.methodCaller(this, "onCloseHandler");
    this.closeBtn.setParent(this);
    this.closeBtn.doLayout();

    // hTitleStack holds the icon + label and is centered within the header
    this.hTitleStack = new SJ.StackPanel(0, 0);
    this.hTitleStack.setParent(this);
        
    this.savedSearchIcon = new SJ.Image(0, 0, "images/SavedSearchIcon.png");
    this.savedSearchIcon.setImageSize(24, 20);
    this.savedSearchIcon.setParent(this.hTitleStack);
    
    this.labelTextBlock = new SJ.TextBlock(0, 0, 200, 0, "",
        "TextWrapping='NoWrap' FontFamily='Arial' FontSize='15' Foreground='#122042'");
    this.labelTextBlock.ellipsis = true;
    this.labelTextBlock.margin = {left: 5};
    this.labelTextBlock.setParent(this.hTitleStack);
}

SJ.SavedSearchHeader.prototype = new SJ.Control;

SJ.SavedSearchHeader.prototype.toString = function () {
    return "SJ.SavedSearchHeader";
}

SJ.SavedSearchHeader.prototype.sizeChanged = function (width, height) {
    this.vStack.setWidth(width);
    this.closeBtn.move(width - 75, 20);
    SJ.Control.prototype.sizeChanged.call(this,width,height);
}

SJ.SavedSearchHeader.prototype.measure = function (availWidth, availHeight) {
    this.vStack.measure(availWidth, availHeight);
    this.hTitleStack.measure(availWidth, availHeight);
    this.desiredWidth = this.vStack.desiredWidth;
    this.desiredHeight = this.vStack.desiredHeight;
    SJ_logCall("measure", this, this.desiredWidth, this.desiredHeight);
}

SJ.SavedSearchHeader.prototype.arrange = function (left, top, width, height) {
    SJ.Control.prototype.arrange.call(this, left, top, width, height);
    
    this.vStack.arrange(0, 0, this.vStack.desiredWidth, this.vStack.desiredHeight);
    this.hTitleStack.arrange(0, 0, this.hTitleStack.desiredWidth, this.hTitleStack.desiredHeight);

    // center the icon + label in the header
    // HACK: there's probably a better way to do this
    var titleWidth = this.savedSearchIcon.getWidth() + this.labelTextBlock.textBlock.ActualWidth;
    this.hTitleStack.visual["Canvas.Left"] = (width/2) - (titleWidth/2);
    this.hTitleStack.visual["Canvas.Top"] = 15;
}

SJ.SavedSearchHeader.prototype.updateInfo = function (label) {
    this.labelTextBlock.setText(SJ.xmlEscape(label));
    this.updateLayout();
}

SJ.SavedSearchHeader.prototype.onCloseHandler = function () {
    if (this.onCloseClicked)
        this.onCloseClicked(this, null);
}

SJ.SavedSearchHeader.prototype.emailSnapshot = function () {
    this.emailSnapshotDialog = new SJ.DialogBox(300, 100);
    var esDialog = new SJ.EmailSnapshotDialog(this);
    var border = new SJ.Border(8, "Background='#ffffff'");
    border.setContent(esDialog);
    this.emailSnapshotDialog.setContent(border);

    this.emailSnapshotDialog.center();
    this.emailSnapshotDialog.updateLayout();
    this.emailSnapshotDialog.placeTextEdits();
    this.emailSnapshotDialog.show();
}

SJ.SavedSearchHeader.prototype.closeEmailSnapshotDialog = function () {
    this.emailSnapshotDialog.close();
    this.emailSnapshotDialog = null;
}

SJ.SavedSearchHeader.prototype.postToSpace = function () {
    this.postDialog = new SJ.DialogBox(300, 100);
    var psDialog = new SJ.PostToSpaceDialog(this);
    var border = new SJ.Border(8, "Background='#ffffff'");
    border.setContent(psDialog);
    this.postDialog.setContent(border);
    
    this.postDialog.center();
    this.postDialog.updateLayout();
    this.postDialog.placeTextEdits();
    this.postDialog.show();
}

SJ.SavedSearchHeader.prototype.closePostDialog = function () {
    this.postDialog.close();
    this.postDialog = null;
}

SJ.SavedSearchHeader.prototype.getLabel = function () {
    return this.labelTextBlock.getText();
}

SJ.SavedSearchHeader.prototype.getResults = function () {
    return this.savedResultsView.results;
}

SJ.SavedSearchHeader.prototype.generateHtml = function () {
    var html = "";
    var results = this.getResults();
    for (var i = 0; i < results.length; i++) {
        html += SJ.RenderResultAsHtml(results[i]) + "\n";
        if (i < results.length - 1)
            html += "<hr style='border: 0; color: #888; background-color: #888; height: 2px;' />";
    }
    return html;
}

// DomainResultsView

SJ.DomainResultsView = function (left, top, width, height, domain) {
    SJ.ResultsPage.call(this, left, top, width, height);
    
    this.visual.Background = "#faf7f1";
    
    this.domain = domain || "web";
    this.results = [];
    
    var listPanel = null;
    if (this.domain == "images")
        listPanel = new SJ.FlowPanel(0, 0, 0, 0, "center");
    
    var stack = new SJ.StackPanel(0, 0, true);
        
    this.header = new SJ.ResultsHeader(this.domain, width);
    this.header.setParent(stack);
    
    // todo: use dockpanel
    this.header.updateLayout();
    var listHeight = height - this.header.getHeight();

    if (this.domain == "news")
        this.resultsList = new SJ.NewsListView(0, 0, width, listHeight);
    else
        this.resultsList = new SJ.ListView(0, 0, width, listHeight, listPanel);
    
    this.resultsList.setParent(stack);
    this.setContent(stack);
    
    // shown while we wait for query results
    this.progressBtn = new SJ.Button(0, 0, 35, 35, "",
        {idle: "images/progress_light.png", hover: "images/progress_light.png",
         activeDown: "images/progress_light.png", activeUp: "images/progress_light.png",
         disabled: "images/progress_light.png", rotateOnDisabled: true});    
}

SJ.DomainResultsView.prototype = new SJ.ResultsPage;

SJ.DomainResultsView.prototype.toString = function () {
    return "SJ.DomainResultsView";
}

SJ.DomainResultsView.prototype.sizeChanged = function (width, height) {
    if (this.domain == "feeds") {
        for (var i = 0; i < this.resultsList.stack.children.length; i++) {
            var resultView = this.resultsList.stack.children[i];
            if (i % 2 == 0)
                resultView.margin = {left: 20, top: 8, bottom: 25};
            else
                resultView.margin = {left: Math.max(80,width-550), top: 8, bottom: 25};
        }
    }

    var listHeight = height - this.header.getHeight();
    this.resultsList.resize(width,listHeight);
    SJ.ResultsPage.prototype.sizeChanged.call(this,width,height);
}

SJ.DomainResultsView.prototype.lineUp = function() {
    this.resultsList.lineUp();
}

SJ.DomainResultsView.prototype.lineDown = function() {
    this.resultsList.lineDown();
}

SJ.DomainResultsView.prototype.pageUp = function() {
    this.resultsList.pageUp();
}

SJ.DomainResultsView.prototype.pageDown = function() {
    this.resultsList.pageDown();
}

SJ.DomainResultsView.prototype.reset = function (queryText) {
    this.queryText = queryText;
    this.searchInvoked = false;
    this.resultsList.clearItems();
    this.results = [];
    this.header.updateInfo(queryText, null);
}

SJ.DomainResultsView.prototype.beginQuery = function (queryText, maxResults, resultsPerChunk, offset) {
    offset = offset || 0;
    if (offset == 0 && this.searchInvoked)
        return;

    this.progressBtn.move(this.getWidth()/2, this.getHeight()/2);
    this.progressBtn.setParent(this.visual);
    this.progressBtn.setEnabled(false);
    
    this.searchInvoked = true;
    this.client = new SearchClient();
    this.client.search(this.domain, this.queryText, offset, resultsPerChunk,
                       this.makeSearchCallback(offset, maxResults, resultsPerChunk));
}

SJ.DomainResultsView.prototype.makeSearchCallback = function (offset, maxResults, resultsPerChunk) {
    var me = this;
    return function (result) {
        var completed = true;
        var succeeded = false;
        if (result.succeeded) {
            var response = null;
            try {
                response = eval( '(' + result.response + ')' );
                succeeded = true;
            } catch (e) { }
            if (response && me.convertResponse(response, offset)) {
                // offset += resultsPerChunk;
                // if (offset < maxResults) {
                //     me.client.search(me.domain, me.queryText, offset, resultsPerChunk,
                //                      me.makeSearchCallback(offset, maxResults, resultsPerChunk));
                //     completed = false;
                // }
            }
        }

        if (completed) {
            me.progressBtn.setEnabled(true);        
            me.progressBtn.setParent(null);
            if (me.onQueryComplete)
                me.onQueryComplete(me, succeeded);
        }
    }
}

// Normalize (to some extent) the various response formats from the Live Search domains.
// Return value is {_total: <total results>, document: [result, result, ...]}

SJ.DomainResultsView.prototype.findDocsetInResponse = function (response) {
        
    var source;
    if (this.domain == "news")
        source = "FEDERATOR_BACKFILL_NEWS";
    else
        source = "FEDERATOR_MONARCH";
    
    var docsetList = response.searchresult.documentset;
    var docset;
    if (docsetList) {
        if (docsetList._source && docsetList._source == source) {
            docset = docsetList;
        }
        else {
            for (var i = 0; i < docsetList.length; i++) {
                if (docsetList[i]._source == source) {
                    docset = docsetList[i];
                    break;
                }
            }
        }
    }
        
    if (docset)
        return docset;
    
    return null;
}

SJ.DomainResultsView.prototype.convertResponse = function (response, expectedOffset) {
    var resultsInResponse = this.findDocsetInResponse(response);
    if (resultsInResponse && resultsInResponse.document) {
        if (resultsInResponse._count == 1)
            resultsInResponse.document = [resultsInResponse.document];
            
        // Occationally, the result set overlaps with the previous result set. I.e.,
        // we requested results #20-39, but got results #19-39. Deal with that here
        // by skipping results before the "expectedOffset".
        var start = parseInt(resultsInResponse._start);
        start = (expectedOffset > start) ? expectedOffset - start : 0;
        start = Math.min(start, resultsInResponse.document.length-1);
        for (var i = start; i < resultsInResponse.document.length; i++) {
            this.results.push(resultsInResponse.document[i]);
            this.addResultView(resultsInResponse.document[i]);
        }
        
        this.totalResults = parseInt(resultsInResponse._total);
        this.header.updateInfo(this.queryText,
            "1-" + this.results.length + " of " + SJ.commaizeNumber(this.totalResults) + " results");
        
        return this.results.length < this.totalResults;
    }
    else {
        this.totalResults = 0;
        this.header.updateInfo(this.queryText, "No results");
        return false;
    }
}

SJ.DomainResultsView.prototype.addResultView = function (result) {

    switch (this.domain) {
        case "web":
            var resultView = new SJ.WebResultView(result);
            resultView.margin = {top: 8, bottom: 8};
            this.resultsList.addItem(resultView);
            break;
        
        case "images":
            var resultView = new SJ.ImageResultView(result, this.queryText);
            resultView.margin = {top: 8, bottom: 8, left: 8, right: 8};
            resultView.vAlign = "center";
            this.resultsList.addItem(resultView);
            break;

        case "news":
            this.resultsList.addResult(result);
            break;

        case "feeds":
            var resultView = new SJ.FeedResultView(result);
            var left = (this.resultsList.itemCount() % 2 == 0) ? 20 : Math.max(80, this.getWidth()-550);
            resultView.margin = {left: left, top: 8, bottom: 25};
            this.resultsList.addItem(resultView);
            break;

        case "phonebook":
            var resultView = new SJ.WebResultView(result);
            resultView.margin = {top: 8, bottom: 8};
            this.resultsList.addItem(resultView);
            break;
    }
}

SJ.DomainResultsView.prototype.filterCheck = function (result, filterRegExp) {
    switch (this.domain) {
        case "web":
        case "feeds":
        case "phonebook":
            return (result.title && result.title.search(filterRegExp) != -1) ||
                   (result.description && result.description.search(filterRegExp) != -1) ||
                   (result.url && result.url.search(filterRegExp) != -1);
        
        case "images":
            return (result.url && result.url.search(filterRegExp) != -1);

        case "news":
            return (result.title && result.title.search(filterRegExp) != -1) ||
                   (result.description && result.description.search(filterRegExp) != -1) ||
                   (result.source && result.source.search(filterRegExp) != -1) ||
                   (result.url && result.url.search(filterRegExp) != -1);
    }
    
    return false;
}

SJ.DomainResultsView.prototype.setFilter = function (filter) {
    this.resultsList.clearItems();
    
    // We use a regexp for fast case-insensitive string search.
    // review: is there a better way?
    var filterPattern = "";
    if (BrowserDetect.browser == "Safari") {
        // Safari doesn't like unicode characters in regexps;
        // See http://bugzilla.opendarwin.org/show_bug.cgi?id=8043
        filterPattern = filter;
    }
    else {
        for (var i = 0; i < filter.length; i++) {
            // Avoid infinite knowledge of regexp syntax by just turning everything into \uxxxx chars
            var code = "000" + filter.charCodeAt(i).toString(16);
            filterPattern += "\\u" + code.substr(code.length - 4);
        }
    }
    var filterRegExp = new RegExp(filterPattern, "i");
    
    for (var i = 0; i < this.results.length; i++) {
        if (filter == "" || this.filterCheck(this.results[i], filterRegExp))
            this.addResultView(this.results[i]);
    }
}

// SavedResultsView

SJ.SavedResultsView = function (left, top, width, height) {
    SJ.ResultsPage.call(this, left, top, width, height);
    
    var stack = new SJ.StackPanel(0, 0, true);
    
    this.callOnDisplay = [];
        
    this.header = new SJ.SavedSearchHeader(width, this);
    this.header.setParent(stack);
    this.header.onCloseClicked = SJ.methodCaller(this, "onCloseHandler");
    
    this.conversationSection = new SJ.FlowPanel(0, 0, 0, 0, "left", '<Canvas> \
        <Canvas.Background> \
            <LinearGradientBrush StartPoint="0,0" EndPoint="0,1"> \
              <GradientStop Color="#FEEE" Offset="0.0" /> \
              <GradientStop Color="#FFFF" Offset="0.5" /> \
              <GradientStop Color="#FFFF" Offset="1.0" /> \
            </LinearGradientBrush> \
        </Canvas.Background> \
        </Canvas>');
    this.conversationMembers = new SJ.ListView(0, 0, 200, 150);
    this.conversationHistory = new SJ.ListView(0, 0, width - this.conversationMembers.getWidth(), this.conversationMembers.getHeight() - 15);
    this.conversationHistory.setParent(this.conversationSection);
    
    this.conversationMembers.setParent(this.conversationSection);
    
    this.conversationHistory.hideScrollbar();

    var conversationHistoryHeaderLabel = new SJ.TextBlock(0, 0, 200, 15, "Conversation History");
    conversationHistoryHeaderLabel.margin = { left: 5 };
   
    this.conversationHistoryComments = new SJ.ListView(0, 0, this.conversationHistory.getWidth(), 
            this.conversationHistory.getHeight() - (conversationHistoryHeaderLabel.getHeight() + 15));

    this.addCommentStack = new SJ.StackPanel(0, 0, false);

    this.conversationHistory.addItem(conversationHistoryHeaderLabel);
    this.conversationHistory.addItem(this.conversationHistoryComments);
    this.conversationHistory.addItem(this.addCommentStack);
        
    this.ownersHeaderLabel = new SJ.TextBlock(0, 0, 200, 15, "co-owners");
    this.ownersHeaderLabel.margin = { left: 5 };

    this.conversationMembers.ownersList = new SJ.StackPanel(0, 0, true);

    this.contactsHeaderLabel = new SJ.TextBlock(0, 0, 200, 15, "my online contacts");
    this.contactsHeaderLabel.margin = { left: 5 };
    
    this.conversationMembers.contactsList = new SJ.StackPanel(0, 0, true);

    this.header.updateLayout();
    var listHeight = height - this.header.getHeight() - this.conversationHistory.getHeight();
    
    var listPanel = new SJ.FlowPanel(0, 0, 0, 0, "center");
    this.resultsList = new SJ.ListView(0, 0, width, listHeight, listPanel);
    this.resultsList.setParent(stack);
    
    this.conversationSection.setParent(stack);

    this.setContent(stack);
}

SJ.SavedResultsView.prototype = new SJ.ResultsPage;

SJ.SavedResultsView.prototype.toString = function () {

    return "SJ.SavedResultsView";
}

SJ.SavedResultsView.prototype.onAddCommentKeyPressed = function (e) {
	var code;
	if (!e) var e = window.event;
	if (e.keyCode) code = e.keyCode;
	else if (e.which) code = e.which;
	this.lastKeyCode = code;
    if (code == 13) // enter key
    {
        this.addComment();
    }
}

SJ.SavedResultsView.prototype.addComment = function () {
    if (this.addCommentText.getText() == "") { return; }
    WLQuickApps.Tafiti.Scripting.CommentManager.addComment(this.shelfStack.shelfStackID, this.addCommentText.getText());
    this.addCommentText.setText("")
    this.updateConversationHistory();
}

SJ.SavedResultsView.prototype.sizeChanged = function (width,height) {
    var listHeight = height - this.header.getHeight() - this.conversationHistory.getHeight();
    this.resultsList.setHeight(listHeight);
    
    this.conversationSection.resize(width, this.conversationMembers.getHeight());
    this.conversationSection.move(0, height - this.conversationSection.getHeight());
    this.conversationHistory.setWidth(width - this.conversationMembers.getWidth());
    this.conversationMembers.move(this.conversationHistory.getWidth(), 0);
        
    if (this.addCommentText)
    {
        this.refreshAddCommentStack();
    }

    SJ.ResultsPage.prototype.sizeChanged.call(this,width,height);
}

SJ.SavedResultsView.prototype.lineUp = function () {
    this.resultsList.lineUp();
}

SJ.SavedResultsView.prototype.lineDown = function () {
    this.resultsList.lineDown();
}

SJ.SavedResultsView.prototype.pageUp = function () {
    this.resultsList.pageUp();
}

SJ.SavedResultsView.prototype.pageDown = function () {
    this.resultsList.pageDown();
}

SJ.SavedResultsView.prototype.onCloseHandler = function (sender, eventArgs) {
    if (this.onCloseClicked)
        this.onCloseClicked(sender, eventArgs);
}

SJ.SavedResultsView.prototype.setResults = function (shelfStack) {
    var label = shelfStack.label;
    this.resultsList.clearItems();
    this.results = shelfStack.shelfStackItems;
    this.shelfStack = shelfStack;

    this.header.updateInfo(label);
    
    // Render results
    for (var i = 0; i < shelfStack.shelfStackItems.length; i++) {
        var shelfStackItem = shelfStack.shelfStackItems[i];
        var resultView;
        switch (shelfStackItem.domain) {
            case "web":
                resultView = new SJ.Border(16);
                resultView.setBackgroundImage("images/tornpaper.png");
                resultView.setContent(new SJ.WebResultView(shelfStackItem, true));
                resultView.margin = {top: 8, bottom: 8};
                SJ.mixinRemoveButton(resultView, shelfStackItem, 400, 0);
                SJ.mixinOwnerLabel(resultView, shelfStackItem.owner.get_displayName(), 300, resultView.visual.Height - 10);
                break;
            case "images":
                resultView = new SJ.ImageResultView(shelfStackItem);
                resultView.margin = {top: 8, bottom: 8, left: 8, right: 8};
                resultView.vAlign = "center";
                SJ.mixinRemoveButton(resultView, shelfStackItem, resultView.visual.Width - 65, 0);
                SJ.mixinOwnerLabel(resultView, shelfStackItem.owner.get_displayName(), resultView.visual.Width - 200, resultView.visual.Height - 10);
                break;
            case "news":
                resultView = new SJ.NewsResultView(300, shelfStackItem);
                resultView.margin = {top: 8, bottom: 8, left: 8, right: 8};
                SJ.mixinRemoveButton(resultView, shelfStackItem, 400, 0);
                SJ.mixinOwnerLabel(resultView, shelfStackItem.owner.get_displayName(), resultView.visual.Width - 400, resultView.visual.Height - 10);
                break;
            case "feeds":
                resultView = new SJ.FeedResultView(shelfStackItem);
                resultView.margin = {top: 8, bottom: 8};
                SJ.mixinRemoveButton(resultView, shelfStackItem, 300, 0);
                SJ.mixinOwnerLabel(resultView, shelfStackItem.owner.get_displayName(), 0, 300);
                break;
            case "phonebook":
                resultView = new SJ.Border(16);
                resultView.setBackgroundImage("images/tornpaper.png");
                resultView.setContent(new SJ.WebResultView(shelfStackItem, true));
                resultView.margin = {top: 8, bottom: 8};
                SJ.mixinRemoveButton(resultView, shelfStackItem, 400, 0);
                SJ.mixinOwnerLabel(resultView, shelfStackItem.owner.get_displayName(), 300, resultView.visual.Height - 10);
                break;                
            case "query":
                resultView = new SJ.QueryResultView(shelfStackItem.title);
                resultView.margin = {top: 8, bottom: 8};
                resultView.onNavigate = SJ.methodCaller(this, 'onQueryResultViewClickedHandler');
                SJ.mixinRemoveButton(resultView, shelfStackItem, 60, -10);
                break;

        }

        resultView.onRemoveResult = SJ.methodCaller(this, 'removeResult');
        this.resultsList.addItem(resultView);
    }
 
    this.refreshAddCommentStack();
    this.updateConversationHistory();    

    // Sort contacts, display dummy contacts for people you don't know
    this.registeredOwners = new Array();
    if (this.unknownContacts)
        for (var i = 0; i < this.unknownContacts.length; i++)
            this.unknownContacts[i].setParent(null);
    this.unknownContacts = new Array();
    for (var lcv = 0; lcv < this.callOnDisplay.length; lcv++)
        if (this.callOnDisplay[lcv]) this.callOnDisplay[lcv].sortContact();
    var shelfOwners = this.shelfStack.get_owners();
    for (var i = 0; i < shelfOwners.length; i++)
        if (!this.registeredOwners[shelfOwners[i].emailHash] && !shelfOwners[i].get_isLoggedInUser())
            this.unknownContacts.push(new SJ.UnknownContact(shelfOwners[i].get_displayName(), shelfOwners[i].messengerPresenceID, shelfOwners[i].emailHash));
        
    // Jostle everything to update the UI.
    this.conversationMembers.removeItem(this.ownersHeaderLabel);
    this.conversationMembers.removeItem(this.conversationMembers.ownersList);
    this.conversationMembers.removeItem(this.contactsHeaderLabel);
    this.conversationMembers.removeItem(this.conversationMembers.contactsList);        
    this.conversationMembers.addItem(this.ownersHeaderLabel);
    this.conversationMembers.addItem(this.conversationMembers.ownersList);
    this.conversationMembers.addItem(this.contactsHeaderLabel);
    this.conversationMembers.addItem(this.conversationMembers.contactsList);

    this.updateLayout(); 
}

SJ.SavedResultsView.prototype.refreshAddCommentStack = function () {
    // If these elements were in the UI we rip and replace in order to keep the experience smooth.
    if (this.addCommentText)
    {
        this.addCommentText.dispose();
        this.addCommentButton.dispose();
    }
    
    this.addCommentText = new SJ.TextEdit(0, 0, this.conversationHistory.getWidth() - 130, 20, "", true, true, false);
    this.addCommentText.doLayout(); // force create input element so we can use setText()
    this.addCommentText.setInputVisible(true);
    this.addCommentText.setParent(this.addCommentStack);
    this.addCommentText.input.onkeydown = SJ.methodCaller(this, "onAddCommentKeyPressed");
    
    this.addCommentButton = new SJ.Button(this.addCommentText.getWidth() + 5, 0, 100, 20, "Post Comment");
    this.addCommentButton.margin = { right: 5 };
    this.addCommentButton.hAlign = "right";
    this.addCommentButton.onClick = SJ.methodCaller(this, "addComment");
    this.addCommentButton.setParent(this.addCommentStack);
}

SJ.SavedResultsView.prototype.updateConversationHistory = function () {
    // Update conversation window
    if (WLQuickApps.Tafiti.Scripting.Utilities.userIsLoggedIn() && WLQuickApps.Tafiti.Scripting.MessengerManager.get_isSignedIn() && this.shelfStack)
    {
        this.conversationSection.setParent(this);
        this.placeTextEdits();
        
        this.conversationHistoryComments.clearItems();
        var messages = WLQuickApps.Tafiti.Scripting.CommentManager.getCommentsForShelfStack(this.shelfStack.shelfStackID);
        for (var lcv = 0; lcv < messages.length; lcv++)    
        {
            var presenceView = new SJ.ListView(0, 0, this.conversationHistory.getWidth(), 20, null, false);
            presenceView.scroller.setParent(null);
            presenceView.margin = { left: 5, right: 5 }
            
            var presenceImageUrl = "images/Status_Offline.png";
            if ((messages[lcv].get_owner().get_messengerAddress() != null) &&
                (messages[lcv].get_owner().get_messengerAddress().get_isOnline() || messages[lcv].get_owner().get_isLoggedInUser()))
            {
                presenceImageUrl = "images/Status_Online.png";
            }
            
            var presenceImage = new SJ.Image(0, 0, presenceImageUrl, false, false);
            presenceImage.setParent(presenceView);
            
            var headerText = messages[lcv].get_owner().get_displayName() + " said (" + messages[lcv].timestamp.toString() + "):";        
            var headerBlock = new SJ.TextBlock(15, 0, this.conversationHistory.getWidth() - 100, 0, headerText,
                    "TextWrapping='Wrap' FontSize='12' FontWeight='Bold' Foreground='#000000'");
            headerBlock.setParent(presenceView);

            this.conversationHistoryComments.addItem(presenceView);

            var messageBlock = new SJ.TextBlock(0, 0, 200, 0, messages[lcv].text,
                    "TextWrapping='Wrap' FontSize='10' Foreground='#0067a6'");
            messageBlock.margin = { left: 5, right: 5 }
            this.conversationHistoryComments.addItem(messageBlock);            
        }
        
        this.conversationHistoryComments.scrollToBottom();
    }
    else
    {
        this.conversationSection.setParent(null);
    }
}

SJ.SavedResultsView.prototype.removeResult = function (sender, eventArgs) {
    if (this.onRemoveResult)
        this.onRemoveResult(sender, eventArgs);
}

SJ.SavedResultsView.prototype.onQueryResultViewClickedHandler = function (sender, eventArgs) {
    if (this.onSavedQueryNavigate)
        this.onSavedQueryNavigate(sender, eventArgs);
}

// EmailSnapshotDialog

SJ.EmailSnapshotDialog = function (savedResultsHeader) {
    SJ.StackPanel.call(this, 0, 0, true);
    this.visual.Background = "#ffffff";
    
    this.savedResultsHeader = savedResultsHeader;

    if (!userIsAuthenticated) {
        var innerStack = new SJ.StackPanel(0, 0, false);
        innerStack.margin = {top: 50, bottom: 50, left: 64, right: 64};   
        innerStack.hAlign = "center"; 
        innerStack.setParent(this);

        var el;
        el = new SJ.TextBlock(0, 0, 42, 0, "Please", "FontSize='12'");
        el.setParent(innerStack);
        el = new SJ.Hyperlink(0, 5, 45, 0, "sign in", "FontSize='12' Foreground='#1111ff'", userSigninUrl); // FontFamily='Verdana' 
        el.openWithName = "_self";
        el.setParent(innerStack);
        el = new SJ.TextBlock(0, 0, 105, 0, "to use this feature.", "FontSize='12'");
        el.setParent(innerStack);
        
        var cancelBtn = new SJ.Button(0, 0, 80, 20, "Cancel");
        cancelBtn.margin = {right: 12};
        cancelBtn.hAlign = "right";
        cancelBtn.onClick = SJ.methodCaller(this, "cancel");
        cancelBtn.setParent(this);
        return;
    }
    
    var outerStack = new SJ.StackPanel(0, 0, true);
    outerStack.margin = {top: 25, left: 64, right: 25};
    outerStack.setParent(this);
    
    var col1Width = 100;
    var col2Width = 400;
    var fields = [
        // label, member variable (null implied read-only field), default value */
        ["To", "toAddresses"],
        ["Subject", "messageSubject", this.savedResultsHeader.getLabel()],
        ["Message", "messageBody"]
    ];
    for (var i = 0; i < fields.length; i++) {
        var innerStack = new SJ.StackPanel(0, 0, false);
        innerStack.margin = {bottom: 8};
        innerStack.setParent(outerStack);

        var labelStack = new SJ.StackPanel(0, 0, false);
        labelStack.setParent(innerStack);
        labelStack.margin = {right: 10};
        var label = new SJ.TextBlock(0, 0, col1Width, 0, fields[i][0], "FontSize='12' Foreground='#286ace'");
        label.hAlign = 'right';
        label.setParent(labelStack);
        
        if (fields[i][1]) {
            textEdit = new SJ.TextEdit(0, 0, col2Width, (i == 3) ? 200 : 20, "", true, true, (i == 3));
            textEdit.doLayout(); // force create input element so we can use setText()
            if (fields[i].length >= 3)
                textEdit.setText(fields[i][2]);
            textEdit.setParent(innerStack);
            this[fields[i][1]] = textEdit;
        }
        else {
            var label2 = new SJ.TextBlock(0, 0, col2Width, 0, fields[i][2], "FontSize='12'");
            label2.setParent(innerStack);
        }
    }
    
    var buttonStack = new SJ.StackPanel(0, 0, false);
    buttonStack.hAlign = "right";
    buttonStack.setParent(outerStack);
    var cancelBtn = new SJ.Button(0, 0, 80, 20, "Cancel");
    cancelBtn.margin = {right: 12};
    cancelBtn.onClick = SJ.methodCaller(this, "cancel");
    cancelBtn.setParent(buttonStack);
    var postBtn = new SJ.Button(0, 0, 80, 20, "Send");
    postBtn.onClick = SJ.methodCaller(this, "send");
    postBtn.setParent(buttonStack);
}

SJ.EmailSnapshotDialog.prototype = new SJ.StackPanel;

SJ.EmailSnapshotDialog.prototype.toString = function () {
    return "SJ.EmailSnapshotDialog";
}

SJ.EmailSnapshotDialog.prototype.send = function () {
    var doc = {};
    doc.label = this.savedResultsHeader.getLabel();
    doc.shelfSlot = this.savedResultsHeader.getResults();
    
    doc = Sys.Serialization.JavaScriptSerializer.serialize(doc);

    SJ.EmailSnapshot(this.toAddresses.getText().splitAny(',;'), this.messageSubject.getText(), this.messageBody.getText(), doc);
    this.savedResultsHeader.closeEmailSnapshotDialog();
}

SJ.EmailSnapshotDialog.prototype.cancel = function () {
    this.savedResultsHeader.closeEmailSnapshotDialog();
}

// PostToSpaceDialog

SJ.PostToSpaceDialog = function (savedResultsHeader) {
    SJ.StackPanel.call(this, 0, 0, true);
    this.visual.Background = "#ffffff";
    
    this.savedResultsHeader = savedResultsHeader;
    
    var outerStack = new SJ.StackPanel(0, 0, true);
    outerStack.margin = {top: 25, left: 64, right: 25};
    outerStack.setParent(this);
    
    var helpText = new SJ.TextBlock(0, 0, 550, 0, 
        "Tafiti lets you post your stack of results to your Windows Live Space. When you’re done, click “Save” at the bottom of the page.)",
        "TextWrapping='Wrap' FontSize='12' Foreground='#286ace'");
    helpText.margin = {left: -30, bottom: 20};
    helpText.setParent(outerStack);

    var col1Width = 100;
    var col2Width = 400;
    var fields = [
        ["Post title", "postTitle"],
        ["Body", "introduction"]
    ];
    for (var i = 0; i < fields.length; i++) {
        var innerStack = new SJ.StackPanel(0, 0, false);
        innerStack.margin = {bottom: 8};
        innerStack.setParent(outerStack);

        var labelStack = new SJ.StackPanel(0, 0, false);
        labelStack.margin = {right: 10};
        labelStack.setParent(innerStack);
        label = new SJ.TextBlock(0, 0, col1Width, 0, fields[i][0], "FontSize='12' Foreground='#286ace'");
        label.hAlign = 'right';
        label.setParent(labelStack);
        
        textEdit = new SJ.TextEdit(0, 0, col2Width, (i == 3) ? 200 : 20, "", true, true, (i == 3));
        textEdit.setParent(innerStack);
        
        this[fields[i][1]] = textEdit;
    }
    
    var buttonStack = new SJ.StackPanel(0, 0, false);
    buttonStack.hAlign = "right";
    buttonStack.setParent(outerStack);
    var cancelBtn = new SJ.Button(0, 0, 80, 20, "Cancel");
    cancelBtn.margin = {right: 12};
    cancelBtn.onClick = SJ.methodCaller(this, "cancel");
    cancelBtn.setParent(buttonStack);
    var postBtn = new SJ.Button(0, 0, 80, 20, "Post");
    postBtn.onClick = SJ.methodCaller(this, "post");
    postBtn.setParent(buttonStack);
}

SJ.PostToSpaceDialog.prototype = new SJ.StackPanel;

SJ.PostToSpaceDialog.prototype.toString = function () {
    return "SJ.PostToSpaceDialog";
}

SJ.PostToSpaceDialog.prototype.post = function () {
    // TODO: This is broken for all but small shelf stacks.
    var content =
        "<p>" + SJ.xmlEscape(this.introduction.getText()) + "</p>\n" +
        "<hr style='border: 0; color: #888; background-color: #888; height: 2px;' />\n" +
        this.savedResultsHeader.generateHtml() +
        "<hr style='border: 0; color: #888; background-color: #888; height: 2px;' />\n" +
        "<p>This post created in <a href=\"http://www.tafiti.com/\">Tafiti</a>.</p>\n";
    var url = "http://spaces.msn.com/BlogIt.aspx?Title=" + escape(this.postTitle.getText()) + "&SourceURL=" + escape("http://www.tafiti.com")+"&description=" + escape(content);
    alert(url);
    this.savedResultsHeader.closePostDialog();
}

// Remove Live Search highlight markers.

SJ.removeHighlightMarkers = function (text) {
    return text.replace(/[\uE000\uE001]/g, "");
}

SJ.PostToSpaceDialog.prototype.cancel = function () {
    this.savedResultsHeader.closePostDialog();
}

// HTML result rendering

SJ.RenderResultAsHtml = function (result) {
    var html;
    switch (result.domain) {
        case "web":
            html = "<table><tr><td width='200px'><div style='color: #0067a6; font-weight: bold;'>" + SJ.xmlEscape(result.title) + "</div>";
            var matches = result.url.match(/:\/\/([^/]+)\//);
            if (matches) {
                var host = matches[1];
                html += "<a style='color: #37a100;' href='" + SJ.xmlEscape(result.url) + "'>" + SJ.xmlEscape(host) + "</a>";
            }
            html += "</td><td><div>" + SJ.xmlEscape(result.description) + "</div></td></tr></table>";
            break;
        case "images":
            html = "<div align='center'><a href='" + SJ.xmlEscape(result.url) +
                "'><img src='" + SJ.xmlEscape(result.imageUrl) + "'></a></div>";
            break;
        case "news":
            html = "<div style='font-size: 120%; color: #0067a6;'>" + SJ.xmlEscape(result.title) + "</div>" +
                "<div>" + SJ.xmlEscape(result.description) + "</div>" +
                "<div><a style='color: #37a100;' href='" + SJ.xmlEscape(result.url) + "'>" + SJ.xmlEscape(result.source) + "</a></div>";
            break;
        case "feeds":
            html = "<div style='font-size: 120%; color: #0067a6;'>" + SJ.xmlEscape(result.title) + "</div>" +
                "<div>" + SJ.xmlEscape(result.description) + "</div>" +
                "<div style='background: #e0e0e0'><img src='http://sc2.sclive.net/00.00.0000.0000/Web/parts/RSSPart/images/thumb_rss.gif'/>" +
                "&nbsp;<a style='color: #0067a6;' href='" + SJ.xmlEscape(result.url) + "'>Subscribe</a></div>";
            break;
        case "phonebook":
            html = "<table><tr><td width='200px'><div style='color: #0067a6; font-weight: bold;'>" + SJ.xmlEscape(result.title) + "</div>";
            var matches = result.url.match(/:\/\/([^/]+)\//);
            if (matches) {
                var host = matches[1];
                html += "<a style='color: #37a100;' href='" + SJ.xmlEscape(result.url) + "'>" + SJ.xmlEscape(host) + "</a>";
            }
            html += "</td><td><div>" + SJ.xmlEscape(result.description) + "</div></td></tr></table>";
            break;
        default:
            html = "";
            break;
    }
    
    html = SJ.removeHighlightMarkers(html);
    
    return html;
}

// PostToBlog

SJ.PostToBlog = function (spaceName, secretWord, title, content) {
    var xmlrpcMessage =
        "<methodCall> \
            <methodName>metaWeblog.newPost</methodName> \
            <params> \
                <param><value><string>MyBlog</string></value></param> \
                <param><value><string>" + SJ.xmlEscape(spaceName) + "</string></value></param> \
                <param><value><string>" + SJ.xmlEscape(secretWord) + "</string></value></param> \
                <param><value><struct> \
                    <member><name>title</name><value><string>" + SJ.xmlEscape(title) + "</string></value></member> \
                    <member><name>description</name><value><string>" + SJ.xmlEscape(content) + "</string></value></member> \
                </struct></value></param> \
                <param><value><boolean>1</boolean></value></param> \
            </params> \
        </methodCall>";        
    SJ.AsyncRequest("POST", "PostToSpace.aspx", xmlrpcMessage, SJ.OnPostToBlogCompleted);
}

SJ.OnPostToBlogCompleted = function (result) {
    if (!result.succeeded) {
        // TODO: handle error
    }
}

// EmailSnapshot

SJ.EmailSnapshot = function (toAddresses, subject, body, shelfSlotContent) {
    var doc = {};
    doc.To = toAddresses;
    doc.Subject = subject;
    doc.Message = body;
    doc.ShelfStackID = slotShowing.shelfStackID;
    doc = Sys.Serialization.JavaScriptSerializer.serialize(doc);
    return SJ.AsyncRequest("POST", "Snapshot/Email.aspx", doc, SJ.OnEmailSnapshotCompleted);
}

SJ.OnEmailSnapshotCompleted = function (result) {
    if (!result.succeeded) {
        // TODO: handle errors
    }
}

// Shelf

SJ.Shelf = function (left, top, width, height) {
    SJ.Control.call(this);
    
    this.height = height;
    this.width = width;
    
    var xaml = 
        '<Canvas> \
            <Image Source="images/Glass_shelf.png" /> \
        </Canvas>';
    
    this.visual = SJ.createFromXaml(xaml);
    this.surface = this.visual.Children.GetItem(0);
    
    SJ.placeElement(this.visual, left, top, width, height);
    
    this.slots = [];
    
    this.enabled = true;
    this.statusText = new SJ.TextBlock(60,0,0,0,'','Foreground="#ffffff" TextWrapping="NoWrap" FontSize="10"');
    this.statusText.setParent(this);
    
    this.placeTextEdits();

    this.hookUpEvent("MouseEnter");
    this.hookUpEvent("MouseLeave");
    
    this.onShelfMouseEnter = null;
    
    this.selectionIndicator = new SJ.Image(3, this.height / 5 / 2 - 10, "images/arrow_shelf_indicator.png");
    this.updateAddShelfStackButton();
}

SJ.Shelf.prototype = new SJ.Control;

SJ.Shelf.prototype.toString = function () {
    return "SJ.Shelf";
}

SJ.Shelf.prototype.selectSlot = function (slot) {
    this.selectionIndicator.setParent(slot);
}

SJ.Shelf.prototype.isEmpty = function () {
    for (var i = 0; i < this.slots.length; i++) {
        if (!this.slots[i].isEmpty())
            return false;
    }
    return true;
}

SJ.Shelf.prototype.notifyFullScreen = function (sender, args) {
    if (this.onFullScreen)
        this.onFullScreen(sender, args);
}

SJ.Shelf.prototype.notifyEditLabelActive = function (sender, args) {
    for (var i = 0; i < this.slots.length; i++) {
        var slot = this.slots[i];
        if (slot != sender)
            slot.hideLabel();
    }
}

SJ.Shelf.prototype.MouseEnter = function (sender, eventArgs) {
    if (this.onShelfMouseEnter)
        this.onShelfMouseEnter(this, eventArgs);
}

SJ.Shelf.prototype.MouseLeave = function (sender, eventArgs) {
}

SJ.Shelf.prototype.deserialize = function (shelfStacks) {
// We'll let the Interop callback from Script# add initial shelves    
//    for (var i = 0; i < shelfStacks.length; i++) 
//    {
//        this.addShelfStack(shelfStacks[i]);
//    }
}

SJ.Shelf.prototype.removeSlot = function (slot)
{
    var lcv;
    for (lcv = 0; lcv < this.slots.length; lcv++)
    {
        if (this.slots[lcv] == slot)
        {
            for (var adjustLcv = this.slots.length - 1; adjustLcv > lcv; adjustLcv--)
            {
                this.slots[adjustLcv].visual["Canvas.Top"] = this.slots[adjustLcv - 1].visual["Canvas.Top"];
            }
            slot.setParent(null);
            this.slots.remove(slot);
            
        }
    }
    
    this.updateAddShelfStackButton();
}

SJ.Shelf.prototype.addEmptyShelfStack = function (sender, eventArgs)
{
    // Add a shelf stack async. When this gets pulled down we'll get updated and it will be added.
    WLQuickApps.Tafiti.Scripting.ShelfStackManager.beginAddShelfStack("New Shelf")
    
    hideShelfFirstExperience();
}

SJ.Shelf.prototype.updateAddShelfStackButton = function (sender, eventArgs)
{
    var items = 5;

    if (!this.addShelfStackButton)
    {
        this.addShelfStackButton = new SJ.Button(10, ((this.height / items) * this.slots.length) + 10, 60, 0, "+ Add Stack");
        this.addShelfStackButton.onClick = SJ.methodCaller(this, "addEmptyShelfStack");
        this.addShelfStackButton.label.FontSize = '15';
        this.addShelfStackButton.label.Foreground = '#ffffff';
        this.addShelfStackButton.MouseEnter = function(sender, eventArgs) { SJ_ShowToolTip(this.visual, "Click here to add a new stack.", eventArgs.getPosition(SJ.topCanvas)); }
        this.addShelfStackButton.MouseLeave = function(sender, eventArgs) { SJ_HideToolTip(); }
    }
    else
    {
        this.addShelfStackButton.visual["Canvas.Top"] = ((this.height / items) * this.slots.length) + 10;
    }
   
    if (this.slots.length < items)
    {
        this.addShelfStackButton.setParent(this);
    }
    else
    {
        this.addShelfStackButton.setParent(null);
    }
}

SJ.Shelf.prototype.addShelfStack = function (shelfStack)
{
    if (this.slots.length > 5) { return; }
    
    // TODO: Right now we're hardcoding this to make things line up as expected. To fix this, we would need
    // to change the UI to allow for dynamic stacks (rather than having 5 stacks baked into the background).
    var size = Math.max(this.slots.length, 5);        

    var slot = new SJ.ShelfSlot(0, (this.height / size) * this.slots.length, this.width, this.height / size);    
    slot.setParent(this);
    this.slots.push(slot);
    slot.deserialize(shelfStack);

    this.updateAddShelfStackButton();
}

SJ.Shelf.prototype.onSaveResult = function (result) {
    this.pendingRequest = false;
    if (!result.succeeded)
        SJ.log("Failed to save shelf.");
        
    if (result.userState)
        result.userState(this, result);
}

SJ.Shelf.prototype.load = function(callback) {

    this.deserialize(WLQuickApps.Tafiti.Scripting.ShelfStackManager.get_myShelfStacks());
    
    if (callback)
            callback(this, null);
}

SJ.Shelf.prototype.loadShelfStacks = function (shelfStacks, callback) {
    this.pendingRequest = false;
    this.deserialize(shelfStacks);
    
    if (callback)
        callback(this, null);
}

// Shelf slot

SJ.ShelfSlot = function (left, top, width, height) {
    SJ.Control.call(this);
    
    // Must set Background so we get mouse events
    var xaml =
        '<Canvas Background="#00000000" />';
    var xamlOnDragHover =
        '<Image Source="images/addToShelf.png" />';
    
    this.visual = SJ.createFromXaml(xaml, this.names);
    this.onDragHover = SJ.createFromXaml(xamlOnDragHover);
    
    this.label = new SJ.LabelEdit(0, 0, 120, 20, "type to label stack", {left: 15, right: 15, top: 14, bottom: 30});
    this.label.setParent(null);
    this.label.setBorderBackgroundImage( {idle: "images/label_finished.png", hover: "images/label_glow.png"} );
    this.label.labelBorder.visual.Opacity = 0.8;
    this.label.onTextChanged = SJ.methodCaller(this, "onLabelChanged");
    this.label.onInputFocus = SJ.methodCaller(this, "onLabelInputFocus");
    this.label.onInputBlur = SJ.methodCaller(this, "onLabelInputBlur");
    
    this.clearBtn = new SJ.Button(160, 10, 15, 14, "",
                            {idle: "images/Clear_Shelf_X.png", hover: "images/Clear_Shelf_X.png",
                             activeDown: "images/Clear_Shelf_X.png", activeUp: "images/Clear_Shelf_X.png"});
    this.clearBtn.onClick = SJ.methodCaller(this, "removeAll");
    this.clearBtn.MouseEnter = function(sender, eventArgs) { SJ_ShowToolTip(this.visual, "Click here to leave this stack.", eventArgs.getPosition(SJ.topCanvas)); }
    this.clearBtn.MouseLeave = function(sender, eventArgs) { SJ_HideToolTip(); }

    this.fullScreenBtn = new SJ.FullScreenButton(130, 3);
    this.fullScreenBtn.onClick = SJ.methodCaller(this, "onFullScreen");
    
    SJ.placeElement(this.visual, left, top, width, height);
    SJ.placeElement(this.onDragHover, width - 40, 15, 20, 20);
    SJ.placeElement(this.label.visual, width/2 - 65, height/2 - 20);
    
    SJ.makeDroppable(this);
    
    this.hookUpEvent("MouseLeftButtonDown");
    this.hookUpEvent("MouseEnter");
    this.hookUpEvent("MouseLeave");
    
    // It seems we can't get into the TransformGroup to see the current
    // rotation, so we have to keep track of them separately.
    // Each entry is {item, visual, rotation}
    this.items = [];
}

SJ.ShelfSlot.prototype = new SJ.Control;

SJ.ShelfSlot.prototype.toString = function () {
    return "SJ.ShelfSlot";
}

SJ.ShelfSlot.prototype.isEmpty = function () {
    return this.items.length == 0;
}

SJ.ShelfSlot.prototype.getQueries = function () {
    var result = [];
    for (var i = 0; i < this.items.length; i++) {
        var item = this.items[i].item;
        if (item.query)
            result.push(item.query);
    }
    return (result.length > 0) ? result : null;
}

SJ.ShelfSlot.prototype.onFullScreen = function () {
    this.getParent().notifyFullScreen(this, null);
}

SJ.ShelfSlot.prototype.onLabelChanged = function () {
    
    var labelText = this.label.getText();
    
    if (savedResultsPanel && (slotShowing == this))
    {
        savedResultsPanel.header.updateInfo(labelText);
    }
    
    WLQuickApps.Tafiti.Scripting.ShelfStackManager.setShelfStackLabel(this.shelfStackID, labelText);
}

SJ.ShelfSlot.prototype.onLabelInputFocus = function () {
    this.label.labelBorder.visual.Opacity = 1.0;
    this.getParent().notifyEditLabelActive(this, null);
}

SJ.ShelfSlot.prototype.onLabelInputBlur = function () {
    this.label.labelBorder.visual.Opacity = 0.8;
}

SJ.ShelfSlot.prototype.showLabel = function () {
    this.label.setParent(this);
}

SJ.ShelfSlot.prototype.hideLabel = function () {
    this.label.setParent(null);
}

SJ.ShelfSlot.prototype.MouseEnter = function (sender, eventArgs) {
    if (!this.label.hasInputFocus())
        this.showLabel();
    if (!SJ.dragDrop.dragging) {
        this.clearBtn.setParent(this);
        if (this.getQueries() != null)
            this.fullScreenBtn.setParent(this);
    }
}

SJ.ShelfSlot.prototype.MouseLeave = function (sender, eventArgs) {
    if (!this.label.hasInputFocus())
        this.hideLabel();
    this.clearBtn.setParent(null);
    this.fullScreenBtn.setParent(null);
}

SJ.ShelfSlot.prototype.deserialize = function (shelfStack) {
    var centerX = this.visual.Width / 2 - 48;
    var centerY = this.visual.Height / 2 - 48;
    
    var items = shelfStack.shelfStackItems;
    for (var j = 0; j < items.getLength(); j++) {
        var visual = SJ.GetDragVisual(items[j].domain);
        this.addItemCore(items[j], visual, centerX, centerY);
    }
    
    this.shelfStackID = shelfStack.shelfStackID;
    this.label.setText(shelfStack.label);
    
    this.fixStack(false);
}

SJ.ShelfSlot.prototype.exists = function (newItem) {
    for (var i = 0; i < this.items.length; i++) {
        var item = this.items[i].item;
        if (newItem.query) {
            if (item.query && item.query == newItem.query) {
                return true;
            }
        }
        else if (newItem.domain && item.domain == newItem.domain) {
            if (item.url == newItem.url) {
                return true;
            }        
        }
    }
    return false;
}

SJ.ShelfSlot.prototype.addItem = function (item, visual, left, top) {
    if (!this.exists(item)) {
        if (item.user)
        {
            WLQuickApps.Tafiti.Scripting.ShelfStackManager.addUserToShelfStack(this.shelfStackID, item.user.emailHash);
        }
        else
        {
            if (item.query)
            {
                item.shelfStackItemID = WLQuickApps.Tafiti.Scripting.ShelfStackManager.beginAddShelfStackItem(this.shelfStackID, "query", item.query, 
                        "", "", "", 0, 0, "");                        
            }
            else
            {
                switch (item.domain)
                {
                    case "images":
                        item.shelfStackItemID = WLQuickApps.Tafiti.Scripting.ShelfStackManager.beginAddShelfStackItem(this.shelfStackID, item.domain, "", 
                            "", item.url, item.imageUrl, item.width, item.height, "");
                        break;

                    case "news":
                        item.shelfStackItemID = WLQuickApps.Tafiti.Scripting.ShelfStackManager.beginAddShelfStackItem(this.shelfStackID, item.domain, item.title, 
                            item.description, item.url, "", 0, 0, item.source);
                        break;
                    
                    default:
                        item.shelfStackItemID = WLQuickApps.Tafiti.Scripting.ShelfStackManager.beginAddShelfStackItem(this.shelfStackID, item.domain, item.title, 
                            item.description, item.url, "", 0, 0, "");
                        break;
                }
            }
        }                
    }
}

SJ.ShelfSlot.prototype.addItemCore = function (item, visual, left, top) {
    this.items.push({item: item, visual: visual, rotation: 0, shelfStackItemID: item.shelfStackItemID});
    SJ.placeElement(visual, left, top);
    // Insert on top, but behind label if present
    if (this.label.parentControl == null)
        this.visual.Children.Add(visual);
    else
        this.visual.Children.Insert(this.visual.Children.Count - 1, visual);
}

SJ.ShelfSlot.prototype.removeItem = function (item) 
{
    WLQuickApps.Tafiti.Scripting.ShelfStackManager.removeShelfStackItem(item.shelfStackItemID);
}

SJ.ShelfSlot.prototype.removeItemVisual = function (item) 
{
    for (var i = 0; i < this.items.length; i++) {
        if (this.items[i].item.shelfStackItemID == item.shelfStackItemID) {
            this.visual.Children.Remove(this.items[i].visual);
            this.items.splice(i, 1);
            this.fixStack(false);
            return true; 
        }
    }
    return false;
}

SJ.ShelfSlot.prototype.removeAll = function() {
    var answer = confirm("Are you sure, you want to delete the shelf ? ")
    if (answer != "0") 
    {
        SJ_HideToolTip();
        this.removeAllCore();
        if (this.shelfStackID)
            WLQuickApps.Tafiti.Scripting.ShelfStackManager.leaveShelfStack(this.shelfStackID);

        this.getParent().removeSlot(this);
        
        if ((this == slotShowing) && savedResultsPanel && savedResultsPanel.onCloseClicked)
        {
            savedResultsPanel.onCloseClicked(this, null);
        }
    }
}

SJ.ShelfSlot.prototype.removeAllCore = function() {
    for (var i = 0; i < this.items.length; i++)
        this.visual.Children.Remove(this.items[i].visual);
    this.items = [];
    this.label.setText('');
}

SJ.ShelfSlot.prototype.fixStack = function (scaleTopItem) {
    // Unfortunately we may do this before the images have loaded so we have to know the image sizes.
    
    var angle = 0;
    var centerX = this.visual.Width / 2 - 48;
    var centerY = this.visual.Height / 2 - 48;
    
    var iTopItem = this.items.length - 1;
    for (var i = iTopItem; i >= 0 ; i--) {
        var child = this.items[i].visual;
        var childCenterX = 48;
        var childCenterY = 48;
        
        var names = {};
        var xaml =
            "<TransformGroup xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'> \
                <RotateTransform x:Name='%rotate%' Angle='" + this.items[i].rotation + "' \
                    CenterX='" + childCenterX + "' CenterY='" + childCenterY + "' /> \
                <TranslateTransform x:Name='%translate%' /> \
                <ScaleTransform x:Name='%scale%' /> \
            </TransformGroup>";
        child.RenderTransform = SJ.createFromXaml(xaml, names);
        
        if (child["Canvas.Left"] != centerX || child["Canvas.Top"] != centerY) {
            var storyboardNames = {};
            var aniXaml =
                "<Storyboard xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml' x:Name='%name%'>  \
                    <DoubleAnimation Storyboard.TargetName='" + names["translate"] + "' Storyboard.TargetProperty='X' \
                        Duration='0:0:0.5' From='" + (child["Canvas.Left"] - centerX) + "' To='0' /> \
                    <DoubleAnimation Storyboard.TargetName='" + names["translate"] + "' Storyboard.TargetProperty='Y' \
                        Duration='0:0:0.5' From='" + (child["Canvas.Top"] - centerY) + "' To='0' /> \
                </Storyboard>";
            var animation = SJ.createFromXaml(aniXaml, storyboardNames);
            child.Resources.Add(animation);
            SJ.placeElement(child, centerX, centerY);
            animation.Begin();
        }

        if (this.items[i].rotation != angle) {
            var storyboardNames = {};
            var aniXaml =
                "<Storyboard xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml' x:Name='%name%'>  \
                    <DoubleAnimation Storyboard.TargetName='" + names["rotate"] + "' Storyboard.TargetProperty='Angle' \
                        Duration='0:0:0.5' From='" + this.items[i].rotation + "' To='" + angle + "' /> \
                </Storyboard>";
            var animation = SJ.createFromXaml(aniXaml, storyboardNames);
            child.Resources.Add(animation);
            animation.Begin();
        }
        
        if (scaleTopItem && i == iTopItem) {
            var storyboardNames = {};
            var aniXaml =
                "<Storyboard xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml' x:Name='%name%'>  \
                    <DoubleAnimation Storyboard.TargetName='" + names["scale"] + "' Storyboard.TargetProperty='ScaleX' \
                        Duration='0:0:0.5' From='0.5' To='1' /> \
                    <DoubleAnimation Storyboard.TargetName='" + names["scale"] + "' Storyboard.TargetProperty='ScaleY' \
                        Duration='0:0:0.5' From='0.5' To='1' /> \
                </Storyboard>";
            var animation = SJ.createFromXaml(aniXaml, storyboardNames);
            child.Resources.Add(animation);
            animation.Begin();
        }

        this.items[i].rotation = angle;
        
        angle += 15;
    }
}

SJ.ShelfSlot.prototype.OnDragDropMouseEnter = function (sender, eventArgs) {
    if (this.getParent().enabled)
        this.visual.Children.Add(this.onDragHover);
}

SJ.ShelfSlot.prototype.OnDragDropMouseLeave = function (sender, eventArgs) {
    this.visual.Children.Remove(this.onDragHover);
}

SJ.ShelfSlot.prototype.OnDragDropItemDropped = function (sender, eventArgs) {
    SJ.endDragDrop();
    this.visual.Children.Remove(this.onDragHover);
    if (this.getParent().enabled && eventArgs.item) {
        var mousePos = eventArgs.mouseEventArgs.getPosition(this.visual);
        
        this.addItem(eventArgs.item, eventArgs.dragImage, mousePos.x+1, mousePos.y+1);
    }
}

SJ.ShelfSlot.prototype.MouseLeftButtonDown = function (sender, eventArgs) {
    if (!SJ.cancelBubble) {
        SJ.cancelBubble = true;
        var shelf = this.parentControl;
        if (shelf && shelf.onSlotClick) {
            var results = [];
            for (var i = 0; i < this.items.length; i++)
                results.push(this.items[i].item);
            shelf.onSlotClick(this, { results: results });
        }
    }
}

// Result Aggregator

SJ.ResultAggregator = function () {
    this.resultsPerQuery = 50;
    this.queries = [];
}

SJ.ResultAggregator.prototype.toString = function () {
    return "SJ.ResultAggregator";
}

SJ.ResultAggregator.prototype.setQueries = function (queries) {
    this.queries = queries;
    this.page = 0;
    this.client = new SearchClient();
}

SJ.ResultAggregator.prototype.getMoreResults = function (callback) {
    if (this.pending || (this.queries.length == 0))
        return;
        
    this.pending = true;
    this.callback = callback;

    var query = this.queries[this.page % this.queries.length];
    var offset = Math.floor(this.page / this.queries.length) * this.resultsPerQuery;
    this.client.search("web", query, offset, this.resultsPerQuery,
                       SJ.methodCaller(this, "onQueryCompleted"));
    this.page++;
}

SJ.ResultAggregator.prototype.onQueryCompleted = function (result) {
    var results = [];
    this.pending = false;
    if (result.succeeded) {
        var response = null;
        try {
            response = eval( '(' + result.response + ')' );
        } catch (e) { }
        if (response && response.searchresult && response.searchresult.documentset) {
            var docsetList = response.searchresult.documentset ;
            var docset = null;
            if (docsetList._source && docsetList._source == "FEDERATOR_MONARCH") {
                docset = docsetList;
            }
            else {
                for (var i = 0; i < docsetList.length; i++) {
                    if (docsetList[i]._source == "FEDERATOR_MONARCH") {
                        docset = docsetList[i];
                        break; 
                    }
                }
            }
            
            if (docset && docset.document)
                results = docset.document;
        }
    }
  
    if (this.callback)
        this.callback(this, results);
}

// Workaround a Silverlight bug to change the maxFrameRate property 
// on mouse enter/leave since changing this property in the click
// handler doesn't work.

SJ.FullScreenButton = function (left, top, showGleamAnimation) {
    SJ.Control.call(this);

    var xaml = "<Canvas></Canvas>";
    this.visual = SJ.createFromXaml(xaml);

    this.treeBtn = new SJ.Button(0, 0, 29, 32, "Tree View", 
                {idle: "images/tree.png", hover: "images/tree.png",
                 activeDown: "images/tree.png", activeUp: "images/tree.png"});
    this.treeBtn.labelAlign = "left";
    this.treeBtn.label.FontSize = '13';
    this.treeBtn.label.Foreground = '#0067a6';
    this.treeBtn.onClick = SJ.methodCaller(this, "onMouseClick");
    this.treeBtn.onMouseEnter = SJ.methodCaller(this, "onMouseEnter");
    this.treeBtn.onMouseLeave = SJ.methodCaller(this, "onMouseLeave");
    this.treeBtn.setParent(this);
    this.treeBtn.doLayout();
    
    if (showGleamAnimation) {
        this.gleam = new SJ.AnimatedImage(0,0,174,32,"images/tree_gleam_strip.png",6,75);
        this.gleam.visual.IsHitTestVisible = false;
        this.gleam.setParent(this);
        this.gleam.timer = setTimeout(SJ.methodCaller(this, "onAnimateTimer"), 15000);
    }

    SJ.placeElement(this.visual, left, top);
}

SJ.FullScreenButton.prototype = new SJ.Control;

SJ.FullScreenButton.prototype.toString = function () {
    return "SJ.FullScreenButton";
}

SJ.FullScreenButton.prototype.onAnimateTimer = function () {
    this.gleam.animate();
    this.gleam.timer = setTimeout(SJ.methodCaller(this, "onAnimateTimer"), 15000);
}

SJ.FullScreenButton.prototype.dispose = function () {
    if (this.gleam && this.gleam.timer)
        clearTimeout(this.gleam.timer);
    SJ.Control.prototype.dispose.call(this);
}

SJ.FullScreenButton.prototype.showLabel = function (show) {
    this.treeBtn.setLabel(show ? "Tree View" : "");
}

SJ.FullScreenButton.prototype.onMouseClick = function (sender, eventArgs) {
    this.clicked = true;
    if (this.onClick)
        this.onClick(this, eventArgs);
}

SJ.FullScreenButton.prototype.onMouseEnter = function (sender, eventArgs) {
    this.clicked = false;
    SJ.wpfeHost.settings.maxFrameRate = 8;
}

SJ.FullScreenButton.prototype.onMouseLeave = function (sender, eventArgs) {
    if (!this.clicked)
        SJ.wpfeHost.settings.maxFrameRate = 64;
}

SJ.UnknownContact = function(displayName, messengerPresenceID, emailHash) {
    SJ.StackPanel.call(this, 0, 0);
    
    this.emailHash = emailHash;
    
    this.margin = { left: 10, bottom: 15, top: 5};
    
    this.messengerPresenceID = messengerPresenceID;
    this.unknownPresenceImage = new SJ.Image(0, 0, "images/Status_Unknown.png", false, false);
    this.unknownPresenceImage.setParent(this);
    this.displayNameLabel = new SJ.TextBlock(15, 0, 175, 15, SJ.xmlEscape(displayName || "Unknown"), "FontSize='11'");
    this.displayNameLabel.margin = { left: 15 };
    this.displayNameLabel.ellipsis = true;
    this.displayNameLabel.setParent(this);
    
    if (this.messengerPresenceID.length > 0)
    {
        this.visual.Cursor = "Hand";
    }
    
    this.hAlign = "left";
    this.names["text"] = this.displayNameLabel.names["top"];
    
    this.setParent(savedResultsPanel.conversationMembers.ownersList);
    savedResultsPanel.conversationMembers.contactsList.invalidateLayout();
    savedResultsPanel.conversationMembers.contactsList.updateLayout();
    savedResultsPanel.conversationMembers.invalidateLayout();
    savedResultsPanel.conversationMembers.updateLayout();
    
    this.hookUpEvent("MouseLeftButtonUp");
    this.hookUpEvent("MouseEnter");
    this.hookUpEvent("MouseLeave");
}

SJ.UnknownContact.prototype = new SJ.Control;

SJ.UnknownContact.prototype.MouseEnter = function(sender, eventArgs) {
    if (this.messengerPresenceID.length > 0)
    {
        SJ_ShowToolTip(this.visual, "Click this contact to begin an IM<LineBreak/>conversation.", eventArgs.getPosition(SJ.topCanvas));
    }
}

SJ.UnknownContact.prototype.MouseLeave = function(sender, eventArgs) {
    SJ_HideToolTip();
}

SJ.UnknownContact.prototype.MouseLeftButtonUp = function(sender, eventArgs) {
    if (this.messengerPresenceID)
        SJ.openWindow("http://settings.messenger.live.com/Conversation/IMMe.aspx?invitee=" + this.messengerPresenceID);
}

SJ.UnknownContact.prototype.toString = function() {
    return "SJ.UnknownContact";
}

SJ.ContactVisual = function (tafitiUser) {
    SJ.StackPanel.call(this, 0, 0);

    savedResultsPanel.callOnDisplay.push(this);
    
    if (savedResultsPanel.unknownContacts)
    {
        for (var lcv = 0; lcv < savedResultsPanel.unknownContacts.length; lcv++)
        {
            if (savedResultsPanel.unknownContacts.emailHash = tafitiUser.emailHash)
            {
                savedResultsPanel.unknownContacts[lcv].setParent(null);
                savedResultsPanel.unknownContacts.splice(lcv, 1);
                break;
            }
        }
    }

    this.user = tafitiUser;
    this.isOnline = tafitiUser.get_messengerAddress().get_isOnline();
    this.emailHash = tafitiUser.emailHash;
    this.displayName = tafitiUser.get_displayName() || "unknown";
    this.isOwner = false;
        
    this.margin = { left: 10, bottom: 15, top: 5 };
    
    this.offlinePresenceImage = new SJ.Image(0, 0, "images/Status_Offline.png", false, false);
    this.onlinePresenceImage = new SJ.Image(0, 0, "images/Status_Online.png", false, false);
    this.offlinePresenceImage.setParent(this);
    this.displayNameLabel = new SJ.TextBlock(15, 0, 175, 15, SJ.xmlEscape(this.displayName), "FontSize='11'");
    this.displayNameLabel.margin = { left: 15 };
    this.displayNameLabel.ellipsis = true;
    this.displayNameLabel.setParent(this);
    
    this.hAlign = "left";
    this.names["text"] = this.displayNameLabel.names["top"];
    
    this.hookUpEvent("MouseEnter");
    this.hookUpEvent("MouseLeave");
    this.hookUpEvent("MouseLeftButtonDown");
    this.hookUpEvent("MouseLeftButtonUp");
    this.updateUI();
}

SJ.ContactVisual.prototype = new SJ.Control;

SJ.ContactVisual.prototype.MouseEnter = function(sender, eventArgs) {
    SJ_ShowToolTip(this.visual, "Drag this contact onto a stack to add<LineBreak/>them as an owner or click on them<LineBreak/>to begin an IM conversation.", eventArgs.getPosition(SJ.topCanvas));
}

SJ.ContactVisual.prototype.MouseLeave = function(sender, eventArgs) {
    SJ_HideToolTip();
}

SJ.ContactVisual.prototype.sortContact = function() {
    
    this.isOwner = false;
    
    this.visual.Cursor = this.isOnline ? "Hand" : "Arrow";

    if (!slotShowing || !slotShowing.shelfStackID) return;

    var shelfStack = WLQuickApps.Tafiti.Scripting.ShelfStackManager.getShelfStack(slotShowing.shelfStackID);
    if (!shelfStack) return;

    for (var lcv = 0; lcv < shelfStack.get_owners().length; lcv++)
    {
        if (shelfStack.get_owners()[lcv].emailHash == this.emailHash)
        {
            this.isOwner = true;
            savedResultsPanel.registeredOwners[this.emailHash] = true;
            break;
        }
    }
    
    this.updateUI();
}

SJ.ContactVisual.prototype.remove = function() {
    for (var lcv = 0; lcv < savedResultsPanel.callOnDisplay.length; lcv++)
    {
        if (savedResultsPanel.callOnDisplay[lcv] == this)
        {
            savedResultsPanel.callOnDisplay.removeAt(lcv);
            return;
        }
    }
}

SJ.ContactVisual.prototype.updateUI = function() {
    this.displayNameLabel.setText(SJ.xmlEscape(this.displayName));
    this.offlinePresenceImage.setParent(this.isOnline ? null : this);
    this.onlinePresenceImage.setParent(this.isOnline ? this : null);
    
    if (this.isOwner) {
        this.setParent(savedResultsPanel.conversationMembers.ownersList);
        savedResultsPanel.conversationMembers.ownersList.invalidateLayout();
        savedResultsPanel.conversationMembers.ownersList.updateLayout();
    }
    else if (this.isOnline) {
        this.setParent(savedResultsPanel.conversationMembers.contactsList);
        savedResultsPanel.conversationMembers.contactsList.invalidateLayout();
        savedResultsPanel.conversationMembers.contactsList.updateLayout();
    }
    else
        this.setParent(null);
        
    savedResultsPanel.conversationMembers.invalidateLayout();
    savedResultsPanel.conversationMembers.updateLayout();
    
    this.updateLayout();
    
}

SJ.ContactVisual.prototype.toString = function () {
    return "SJ.ContactVisual";
}

SJ.ContactVisual.prototype.MouseLeftButtonDown = function(sender, eventArgs) {
    var dragImage = SJ.GetDragVisual('contact');
    SJ.beginDragDrop({ user: this.user }, dragImage, eventArgs);
}

SJ.ContactVisual.prototype.MouseLeftButtonUp = function(sender, eventArgs) {
    if (this.isOnline)
    {
        openIMWindow(this.emailHash);
    }
}

SJ.mixinOwnerLabel = function (obj, name, left, top) {
    obj.labelPanel = new SJ.StackPanel(left, top, true);
    obj.ownerLabel = new SJ.TextBlock(0, 0, 180, 20, SJ.xmlEscape("Added by " + (name ? name : "you")), "FontSize='12'");
    obj.ownerLabel.ellipsis = true;
    obj.ownerLabel.setParent(obj.labelPanel);
    obj.border = new SJ.Border(4);
    obj.border.setBackgroundImage("images/createdby.png");
    obj.border.setContent(obj.ownerLabel);
    obj.border.setParent(obj.labelPanel);
    obj.labelPanel.setParent(obj);
}

SJ.Interop = function()
{
}

SJ.Interop.prototype.updateShelfStack = function (shelfStack) {

    if (typeof(shelf) == "undefined")
    {
        return;
    }
    
    if (viewMode != ViewMode.Standard)
    {
        changeViewMode(ViewMode.Standard);
        hideShelfFirstExperience();
    }
    
    var shelfFound = false;
    for (var lcv = 0; lcv < shelf.slots.length; lcv++)
    {
        var shelfSlot = shelf.slots[lcv];
        if (shelfSlot.shelfStackID == shelfStack.shelfStackID)
        {
            shelfFound = true;
            
            var isStackShelfVisible = (shelfSlot == slotShowing);
            
            var shelfItemDictionary = {};
            for (var shelfItemLcv = 0; shelfItemLcv < shelfStack.shelfStackItems.length; shelfItemLcv++)
            {
                shelfItemDictionary[shelfStack.shelfStackItems[shelfItemLcv].shelfStackItemID] = shelfStack.shelfStackItems[shelfItemLcv];
            }

            var existingShelfItems = {};                
            for (var shelfItemLcv = shelfSlot.items.length - 1; shelfItemLcv >= 0; shelfItemLcv--)
            {
                // Find out if any were removed.
                if (!shelfItemDictionary[shelfSlot.items[shelfItemLcv].shelfStackItemID])
                {
                    shelfSlot.removeItemVisual(shelfSlot.items[shelfItemLcv]);
                    
                    if (isStackShelfVisible)
                    {
                        var item = savedResultsPanel.resultsList.itemAt(shelfItemLcv);
                        if (item)
                        {
                            savedResultsPanel.resultsList.removeItem(item);
                        }
                    }
                }
                // Otherwise remember the ones we already have.
                else
                {
                    existingShelfItems[shelfSlot.items[shelfItemLcv].shelfStackItemID] = shelfSlot.items[shelfItemLcv];
                }
            }
            
            for (var shelfItemLcv = 0; shelfItemLcv < shelfStack.shelfStackItems.length; shelfItemLcv++)
            {
                var shelfStackItem = shelfStack.shelfStackItems[shelfItemLcv];
                if (!existingShelfItems[shelfStackItem.shelfStackItemID])
                {                                
                    if (isStackShelfVisible)
                    {
                        var resultView;
                        switch (shelfStackItem.domain) 
                        {
                            case "web":
                                resultView = new SJ.Border(16);
                                resultView.setBackgroundImage("images/tornpaper.png");
                                resultView.setContent(new SJ.WebResultView(shelfStackItem, true));
                                resultView.margin = {top: 8, bottom: 8};
                                SJ.mixinRemoveButton(resultView, shelfStackItem, 400, 0);
                                SJ.mixinOwnerLabel(resultView, shelfStackItem.owner.get_displayName(), 300, resultView.visual.Height - 10);
                                break;
                            case "images":
                                resultView = new SJ.ImageResultView(shelfStackItem);
                                resultView.margin = {top: 8, bottom: 8, left: 8, right: 8};
                                resultView.vAlign = "center";
                                SJ.mixinRemoveButton(resultView, shelfStackItem, resultView.visual.Width - 65, 0);
                                SJ.mixinOwnerLabel(resultView, shelfStackItem.owner.get_displayName(), resultView.visual.Width - 200, resultView.visual.Height - 10);
                                break;
                            case "news":
                                resultView = new SJ.NewsResultView(300, shelfStackItem);
                                resultView.margin = {top: 8, bottom: 8, left: 8, right: 8};
                                SJ.mixinRemoveButton(resultView, shelfStackItem, 400, 0);
                                SJ.mixinOwnerLabel(resultView, shelfStackItem.owner.get_displayName(), resultView.visual.Width - 400, resultView.visual.Height - 10);
                                break;
                            case "feeds":
                                resultView = new SJ.FeedResultView(shelfStackItem);
                                resultView.margin = {top: 8, bottom: 8};
                                SJ.mixinRemoveButton(resultView, shelfStackItem, 300, 0);
                                SJ.mixinOwnerLabel(resultView, shelfStackItem.owner.get_displayName(), 0, 300);
                                break;
                            case "phonebook":
                                resultView = new SJ.Border(16);
                                resultView.setBackgroundImage("images/tornpaper.png");
                                resultView.setContent(new SJ.WebResultView(shelfStackItem, true));
                                resultView.margin = {top: 8, bottom: 8};
                                SJ.mixinRemoveButton(resultView, shelfStackItem, 400, 0);
                                SJ.mixinOwnerLabel(resultView, shelfStackItem.owner.get_displayName(), 300, resultView.visual.Height - 10);
                                break;
                            case "query":
                                resultView = new SJ.QueryResultView(shelfStackItem.title);
                                resultView.margin = {top: 8, bottom: 8};
                                resultView.onNavigate = SJ.methodCaller(this, 'onQueryResultViewClickedHandler');
                                SJ.mixinRemoveButton(resultView, shelfStackItem, 60, -10);                                
                                break;
                        }
                        
                        resultView.onRemoveResult = SJ.methodCaller(savedResultsPanel, 'removeResult');
                        savedResultsPanel.resultsList.addItem(resultView);
                        savedResultsPanel.results.push(shelfStackItem);
                    }
                    
                    var dragVisual = SJ.GetDragVisual(shelfStackItem.domain);                   
                    shelfSlot.addItemCore(shelfStackItem, dragVisual, 47, 47);
                    shelfSlot.fixStack(true);
                }
            }

            shelfSlot.label.setText(shelfStack.label);
            
            if (isStackShelfVisible)
            {
                savedResultsPanel.header.updateInfo(shelfStack.label);
                savedResultsPanel.updateConversationHistory();
                
                for (var lcv = 0; lcv < savedResultsPanel.callOnDisplay.length; lcv++)
                {
                    if (savedResultsPanel.callOnDisplay[lcv]) savedResultsPanel.callOnDisplay[lcv].sortContact();
                }
                
                savedResultsPanel.conversationMembers.removeItem(savedResultsPanel.ownersHeaderLabel);
                savedResultsPanel.conversationMembers.removeItem(savedResultsPanel.conversationMembers.ownersList);
                savedResultsPanel.conversationMembers.removeItem(savedResultsPanel.contactsHeaderLabel);
                savedResultsPanel.conversationMembers.removeItem(savedResultsPanel.conversationMembers.contactsList);        
                savedResultsPanel.conversationMembers.addItem(savedResultsPanel.ownersHeaderLabel);
                savedResultsPanel.conversationMembers.addItem(savedResultsPanel.conversationMembers.ownersList);
                savedResultsPanel.conversationMembers.addItem(savedResultsPanel.contactsHeaderLabel);
                savedResultsPanel.conversationMembers.addItem(savedResultsPanel.conversationMembers.contactsList);
            }
            
            break;
        }
    }
    
    if (!shelfFound)
    {
        shelf.addShelfStack(shelfStack);
    }
}

SJ.Interop.prototype.messengerStatusChanged = function (isSignedIn) 
{
    if (savedResultsPanel)
    {
        savedResultsPanel.updateConversationHistory();
    }
}

var emailHashesToWindows = {};

SJ.Interop.prototype.onIncomingTextMessage = function (emailHash, displayName, messageText) 
{
    openIMWindow(emailHash);
    receiveIM(emailHash, displayName, messageText);
}

SJ.Interop.prototype.toString = function () {
    return "SJ.Interop";
}

function openIMWindow(emailHash)
{
    if (!emailHashesToWindows[emailHash])
    {
        emailHashesToWindows[emailHash] = window.open("IMWindow.aspx?emailHash=" + emailHash, emailHash, "width=420,height=580,location=no,menubar=no,status=no,directories=no");
    }
    else
    {
        emailHashesToWindows[emailHash].focus();
    }
}

function isContact(emailHash)
{
    return WLQuickApps.Tafiti.Scripting.MessengerManager.isContact(emailHash);
}

function getDisplayName(emailHash)
{
    return WLQuickApps.Tafiti.Scripting.MessengerManager.getDisplayName(emailHash);
}

function receiveIM(emailHash, displayName, messageText)
{
    if (!emailHashesToWindows[emailHash])
    {
        openIMWindow(emailHash);
    }
    
    // The page might not be loaded yet, so we need to try to deliver the message again in a little while.
    if (!emailHashesToWindows[emailHash].receiveMessage)
    {
        setTimeout("receiveIM('" + emailHash + "', '" + displayName + "', '" + messageText + "')", 1000);
        return;
    }
    
    emailHashesToWindows[emailHash].receiveMessage(displayName, messageText)
    emailHashesToWindows[emailHash].focus();
}

function sendIM(emailHash, messageText)
{
    WLQuickApps.Tafiti.Scripting.MessengerManager.sendTextMessage(emailHash, messageText);
}

function closeIM(emailHash)
{
    if (emailHashesToWindows[emailHash])
    {
        emailHashesToWindows[emailHash] = null;    
    }
}

function addContact(emailHash)
{
    WLQuickApps.Tafiti.Scripting.MessengerManager.addContact(emailHash);
}

SJ.Interop.prototype.popIMConsentDialog = function (shelfStackID) 
{
    if (this.imConsentDialog) { return; }
    
    this.imConsentDialog = new SJ.DialogBox(300, 100);
    var esDialog = new SJ.IMConsentDialog(shelfStackID);
    var border = new SJ.Border(8, "Background='#ffffff'");
    border.setContent(esDialog);
    this.imConsentDialog.setContent(border);

    this.imConsentDialog.center();
    this.imConsentDialog.updateLayout();
    this.imConsentDialog.show();
}

SJ.Interop.prototype.popAcceptContactDialog = function (displayName, emailAddress, inviteMessage) 
{
    if (this.acceptContactDialog) { return; }
    
    this.acceptContactDialog = new SJ.DialogBox(300, 100);
    var esDialog = new SJ.AcceptContactDialog(displayName, emailAddress, inviteMessage);
    var border = new SJ.Border(8, "Background='#ffffff'");
    border.setContent(esDialog);
    this.acceptContactDialog.setContent(border);

    this.acceptContactDialog.center();
    this.acceptContactDialog.updateLayout();
    this.acceptContactDialog.show();
}

SJ.Interop.prototype.closeConsentDialog = function ()
{
    if (this.imConsentDialog)
    {
        this.imConsentDialog.close();
        this.imConsentDialog = null;
    }
}

SJ.Interop.prototype.closeAcceptContactDialog = function ()
{
    if (this.acceptContactDialog)
    {
        this.acceptContactDialog.close();
        this.acceptContactDialog = null;
    }
}

// IMConsentDialog

SJ.IMConsentDialog = function (pendingShelfStack) {
    SJ.StackPanel.call(this, 0, 0, true);
    this.visual.Background = "#ffffff";
    
    this.pendingShelfStack = pendingShelfStack;
    
    var outerStack = new SJ.StackPanel(0, 0, true);
    outerStack.margin = {top: 25, left: 64, right: 25};
    outerStack.setParent(this);
    
    var helpText = new SJ.TextBlock(0, 0, 550, 0, 
        "Tafiti would like to send IMs to other shelf stack owners to keep them up to date with your changes. Do you want to allow these messages to be sent?",
        "TextWrapping='Wrap' FontSize='12' Foreground='#286ace'");
    helpText.margin = {left: -30, bottom: 20};
    helpText.setParent(outerStack);

    var buttonStack = new SJ.StackPanel(0, 0, false);
    buttonStack.hAlign = "right";
    buttonStack.setParent(outerStack);
    var yesBtn = new SJ.Button(0, 0, 80, 20, "Yes");
    yesBtn.onClick = SJ.methodCaller(this, "onYes");
    yesBtn.setParent(buttonStack);
    var noBtn = new SJ.Button(0, 0, 80, 20, "No");
    noBtn.onClick = SJ.methodCaller(this, "onNo");
    noBtn.setParent(buttonStack);
    var alwaysBtn = new SJ.Button(0, 0, 80, 20, "Always");
    alwaysBtn.onClick = SJ.methodCaller(this, "onAlways");
    alwaysBtn.setParent(buttonStack);

}

SJ.IMConsentDialog.prototype = new SJ.StackPanel;

SJ.IMConsentDialog.prototype.toString = function () {
    return "SJ.IMConsentDialog";
}

SJ.IMConsentDialog.prototype.onYes = function () {
    WLQuickApps.Tafiti.Scripting.InteropManager.get_interop().closeConsentDialog();
    WLQuickApps.Tafiti.Scripting.ShelfStackManager.sendShelfStackUpdateApproved(this.pendingShelfStack);
}

SJ.IMConsentDialog.prototype.onNo = function () {
    WLQuickApps.Tafiti.Scripting.InteropManager.get_interop().closeConsentDialog();
}

SJ.IMConsentDialog.prototype.onAlways = function () {
    WLQuickApps.Tafiti.Scripting.TafitiUserManager.set_alwaysSendMessages(true);
    this.onYes();
}

// AcceptContactDialog

SJ.AcceptContactDialog = function (displayName, emailAddress, inviteMessage) {
    SJ.StackPanel.call(this, 0, 0, true);
    this.visual.Background = "#ffffff";
    
    this.emailHash = WLQuickApps.Tafiti.Scripting.Utilities.hash(emailAddress);
    
    var outerStack = new SJ.StackPanel(0, 0, true);
    outerStack.margin = {top: 25, left: 64, right: 25};
    outerStack.setParent(this);
    
    var dialogText = "(" + emailAddress + ") has added you to their Windows Live Messenger Contacts";
    if (displayName.length > 0)
    {
        dialogText = displayName + " " + dialogText;
    }
    
    if (inviteMessage.length > 0)
    {
        dialogText = dialogText + ': "' + inviteMessage + '"';
    }
        
    dialogText = dialogText + ". Would you like to accept this invite?";
             
    var helpText = new SJ.TextBlock(0, 0, 550, 0, dialogText, 
        "TextWrapping='Wrap' FontSize='12' Foreground='#286ace'");
    helpText.margin = {left: -30, bottom: 20};
    helpText.setParent(outerStack);

    var buttonStack = new SJ.StackPanel(0, 0, false);
    buttonStack.hAlign = "right";
    buttonStack.setParent(outerStack);
    var acceptBtn = new SJ.Button(0, 0, 80, 20, "Accept");
    acceptBtn.onClick = SJ.methodCaller(this, "onAccept");
    acceptBtn.setParent(buttonStack);
    var ignoreBtn = new SJ.Button(0, 0, 80, 20, "Ignore");
    ignoreBtn.onClick = SJ.methodCaller(this, "onIgnore");
    ignoreBtn.setParent(buttonStack);

}

SJ.AcceptContactDialog.prototype = new SJ.StackPanel;

SJ.AcceptContactDialog.prototype.toString = function () {
    return "SJ.AcceptContactDialog";
}

SJ.AcceptContactDialog.prototype.onAccept = function () {
    WLQuickApps.Tafiti.Scripting.MessengerManager.addContact(this.emailHash);
    WLQuickApps.Tafiti.Scripting.InteropManager.get_interop().closeAcceptContactDialog();}

SJ.AcceptContactDialog.prototype.onIgnore = function () {    
    WLQuickApps.Tafiti.Scripting.InteropManager.get_interop().closeAcceptContactDialog();}
