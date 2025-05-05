/*{
	"DESCRIPTION": "Makes a grid of frame delayed images",
	"ISFVSN": "2",
	"CREDIT": "by VIDVOX",
	"CATEGORIES": [
		"Tile Effect", "Stylize"
	],
	"INPUTS": [
		{
			"NAME": "inputImage",
			"TYPE": "image"
		},
		{
			"NAME": "corner",
			"LABEL": "Start Corner",
			"VALUES": [
				0,
				1,
				2,
				3
			],
			"LABELS": [
				"Top Left",
				"Top Right",
				"Bottom Left",
				"Bottom Right"
			],
			"DEFAULT": 0,
			"TYPE": "long"
		},
		{
			"NAME": "cols",
			"LABEL": "Columns",
			"VALUES": [
				1,
				2,
				3,
				4,
				5,
				6,
				7,
				8,
				9,
				10
			],
			"LABELS": [
				"1",
				"2",
				"3",
				"4",
				"5",
				"6",
				"7",
				"8",
				"9",
				"10"
			],
			"DEFAULT": 4,
			"TYPE": "long"
		},
		{
			"NAME": "rows",
			"LABEL": "Rows",
			"VALUES": [
				1,
				2,
				3,
				4,
				5,
				6,
				7,
				8,
				9,
				10
			],
			"LABELS": [
				"1",
				"2",
				"3",
				"4",
				"5",
				"6",
				"7",
				"8",
				"9",
				"10"
			],
			"DEFAULT": 4,
			"TYPE": "long"
		},
		{
			"NAME": "direction",
			"LABEL": "Direction",
			"VALUES": [
				0,
				1
			],
			"LABELS": [
				"Horizontal",
				"Vertical"
			],
			"DEFAULT": 0,
			"TYPE": "long"
		},
		{
			"NAME": "edgeMode",
			"LABEL": "Edge Mode",
			"VALUES": [
				0,
				1
			],
			"LABELS": [
				"Wrap",
				"Snake"
			],
			"DEFAULT": 1,
			"TYPE": "long"
		},
		{
			"NAME": "fpsThrottle",
			"LABEL": "FPS Lag",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": 0.0,
			"MAX": 5.0
		},
		{
			"NAME": "imageSizeMode",
			"LABEL": "Size Mode",
			"VALUES": [
				0,
				1,
				2
			],
			"LABELS": [
				"Fit",
				"Fill",
				"Stretch"
			],
			"DEFAULT": 1,
			"TYPE": "long"
		},
		{
			"NAME": "clear",
			"LABEL": "Clear Buffer",
			"TYPE": "event"
		},
		{
			"NAME": "autoClear",
			"LABEL": "Auto Clear",
			"TYPE": "bool",
			"DEFAULT": 1.0
		}
	],
	"PASSES": [
		{
			"TARGET":"drawState",
			"PERSISTENT": true,
			"FLOAT": true,
			"WIDTH": "1",
			"HEIGHT": "1"
		},
		{
			"TARGET":"frameCounter",
			"PERSISTENT": true,
			"FLOAT": true,
			"WIDTH": "1",
			"HEIGHT": "1"
		},
		{
			"TARGET":"lastBuffer",
			"FLOAT": true,
			"PERSISTENT": true
		}
	]
	
}*/


//	this returns which cell a particular pixel is in, ordered based on the direction / start corner / wrap mode
float cellForCoord(vec2 st, vec2 size, int dir, int corn, int edge){
	//	st contains the pixel coordinate
	//	size contains the grid size
	//	dir contains the direction (horizontal / vertical)
	//	corn contains the start corner (top left, top right, bottom left, bottom right)
	//	edge contains the edge mode (wrap, snake)
	
	//	here's the general gist of this logic...
	//	start by assuming that it's bottom left, horizontal, with wrap around
	//	then do modifications on the result based on the settings
	//	eg if the corner is top right, pt.x = 1.0 - st.x
	//	eg if doing snake, check which col / row, and reverse within that col / row if needed
	vec2	loc = st;
	if (corn == 0)	{
		loc.y = 1.0 - loc.y;
	}
	else if (corn == 1)	{
		loc = 1.0 - loc;
	}
	else if (corn == 2)	{
		//	this is actually the normal state for st!
	}
	else if (corn == 3)	{
		loc.x = 1.0 - loc.x;
	}
	
	//	figure out which cell we are in within 2D space
	//	st contains the normalized x & y value (0.0 to 1.0)
	//		say we have 4 cols and 3 rows
	//		the gridLoc should range from 0 to 3 for x and 0 to 2 for y
	//		and the cell indexes would range from 0 to 11
	vec2	gridLoc = floor(loc * size);
	
	//	now the is the row number times the number of cells per row (number of columns), plus the number of columns on the current row
	//	starting at 0, ending at total number of cells (grid.x * grid.y - 1)
	float	cellIndex = gridLoc.y * size.x + gridLoc.x;
	
	//	of course if we are doing this vertically...
	if (dir == 1)	{
		cellIndex = gridLoc.x * size.y + gridLoc.y;
	}
	
	//	if we're doing snake...
	if (edge == 1)	{
		//	horizontal, odd row?
		if (dir == 0)	{
			float	oddRow = mod(gridLoc.y, 2.0);
			if (oddRow == 1.0)	{
				cellIndex = (gridLoc.y + 1.0) * size.x - gridLoc.x - 1.0;
			}
		}
		//	vertical, odd col?
		else {
			float	oddCol = mod(gridLoc.x, 2.0);
			if (oddCol == 1.0)	{
				cellIndex = (gridLoc.x + 1.0) * size.y - gridLoc.y - 1.0;
			}
		}
	}
	
    return cellIndex;
}

//	this returns the normalized offset coordinates of a particular cell, ordered based on the direction / start corner / wrap mode
vec2 offsetCoordForCell(int cellIndex, vec2 size, int dir, int corn, int edge){

	//	convert this 1D cellIndex into a 2D grid cell based on the rules
	//	this will be ranged 0 to size - 1
	//	we can convert these to normalized values later
	vec2	gridCell = vec2(0.0);
	
	//	start by assuming bottom left...
	
	//	if the direction is horizontal
	if (dir == 0)	{
		gridCell.x = mod(float(cellIndex), size.x);
		gridCell.y = floor(float(cellIndex) / size.x);	
	}
	//	if the direction is vertical
	else	{
		gridCell.x = floor(float(cellIndex) / size.y);
		gridCell.y = mod(float(cellIndex), size.y);	
	}
	
	//	now deal with snake mode
	//	if we're doing snake...
	if (edge == 1)	{
		//	horizontal, odd row?
		if (dir == 0)	{
			float	oddRow = mod(gridCell.y, 2.0);
			if (oddRow == 1.0)	{
				gridCell.x = size.x - gridCell.x - 1.0;
			}
		}
		//	vertical, odd col?
		else {
			float	oddCol = mod(gridCell.x, 2.0);
			if (oddCol == 1.0)	{
				gridCell.y = size.y - gridCell.y - 1.0;
			}
		}
	}
	
	//	if this is top left...
	if (corn == 0)	{
		//	keep the x, flip the y
		gridCell.y = size.y - 1.0 - gridCell.y;
	}
	else if (corn == 1)	{
		gridCell.y = size.y - 1.0 - gridCell.y;
		gridCell.x = size.x - 1.0 - gridCell.x;
	}
	else if (corn == 2)	{
		//	this is our normal state!
	}
	else if (corn == 3)	{
		gridCell.x = size.x - 1.0 - gridCell.x;
	}
	
	vec2	cellOffset = gridCell / size;

	return cellOffset;
}

//	this returns the local normalized coordinates for a pixel relative to a specified cell, ordered based on the direction / start corner / wrap mode
vec2 localCoordWithinCell(vec2 st, int cellIndex, int indexOutOffset, vec2 size, int dir, int corn, int edge){
	//	figure out the local origin for this cell
	vec2	localOriginIn = offsetCoordForCell(cellIndex, size, dir, corn, edge);
	vec2	localOriginOut = offsetCoordForCell(cellIndex + indexOutOffset, size, dir, corn, edge);
	
	//	useful for debug
	//return localOrigin;
	
	//	start at our existing point offset by the local origin
	vec2	loc = st - localOriginIn;
	
	//	now figure how much we need to shrink the x / y down to normalize for scale here
	//loc /= (size);
	
	//	now add back the local origin
	loc = loc + localOriginOut;
	
	return loc;
}

//	'a' and 'b' are rects (x and y are the originx, z and w are the width and height)
//	'm' is the sizing mode as described above (fit/fill/stretch/copy)
vec4 RectThatFitsRectInRect(vec4 a, vec4 b, int m);



void main()
{
	//	first pass: read the "buffer7" into "buffer8"
	//	apply lag on each pass
	//	if this is the first pass, i'm going to read the position from the "lastRow" image, and write a new position based on this and the hold variables
	if (PASSINDEX == 0)	{
		vec4		srcPixel = IMG_PIXEL(drawState,vec2(0.5));
		float		doClear = 0.0;
		//	check to see if the manual clear event was set
		//	check to see if the number of rows & cols has changed
			//	if auto clear is enabled, we need to do a clear
		if (clear == true)	{
			doClear = 1.0;
		}
		else if (srcPixel.r != float(cols) || srcPixel.g != float(rows))	{
			doClear = (autoClear) ? 1.0 : 0.0;
		}
		
		//	fill into the r channel the new number of cols
		//	fill into the g channel the new number of rows
		//	fill into the b channel the doClear state
		gl_FragColor = vec4(cols, rows, doClear, 1.0);
	}
	else if (PASSINDEX == 1)	{
		vec4		srcPixel = IMG_PIXEL(frameCounter,vec2(0.5));
		float		frameCount = srcPixel.r;
		
		if (frameCount >= fpsThrottle)	{
			frameCount = 0.0;
		}
		else	{
			frameCount += 1.0;
		}
		
		gl_FragColor = vec4(frameCount, 0.0, 0.0, 1.0);
	}
	else if (PASSINDEX == 2)	{
		//	get the state pixel, this will tell us if we need to clear
		//	(either because there was a manual clear command or an auto-clear because the cols / rows changed)
		vec4	statePixel = IMG_PIXEL(drawState,vec2(0.5));
		vec4	fcPixel = IMG_PIXEL(frameCounter,vec2(0.5));
		float	doClear = statePixel.b;
		float	frameCount = fcPixel.r;
		
		//	prep our variables for the pixel coordinate, the grid size, and the final output color
		vec2 	st = isf_FragNormCoord;
		vec2	grid = vec2(cols, rows);
		vec4	color = vec4(0.0);
		
		//	if clearing...
		if (doClear == 1.0)	{
			color = vec4(0.0);
		}
		//	if we're throttling the fps just return the last frame
		else if (frameCount > 0.0)	{
			color = IMG_THIS_PIXEL(lastBuffer);
		}
		//	if there's only one cell to draw...
		else if (cols <= 1 && rows <= 1)	{
			color = IMG_NORM_PIXEL(inputImage, st);
		}
		//	otherwise let's do our thing!
		else	{
			//	get the cell index that we are in
			float	cellIndex = cellForCoord(st, grid, direction, corner, edgeMode);
			
			//	if the cellIndex is 0, we are reading from the inputImage
			if (cellIndex == 0.0)	{
				//	get the local coordinate within that cell
				vec2	localOrigin = offsetCoordForCell(int(cellIndex), grid, int(direction), int(corner), int(edgeMode));
				//	create a pt that is this pixel offset by the local origin
				vec2	loc = st - localOrigin;
				//	scale this up so that within this cell loc ranges from 0 to 1 in both x & y
				loc *= (grid);
				//	scale this up so that x & y are now within pixel coordinate space, taking into account our aspect ratio
				//loc *= RENDERSIZE;
				
				//	now handle aspect ratio mismatches
				vec4	inputRect = vec4(0.0, 0.0, RENDERSIZE);
				vec4	outputRect = vec4(localOrigin * RENDERSIZE, RENDERSIZE / grid);
				vec4	sizedRect = RectThatFitsRectInRect(outputRect, inputRect, imageSizeMode);
				
				loc *= sizedRect.zw;
				loc += sizedRect.xy;
				
				if (loc.x >= 0.0 && loc.x <= RENDERSIZE.x && loc.y >= 0.0 && loc.y <= RENDERSIZE.y)	{
					color = IMG_PIXEL(inputImage, loc);
				}
				//	use this for debug
				//color = vec4(loc / RENDERSIZE,0.0,1.0);
				//color = vec4(loc.x, loc.y, cellIndex / float(cols * rows), 1.0);
			}
			//	for any other cell, if we aren't doing a clear...
			else	{
				//	get the local coordinate within that cell (cellIndex - 1)
				vec2	local = localCoordWithinCell(st, int(cellIndex), -1, grid, int(direction), int(corner), int(edgeMode));
				//	read this pixel from the last stored buffer
				color = IMG_NORM_PIXEL(lastBuffer, local);
			
				//	use this for debug
				//local = localCoordWithinCell(st, int(cellIndex), grid, int(direction), int(corner), int(edgeMode));
				//color = vec4(local,0.0,1.0);
				//color = vec4(local.x, local.y, cellIndex / float(cols * rows), 1.0);
			}
		}
		
		gl_FragColor = color;
	}
}






//	rect that fits 'a' in 'b' using sizing mode 'm'
vec4 RectThatFitsRectInRect(vec4 a, vec4 b, int m)	{
	float		bAspect = b.z/b.w;
	float		aAspect = a.z/a.w;
	if (aAspect==bAspect)	{
		return b;
	}
	vec4		returnMe = vec4(0.0);
	//	fill
	if (m==1)	{
		//	if the rect i'm trying to fit stuff *into* is wider than the rect i'm resizing
		if (bAspect > aAspect)	{
			returnMe.w = b.w;
			returnMe.z = returnMe.w * aAspect;
		}
		//	else if the rect i'm resizing is wider than the rect it's going into
		else if (bAspect < aAspect)	{
			returnMe.z = b.z;
			returnMe.w = returnMe.z / aAspect;
		}
		else	{
			returnMe.z = b.z;
			returnMe.w = b.w;
		}
		returnMe.x = (b.z-returnMe.z)/2.0+b.x;
		returnMe.y = (b.w-returnMe.w)/2.0+b.y;
	}
	//	fit
	else if (m==0)	{
		//	if the rect i'm trying to fit stuff *into* is wider than the rect i'm resizing
		if (bAspect > aAspect)	{
			returnMe.z = b.z;
			returnMe.w = returnMe.z / aAspect;
		}
		//	else if the rect i'm resizing is wider than the rect it's going into
		else if (bAspect < aAspect)	{
			returnMe.w = b.w;
			returnMe.z = returnMe.w * aAspect;
		}
		else	{
			returnMe.z = b.z;
			returnMe.w = b.w;
		}
		returnMe.x = (b.z-returnMe.z)/2.0+b.x;
		returnMe.y = (b.w-returnMe.w)/2.0+b.y;
	}
	//	stretch
	else if (m==2)	{
		returnMe = vec4(b.x, b.y, b.z, b.w);
	}
	//	copy
	else if (m==3)	{
		returnMe.z = float(int(a.z));
		returnMe.w = float(int(a.w));
		returnMe.x = float(int((b.z-returnMe.z)/2.0+b.x));
		returnMe.y = float(int((b.w-returnMe.w)/2.0+b.y));
	}
	return returnMe;
}