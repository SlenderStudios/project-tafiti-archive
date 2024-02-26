// Copyright 2007 Microsoft Corp. All Rights Reserved.

// branch object
function Branch(root, id, uid)
{
	var o = this;

	// Properties - common
	o._id = id;
	o._uid = uid;
	o._entType = "branch";		// Entity type identifier
	o._stale = false;			// Entity render state is current
	o.vertices = new Array(3);	// Vertices to define all coordinates
	o._visible = false;			// Visibility state
	o._animating = false;		// Running animation.. selective rendering
	
	// Properties - unique
	o.root = root;			// XAML host container
	o.xaml = null;			// XAML component container
	o.slc = root.getHost();	// Base control for creating XAML objects
	o.branchType = BT_Leaf;	// Type of branch (i.e. endnode or transient)
	o.oLeafNode = null;		// Extra flair on a branch, if an end-node
	o.fUpdated = false;		// Received initial info update
	o.aEnts = [ o ];		// List of entities for this branch
	o.childCount = 0;       // Keep track of child branches.
	o.depth = 0;            // distance from tree trunk
	o.branchTransition = null; // Link to the transition branch for this class (only set for Leaf branches)
	o.branchLeaf = null;       // Link to leaf branch for this class (only set for Transition branches)

	// Methods - common
	o.Initialize = Branch_Initialize;
	o.Update = Branch_Update;
	o.Render = Branch_Render;
	o.Visibility = Branch_Visibility;

	// Methods - unique
	o.UpdateInfo = Branch_UpdateInfo;
	o.FinalizeVisibility = Branch_FinalizeVisibility;
	o.AddChildBranch = Branch_AddChildBranch;
	o.AdjustOffset = Branch_AdjustOffset;
	o.GetChildCount = Branch_GetChildCount;
	o.UpdateLeaves = Branch_UpdateLeaves;
}

//////////////////////////////////////////////////////////////////////////////
// Branch setup
function Branch_Initialize(branchType, dir)
{
	var o = this;

	// Create fragment and attach to world
	o.xaml = { path:CreateBranchClip(o.slc) };
	o.root.children.add(o.xaml.path);
	o.xaml.path.Visibility = "Collapsed";

	// Map XAML components
	o.xaml.start = o.xaml.path.findName("start");
	o.xaml.segment = o.xaml.path.findName("seg");
	o.xaml.rot = o.xaml.path.findName("rot");

	// Set up leaf node, if this branch is of type BT_Leaf
	o.branchType = branchType;
	if(branchType == BT_Leaf)
	{
		o.oLeafNode = new LeafNode(o.root, ((Math.random() > 0.15) ? 2 : 1));	// Handle nub/leaves
		o.oLeafNode.Initialize(o._uid);

		// Add the newly added leaf node to the list of ents to update
		o.aEnts.push(o.oLeafNode);
	}

	// Set up vertex info
	for(var i=0; i<o.vertices.length; i++)
	{
		o.vertices[i] = new Vertex(0,0,0);
	}

	// Return back all ents that have world coordinates
	return(o.aEnts);
}

///////////////////////////////////////////////////////////////////////////////
// Update an already-generated branch --> world coordinate update
function Branch_Update(params)
{
	var o = this;

	o._cachedParams = params;

	// FIXME: Validate passed pts array structure
	for(var i=0; i<o.vertices.length; i++)
	{
		// Deep copy to keep object references intact
		o.vertices[i].wx = params.pts[i].x;
		o.vertices[i].wy = params.pts[i].y;
		o.vertices[i].wz = params.pts[i].z;
	}

	// Calculate angle of unit tangent vector at EP of branch curve
	var rot = Math.atan((o.vertices[2].wy - o.vertices[1].wy) / (o.vertices[2].wx - o.vertices[1].wx));	
	if(o.vertices[2].wx > o.vertices[1].wx) rot += Math.PI;		// Adjust for right-side quadrants
	params.rot = rot;

	// Copy params rendering info (as needed)
	o.scaleBranch = params.scaleBranch;
	o.xaml.path.StrokeThickness = 8 * o.scaleBranch;
	o.xaml.path.Stroke = GenerateColor(g_branchColorRange0, g_branchColorRange1, o.scaleBranch);

	// Update LeafNode
	if(o.oLeafNode)
	{
		o.oLeafNode.Update(params);
	}

	// Cache
	o.zRot = params.view.zRot;
	o.zRot_n = (o.zRot < 0) ? -(Math.PI + o.zRot) : (Math.PI - o.zRot);

	// Update verts
	TransformVertices(params.view, o.vertices);

	// Flag we need an update
	o._stale = true;
}

///////////////////////////////////////////////////////////////////////////////
// Perform the screen update of the Branch
//	--> Children are updated through world render pass
function Branch_Render()
{
	var o = this;
	var x = o.xaml;
	var r = o.vertices;
	var op = ((o._currScale > 0.3) ? o._currScale * o._currScale : 0.1);

	o._currLevel = ((o._currScale != 0) ? Math.round(o._currScale * g_maxLevel) : 1);

	if(!o._animating)
	{	
		x.path.Opacity = op;
	}

	x.path["Canvas.ZIndex"] = o._currLevel;
	x.start.StartPoint = r[0].sx + ',' + r[0].sy;
	x.segment.Point1 = r[1].sx + ',' + r[1].sy;
	x.segment.Point2 = r[2].sx + ',' + r[2].sy;

	// Set LeafNode state
	if(o.oLeafNode)
	{
		o.oLeafNode._currLevel = o._currLevel + 1;
		o.oLeafNode._currScale = o._currScale;
		o.oLeafNode._currOp = op;
	}

	// Render is current until next update
	o._stale = false;

	return;
}

///////////////////////////////////////////////////////////////////////////////
// Update link info for this branch's leafnode
function Branch_UpdateInfo(oItem)
{
	var o = this;
	if(o.oLeafNode == null)
	{
		// Not supported on this branch
		return(false);
	}

	// Don't change anything besides leaf colors if we have a null result
	if(oItem == null)
	{
		if(!o.fUpdated)
		{
			o.oLeafNode.UpdateColors(0, -1);
		}
		return(true);
	}

	if(!o.fUpdated || !o._visible)
	{
		// Set immediately on first update or if not rendered
		o.fUpdated = true;
		o.oLeafNode.UpdateColors(0, -1);
		o.oLeafNode.date = ((oItem.date != null) ? oItem.date : g_defaultDate);
		o.oLeafNode.link = oItem.link;
		o.oLeafNode.info = oItem.name.toLocaleUpperCase() + ": " + oItem.info;
		ClipContent(o.oLeafNode.oText, oItem.name);
	}
	else
	{
		// Cycle in on subsequent updates
		o.newInfo = oItem;
		o.oLeafNode._animating = true;
		o.oLeafNode.oText.base.Foreground = "Gold";
		stage.Animations.anim_generic(o, o.oLeafNode.oText.base, "Opacity", g_a_updateItem_T, 0, 1, 0, true, UpdateLeafInfo);
		stage.Animations.anim_generic(o, o.oLeafNode.oText.base, "Opacity", g_a_updateItem_T, g_a_updateItem_T, 0, 1, true, FinalizeLeafInfo);
	}
	
	return(true);
}

function UpdateLeafInfo()
{
	var o = this;

	o.oLeafNode.date = ((o.newInfo.date != null) ? o.newInfo.date : g_defaultDate);
	o.oLeafNode.link = o.newInfo.link;
	o.oLeafNode.info = o.newInfo.name.toLocaleUpperCase() + ": " + o.newInfo.info;
	ClipContent(o.oLeafNode.oText, o.newInfo.name);
	o.newInfo = null;
	o.oLeafNode.oText.base.Foreground = "White";
}

// Trim text to a max width, adding ellipses as needed
function ClipContent(o, txt)
{
	var len = txt.length;
	var oSX = o.scale.ScaleX;
	var oOp = o.base.Opacity;

	// Strange, if Opacity is 0, ActualWidth becomes 0, along with
	// positional artifacts if changing Text. Simply toggling while
	// changes are made seems to work out fine.
	o.base.Opacity = 1;
	o.base.Text = txt;
	o.scale.ScaleX = 1 + g_txtMaxScale;	// Worry about largest size
	o.scale.ScaleY = 1 + g_txtMaxScale;
	
	while(o.base.ActualWidth > g_txtMaxWidth)
	{
		o.base.Text = txt.substr(o, --len) + "...";
	}
	
	o.base.Opacity = oOp;
	o.scale.ScaleX = oSX;
	o.scale.ScaleY = oSX;
}

function FinalizeLeafInfo()
{
	this.oLeafNode._animating = false;
}

///////////////////////////////////////////////////////////////////////////////
// Transition branch's leaf colors
function Branch_UpdateLeaves(idxColor, delay)
{
	var o = this.oLeafNode;

	if(o == null)
	{
		// Not supported on this branch
		return(false);
	}

	o.UpdateColors(idxColor, delay);
	return(true);
}

///////////////////////////////////////////////////////////////////////////////
// Update visibility info for this branch
function Branch_Visibility(fVisible)
{
	var o = this;

	if(fVisible == o._visible)
	{
		return;
	}

	o._visible = fVisible;
	o._animating = true;
	o.xaml.path.Visibility = "Visible";

	var T = ((fVisible) ? g_a_branch_TfadeIn : g_a_branch_TfadeOut);
	stage.Animations.Finalize(o);
	stage.Animations.anim_generic(o, o.xaml.path, "Opacity", T, 0, o.xaml.path.Opacity, ((fVisible) ? 1.0 : 0), false, o.FinalizeVisibility);

	if(o.oLeafNode != null)
	{
		o.oLeafNode.Visibility(fVisible);
	}
}

function Branch_FinalizeVisibility()
{
	var o = this;

	o._animating = false;
	o.xaml.path.Visibility = ((o._visible) ? "Visible" : "Collapsed");
}

///////////////////////////////////////////////////////////////////////////////
// 
function Branch_AddChildBranch(branch)
{
	var o = this;

	o.childCount++;

	// Set the parent id used for getting the zRot. 
	// Use the root ancestor's id to calculate zRot angles. If undefined, o._id is the root.
	branch.depth = o.depth + 1;
}

///////////////////////////////////////////////////////////////////////////////
// Get the count of all children on the leaf or transition branches 
// associated with a leaf branch
function Branch_GetChildCount()
{
	var o = this;
	var childCount = o.childCount;
	if (o.branchType == BT_Leaf 
		&& o.branchTransition != null)
	{
		// For Leaf branch, include the Transition branch's children.
		childCount += o.branchTransition.childCount;
	}
	if (o.branchType == BT_Transition
		&& o.branchLeaf != null)
	{
		// For Transition branch, include the Leaf branch's children.
		childCount += o.branchLeaf.childCount;	
	}
		
	return childCount;
}

///////////////////////////////////////////////////////////////////////////////
// Hack to get offset angles. Real fix is to properly set z angle when computing curves.
function Branch_AdjustOffset(ang)
{
	var o = this;

	// Get current transformed coordinates
	var tmp_rx = o.vertices[0].rx;
	var tmp_ry = o.vertices[0].ry;
	var tmp_rz = o.vertices[0].rz;
	var orig_zrot = o.zRot;

	// Rotate to desired angle
	o._cachedParams.view.zRot += ang;
	o.Update(o._cachedParams);
	o._cachedParams.view.zRot = orig_zrot;
	o.zRot = orig_zrot;
	o.zRot_n = (o.zRot < 0) ? -(Math.PI + o.zRot) : (Math.PI - o.zRot);

	var dx = tmp_rx - o.vertices[0].rx;
	var dy = tmp_ry - o.vertices[0].ry;
	var dz = tmp_rz - o.vertices[0].rz;

	// Apply offset
	for(var i=0; i<o.vertices.length; i++)
	{
		o.vertices[i].rx += dx;
		o.vertices[i].ry += dy;
		o.vertices[i].rz += dz;
	}

	// Leaf components too, if present
	if(o.oLeafNode != null)
	{
		for(var i=0; i<o.oLeafNode.vertices.length; i++)
		{
			o.oLeafNode.vertices[i].rx += dx;
			o.oLeafNode.vertices[i].ry += dy;
			o.oLeafNode.vertices[i].rz += dz;
		}
	}
}