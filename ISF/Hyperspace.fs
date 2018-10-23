/*
{
  "CATEGORIES" : [
    "Stylize"
  ],
  "DESCRIPTION" : "",
  "ISFVSN" : "2",
  "INPUTS" : [
    {
      "NAME" : "inputImage",
      "TYPE" : "image"
    },
    {
      "NAME" : "centerX",
      "TYPE" : "float",
      "MAX" : 1,
      "DEFAULT" : 0.5,
      "MIN" : 0
    },
    {
      "NAME" : "scrollAmount",
      "TYPE" : "float",
      "MAX" : 2,
      "DEFAULT" : 0,
      "MIN" : 0
    },
    {
      "NAME" : "rightScrollOffset",
      "TYPE" : "float",
      "MAX" : 1,
      "DEFAULT" : 0,
      "MIN" : 0
    },
    {
      "NAME" : "midHeight",
      "TYPE" : "float",
      "MAX" : 1,
      "DEFAULT" : 0.5,
      "MIN" : 0,
      "IDENTITY" : 1
    },
    {
      "NAME" : "seamless",
      "TYPE" : "bool",
      "DEFAULT" : 1
    }
  ],
  "CREDIT" : "VIDVOX"
}
*/

void main()	{
	vec2		loc = isf_FragNormCoord.xy;
	if (centerX == 0.0)	{
		
	}
	else if (centerX == 1.0)	{
		
	}
	else if (loc.x < centerX)	{
		loc.x = loc.x / centerX;
		loc.y = mix(0.5 + (loc.y-0.5) / midHeight,loc.y,1.0-loc.x);
		loc.x = loc.x+scrollAmount;
		if (seamless)	{
			loc.x = ((loc.x <= 1.0)||(loc.x >= 2.0)) ? loc.x : 3.0 - loc.x;
		}
		loc.x = mod(loc.x,1.0);
	}
	else	{
		float		rightScroll = mod(rightScrollOffset+scrollAmount,2.0);
		loc.x = (1.0-loc.x) / (1.0-centerX);
		loc.y = mix(0.5 + (loc.y-0.5) / midHeight,loc.y,1.0-loc.x);
		loc.x = loc.x+rightScroll;
		if (seamless)	{
			loc.x = ((loc.x <= 1.0)||(loc.x >= 2.0)) ? loc.x : 3.0 - loc.x;
		}
		loc.x = mod(loc.x,1.0);
	}
	
	vec4		inputPixelColor = vec4(0.0);
	
	if ((loc.y >= 0.0)&&(loc.y <= 1.0))
		inputPixelColor = IMG_NORM_PIXEL(inputImage,loc);
	
	gl_FragColor = inputPixelColor;
}
