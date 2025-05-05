/*
{
  "CATEGORIES" : [
    "Glitch", "Distortion Effect", "v002"
  ],
  "DESCRIPTION" : "Emulates classic analog video interference, distortion and sync issues.",
  "ISFVSN" : "2",
  "INPUTS" : [
    {
      "NAME" : "inputImage",
      "TYPE" : "image"
    },
    {
      "NAME" : "inputDistortionImage",
      "TYPE" : "image"
    },
    {
      "NAME" : "inputDistortion",
      "TYPE" : "float",
      "MAX" : 5,
      "DEFAULT" : 0.0,
      "LABEL" : "Distortion",
      "MIN" : 0
    },
    {
      "NAME" : "inputBarsAmount",
      "TYPE" : "float",
      "MAX" : 1,
      "DEFAULT" : 0.25,
      "LABEL" : "Distortion Mix",
      "MIN" : 0
    },
    {
      "NAME" : "inputVSYNC",
      "TYPE" : "float",
      "MAX" : 2,
      "DEFAULT" : 0.0,
      "LABEL" : "V Sync",
      "MIN" : 0
    },
    {
      "NAME" : "inputHSYNC",
      "TYPE" : "float",
      "MAX" : 2,
      "DEFAULT" : 0.0,
      "LABEL" : "H Sync",
      "MIN" : 0
    },
    {
      "NAME" : "inputResolution",
      "TYPE" : "float",
      "MAX" : 10,
      "DEFAULT" : 5,
      "LABEL" : "Scan Line Size",
      "MIN" : 1
    },
    {
      "NAME" : "inputResolutionMix",
      "TYPE" : "float",
      "MAX" : 1,
      "DEFAULT" : 0.5,
      "LABEL" : "Scan Lin Mix",
      "MIN" : 0
    }
  ],
  "CREDIT" : "Vade / VIDVOX"
}
*/



//	Original v002 Analog Glitch by Vade
//	https://github.com/v002/v002-Glitch/blob/master/v002AnalogGlitchPlugIn.m

//	Adapted by VIDVOX 2025


void main (void) 	{ 		
	vec2 point = isf_FragNormCoord.xy;
	
    // sample center of the 1st pixel, down the height
	vec4 bars = IMG_NORM_PIXEL(inputDistortionImage, vec2(0.5, point.y));

	// scanlines
	float stripe = mod(floor(gl_FragCoord.y), floor(inputResolution)) / floor(inputResolution);
	float scanlineMask = mix(1.5, 0.75, stripe);
	scanlineMask = mix(1.0, scanlineMask, inputResolutionMix);
	
	// get rough luma 
	vec4 key = IMG_PIXEL(inputImage, (vec2(point.y, point.y)) * RENDERSIZE);
	key += IMG_PIXEL(inputImage, (1.0 - vec2(point.y, point.y)) * RENDERSIZE);
	key -= bars.r;
	float d = (key.r + key.g + key.b) / 3.0 - 0.5;
	point.x -= d * inputDistortion * 0.1;

	//sync			
	vec2 texcoord = point + ( mod(vec2(inputHSYNC, inputVSYNC), 1.0)); 
	
	// wrap
	texcoord = mod(texcoord, 1.0);
	
	// outout
	vec4 result = IMG_PIXEL(inputImage, texcoord * RENDERSIZE);
	result.rgb = result.rgb * scanlineMask;
    
    gl_FragColor = mix(result, bars*result, inputBarsAmount);
}

