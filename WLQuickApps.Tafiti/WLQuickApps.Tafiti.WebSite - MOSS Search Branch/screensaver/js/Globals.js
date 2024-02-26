// Copyright 2007 Microsoft Corp. All Rights Reserved.

// Global definitions
var g_x_space = 'xmlns="http://schemas.microsoft.com/client/2007" xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"'

var g_iRad = 180 / Math.PI;
var g_2PI = Math.PI * 2;

// SL control settings
var slc_MaxFPS = 8;
var slc_Width = 1024;
var slc_Height = 768;
var slc_FramerateCounter = 1;
var slc_scale_x = 1;
var slc_scale_y = 1;

// projector settings
var g_r_fov = 70;
var g_r_xPos = -650;
var g_r_zPos = 100;
var g_a_step_o = (1/5) / (2 * Math.PI * 1 * 1000);
var g_a_step = g_a_step_o;								// Rotation step
var g_a_delta = 1 / (2 * Math.PI * 1 * 1000 * 5);		// Direction delta
var g_maxLevel = 10000;

// Site params
var g_totalBranches = 30;
var g_activeBranches = g_totalBranches;
var g_xStart = 0;
var g_xDelta = 0;
var g_yStart = -310;	// Adjusts overall vertical position (lower = higher)
var g_yDelta = -1200;

// Window-dressing
var g_txtDefaultSize = 8.0;
var g_txtMaxWidth = 150;	// px
var g_txtMaxScale = 1.5;
var g_defaultDate = "";

// Slider state
var g_sl_start = 0.9999;		// start point
var g_sl_zoom_range = 200;	// Max zoom in when all branches are showing
var g_sl_min = 99;
var g_sl_max = 755;
var g_sl_mag = g_sl_max - g_sl_min;

// Branching params
//var g_treeShape = "sphere";            // Supported shapes: cone, sphere, and apple
var g_treeShape = "apple";            // Supported shapes: cone, sphere, and apple
//var g_treeShape = "cone";            // Supported shapes: cone, sphere, and apple
var g_nRootPoints = 1;                // Number of points on the trunk to branch off of
var g_maxChildBranches = 1;           // Max number of children per branch. 
var g_maxTreeDepth = 1;               // Max depth of the tree; branches at max depth can't have children.
var g_yTrunkStart = 300;              // y-value of the bottom of the trunk
var g_yTrunkEnd = g_yStart;           // y-value of the top of the trunk
var g_dyRootPoint = (g_yTrunkStart-g_yTrunkEnd) * 0.65/g_nRootPoints; // delta-y between root points on the trunk 
var g_dxRootPoint = 0.30;             // delta-x between trunk branches
var g_treeHeight = slc_Height * 0.70; // Height of the tree
var g_treeWidth = slc_Width * 0.40;   // Width of the tree
var g_maxBranchLength = g_treeWidth;  // Max length of a branch before we split it into transition + leaf branches
var g_dyChildMin = 75;                // Min y-distance from child to parent leaves
var g_dxChildMin = 75;                // Min x-distance from child to parent leaves

// Branch types
var BT_Transition = 0;
var BT_Leaf = 1;
var BT_Trunk = 2;

// Bezier curve direction 
var BZ_Concave = false;
var BZ_Convex = true;

// Static ids for trunk and transition branches
var BID_Trunk = 0xFFFF;
var BID_Transition = 0xFFFE;
var g_azRot = new Array();

var g_leafColorRange0 = [0xFF, 0x00, 0x6C, 0x4B];
var g_leafColorRange1 = [0xFF, 0x00, 0xA0, 0x4B];
var g_leafColors = [
					[	// Greens
						[0xFF, 0x00, 0x6C, 0x4B]	//"#FF006C4B"
					  ,	[0xFF, 0x00, 0xA0, 0x4B]	//"#FF00A04B"
					  ,	[0xFF, 0x45, 0x81, 0x2B]	//"#FF45812B"
					  ,	[0xFF, 0x00, 0x75, 0x34]	//"#FF007534"
					  ,	[0xFF, 0x49, 0xA8, 0x42]	//"#FF49A842"
					  ,	[0xFF, 0x6A, 0x8A, 0x22]	//"#FF6A8A22"	// mid yellow-green
					  ,	[0xFF, 0x00, 0x72, 0x3F]	//"#FF00723F"	// dk warm green
					  ,	[0xFF, 0x0D, 0xB1, 0x4B]	//"#FF0DB14B"	// mid warm green
					  ,	[0xFF, 0x00, 0x81, 0x59]	//"#FF008159"	// dk blue-green
					],
					  
					[	// Mids
						[0xFF, 0xF1, 0x5A, 0x22]	//"#FFF15A22"	// orange-red
					  ,	[0xFF, 0xF3, 0x70, 0x21]	//"#FFF37021"	// orange
					  ,	[0xFF, 0xCF, 0x91, 0x12]	//"#FFCF9112"	// golden brown
					  ,	[0xFF, 0x70, 0x93, 0x45]	//"#FF709345"	// dk olive green
					  ,	[0xFF, 0xA8, 0xC5, 0x6B]	//"#FFA8C56B"	// lt olive green
					  ,	[0xFF, 0xA1, 0xBF, 0x2F]	//"#FFA1BF2F"	// yellow green
					  ,	[0xFF, 0xE5, 0xA7, 0x12]	//"#FFE5A712"	// lt orange creme
					  ,	[0xFF, 0xD6, 0xBE, 0x00]	//"#FFD6BE00"	// dirty yellow
					  ,	[0xFF, 0x8B, 0x94, 0x14]	//"#FF8B9414"	// brownish yellow green
					  ,	[0xFF, 0xD0, 0x9B, 0x2C]	//"#FFD09B2C"	// fleshy
					  ,	[0xFF, 0x7A, 0x83, 0x27]	//"#FF7A8327"	// dk yellow green
					],
					
					[	// Olds
						[0xFF, 0xB5, 0x8C, 0x21]	//"#FFB58C21"	// ltr brown
					  ,	[0xFF, 0xB5, 0x5A, 0x43]	//"#FFB55A43"	// reddish brown
					  ,	[0xFF, 0xA7, 0x6A, 0x17]	//"#FFA76A17"	// ltr brown
					  ,	[0xFF, 0xD9, 0x46, 0x31]	//"#FFD94631"	// reddish
					  ,	[0xFF, 0xA6, 0x5D, 0x17]	//"#FFA65D17"	// warm brown
					  ,	[0xFF, 0x8A, 0x4D, 0x1F]	//"#FF8A4D1F"	// dkr brown
					  ,	[0xFF, 0xA1, 0x54, 0x17]	//"#FFA15417"	// mid brown
					]
				   ];

var g_numAges = g_leafColors.length;	// # of available "ages" of the tree
var g_branchColorRange0 = [0x7F, 0x4E, 0x2F, 0x10];	// Closer
var g_branchColorRange1 = [0x7F, 0x4E, 0x2F, 0x10];	// Further back


// Anim settings
var g_a_branch_TfadeIn = 1500;		// Branch "show" length
var g_a_branch_TfadeOut = 900;		// Branch "hide" length
var g_a_start_T = 100;				// Branch initial roll-in length (between branches)
var g_a_updateItem_T = 1000;		// Item update length (between branches)
var g_a_nextItems_T = 10000 * 2 / 3;		// Get next items interval after update/interaction
var g_a_ScrollBtnFade_T = 1750;		// Fade time for showing scroll rotation buttons
var g_a_Tooltip_T = 300;			// Fade time for tooltip display
var g_a_TooltipPauseIn_T = 350;		// Delay showing tooltip
var g_a_TooltipPauseOut_T = 250;	// Delay fading tooltip
var g_T_leafTransition = 1000;		// Leaf color transition period
var g_GetResults_T = 5000;			// Time interval to wait to query new results