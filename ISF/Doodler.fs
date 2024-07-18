/*
{
  "CATEGORIES" : [
    "Drawing"
  ],
  "DESCRIPTION" : "Uses a virtual pen to draw colors with a random walk.",
  "ISFVSN" : "2",
  "INPUTS" : [
    {
      "NAME" : "penDown",
      "TYPE" : "bool",
      "DEFAULT" : 1,
      "LABEL" : "Pen Down"
    },
    {
      "NAME" : "eraseMode",
      "TYPE" : "bool",
      "DEFAULT" : 0,
      "LABEL" : "Eraser Mode"
    },
    {
      "NAME" : "eraseAndReset",
      "TYPE" : "event",
      "LABEL" : "Erase All"
    },
    {
      "NAME" : "penRate",
      "LABEL" : "Auto Rate",
      "TYPE" : "float",
      "MAX" : 1,
      "DEFAULT" : 1,
      "MIN" : 0
    },
    {
      "NAME" : "drawColor",
      "LABEL" : "Pen Color",
      "TYPE" : "color",
      "DEFAULT" : [
        0,
        0,
        1,
        1
      ]
    },
    {
      "NAME" : "penSize",
      "LABEL" : "Tip Size",
      "TYPE" : "float",
      "MAX" : 0.2,
      "DEFAULT" : 0.01,
      "MIN" : 0
    },
    {
      "NAME" : "penLoc",
      "LABEL" : "Pen Start",
      "TYPE" : "point2D",
      "MAX" : [
        1,
        1
      ],
      "DEFAULT" : [
        0.5,
        0.5
      ],
      "MIN" : [
        0,
        0
      ]
    },
    {
      "NAME" : "dirtyTip",
      "LABEL" : "Texture",
      "TYPE" : "float",
      "MAX" : 1,
      "DEFAULT" : 0,
      "MIN" : 0
    },
    {
      "NAME" : "bgColor",
      "LABEL" : "Eraser Color",
      "TYPE" : "color",
      "DEFAULT" : [
        0,
        0,
        0,
        0
      ]
    }
  ],
  "PASSES" : [
    {
      "WIDTH" : "1",
      "HEIGHT" : "1",
      "TARGET" : "bufferPosition",
      "PERSISTENT" : true,
	  "FLOAT": true
    },
    {
      "TARGET" : "lastBuffer",
      "PERSISTENT" : true,
      "FLOAT": true
    }
  ],
  "CREDIT" : "VIDVOX"
}
*/



float seed = 1.239;



float rand(vec2 co){
    return fract(sin(dot(co.xy ,vec2(12.9898,78.233))) * 43758.5453);
}


void main()	{
	vec4		returnMe = vec4(0.0);
	bool		doReset = false;
	if ((eraseAndReset)||(FRAMEINDEX<=1))
		doReset = true;
	
	if (PASSINDEX==0)	{
		//	if needed, reset this to the default pen position
		if (doReset)	{
			returnMe = vec4(penLoc.x,penLoc.y,0.0,1.0);
		}
		//	get the last position, move randomly by up to 1 pixel
		else	{
			returnMe = IMG_THIS_PIXEL(bufferPosition);
			float	newDirection = rand(vec2(TIME,seed));
			vec2	shift = vec2(0.0,0.0);
			
			if (newDirection < 0.25)
				shift = vec2(1.0,0.0);
			else if (newDirection < 0.5)
				shift = vec2(-1.0,0.0);
			else if (newDirection < 0.75)
				shift = vec2(0.0,1.0);
			else
				shift = vec2(0.0,-1.0);
			shift = shift * penSize * penRate;
			returnMe.rg += shift;
			
			if (returnMe.r > 1.0)
				returnMe.r = 1.0;
			else if (returnMe.r < 0.0)
				returnMe.r = 0.0;
			if (returnMe.g > 1.0)
				returnMe.g = 1.0;
			else if (returnMe.g < 0.0)
				returnMe.g = 0.0;
		}
	}
	else	{
		if (doReset)	{
			//	reset to our bgColor
			//returnMe = vec4(0.0,0.0,0.0,0.0);
			returnMe = bgColor;
		}
		else	{
			vec4	lastPos = IMG_NORM_PIXEL(bufferPosition,vec2(0.5));
			if ((penDown)&&(distance(isf_FragNormCoord,lastPos.rg) < penSize))	{
				//	if we are erasing, replace with the bgColor
				returnMe = (eraseMode) ? bgColor : drawColor;
				if (dirtyTip > 0.0)	{
					float	dirtRand = rand(isf_FragNormCoord*vec2(TIME,1.0+TIME));
					if (dirtRand <= dirtyTip)	{
						returnMe = IMG_NORM_PIXEL(lastBuffer,isf_FragNormCoord);
					}
				}
			}
			else	{
				returnMe = IMG_NORM_PIXEL(lastBuffer,isf_FragNormCoord);
			}

		}
	}
	
	gl_FragColor = returnMe;
}
