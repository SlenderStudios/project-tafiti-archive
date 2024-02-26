// Copyright 2007 Microsoft Corp. All Rights Reserved.

// Animation routines available for clip actions
function Animations(rootElement)
{
	var o = this;

	// Properties
	o.clips = new Array();	// List of registered clips to think

	// XAML references (timer Storyboard for think loop)
	o.timer = rootElement.findName("timer");
//	o.timer.addEventListener("completed", TimerCompleted);	// FUTURE: What is context on this callback??

	// Methods
	o.Link = Animations_Link;
	o.Unlink = Animations_Unlink;
	o.Finalize = Animations_Finalize;
	o.AddAnimation = Animations_AddAnimation;
	o.Think = Animations_Think;

	// Anims
	o.anim_generic = initAnim_generic;
	o.anim_null = initAnim_Null;
	o.anim_color = initAnim_color;
	
	o._tid = null;
}


function TimerCompleted(sender, args)
{
    // TODO: temporary hack to stop animations
	if (!stage)
	{
		return;
	}
        
	var dt = new Date();
	var t = dt.getTime();
	var p, o = stage.Animations;
	var fC = false;					// fContinue
	var cd = [ ];					// clipDelistment
	
	// On callback, walk through each of the registered clips and
	// perform work, if they are active.
	for(var i=0; i<o.clips.length; i++)
	{
		p = o.clips[i]; 
		if(p.doThink != null)
		{
			p.doThink(t);

			// Ensure we have more work to do on this clip
			fC = (fC || (p.doThink != null));
		}
		else
		{
			// Stage the clip for delistment
			p._anims = null;
			cd.push(i);
		}
	}

	//stage.WorldUpdate();

	// Remove "dead" clips
	while(cd.length > 0)
	{
		o.clips.splice(cd.pop(), 1);
	}

	//stage._debug0.Text = ("clips: " + o.clips.length);

	// Fire up for the next think
	if(fC)
	{
		//o.timer.begin();
	}
	else
	{
		window.clearInterval(o._tid);
		o._tid = null;
	}
	
	return;
}
	
// Set up a clip to handle animations
function Animations_Link(clip)
{
	var o = this;
	var fFound = false;

	// See if this clip is already registered
	for(var i=0; i<o.clips.length; i++)
	{
		if(clip == o.clips[i])
		{
			fFound = true;
			break;
		}
	}

	// If not, add to the list to perform work
	if(!fFound)
	{
		o.clips.push(clip);
	}

	clip._anims = [ { _active:false, args:{} } ];
}

// Kill any animations on the given clip
function Animations_Unlink(o)
{
	if(o._anims == null)
	{
		this.Link(o);
		return;
	}

	o.doThink = null;

	for(var i=0; i<o._anims.length; i++)
	{
		o._anims[i]._active = false;
	}
}

// Kill any animations on the given clip, but allow one final "think"
function Animations_Finalize(o)
{
	if(o._anims == null)
	{
		this.Link(o);
		return;
	}

	o.doThink = null;

	var p;
	for(var i=0; i<o._anims.length; i++)
	{
		p = o._anims[i];
		if(p._active)
		{
			p._active = false;
			p.fn(o, p.args, (p.args._endTime + 1));
			if(p.args._fn != null)
			{
				o._fn = p.args._fn;
				o._fn();
			}
		}
	}
}

// Find an empty slot for a new animation
function Animations_AddAnimation(o)
{
	if(o._anims == null)
	{
		this.Link(o);
	}

	// Since we're adding an animation, start up the animation "think"
	o.doThink = this.Think;

	// Kick off the timer (always)
	//this.timer.begin();
	if(this._tid == null)
	{
		this._tid = window.setInterval(TimerCompleted, 1000 / 12);
	}

	for(var i=0; i<o._anims.length; i++)
	{
		if(!o._anims[i]._active)
		{
			o._anims[i]._active = true;
			return(o._anims[i]);
		}
	}

	// Nothing is available, so add one to the list
	var p = { _active:true, args:{} };
	o._anims.push(p);

	return(p);
}

// Normal timer callback on an object to do work, iterating through
// a list of registered methods for the actual work.
function Animations_Think(t)
{
	var p, o = this;

	// Default is to kill current think loop
	o.doThink = null;

	// Run through all of the animations registered for the given glip
	for(var i=0; i<o._anims.length; i++)
	{
		p = o._anims[i];
		// If this slot is active, run the animation
		if(p._active)
		{
			// Make slot available for reuse immediately
			p._active = false;

			if(p.args._startTime >= t || p.fn(o, p.args, t))
			{
				// If still active, flag we'll need another frame
				p._active = true;
				o.doThink = stage.Animations.Think;
			}
			else
			{
				// Animation is complete, execute any code on the clip
				// to allow for next step, if required
				o._fn = p.args._fn;
				if(o._fn != null)
				{
					o._fn(p.args._args);
				}
			}
		}
	}
}

// Basic delayed execution... not useful without a valid fn ptr
function initAnim_Null(o, T, fn, args)
{
	var p = this.AddAnimation(o);
	var dt = new Date();

	p.args._startTime = dt.getTime() + T;
	p.args._fn = fn;
	p.args._args = args;

	p.fn = doAnim_null;
}

function doAnim_null(o, args, t)
{
	return(false);
}
