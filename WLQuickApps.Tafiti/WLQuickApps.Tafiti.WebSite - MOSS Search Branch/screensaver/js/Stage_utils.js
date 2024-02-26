// Copyright 2007 Microsoft Corp. All Rights Reserved.

///////////////////////////////////////////////////////////////////////////////
// Return the control point, cp2, of the Bezier curve, [sp2, cp2, ep2] 
// which originates on another Bezier curve (pts1) where 
//     sp2 is defined as sp2pct % (from 0-1) along pts1, and
//     cp2 is along the tangent of sp2 to pts1. 
function GetConnectingBezierCurve(pts1, sp2pct, ep2)
{
	var sp2Info = GetPointOnQuadBezier(pts1, sp2pct);
    var sp2 = sp2Info.pt;
    
    // Q - How should we calculate offset on curve1's tangent for curve2's control point?    
    // A -Let's make SP2-CP2 and CP2-EP2 equal so that the curve isn't lopsided.    
    // Calculate a1, angle between SP2-EP2 and x-axis
    var dy_se = ep2.y - sp2.y;
    var dx_se = ep2.x - sp2.x;
    var a1 = null;
    if (dx_se == 0)
        a1 = Math.PI/2; // 90 degrees
    else
        a1 = Math.atan(dy_se/dx_se);
    
    // Calculate a3, angle between SP-CP (tangent from sp2info) and x-axis
    var a3 = Math.atan(sp2Info.slope);
    
    // Calculate a2, angle between SP-CP and SP-EP
    var a2 = a3-a1;
    
    // Calculate hyp as the hypoteneuse of triangle SP-MP-CP
    var mp2 = GetMidPoint(sp2, ep2);
    hyp = Math.sqrt(DistanceSquared(sp2, mp2))/Math.cos(a2);
    
    // Now we can get the cx and cy values, relative to SP2.
    var cy = hyp * Math.sin(a3);
    var cx = hyp * Math.cos(a3);
    
    var cp2 = { x:sp2.x + cx, y:sp2.y + cy, z:0 };              
    
    // Validate cp2: we want it to be equidistant from sp2 and ep2
    var dce = DistanceSquared(cp2, ep2);
    var dcs = DistanceSquared(cp2, sp2);
    if (Math.abs(dce - dcs) > 0.01)  // arithmetic not 100% precise
        cp2 = { x:sp2.x - cx, y:sp2.y - cy, z:0 };              

    // Debug code
    dce = DistanceSquared(cp2, ep2);
    dcs = DistanceSquared(cp2, sp2);
    if (Math.abs(dce - dcs) > 0.01)
        alert('Trouble calculating CP');
                
	return cp2;
}

///////////////////////////////////////////////////////////////////////////////
// Get the square of the distance between two points. Don't take the square
// root to save CPU cycles for comparisons requiring multiple distances
function DistanceSquared(pt1, pt2)
{
    var dxsq = Math.pow(pt2.x - pt1.x, 2);
    var dysq = Math.pow(pt2.y - pt1.y, 2);
    var dzsq = Math.pow(pt2.z - pt1.z, 2);

	return (dxsq + dysq + dzsq);
}

///////////////////////////////////////////////////////////////////////////////
// Return the point in the middle of the two given points.
function GetMidPoint(pt1, pt2)
{
    mp = { x:(pt2.x + pt1.x)/2, 
           y:(pt2.y + pt1.y)/2, 
           z:(pt2.z + pt1.z)/2 };
    
    return mp;
}

///////////////////////////////////////////////////////////////////////////////
// Temp function to centralize scale calculations
function GetScale(sp, ep)
{
//	return (ep.y - sp.y) / (0.75 * g_yDelta) * 0.25;
	return Math.max(Math.abs(ep.y - sp.y), Math.abs(ep.x - sp.x)) / Math.abs(0.75 * g_yDelta) * 0.25;
}

///////////////////////////////////////////////////////////////////////////////
// Based on the start point and end point, return a control point that gives 
// the curve desirable characteristics and an attractive appearance. This is done
// by limiting CP to the line perpendicular to the midpoint between SP and EP.
//  sp    : start point for the bezier curve
//  ep    : end point for the bezier curve
//  angle : angle between lines CP-SP and CP-EP that connects 
//          them on the line perpendicular to MP 
//  type  : BZ_Concave for a curve that curves down, BZ_Convex for a curve that curves up.
//
// Notes:
//    See GetConnectingBezierCurve() for tangential curve. 
//    Use GenerateBezierControlPoint() to generate a CP with no parent curve.
function GenerateBezierControlPoint(sp, ep, angle, type)
{    
	var mp = GetMidPoint(sp, ep);   // Get the midpoint
	
	// Solve for the hypotenuse for the right triangle CP-MP-SP
	var opp = Math.sqrt(DistanceSquared(sp, mp));
	a0 = ( angle * Math.PI / 180 ) * 0.5; // Convert to radians and split in half.
	var hyp = opp / Math.sin(a0);

	// Solve for the angle between SP-CP and SP-MP
	var a1 = Math.PI/2 - a0;

	// Get slope of the SP-MP-EP line    
	var dy = sp.y - ep.y;
	var dx = ep.x - sp.x; // Assumes ep.x > sp.x        
	if (dx == 0)
		dx = 0.01;         // can't divide by 0, so approximate things from here.
	var m = dy/dx;         // slope of the sp-mp-ep line.  
	
	var a2 = Math.atan(1/m); // a2 is between y-axis and SP-EP
	a2 = Math.abs(a2);
	
	// a3 is between y-axis and SP-CP
	if (dy > 0) //   if (m > 0)
	{
		var a3 = a1 + a2;
	}          
	else // if (m < 0) 
	{
		var a3 = a2 - a1;    
	}

	// Calculate cx and cy, which are relative to sp (convex) or ep (concave)
	cx = hyp * Math.sin(a3);
	cy = hyp * Math.cos(a3);

	// Calculate CP.x and CP.y in global coordinates.
	if (type == BZ_Concave)
	{
		var cpx = ep.x - cx;
		if (dy > 0) // if (m > 0)
			var cpy = ep.y + cy;
		else // if (m < 0)
			var cpy = ep.y - cy;        
	}
	else if (type == BZ_Convex)
	{
		var cpx = sp.x + cx;
		if (dy > 0) // if (m > 0)
			var cpy = sp.y - cy;        
		else // if (m < 0)
			var cpy = sp.y + cy; 
	}

   return { x:cpx, y:cpy, z:0 };
}


/////////////////////////////////////////////////////////////////////////
// Get a point on a given bezier curve
//	pts : Array of points in the order of - EP0, CP, EP1
//	pct : Percentage along the line from EP0 to EP1 to get the point from (range: 0.0 - 1.0)
// Quadratic Bezier functions for 1 = t2 + 2t(1 - t) + (1 - t)2
function B1(t) { return ((1-t)*(1-t)); }
function B2(t) { return (2*t*(1-t)); }
function B3(t) { return (t*t); } 

function GetPointOnQuadBezier(pts, pct)
{
	var C1 = pts[0];
	var C2 = pts[1];
	var C3 = pts[2];    
	var x = C1.x*B1(pct) + C2.x*B2(pct) + C3.x*B3(pct);
	var y = C1.y*B1(pct) + C2.y*B2(pct) + C3.y*B3(pct);

	var dx = C1.x * (pct - 1) + C2.x * (1 - 2 * pct) + C3.x * pct;
	var dy = C1.y * (pct - 1) + C2.y * (1 - 2 * pct) + C3.y * pct;

	return { pt:{ x:x, y:y, z:0 }, slope:(dy/dx) };
}

/////////////////////////////////////////////////////////////////////////
// Pull world coordinates from a vertex object
function GetWPtFromVertex(vertex)
{
    return { x:vertex.wx, y:vertex.wy, z:vertex.wz };
}
function GetWPtsFromVertices(vertices)
{
    sp = { x:vertices[0].wx, y:vertices[0].wy, z:vertices[0].wz };
    cp = { x:vertices[1].wx, y:vertices[1].wy, z:vertices[1].wz };
    ep = { x:vertices[2].wx, y:vertices[2].wy, z:vertices[2].wz };
    
    return [ sp, cp, ep ];
}


///////////////////////////////////////////////////////////////////////////////
// Get the desired direction of the Bezier curve for root branches. 
// Child branches should derive their CP and dir from the tangent of sp2.
function GetBezierDirection(sp, ep)
{
	// Root branches should be concave, but if the slope of sp-ep
	// is steep, the curve would cross the y-axis so for those
	// ep's, make the root curve convex.
	var dy = sp.y - ep.y;
	var dx = ep.x - sp.x;
	if (dx == 0)
		dx = 0.01; // Can't divide by 0, approximate with a small dx value.
	return ((Math.abs(dy / dx) < 10) ? BZ_Concave : BZ_Convex);
}

///////////////////////////////////////////////////////////////////////////////
// Based on the slope of sp-ep, return the desired angle for the Bezier
// control point between lines cp-ep and sp-cp. 
function GetAngleForBezierControl(id, sp, ep)
{
	var angle = 100;

	var dy = sp.y - ep.y;
	var dx = ep.x - sp.x;
	if (dx == 0) dx = 0.01; // Can't divide by 0

	var m = Math.abs(dy/dx);

	if (m > 3)		angle = 160;
	else if (m > 2)	angle = 140;
	else if (m > 1)	angle = 120;
	return angle;
}


///////////////////////////////////////////////////////////////////////////////
// Return a point for the given branch id
// lower id#s are positioned higher, and higher id#s are lower and wider.
function GetEndPoint(branchid, branchParent)
{		
	var dy = g_treeHeight / g_totalBranches;	
	var dx = g_treeWidth / g_totalBranches;

	// negative y-axis goes up, so start really negative. 
	var y = (g_yTrunkEnd-g_treeHeight) + dy * (branchid + Math.random());
	var x = 0;
	
	switch (g_treeShape)
	{
		case "sphere":
			// top half of the tree: move x from start to max
			// A bit lower than half to keep bottom branches from getting too close to the trunk
			var halfBranches = g_totalBranches/1.5; 
			if (branchid < halfBranches)
			{
				x = g_xStart + 2 * dx * (branchid + Math.random());
			}
			// bottom half of the tree: move x from max back to start
			else
			{
				x = (g_xStart + g_treeWidth) - 2 * dx * (branchid - halfBranches + Math.random());	
			}		
			break;
		case "apple":
			// top quarter of the tree: move x from start to max
			var halfBranches = g_totalBranches/4; 
			if (branchid < halfBranches)
			{
				x = g_xStart + 4 * dx * (branchid + Math.random());
			}
			// bottom 3/4 of the tree: stay around the max
			else
			{
				x = (g_xStart + g_treeWidth) - 2 * dx * (Math.random());
			}		
			break;
		case "cone":
			x = g_xStart + dx * (branchid + Math.random());
			break;
	}

	// Respect minimum distance from the parent.
	if (branchParent != null)
	{
		var epParent = GetWPtFromVertex(branchParent.vertices[2]);
		var dx = Math.abs(epParent.x - x);
		var dy = Math.abs(epParent.y - y);
		if (dy < g_dyChildMin)
		{
			if (epParent.y < y)
				y = epParent.y + g_dyChildMin;
			else 
				y = epParent.y - g_dyChildMin;
		}
		if (dx < g_dxChildMin)
		{
			if (epParent.x < x)
				x = epParent.x + g_dxChildMin;
			else 
				x = epParent.x - g_dxChildMin;
		}
	}
	
	return { x:x, y:y, z:0 };
}

///////////////////////////////////////////////////////////////////////////////
// Return spInfo { pt, sppct } for the given branchid to start on the branchParent
function GetStartPoint(branchid, branchParent)
{		
	var spLeaf = null;
	var pct = 0;
	
	// Branch down from the parent to make the slider hide/show feature
	// consistently bottom up/top down.
	//
	var pct = 0;

	switch (branchParent.branchType)
	{
	case BT_Transition:
		// branching off transition branch tends to look better if SP is near the end
		pct = 0.9 + Math.random()*0.1;
		break;
	case BT_Leaf:
		// Halfway point of the Leaf branch to get an attractive curve. 
		pct = 0.45 + Math.random()*0.1;
		break;
	case BT_Trunk:
		alert("Shouldn't call GetStartPoint for a trunk branch");
		break;
	}	
		
	spLeaf = (GetPointOnQuadBezier(GetWPtsFromVertices(branchParent.vertices), pct)).pt;

	return { pt:spLeaf, pct:pct };
}

///////////////////////////////////////////////////////////////////////////////
// Calculate the control point for given start and end points and a parent branch.
function GetControlPoint(id, branchParent, sp, ep, pct)
{
	var cp = null;

	var fNewCurve = (branchParent.branchType == BT_Trunk);
	
	if (!fNewCurve)
	{
		// Child branch connects to existing Bezier curve on the tangent at sp.
		var pts1 = GetWPtsFromVertices(branchParent.vertices);
		cp = GetConnectingBezierCurve(pts1, pct, ep);
		
		// Limit the control point so it's not too far away, which results in a crazy looking curve.
		var dcp = DistanceSquared(cp, ep);
		var length = DistanceSquared(sp, ep);
		// This limit is abitrary 
		if (dcp > length * 4)
		{
			// Connecting the curve for the given (sp, ep) on branchParent does not look good.
			// Create a new curve for this child branch.
			fNewCurve = true;		
		} 
	}

	if (fNewCurve)
	{		
		// Root branch connecting to the trunk. Create a new curve.
		dir = GetBezierDirection(sp, ep);
		angle = GetAngleForBezierControl(id, sp, ep);
		cp = GenerateBezierControlPoint(sp, ep, angle, dir);
	}

	return cp;
}
