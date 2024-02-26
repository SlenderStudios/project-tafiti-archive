// Copyright 2007 Microsoft Corp. All Rights Reserved.

// Transition a clip's color property from one setting to another
// Set up the think
function initAnim_color(o, clip, attrName, T, delay, sColor, eColor, fReverse, fn)
{
	var p = this.AddAnimation(o);
	var dt = new Date();

	p.args._clip = clip;
	p.args._T = T;
	p.args._startTime = dt.getTime() + delay;
	p.args._endTime = p.args._T + p.args._startTime;
	p.args._reverseDampening = fReverse;
	p.args._cStart = sColor;
	p.args._cEnd = eColor;
	p.args._fn = fn;
	p.args._attr = attrName;

	p.fn = doAnim_color;
}

// Perform interframe actions
function doAnim_color(o, args, t)
{
	// Determine how far along we are
	var d = (args._endTime - t) / args._T;

	// We're done, clean up
	if(d <= 0)
	{
		args._clip[args._attr] = GenerateColor(args._cEnd, args._cEnd, 0);
		return(false);
	}

	if(args._reverseDampening)
	{
//		d = Math.sqrt(Math.pow((1 - d), 4));
	}
	else
	{
//		d = Math.sqrt(1 - Math.pow(d, 4));
	}

	args._clip[args._attr] = GenerateColor(args._cStart, args._cEnd, d);

	return(true);
}