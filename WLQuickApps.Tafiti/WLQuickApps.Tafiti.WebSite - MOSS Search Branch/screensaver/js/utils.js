// Copyright 2007 Microsoft Corp. All Rights Reserved.

/////////////////////////////////////////////////////////////////////////
// Return a linear interpolation between two colors, "#AARRGGBB" string
// format.
//
//		color0/color1: Array of AARRGGBB colors, broken up in an array
//					   of an element per color (0:A, 1:R, 2:B, 3:G)
//		d: 0.0 - 1.0
function GenerateColor(color0, color1, d)
{
	// clamp ranges
	d = ((d > 1) ? 1 : ((d < 0) ? 0 : d));

	var a = (Math.round(color0[0] * d + color1[0] * (1 - d)));
	var r = (Math.round(color0[1] * d + color1[1] * (1 - d)));
	var g = (Math.round(color0[2] * d + color1[2] * (1 - d)));
	var b = (Math.round(color0[3] * d + color1[3] * (1 - d)));

	return("#" + ((a<16)?"0":"") + a.toString(16) +
				 ((r<16)?"0":"") + r.toString(16) + 
				 ((g<16)?"0":"") + g.toString(16) + 
				 ((b<16)?"0":"") + b.toString(16));
}

/////////////////////////////////////////////////////////////////////////
// Handle a window resizing event: scale everything appropriately
/*
function onWindowResize()
{
	var o = stage;
	ScaleElement( { w:o.control.clientWidth,
					h:o.control.clientHeight,
					control:o.control,
					element:o.root,
					fMaintainAspectRatio:false } );
}

/////////////////////////////////////////////////////////////////////////
// Handle full-screen toggling: scale everything appropriately
function onFullScreenChange()
{
	var o = stage;
	ScaleElement( { w:o.control.content.actualWidth,
					h:o.control.content.actualHeight,
					control:o.control,
					element:o.root,
					fMaintainAspectRatio:false } );
}
*/
/////////////////////////////////////////////////////////////////////////
// Scale a given xaml element
function ScaleElement(params)
{
	var offsetX = 0;
	var offsetY = 0;
	var width = params.w;
	var height = params.h;
	var defWidth = params.element.width;
	var defHeight = params.element.height;
	var scaleRatio = defWidth / defHeight;

	if(params.fMaintainAspectRatio)
	{
		if(width / scaleRatio > height)
		{
			offsetX = (width - height * scaleRatio) / 2;
			width = height * scaleRatio;
		}
		else
		{
			offsetY = (height - width / scaleRatio) / 2;
			height = width / scaleRatio;
		}
	}

	var renderTransform = "<TransformGroup>"
	renderTransform += "<ScaleTransform ScaleX='" + (0.64 * width / defWidth) + "' ScaleY='" + (0.64 * height / defHeight) + "'/>";
	renderTransform += "<TranslateTransform X='" + offsetX + "' Y='" + offsetY + "'/>";
	renderTransform += "</TransformGroup>";

	var o = stage.clip_bg;
	o.renderTransform = params.control.content.createFromXaml(renderTransform);

	renderTransform = "<TransformGroup>"
	renderTransform += "<ScaleTransform ScaleX='" + (width / defWidth) + "' ScaleY='" + (height / defHeight) + "'/>";
	//renderTransform += "<TranslateTransform X='" + offsetX + "' Y='" + offsetY + "'/>";
	renderTransform += "</TransformGroup>";

	o = stage.clip_fg;
	o.renderTransform = params.control.content.createFromXaml(renderTransform);
	o["Canvas.Top"] = height - (height / defHeight) * 115;// Doesn't seem to get this right on init... o.Height;

	o = stage.clip_slider;
	o["Canvas.Left"] = (width - 876 - 28) / 2;// Doesn't seem to get this right on init... stage.slider_bg.Width
	o["Canvas.Top"] = height - 147 + 11;// Doesn't seem to get this right on init... stage.slider_bg.Height

	o = stage.clip_windowed;
	o["Canvas.Left"] = 25;	// Doesn't seem to get this right on init...  stage.btn_windowed.Width - 20

	o = stage.clip_fullscreen;
	o["Canvas.Left"] = (width - 75);

	o = stage.clip_scroll_left;
	o["Canvas.Left"] = 100;
	o["Canvas.Top"] = height * 0.3125;

	o = stage.clip_scroll_right;
	o["Canvas.Left"] = (width - 150);
	o["Canvas.Top"] = height * 0.3125;
	
	o = stage.projector;
	o.width = width;
	o.height = height;
	o.Update();

return;
}



///////////////////////////////////////////////////////////////////////////////
// Update vertices with view transform applie
function TransformVertices(p, verts)
{
	var m = new PMatrix();

	m.Scale(p.scale, p.scale, p.scale);

	m.RotateAxis(0, p.xRot);
	m.RotateAxis(1, p.yRot);
	m.RotateAxis(2, p.zRot);

	m.Translate(p.x, p.y, p.z);

	// Apply transform to all passed vertices
	for(var i=0; i<verts.length; i++)
	{
		m.TransformVert(verts[i]);
	}
}
