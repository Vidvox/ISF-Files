/*
{
  "CATEGORIES" : [
    "Glitch"
  ],
  "DESCRIPTION" : "Applies randomized effects to different parts of the image.",
  "ISFVSN" : "2",
  "INPUTS" : [
    {
      "NAME" : "inputImage",
      "TYPE" : "image"
    },
    {
      "NAME" : "maxGlitchSize",
      "TYPE" : "float",
      "MAX" : 1,
      "DEFAULT" : 0.1,
      "LABEL" : "Glitch Level",
      "MIN" : 0
    },
    {
      "NAME" : "glitchRate",
      "TYPE" : "float",
      "MAX" : 1,
      "DEFAULT" : 0.25,
      "LABEL" : "Glitch Rate",
      "MIN" : 0
    },
	{
		"NAME": "patternMode",
		"LABEL": "Pattern Mode",
		"VALUES": [
			0,
			1,
			2,
			3,
			4,
			5,
			6,
			7
		],
		"LABELS": [
			"Random",
			"Black",
			"Checkerboard",
			"Horizontal Stripes",
			"Vertical Stripes",
			"Color Bars",
			"Noise (B&W)",
			"Noise (Color)"
		],
		"DEFAULT": 0,
		"TYPE": "long"
	}
  ],
  "PASSES" : [
    {
      "TARGET" : "lastState",
      "PERSISTENT" : true
    }
  ],
  "CREDIT" : "VIDVOX"
}
*/



const float pi = 3.14159265359;



float rand(vec2 co){
    return fract(sin(dot(co.xy ,vec2(12.9898,78.233))) * 43758.5453);
}

vec4 rand4(vec4 co)	{
	vec4	returnMe = vec4(0.0);
	returnMe.r = rand(co.rg);
	returnMe.g = rand(co.gb);
	returnMe.b = rand(co.ba);
	returnMe.a = rand(co.rb);
	return returnMe;
}



bool pointInRect(vec2 pt, vec4 r)
{
	bool	returnMe = false;
	if ((pt.x >= r.x)&&(pt.y >= r.y)&&(pt.x <= r.x + r.z)&&(pt.y <= r.y + r.w))
		returnMe = true;
	return returnMe;
}


//	within a glitch, we'll
//	replace or mix with a specified or random 'pattern'


vec4 noiseColor(vec2 seeds)	{
	vec2	loc = vec2(27.43, 91.59339) + gl_FragCoord.xy;
	vec4	tmp = vec4(loc.x, loc.y, seeds.x, seeds.y);
	return rand4(tmp);
}

vec4 noiseBW()	{
	vec2	loc = gl_FragCoord.xy;
	float	val = rand(loc);
	return vec4(val, val, val, 1.0);
}

vec4 colorsArray[9];

vec4 colorBars()	{
	vec4		outputPixelColor = vec4(0.0);
	vec2		loc = isf_FragNormCoord.xy;
	
	//	figure out if we are in the top, middle or bottom sections
	//	these are broken into the ratios 3/4, 1/8, 1/8
	
	//	if we are in the top section figure out which of the 9 colors to use
	if (loc.y > 0.25)	{
		colorsArray[0] = vec4(0.412, 0.412, 0.412, 1.0);
		colorsArray[1] = vec4(0.757, 0.757, 0.757, 1.0);
		colorsArray[2] = vec4(0.757, 0.757, 0.000, 1.0);
		colorsArray[3] = vec4(0.000, 0.757, 0.757, 1.0);
		colorsArray[4] = vec4(0.000, 0.757, 0.000, 1.0);
		colorsArray[5] = vec4(0.757, 0.000, 0.757, 1.0);
		colorsArray[6] = vec4(0.757, 0.000, 0.000, 1.0);
		colorsArray[7] = vec4(0.000, 0.000, 0.757, 1.0);
		colorsArray[8] = vec4(0.412, 0.412, 0.412, 1.0);
		
		int		colorIndex = int((9.0 * mod(loc.x, 1.0)));
		
		outputPixelColor = colorsArray[colorIndex];
	}
	//	in the 'middle section we draw the black to white image
	else if (loc.y > 0.125)	{
		outputPixelColor.rgb = vec3(loc.x);
		outputPixelColor.a = 1.0;
	}
	else	{
		colorsArray[0] = vec4(0.169, 0.169, 0.169, 1.0);
		colorsArray[1] = vec4(0.019, 0.019, 0.019, 1.0);
		colorsArray[2] = vec4(1.000, 1.000, 1.000, 1.0);
		colorsArray[3] = vec4(1.000, 1.000, 1.000, 1.0);
		colorsArray[4] = vec4(0.019, 0.019, 0.019, 1.0);
		colorsArray[5] = vec4(0.000, 0.000, 0.000, 1.0);
		colorsArray[6] = vec4(0.019, 0.019, 0.019, 1.0);
		colorsArray[7] = vec4(0.038, 0.038, 0.038, 1.0);
		colorsArray[8] = vec4(0.169, 0.169, 0.169, 1.0);
		
		int		colorIndex = int((9.0 * loc.x));
		
		outputPixelColor = colorsArray[colorIndex];		
	}
	
	return outputPixelColor;
}

vec4 horizontalStripes() {
	//	default to a black tile
	vec4	returnMe = vec4(0.0 ,0.0 ,0.0 ,1.0);
	
	//	get the pixel location
	vec2	loc = gl_FragCoord.xy;
	
	float	gridSize = 5.0;
	
	if (mod(loc.y, gridSize * 2.0) < gridSize)	{
		returnMe = vec4(1.0);
	}
	
	return returnMe;
}

vec4 verticalStripes() {
	//	default to a black tile
	vec4	returnMe = vec4(0.0 ,0.0 ,0.0 ,1.0);
	
	//	get the pixel location
	vec2	loc = gl_FragCoord.xy;
	
	float	gridSize = 5.0;
	
	if (mod(loc.x, gridSize * 2.0) < gridSize)	{
		returnMe = vec4(1.0);
	}
	
	return returnMe;
}

//	does our checkboard pattern
vec4 checkerboard()	{
	//	default to a black tile
	vec4	returnMe = vec4(0.0 ,0.0 ,0.0 ,1.0);
	
	//	get the pixel location
	vec2	loc = gl_FragCoord.xy;
	
	float	gridSize = 5.0;
	
	if ((mod(loc.x, gridSize * 2.0) > gridSize) && (mod(loc.y, gridSize * 2.0) < gridSize))	{
		returnMe = vec4(1.0);
	}
	else if ((mod(loc.x, gridSize * 2.0) < gridSize) && (mod(loc.y, gridSize * 2.0) > gridSize))	{
		returnMe = vec4(1.0);
	}
	
	return returnMe;
}

//	returns the pattern based on the index
vec4 patternSelect(int pIndex, vec2 seed)	{
	
	//	black
	if (pIndex == 0)	{
		return vec4(0.0, 0.0, 0.0, 1.0);
	}
	//	checkerboard
	else if (pIndex == 1)	{
		return checkerboard();
	}
	//	h stripes
	else if (pIndex == 2)	{
		return horizontalStripes();
	}
	//	v stripes
	else if (pIndex == 3)	{
		return verticalStripes();
	}
	//	color bars
	else if (pIndex == 4)	{
		return colorBars();
	}
	//	noise (b&w)
	else if (pIndex == 5)	{
		return noiseBW();
	}
	else if (pIndex == 6)	{
		return noiseColor(seed);
	}
	
	return vec4(0.0);
}

vec4 replaceWithPattern(vec2 seed)	{
	//	seed contains values to use in randomization
	int	pIndex = patternMode;
	if (pIndex == 0)	{
		//	don't include black out in the random options
		float	val = 1.0 + 6.0 * rand(seed);
		val = min(val, 6.0);
		val = max(val, 1.0);
		pIndex = int(val);
	}
	else	{
		pIndex = pIndex - 1;
	}
	
	return patternSelect(pIndex, seed);
}


void main()	{
	vec2	loc = isf_FragNormCoord.xy;
	vec4	inputColor = IMG_THIS_PIXEL(inputImage);
	vec4	returnMe = inputColor;
	int		glitchCount = 3;
	
	//	only do this if our rate and size are both bigger than 0.0
	if (glitchRate > 0.0 && maxGlitchSize > 0.0)	{
		for (int i = 0; i < 10; ++i)	{
			if (i > glitchCount)
				break;
			float	adjustedTime = floor(120.0 * TIME * glitchRate);
			vec4	seeds1 = (float(i)+adjustedTime) * vec4(0.2123,0.34517,0.53428,0.7431);
			vec4	randCoords = rand4(seeds1);
			randCoords.zw *= maxGlitchSize;
			if (randCoords.x + randCoords.z > 1.0)
				randCoords.z = 1.0 - randCoords.x;
			if (randCoords.y + randCoords.w > 1.0)
				randCoords.w = 1.0 - randCoords.y;
	
			bool	isInShape = pointInRect(loc,randCoords);
			if (isInShape)	{
				vec2	seeds2= vec2(1.3491+float(i),3.49123*TIME);
				vec4	newColor = replaceWithPattern(seeds2);
				
				//	keep the original alpha
				newColor.a = inputColor.a;
				
				returnMe = newColor;
			}
		}
	}
	gl_FragColor = returnMe;
}
