// Copyright 2007 Microsoft Corp. All Rights Reserved.

// All XAML fragments used

////////////////////
// BRANCH CLIP
////////////////////
function CreateBranchClip(slc)
{
	// Define the core branch XAML fragment.
	var xaml =
		'<Path Stroke="#FF808080" StrokeThickness="1" Opacity="0">' +
		'	<Path.Data>' +
		'		<PathGeometry>' +
		'			<PathFigure '  + g_x_space + ' x:Name="start" StartPoint="0,0">' +
		'				<QuadraticBezierSegment ' + g_x_space + ' x:Name="seg" ' +
		'					Point1="0,0"' +
		'					Point2="0,0"/>' +
		'			</PathFigure>' +
		'		</PathGeometry>' +
		'	</Path.Data>' +
		'    <Path.RenderTransform>' +
		'	    <TransformGroup>' +
		'		    <RotateTransform ' + g_x_space + ' x:Name="rot" Angle="0"/>' +
		'	    </TransformGroup>' +
		'    </Path.RenderTransform>' +
		'</Path>';

	// Create the XAML fragment and return it.
	return(slc.content.createFromXaml(xaml, true));
}

///////////////
// LEAF CLIP
///////////////
function CreateLeafClip(slc)
{
    var xaml = 
	    '<Path StrokeThickness="0" Opacity="0">' +
	    '	<Path.Data>' +
	    '		<PathGeometry>' +
	    '			<PathFigure ' + g_x_space + ' x:Name="path" StartPoint="0,0">' +
	    '				<BezierSegment ' + g_x_space + ' x:Name="left" Point1="40,-20" Point2="40,-80" Point3="0,-100"/>' +
	    '				<BezierSegment ' + g_x_space + ' x:Name="right" Point1="-40,-80" Point2="-40,-20" Point3="0,0"/>' +
	    '			</PathFigure>' +
	    '		</PathGeometry>' +
	    '	</Path.Data>' +
	    '	<Path.Fill>' +
	    '		<SolidColorBrush ' + g_x_space + ' x:Name="color" Color="Green"/>' +
	    '	</Path.Fill>' +
	    '</Path>';

	return(slc.content.createFromXaml(xaml, true));
}

///////////////
// NUB CLIP
///////////////
function CreateNubClip(slc)
{
	var xaml =
		'<Ellipse Canvas.Left="-4" Canvas.Top="-4"' +
		'		  Opacity="0"' + 
		'		  Fill="#FFFFFFFF"' +
		'		  Height="8"' +
		'		  Width="8"' +
		'		  StrokeThickness="3"' +
		'		  Stroke="#FF404040"/>';
	
	return(slc.content.createFromXaml(xaml, true));
}

///////////////
// TEXT CLIP
///////////////
function CreateLeafTextClip(slc, id)
{
	var xaml = '<TextBlock ' + g_x_space + ' x:Name="' + id + '"' +
		//'  FontFamily="Times New Roman"' +
		'  FontSize="' + g_txtDefaultSize + '"' +
		'  Opacity="0"' + 
		'  Foreground="White">' +
		'  <TextBlock.RenderTransform>' +
		'    <ScaleTransform ' + g_x_space + ' x:Name="scale" ScaleX="1" ScaleY="1" />' +
		'  </TextBlock.RenderTransform>' +
		'</TextBlock>';

	return(slc.content.createFromXaml(xaml, true));    
}