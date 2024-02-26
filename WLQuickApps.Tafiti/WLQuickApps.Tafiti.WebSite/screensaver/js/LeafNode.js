// Copyright 2007 Microsoft Corp. All Rights Reserved.

// Object for the end leaf/nub node of a branch

///////////////////////////////////////////////////////////////////////////////
// Constructor
// Initialize default values only (constructor shouldn't fail)
function LeafNode(root, nLeaves)
{
	var o = this;

	// Properties - common
	o._id = root.children.count;
	o._entType = "leafnode";	// Entity type identifier
	o._stale = false;			// Entity render state is current
	o.vertices = new Array(nLeaves * 7 + 3);	// Vertices to define all coordinates
	o._currLevel = 0;
	o._visible = false;			// Visibility state
	o._animating = false;		// Running animation.. selective rendering

	// Properties - unique
	o.root = root;
	o.slc = root.getHost();
	o.nLeaves = nLeaves;
	o.oNub = null;
	o.oText = null;
	o.aLeaves = new Array(nLeaves);

	// Methods - common
	o.Initialize = LeafNode_Initialize;
	o.Update = LeafNode_Update;
	o.Render = LeafNode_Render;
	o.Visibility = LeafNode_Visibility;

	// Methods - unique
	o.InitParams = LeafNode_InitParams;
	o.handleMouseUp = LeafNode_HandleMouseUp;
	o.handleMouseEnter = LeafNode_HandleMouseEnter;
	o.handleMouseLeave = LeafNode_HandleMouseLeave;
	o.FinalizeVisibility = LeafNode_FinalizeVisibility;
	o.UpdateColors = LeafNode_UpdateColors;
		
	// Generate initial settings/state
	o.InitParams();
}

///////////////////////////////////////////////////////////////////////////////
// Precaculate angles/offsets based on LeafNode model
function LeafNode_InitParams()
{
	var o = this;

	//////////////////////////
	// TODO: Break up the nub and leaves into individual entities!!
	//////////////////////////
	
	// XAML layout (static model, normalized to origin)
	var ox = 0;		// Offset X
	var oy = -7;	// Offset Y
	var s = 0.5;
	var ptsWorld = [ { x:(s * 0   + ox), y:(s *    0 + oy), z:0 },		// StartPoint
					 { x:(s * 40  + ox), y:(s *  -20 + oy), z:0 },		// Left - Point1
					 { x:(s * 40  + ox), y:(s *  -80 + oy), z:0 },		// Left - Point2
					 { x:(s * 0   + ox), y:(s * -100 + oy), z:0 },		// Left - Point3
					 { x:(s * -40 + ox), y:(s *  -80 + oy), z:0 },		// Right - Point1
					 { x:(s * -40 + ox), y:(s *  -20 + oy), z:0 },		// Right - Point2
					 { x:(s * 0   + ox), y:(s *    0 + oy), z:0 } ];	// Right - Point3

	// Calculate base magnitude and angle
	o.aMagAngle = new Array(ptsWorld.length);
	for(var i=0; i<o.aMagAngle.length; i++)
	{
		var p = ptsWorld[i];
		o.aMagAngle[i] = { m:Math.sqrt((p.x * p.x) + (p.y * p.y) + (p.z * p.z)),
						   aY:Math.atan(p.y / p.x),
						   aZ:Math.atan(p.z / p.x) };
		if(p.x < 0)
		{
			o.aMagAngle[i].aY += Math.PI;
		}

		// Cached computation
		o.aMagAngle[i].maYcos = o.aMagAngle[i].m * Math.cos(o.aMagAngle[i].aY);
		o.aMagAngle[i].maYsin = o.aMagAngle[i].m * Math.sin(o.aMagAngle[i].aY);
	}
	
	o.aAngleOffsets = [ { aY:((s *  67 - 90) / g_iRad), aZ:(0 / g_iRad) },		// 1st leaf
						{ aY:((s * -67 - 90) / g_iRad), aZ:(0 / g_iRad) } ];	// 2nd leaf

	// Nub settings
	ox = 0;
	oy = 0;
	s = 1;
	ptsWorld = [ { x:(s * 0   + ox), y:(s * 0   + oy), z:0 },		// StartPoint
				 { x:(s * 8  + ox), y:(s * 0   + oy), z:0 },		// width
				 { x:(s * 0   + ox), y:(s * 8  + oy), z:0 } ];		// height

	// Calculate base magnitude and angle
	o.aMagAngle_nub = new Array(ptsWorld.length);
	for(var i=0; i<o.aMagAngle_nub.length; i++)
	{
		var p = ptsWorld[i];
		o.aMagAngle_nub[i] = { m:Math.sqrt((p.x * p.x) + (p.y * p.y) + (p.z * p.z)),
							   aY:Math.atan(p.y / p.x),
							   aZ:Math.atan(p.z / p.x) };
		if(p.x < 0)
		{
			o.aMagAngle_nub[i].aY += Math.PI;
		}
		if(p.x == 0 && p.y == 0)
		{
			o.aMagAngle_nub[i].aY = Math.PI;
		}

		// Cached computation
		o.aMagAngle_nub[i].maYcos = o.aMagAngle_nub[i].m * Math.cos(o.aMagAngle_nub[i].aY);
		o.aMagAngle_nub[i].maYsin = o.aMagAngle_nub[i].m * Math.sin(o.aMagAngle_nub[i].aY);
	}
	
	// Precache Vertex objects
	for(var i=0; i<o.vertices.length; i++)
	{
		o.vertices[i] = new Vertex(0, 0, 0);
	}
}

///////////////////////////////////////////////////////////////////////////////
// Initializer
function LeafNode_Initialize(id)
{
	var o = this;

	// temp hack
	o._oid = id;

	// Add nub/text only if we have at least one leaf
	if(o.aLeaves.length > 0)
	{
		o.oNub = CreateNubClip(o.slc);
		o.root.children.add(o.oNub);
		o.oNub.Visibility = "Collapsed";

		o.oText = { base: CreateLeafTextClip(o.slc, o._oid) };
		o.oText.base.Text = "";
		o.oText.base.addEventListener("MouseLeftButtonUp", o.handleMouseUp);
		o.oText.base.addEventListener("MouseEnter", o.handleMouseEnter);
		o.oText.base.addEventListener("MouseLeave", o.handleMouseLeave);
		o.root.children.add(o.oText.base);
		o.oText.scale = o.oText.base.findName("scale");
		o.oText.base.Visibility = "Collapsed";
	}
	
	o.baseColor = Math.floor(Math.random() * (g_leafColors[0].length - 1));

	// Generate leaf/nub clips as needed
	for(var i=0; i<o.aLeaves.length; i++)
	{
		// Create fragment and attach to world
		var p = { base:CreateLeafClip(o.slc) };
		o.root.children.add(p.base);
		p.base.Visibility = "Collapsed";
		
		// Map XAML components
		p.path = p.base.findName("path");
		p.left = p.base.findName("left");
		p.right = p.base.findName("right");
		p.color = p.base.findName("color");
		
		// Leaf colors
		p.color.Color = GenerateColor(g_leafColors[0][(o.baseColor + i)], g_leafColors[0][(o.baseColor + i)], 0);
		
		// Link for later usage
		o.aLeaves[i] = p;
	}
}


///////////////////////////////////////////////////////////////////////////////
// Update this node based on parent branch node
//	--> params.pts should be based on world coordinate system
function LeafNode_Update(params)
{
	var o = this;

	var offset = 0;
	var x = params.pts[2].x;
	var y = params.pts[2].y;
	var z = params.pts[2].z;

	var n, ma, d, s;
	var d_cos, d_sin;

	// Map nub points
	n = o.aMagAngle_nub.length;
	d_cos = Math.cos(params.rot);
	d_sin = Math.sin(params.rot);

	for(var j=0; j<n; j++)
	{
		ma = o.aMagAngle_nub[j];
		d = offset + j;
		o.vertices[d].wx = (x + ma.maYcos * d_cos - ma.maYsin * d_sin);
		o.vertices[d].wy = (y + ma.maYsin * d_cos + ma.maYcos * d_sin);
		o.vertices[d].wz = z;
	}

	offset += n;

	// Map leaf points
	n = o.aMagAngle.length;
	for(var i=0; i<o.aLeaves.length; i++)
	{
		d_cos = Math.cos(params.rot + o.aAngleOffsets[i].aY);
		d_sin = Math.sin(params.rot + o.aAngleOffsets[i].aY);
		s = i*n + offset;

		for(var j=0; j<n; j++)
		{
			ma = o.aMagAngle[j];
			d = s + j;
			o.vertices[d].wx = (x + ma.maYcos * d_cos - ma.maYsin * d_sin);
			o.vertices[d].wy = (y + ma.maYsin * d_cos + ma.maYcos * d_sin);
			o.vertices[d].wz = z;
		}
	}

	// Update verts
	TransformVertices(params.view, o.vertices);

	// Flag we need a render update
	o._stale = true;
}

///////////////////////////////////////////////////////////////////////////////
// Perform the screen update of the LeafNode
function LeafNode_Render()
{
	var o = this;

	var d = 0;
	var r = o.vertices;

	// Update the nub -- we need a bounding box for proper scale/position.. upper left/lower right
	// FIXME: This is expensive? use a Path/EllipseGeometry instead(?)
	var h = Math.sqrt(Math.pow(r[d+1].sx - r[d].sx, 2) + Math.pow(r[d+1].sy - r[d].sy, 2));	// need z too
	var w = Math.sqrt(Math.pow(r[d+2].sx - r[d].sx, 2) + Math.pow(r[d+2].sy - r[d].sy, 2));	// need z too
	if(h < w) h = w;
	o.oNub["Canvas.Left"] = r[d].sx - w / 2;
	o.oNub["Canvas.Top"] = r[d].sy - h / 2;
	o.oNub["Canvas.ZIndex"] = o._currLevel + ((o._currScale < 0.5) ? (o.aLeaves.length + 1) : 0);
	o.oNub.Width = w;
	o.oNub.Height = h;

	// Text tag update
	o.oText.base["Canvas.ZIndex"] = o._currLevel + o.aLeaves.length + 2;	// Always on top of nub/leaves
	o.oText.base["Canvas.Left"] = r[d].sx - o.oText.base.ActualWidth * (1 + o._currScale * g_txtMaxScale) / 2;
	o.oText.base["Canvas.Top"] = r[d].sy - h / 2 + 10;
	o.oText.scale.ScaleX = 1 + o._currScale * g_txtMaxScale;
	o.oText.scale.ScaleY = o.oText.scale.ScaleX;//1 + o._currScale * 0.25;

	// Opacities only if not animating!
	if(!o._animating)
	{
		o.oNub.Opacity = o._currOp;
		o.oText.base.Opacity = o._currOp;
	}

	d += 3;

	var p, x;
	var n = o.aMagAngle.length;

	// Update the leaves
	for(var i=0; i<o.aLeaves.length; i++)
	{
		p = o.aLeaves[i];
		x = i * n + d;
		
		if(!o._animating)
		{
			p.base.Opacity = o._currOp;
		}

		p.base["Canvas.ZIndex"] = o._currLevel + i + 1;
		p.path.StartPoint = r[x    ].sx + "," + r[x    ].sy;
		p.left.Point1     = r[x + 1].sx + "," + r[x + 1].sy;
		p.left.Point2     = r[x + 2].sx + "," + r[x + 2].sy;
		p.left.Point3     = r[x + 3].sx + "," + r[x + 3].sy;
		p.right.Point1    = r[x + 4].sx + "," + r[x + 4].sy;
		p.right.Point2    = r[x + 5].sx + "," + r[x + 5].sy;
		p.right.Point3    = r[x + 6].sx + "," + r[x + 6].sy;
	}

	// Render is current until next update
	o._stale = false;

	return;
}


///////////////////////////////////////////////////////////////////////////////
// Event handlers
function LeafNode_HandleMouseEnter(sender, eventArgs)
{
	var o = sender;

	stage._origSpeed = g_a_step;
	g_a_step = 0;
	o.Foreground = "Orange";
	o.textDecorations = "underline";
	stage.root.cursor = "Hand";

	var t = stage.tooltip;
	t._link = stage.branches[Number(o.Name)].oLeafNode.link;
	t.info.Text = stage.branches[Number(o.Name)].oLeafNode.info;
	t.date.Text = stage.branches[Number(o.Name)].oLeafNode.date;
	t.link["Canvas.Top"] = t.info.ActualHeight + 2;
	t.date["Canvas.Top"] = t.info.ActualHeight + 2;
	t.date["Canvas.Left"] = t.bg.Width - t.date.ActualWidth - 6;

	var txt = t._link;
	var len = txt.length;
	var max = t.date["Canvas.Left"] - 8;
	t.link.Text = txt;
	while(t.link.ActualWidth > max)
	{
		t.link.Text = txt.substr(o, --len) + "...";
	}

	t.bg.Height = t.date["Canvas.Top"] + t.date.ActualHeight + 4;
	
	t.root["Canvas.Left"] = sender["Canvas.Left"];
	t.root["Canvas.Top"] = sender["Canvas.Top"] + sender.ActualHeight + 10;
	t.root.Visibility = "Visible";
	stage.Animations.Link(t);
	stage.Animations.anim_generic(t, t.root, "Opacity", g_a_Tooltip_T, g_a_TooltipPauseIn_T, t.root.Opacity, 1.0, false, null);
}

function LeafNode_HandleMouseLeave(sender, eventArgs)
{
	var o = sender;

	g_a_step = stage._origSpeed;
	o.Foreground = "White";
	o.textDecorations = "none";
	o.cursor = "Default";

	var t = stage.tooltip;
	stage.Animations.Link(t);
	stage.Animations.anim_generic(t, t.root, "Opacity", g_a_Tooltip_T, g_a_TooltipPauseOut_T, t.root.Opacity, 0, false, HideTooltip);
}

function HideTooltip()
{
	stage.tooltip.root.Visibility = "Collapsed";
}

function LeafNode_HandleMouseUp(sender, eventArgs)
{   
	SJ.openWindow(stage.tooltip._link);
}


///////////////////////////////////////////////////////////////////////////////
// Update visibility info for a branch's LeafNode
function LeafNode_Visibility(fVisible)
{
	var o = this;

	if(o.aLeaves.length == 0 || o._visible == fVisible)
	{
		return;
	}

	o._animating = true;
	o._visible = fVisible;

	var vis = "Visible";

	o.oNub.Visibility = vis;
//	o.oNub.Opacity = ((fVisible) ? 0 : o.oNub.opacity);
	o.oText.base.Visibility = vis;
//	o.oText.base.Opacity = ((fVisible) ? 0 : o.oNub.opacity);

	var T = ((fVisible) ? g_a_branch_TfadeIn : g_a_branch_TfadeOut);
	stage.Animations.Finalize(o);
	stage.Animations.anim_generic(o, o.oNub, "Opacity", T, 0, o.oNub.Opacity, ((fVisible) ? 1.0 : 0), false, o.FinalizeVisibility);
	stage.Animations.anim_generic(o, o.oText.base, "Opacity", T, 0, o.oNub.Opacity, ((fVisible) ? 1.0 : 0), false, null);

	for(var i=0; i<o.aLeaves.length; i++)
	{
		o.aLeaves[i].base.Visibility = vis;
//		o.aLeaves[i].base.Opacity = ((fVisible) ? 0 : o.oNub.Opacity);
		stage.Animations.anim_generic(o, o.aLeaves[i].base, "Opacity", T, 0, ((fVisible) ? 0 : o.oNub.opacity), ((fVisible) ? 1.0 : 0), false, null);
	}
}

function LeafNode_FinalizeVisibility()
{
	var o = this;
	var vis = ((o._visible) ? "Visible" : "Collapsed");

	o._animating = false;
	o.oNub.Visibility = vis;
	o.oText.base.Visibility = vis;

	for(var i=0; i<o.aLeaves.length; i++)
	{
		o.aLeaves[i].base.Visibility = vis;
	}
}

///////////////////////////////////////////////////////////////////////////////
// Transition to a new color scheme (aging)
function LeafNode_UpdateColors(idxColor, delay)
{
	var o = this;
	var newColor = Math.floor(Math.random() * (g_leafColors[idxColor].length - 1));

	for(var i=0; i<o.aLeaves.length; i++)
	{
		if(idxColor > 0 || delay >= 0)
		{
			stage.Animations.anim_color(o, o.aLeaves[i].color, "Color",
										g_T_leafTransition, delay,
										g_leafColors[o.lastIndex][(o.baseColor + i)],
										g_leafColors[idxColor][(newColor + i)],
										false, null);
		}
		else
		{
			o.aLeaves[i].color.Color = GenerateColor(g_leafColors[idxColor][(newColor + i)], g_leafColors[idxColor][(newColor + i)], 0);
		}
	}
	
	o.baseColor = newColor;
	o.lastIndex = idxColor;
}
