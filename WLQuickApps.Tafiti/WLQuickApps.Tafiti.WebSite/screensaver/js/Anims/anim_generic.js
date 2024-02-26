// Copyright 2007 Microsoft Corp. All Rights Reserved.

// Scale a clip's property from one setting to another
// Set up the think
function initAnim_generic(o, clip, attrName, T, delay, sScale, eScale, fReverse, fn)
{
	var p = this.AddAnimation(o);
	var dt = new Date();

	p.args._clip = clip;
	p.args._T = T;
	p.args._startTime = dt.getTime() + delay;
	p.args._endTime = p.args._T + p.args._startTime;
	p.args._reverseDampening = fReverse;
	p.args._sStart = sScale;
	p.args._sEnd = eScale;
	p.args._fn = fn;
	p.args._attr = attrName;

	p.fn = doAnim_generic;
}

// Perform interframe actions
function doAnim_generic(o, args, t)
{
	// Determine how far along we are
	var d = (args._endTime - t) / args._T;

	// We're done, clean up
	if(d <= 0)
	{
		args._clip[args._attr] = args._sEnd;
		return(false);
	}

	if(args._reverseDampening)
	{
		d = Math.sqrt(Math.pow((1 - d), 4));
	}
	else
	{
		d = Math.sqrt(1 - Math.pow(d, 4));
	}

	args._clip[args._attr] = args._sStart * (1 - d) + args._sEnd * (d);

	return(true);
}