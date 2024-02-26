// Copyright 2007 Microsoft Corp. All Rights Reserved.

if (window.SLTree == null)
{
	window.SLTree = {};
}

SLTree.Stage = function() 
{
}

SLTree.Stage.prototype =
{
	///////////////////////////////////////////////////////////////////////////////
	// API callbacks
	///////////////////////////////////////////////////////////////////////////////

	// Set up after all components (i.e. .xaml) have been loaded
	Init: function(control, userContext, rootElement) 
	{
	    var p, btn;
	    var o = this;		// this = the root "stage" object defined in Main.js

		o._stale = false;		// World is current
		o.control = control;
		o.root = rootElement;
		o.branchSets = [ ];	// List branch sets
		o.branches = [ ];	// List of all branch entities (with LeafNodes, as necessary)
		o.aItems = [ ];		// List of link items to display
		o._animResults = { };	// Animation handler for results query
		o._animBtns = { };	// Animation handler for buttons
		o._animToAll = true;	// Initial tree-grow roll-in animation (flag used in UpdateItems)
		//o.control.settings.maxFrameRate = slc_MaxFPS;
		o._debug0 = o.root.findName("txtDebug0");
		o._debug1 = o.root.findName("txtDebug1");
		g_a_step = g_a_step_o;

		// Hide everything until we get an importItems event
//		o.root.Visibility = "Collapsed";

		// "Full Screen" button
		btn = o.root.findName("btn_fullscreen");
		btn.addEventListener("MouseEnter", "ScrollBtn_onRollOver");
		btn.addEventListener("MouseLeave", "ScrollBtn_onRollOut");
		btn.addEventListener("MouseLeftButtonDown", "WindowedBtn_onPress");
		btn.addEventListener("MouseLeftButtonUp", "FullscreenBtn_onRelease");
		o.btn_fullscreen = btn;
		o.btn_fullscreen_up = o.root.findName("btn_fullscreen_up");
		o.clip_fullscreen = o.root.findName("clip_fullscreen");

		// "Minimize" (exit) button
		btn = o.root.findName("btn_windowed");
		btn.addEventListener("MouseEnter", "ScrollBtn_onRollOver");
		btn.addEventListener("MouseLeave", "ScrollBtn_onRollOut");
		btn.addEventListener("MouseLeftButtonDown", "WindowedBtn_onPress");
		btn.addEventListener("MouseLeftButtonUp", "WindowedBtn_onRelease");
		o.btn_windowed = btn;
		o.btn_windowed_up = o.root.findName("btn_windowed_up");
		o.clip_windowed = o.root.findName("clip_windowed");

		// Slider zoom button ref/handling
		o.slider_bg = o.root.findName("slider_bg");
		o.slider = { root:o.root.findName("slider") };
		o.slider.root["Canvas.Left"] = g_sl_min + g_sl_mag * g_sl_start;
		o.root.addEventListener("MouseEnter", "Slider_onOver");
		o.root.addEventListener("MouseLeave", "Slider_onOut");
		o.root.addEventListener("MouseLeftButtonDown", "Slider_onPress");
		o.root.addEventListener("MouseLeftButtonUp", "Slider_onRelease");

		// Left scroll button
		btn = o.root.findName("scroll_left");		// Show/hide region
		btn.addEventListener("MouseEnter", "ScrollShow_onRollOver");
		btn.addEventListener("MouseLeave", "ScrollShow_onRollOut");
		btn = o.root.findName("scroll_left_btn");	// Functional section
		btn.addEventListener("MouseEnter", "ScrollBtn_onRollOver");
		btn.addEventListener("MouseLeftButtonDown", "ScrollBtn_onPress");
		btn.addEventListener("MouseLeftButtonUp", "ScrollBtn_onRelease");
		btn.addEventListener("MouseLeave", "ScrollBtn_onRollOut");
		o.scroll_left_btn = btn;
		o.scroll_left_btn_up = o.root.findName("scroll_left_btn_up");

		// Right scroll button
		btn = o.root.findName("scroll_right");		// Show/hide region
		btn.addEventListener("MouseEnter", "ScrollShow_onRollOver");
		btn.addEventListener("MouseLeave", "ScrollShow_onRollOut");
		btn = o.root.findName("scroll_right_btn");	// Functional section
		btn.addEventListener("MouseEnter", "ScrollBtn_onRollOver");
		btn.addEventListener("MouseLeftButtonDown", "ScrollBtn_onPress");
		btn.addEventListener("MouseLeftButtonUp", "ScrollBtn_onRelease");
		btn.addEventListener("MouseLeave", "ScrollBtn_onRollOut");
		o.scroll_right_btn = btn;
		o.scroll_right_btn_up = o.root.findName("scroll_right_btn_up");

		// References for scaling
		o.clip_bg = o.root.findName("bg");
		o.clip_fg = o.root.findName("fg");
		o.clip_slider = o.root.findName("clip_slider");
		o.clip_scroll_left = o.root.findName("clip_scroll_left");
		o.clip_scroll_right = o.root.findName("clip_scroll_right");

		// Tooltip
		o.tooltip = { root:o.root.findName("tooltip"),
					  info:o.root.findName("tooltip_info"),
					  bg:o.root.findName("tooltip_bg"),
					  clip:o.root.findName("tooltip_clip"),
					  link:o.root.findName("tooltip_link"),
					  date:o.root.findName("tooltip_date")
					};
		o.tooltip.root.addEventListener("MouseEnter", "Tooltip_onRollOver");
		o.tooltip.root.addEventListener("MouseLeave", "Tooltip_onRollOut");
		o.tooltip.link.addEventListener("MouseEnter", "TooltipLink_onOver");
		o.tooltip.link.addEventListener("MouseLeave", "TooltipLink_onOut");
		o.tooltip.link.addEventListener("MouseLeftButtonUp", "TooltipLink_onRelease");

		// Set up animation object (only one, ever!)
		o.Animations = new Animations(o.root);

		// List of all individual renderable entities
		o.entities = new Array();

		// Initialize and randomize the angles for zRot
		// There should only be as many zRots are there are root branches. 
		// Figure out how many root branches we need.
		var nRootBranches = Math.ceil(g_totalBranches / (g_maxChildBranches + 1));  // Parent + children = number of branches per root branch.
		for (var irot = 0; irot < nRootBranches; irot++)
		{
			g_azRot.push(Math.PI * (2 * (irot % 5) / 5 - Math.floor(irot / 5) / 6));    
		}

		o.trunkBranches = 0; // count of transition and leaf branches in the branches array that
							 // make up the "trunk"

		// Generate leaf branches
		for (var i=0; i < g_totalBranches; i++)
		{
			o.branchSets.push(o.AddLeaf(i));
		}
		

		// Set up the world
		o.projector = new Projector( {	w:slc_Width,
										h:slc_Height,
										fov:(g_r_fov * Math.PI / 180),
										roll:0,
										r:Math.PI,
										x:g_r_xPos,
										y:0,
										z:g_r_zPos
								   } );

		o.projector.rad = g_r_xPos;
		o.projector.SetEntities(o.entities);
		
		// Update the viewplane (render ents)
		o.WorldUpdate();
		o.AdjustBranchView(g_sl_start, 2000);
		
		// Resize/Fullscreen handling
		//	--> Handled by main app
		//o.control.content.onFullScreenChange = "onFullScreenChange";
		//o.control.content.onResize = "onWindowResize";
		
		// Animations setup
		init_WorldThink(o.projector);
	},

	///////////////////////////////////////////////////////////////////////////////
	// Entity render pass
	WorldUpdate: function()
	{
		var o = this;

		o.projector.UpdateRotationMatrix();
		o.projector.Render();
		o._stale = true;

		// Update all registered entities
		for(var i=0; i<o.entities.length; i++)
		{
			var ent = o.entities[i];

			// Update only if the world or the ent state has been changed
			if(ent._stale || o._stale)
			{
				// Render the update
				ent.Render();
			}
		}

		// World is current
		o._stale = false;
		
		return;
	},

	///////////////////////////////////////////////////////////////////////////////
	// Update branch items
	UpdateItems: function(aItems)
	{
		var o = stage;

		// Requery if we get a bad response/data
		if(aItems == null || aItems.length == null || aItems.length == 0)
		{
			o.GetMoreResults(g_GetResults_T);
			return;
		}

		// Verify all items before accepting
		for(var i=0; i<aItems.length; i++)
		{
			var item = aItems[i];
			if(item.name == null || item.weight == null || item.link == null)
			{
				return(i+1);
			}
		}

		// Reset content references
		o.aItems = aItems;
		o.projector._c_content = 0;
		
		// Start the initial animation into the tree
		if(o._animTransitions == null)
		{
			var b_cnt = 0;
			
			// Pass info to branch leaf nodes
			for(var i=0; i<o.aItems.length; i++)
			{
				while(o.branches[b_cnt] != null && !o.branches[b_cnt++].UpdateInfo(o.aItems[i]));
			}

			o._animTransitions = { };
			OpenTree(o._animTransitions);

			// Enable the display
			o.root.Visibility = "Visible";
			
			// Set up cache for the next content update
			o.GetMoreResults(g_GetResults_T);
		}

		return(0);
	},


	///////////////////////////////////////////////////////////////////////////////
	// Set framework callback when all passed items have been processed/timed out
	SetAddMoreResultsCallback: function(fn)
	{
		this._animResults._frameworkCallback = fn;	
	},


	///////////////////////////////////////////////////////////////////////////////
	// Set up a timeout for more results query
	GetMoreResults: function(delay)
	{
		var o = this;

		if(delay == 0)
		{
			o._animResults._frameworkCallback();
			return;
		}

		o.Animations.Unlink(o._animResults);
		o.Animations.anim_null(o._animResults, delay, o._animResults._frameworkCallback, null);
	},

	///////////////////////////////////////////////////////////////////////////////
	// Adjust total branches displayed
	AdjustBranchView: function(pct, T)
	{
		// Only update if we change the count threshold
		var new_branches = Math.floor(pct * g_totalBranches) + 1;
		if(g_activeBranches == new_branches)
		{
			return;
		}

		var o = stage;
		o.entities = [ ];
		g_activeBranches = new_branches;

		// Update the entire branch set
		for(var i=0; i<o.branchSets.length; i++)
		{
			var b = o.branchSets[i];
			for(var j=0; j<b.length; j++)
			{
				if(i < g_activeBranches)
				{
					b[j].Visibility(true);
					o.entities = o.entities.concat(b[j].aEnts);
				}
				else
				{
					b[j].Visibility(false);
				}
			}
		}

		// Update the world view
		o.projector.SetEntities(o.entities);
		o.WorldUpdate();
		
		if(T > 0)
		{
			o.Animations.Unlink(o);
			o.Animations.anim_generic(o, o.projector, "rad", T, 0, o.projector.rad, g_r_xPos + g_sl_zoom_range * (g_activeBranches) / o.branchSets.length, false, null);
		}
	},


	///////////////////////////////////////////////////////////////////////////////
	// Branch defined by pts [SP, CP, EP] and type (BT_Trunk, BT_Leaf or BT_Transition)
	AddBranch: function(id, pts, type, dir, branchParent, branchTransition)
	{
		var o = this;
		// Duplicate branch id's for connected leaf, transition and trunk branches.
		var p = new Branch(o.root, id, o.branches.length);

		// Create world settings here and add to params object, copying 
		//      branchParent info as necessary.		
		var	scale = GetScale(pts[0], pts[2]);
		if (branchParent != null 
			&& type == BT_Leaf          // Only track child branches here. 
			&& id != branchParent._id)  // Trunk and transition branches with same id are tracked elsewhere
		{   
			// Allow parent to keep track of child branches for updates		    
			branchParent.AddChildBranch(p);
		}
		
		// Keep track of transition branch so we can branch off of them
		if (branchTransition != null)
		{			
			p.branchTransition = branchTransition; 		
		}

		var zRot = 0;				
		if (branchParent != null) 
		{
			// Line up the zRot values if:
			//     this is a child branch or 
			//     a Leaf branch connecting to a Transition branch or
			//     a Transition branch connecting to a Trunk branch
			zRot = branchParent.zRot;
		}
		else
		{
			// Assign the root branches a random zRot from an evenly spaced set of angles
			// Parent + children = number of branches per root branch.
			//var nRootBranches = Math.ceil(g_totalBranches/(g_maxChildBranches + 1));  
			var izRot = Math.floor(id / (g_maxChildBranches + 1));		
			zRot = g_azRot[izRot];
		}	

		var view = { scale:0.5,
					 xRot:-Math.PI / 2,
					 yRot:0,
					 zRot:zRot,
					 x:0,		// in/out
					 y:0,		// left/right
					 z:0
					};

		var params = { scaleLeaf:scale,
					   scaleBranch:(3 * scale),
					   pts:pts,
					   view:view };

		o.entities = o.entities.concat(p.Initialize(type, dir));
		p.Update(params);
		o.branches.push(p);	// Track for further updates
		if (p.branchType == BT_Trunk)
		{
			o.trunkBranches++;
		}

		// Hack to get offset angles. Real fix is to properly set z angle when computing curves.
		// Offset per child: every other one is on the opposite side.
		if (branchParent != null 
			&& type == BT_Leaf 
			&& branchParent._id != id)
		{		
			var childCount = branchParent.GetChildCount();
			offset = (0.5 + 0.9 * Math.random()) * Math.PI / 3 * (childCount % 2 == 1 ? -1 : 1);
			p.AdjustOffset(offset);
			if (p.branchTransition != null)
			{
				p.branchTransition.AdjustOffset(offset);
			}
		}

		return(p);
	},

	
	///////////////////////////////////////////////////////////////////////////////
	// Get parent branch for a new leaf node 
	GetParentBranch: function(id)
	{
		var o = this;
		var branchParent = null;
		// Simplistic parenting v2: parent branchs can have g_maxChildBranches so
		//    - childid >= parentid + 1 && <= parentid + g_maxChildBranches.
		//    - branch.depth < g_maxTreeDepth
		// In the above two cases, return null and the branch will be a new root branch.
		for(var i=0; i<o.branches.length; i++)
		{
			var branch = o.branches[i];
			if (branch.branchType == BT_Leaf)
			{											
				if (branch.depth < g_maxTreeDepth &&
					branch.GetChildCount() < g_maxChildBranches && 
					(id > branch._id && id <= branch._id + g_maxChildBranches))
				{
					// Randomly branch off either Leaf branch or transition branch (if exists)
					var fUseLeaf = true;
					if (branch.branchTransition != null)
					{
						fUseLeaf = (Math.random() >= 0.5);
					}
					branchParent = fUseLeaf ? branch : branch.branchTransition;
					break;
				}
			}
		}

		return(branchParent);
	},


	///////////////////////////////////////////////////////////////////////////////
	AddLeaf: function(id)
	{
		var o = this;

		var b_list = [ ];
		// Points that define the branch that connects to the Leaf
		var dir;
		var sppct = 0;
		var branchParent = o.GetParentBranch(id);
		var spLeaf = null;
		var cpLeaf = null;
		var epLeaf = GetEndPoint(id, branchParent);
		var branchTrunk = null;
		var branchTransition = null;
		
		if (branchParent != null)
		{
			spInfo = GetStartPoint(id, branchParent);
			sppct = spInfo.pct;
			spLeaf = spInfo.pt;
		}
		else
		{		
			// Root branches each contribute a transitional branch to the trunk. 
			// Increment x value so branches build up beside each other
			// Get the points for the trunk branch
			var rootPoint = 0;
			for (iRoot = 0; iRoot < g_nRootPoints; iRoot++)
			{
				if (id < (iRoot + 1) * g_totalBranches/g_nRootPoints)
				{
					rootPoint = iRoot;
					break;
				}
			} 			
			xTrunk = g_xStart + o.trunkBranches * g_dxRootPoint;
			yTrunk = g_yTrunkEnd + (rootPoint * g_dyRootPoint);
			spTrunk = { x:xTrunk, y:g_yTrunkStart, z:0 };
			epTrunk = { x:xTrunk, y:yTrunk, z:0 };
			cpTrunk = GetMidPoint(spTrunk, epTrunk);
			var ptsTrunk = [ spTrunk, cpTrunk, epTrunk ];
			
			// Create and add trunk branch
			branchTrunk = o.AddBranch(id, ptsTrunk, BT_Trunk);
			branchParent = branchTrunk;
			b_list.push(branchTrunk);

			// Set up new leaf coordinates that start from trunk branch.
			spLeaf = epTrunk;
			sppct = 1.0;
		}		
		
		// Calculate the Bezier curve control point for our branch.
		if (Math.sqrt(DistanceSquared(spLeaf, epLeaf)) > g_maxBranchLength)
		{
			// For long branches, use transitional branches      
			// Get the points for the transition branch
			var spTransition = spLeaf;			
			var epTransition = GetMidPoint(spLeaf, epLeaf);			
			var cpTransition = GetControlPoint(id, branchParent, spTransition, epTransition, sppct);
			var ptsTransition = [ spTransition, cpTransition, epTransition ];

			// Create and add transition branch
			branchParent = o.AddBranch(id, ptsTransition, BT_Transition, dir, branchParent);
			branchTransition = branchParent;
			b_list.push(branchParent);

			// Set up new leaf coordinates that start from transitional branch.
			spLeaf = epTransition;
			cpLeaf = GetConnectingBezierCurve(ptsTransition, 1.0, epLeaf);
		}
		else
		{	
			cpLeaf = GetControlPoint(id, branchParent, spLeaf, epLeaf, sppct);
		}		

		ptsLeaf = [ spLeaf, cpLeaf, epLeaf ];
		var branchLeaf = o.AddBranch(id, ptsLeaf, BT_Leaf, dir, branchParent, branchTransition);
		b_list.push(branchLeaf);

		// Keep track of Leaf branch so we can keep track of all children from a Transition branch
		if (branchTransition != null)
		{			
			branchTransition.branchLeaf = branchLeaf; 		
		}
		
		// Adjust trunk scale to match the rest of the branch
		if (branchTrunk != null)
		{
			branchTrunk._cachedParams.scaleBranch = branchLeaf.scaleBranch;
			branchTrunk.Update(branchTrunk._cachedParams);
		}
		return(b_list);
	}	
}


///////////////////////////////////////////////////////////////////////////////
// Set up core pre-frame animation/logic
function init_WorldThink(o)
{
	var p = stage.Animations.AddAnimation(o);
	var dt = new Date();
	var t = dt.getTime();
	
	// Rotation animation
	p.args._T = 1000;
	p.args._startTime = t + 0;
	p.args._fn = null;
	p.args._lastTime = p.args._startTime;

	p.fn = WorldThink;

	// Age cycling
	o._c_content = 0;
	
	// Timers for each age. Index is the age identifier.
	o._T_ages = new Array(g_numAges);
	o._T_ages[1] = t + (g_a_updateItem_T * g_totalBranches) + (g_a_nextItems_T +  (0 * 3 * g_T_leafTransition));	// orange first
	o._T_ages[2] = t + (g_a_updateItem_T * g_totalBranches) + (g_a_nextItems_T +  (10 * 3 * g_T_leafTransition));	// rustic next
	o._T_ages[0] = t + (g_a_updateItem_T * g_totalBranches) + (g_a_nextItems_T + (15 * 3 * g_T_leafTransition));	// back to green last

	// Current branch index for a given age
	o._c_ages = new Array(g_numAges);
	o._c_ages[1] = 0;
	o._c_ages[2] = 0;
	o._c_ages[0] = 0;
}

///////////////////////////////////////////////////////////////////////////////
// World rotate "think"
function WorldThink(o, args, t)
{
	// Rotation
	if(g_a_step != 0)
	{
		o.r += (t - args._lastTime) * g_a_step;

		// FIXME: Need to cache Math.* properly
		o.x = o.rad * Math.cos(o.r);
		o.y = o.rad * Math.sin(o.r);

		stage.WorldUpdate();
	}
	
	// For next delta
	args._lastTime = t;

	// Aging update checks
	for(var i=0; i<o._T_ages.length; i++)
	{
		if(o._T_ages[i] < t)
		{
			var fFound = false;
			while(!fFound)
			{
				for(var j=o._c_ages[i]; j<stage.branches.length; j++)
				{
					if(stage.branches[j].UpdateLeaves(i, 0))
					{
						fFound = true;
						o._c_ages[i] = j + 1;
						// Special case for age==0 to also do a content update
						if(i == 0)
						{
							stage.branches[j].UpdateInfo(stage.aItems[o._c_content++]);
							
							// If we need to also do a query for the next run
							if(o._c_content == g_totalBranches)
							{
								stage.GetMoreResults(0);
							}
						}
						break;
					}
				}
				if(!fFound)
				{
					o._c_ages[i] = 0;
				}
			}
			o._T_ages[i] = t + 3 * g_T_leafTransition;
		}
	}

	return(true);
}

///////////////////////////////////////////////////////////////////////////////
// Slider button handling - SL doesnt support dragging properly, so have
// to use the root stage to allow for fast mouse movement
function Slider_onOver(sender, args)
{
}
function Slider_onOut(sender)
{
}
function Slider_onPress(sender, args)
{
	var o = stage.slider.root;

	pt = args.getPosition(o);
	if(pt.x >= 0 && pt.x <= o.Width && pt.y >= 0 && pt.y <= o.Height)
	{
		stage.slider.eventToken = sender.addEventListener("MouseMove", "Slider_onMove");
	}
}
function Slider_onRelease(sender, args)
{
	// Race with main app that we get pulled out before the event is properly serviced
	if(stage == null || stage.scroll_left_btn_up == null || stage.scroll_right_btn_up == null)
	{
		return;
	}

	sender.removeEventListener("MouseMove", stage.slider.eventToken);
}
function Slider_onMove(sender, args)
{
	var o = stage;
	var pt = args.getPosition(o.clip_slider);
	var x = (pt.x - o.slider.root.Width / 2);

	// Clamp ranges
	if(x < g_sl_min) x = g_sl_min;
	else if(x > g_sl_max) x = g_sl_max;

	o._animToAll = false;
	o.slider.root["Canvas.Left"] = x;
	
	var pct = (x - g_sl_min) / (g_sl_mag + 1);	// Ensure we never get 100%

	// Update the branch view
	stage.AdjustBranchView(pct, 2000);
}

///////////////////////////////////////////////////////////////////////////////
// Show/hide rotation buttons
function ScrollShow_onRollOver(sender, args)
{
	var o = stage._animBtns;

	stage.Animations.Unlink(o);

	var p = stage.scroll_left_btn_up;
	stage.Animations.anim_generic(o, p, "Opacity", g_a_ScrollBtnFade_T, 0, p.Opacity, 100, true, null);
	p = stage.scroll_right_btn_up;
	stage.Animations.anim_generic(o, p, "Opacity", g_a_ScrollBtnFade_T, 0, p.Opacity, 100, true, null);
}
function ScrollShow_onRollOut(sender)	// man, would sure like an args on this event :)
{
	var o = stage._animBtns;

	stage.Animations.Unlink(o);

	var p = stage.scroll_left_btn_up;
	stage.Animations.anim_generic(o, p, "Opacity", g_a_ScrollBtnFade_T/2, 0, p.Opacity, 0, false, null);
	p = stage.scroll_right_btn_up;
	stage.Animations.anim_generic(o, p, "Opacity", g_a_ScrollBtnFade_T/2, 0, p.Opacity, 0, false, null);
}

///////////////////////////////////////////////////////////////////////////////
// Rotation button handling
function ScrollBtn_onRollOver(sender, args)
{
	sender.cursor = "Hand";

	stage[sender.Name].Opacity = 1;
	stage[(sender.Name + "_up")].Visibility = "Collapsed";

	// Ugh, lower layer handling is lacking...
	stage.Animations.Unlink(stage._animBtns);
}
function ScrollBtn_onRollOut(sender)
{
	sender.cursor = "Arrow";

	stage[sender.Name].Opacity = 0;
	stage[(sender.Name + "_up")].Visibility = "Visible";
}
function ScrollBtn_onPress(sender, args)
{
	g_a_step += g_a_delta * ((sender.Name == "scroll_left_btn") ? -1 : 1);
	if(Math.abs(g_a_step) < g_a_delta)
	{
		g_a_step = 0;
	}
	stage[sender.Name].Opacity = 0;
	stage[(sender.Name + "_up")].Visibility = "Visible";
}
function ScrollBtn_onRelease(sender, args)
{
	stage[sender.Name].Opacity = 1;
	stage[(sender.Name + "_up")].Visibility = "Collapsed";
}

///////////////////////////////////////////////////////////////////////////////
// Windowed button handling
function WindowedBtn_onPress(sender, args)
{
	stage[sender.Name].Opacity = 0;
	stage[(sender.Name + "_up")].Visibility = "Visible";
}
function WindowedBtn_onRelease(sender, args)
{
	sender.Opacity = 1;
	stage[(sender.Name + "_up")].Visibility = "Collapsed";
	stage.clip_windowed.Visibility = "Collapsed";
	stage.clip_fullscreen.Visibility = "Visible";
	stage.control.settings.maxFrameRate = slc_MaxFPS;
	stopScreenSaver();
}
function FullscreenBtn_onRelease(sender, args)
{
	sender.Opacity = 1;
	stage[(sender.Name + "_up")].Visibility = "Collapsed";
	Stage_setFullScreen(true);
	stage.clip_fullscreen.Visibility = "Collapsed";
	stage.clip_windowed.Visibility = "Visible";
}

///////////////////////////////////////////////////////////////////////////////
// Tooltip button handling
function Tooltip_onRollOver(sender, args)
{
	sender.cursor = "Arrow";
	stage._origSpeed = g_a_step;
	g_a_step = 0;

	var o = stage.tooltip;
	o.root.Visibility = "Visible";
	stage.Animations.Link(o);
	stage.Animations.anim_generic(o, o.root, "Opacity", g_a_Tooltip_T, 0, o.root.Opacity, 1.0, false, null);
}
function Tooltip_onRollOut(sender)
{
	sender.cursor = "Arrow";
	g_a_step = stage._origSpeed;

	var o = stage.tooltip;
	stage.Animations.Link(o);
	stage.Animations.anim_generic(o, o.root, "Opacity", g_a_Tooltip_T, 0, o.root.Opacity, 0, false, HideTooltip);
}
function TooltipLink_onOver(sender, args)
{
	sender.cursor = "Hand";
	sender.Foreground = "DarkBlue";

	var o = stage.tooltip;
	o.root.Visibility = "Visible";
	stage.Animations.Link(o);
	stage.Animations.anim_generic(o, o.root, "Opacity", g_a_Tooltip_T, 0, o.root.Opacity, 1.0, false, null);
}
function TooltipLink_onOut(sender)
{
	sender.Foreground = "Blue";
}
function TooltipLink_onRelease(sender, args)
{
	SJ.openWindow(stage.tooltip._link);
}

///////////////////////////////////////////////////////////////////////////////
// Set up open tree anim
function OpenTree(o)
{
	var p = stage.Animations.AddAnimation(o);
	var dt = new Date();

	p.args._T = g_a_start_T;
	p.args._startTime = dt.getTime() + p.args._T;
	p.args._fn = null;
	p.args._pct = 0;
	
	p.fn = doOpenTree;

	// Bring in the tree
	var o = stage;
	o.Animations.Unlink(o);
	o.Animations.anim_generic(o, o.projector, "rad", (g_a_start_T * g_totalBranches * 1.8), 0, o.projector.rad, g_r_xPos + g_sl_zoom_range, true, null);
}

///////////////////////////////////////////////////////////////////////////////
// Open tree "think" loop
function doOpenTree(o, args, t)
{
	if(!stage._animToAll)
	{
		return(false);
	}

	args._pct += (1 / stage.branchSets.length);

	if(args._pct <= 1)
	{
		args._startTime = t + args._T;
		stage.AdjustBranchView(Math.min(args._pct, 0.99), 0);

		return(true);
	}

	stage._animToAll = false;

	return(false);
}