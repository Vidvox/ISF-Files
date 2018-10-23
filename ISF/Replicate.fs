/*
{
  "CATEGORIES" : [
    "Geometry Adjustment",
    "Stylize", "Tile Effect"
  ],
  "DESCRIPTION" : "",
  "ISFVSN" : "2",
  "INPUTS" : [
    {
      "NAME" : "inputImage",
      "TYPE" : "image"
    },
    {
      "NAME" : "startSize",
      "TYPE" : "float",
      "MAX" : 2,
      "DEFAULT" : 0.5,
      "MIN" : 0,
      "IDENTITY" : 1
    },
    {
      "NAME" : "startOpacity",
      "TYPE" : "float",
      "MAX" : 1,
      "DEFAULT" : 1,
      "MIN" : 0
    },
    {
      "NAME" : "startCenter",
      "TYPE" : "point2D"
    },
    {
      "NAME" : "startPadding",
      "TYPE" : "float",
      "MAX" : 1,
      "DEFAULT" : 0.25,
      "MIN" : 0
    },
    {
      "NAME" : "endSize",
      "TYPE" : "float",
      "MAX" : 2,
      "DEFAULT" : 0.25,
      "MIN" : 0
    },
    {
      "NAME" : "endOpacity",
      "TYPE" : "float",
      "MAX" : 1,
      "DEFAULT" : 1,
      "MIN" : 0
    },
    {
      "NAME" : "endCenter",
      "TYPE" : "point2D"
    },
    {
      "NAME" : "endPadding",
      "TYPE" : "float",
      "MAX" : 1,
      "DEFAULT" : 0.1,
      "MIN" : 0
    },
    {
      "NAME" : "repetitions",
      "TYPE" : "float",
      "MAX" : 15,
      "DEFAULT" : 5,
      "MIN" : 1
    }
  ],
  "CREDIT" : ""
}
*/


vec2 paddedZoomedPosition(vec2 loc, float zl, vec2 c, float p)	{
	vec2		returnMe = loc;
	float		zoomMult = (1.0/zl);
	vec2		modifiedCenter = 2.0*(1.0+p)*c/RENDERSIZE-(1.0+p);
	float		modifiedPadding = p;
	
	returnMe.x = (returnMe.x)*zoomMult + p/2.0 - modifiedCenter.x;
	returnMe.y = (returnMe.y)*zoomMult + p/2.0 - modifiedCenter.y;
	returnMe.x = mod(returnMe.x,1.0+modifiedPadding) - p/2.0;
	returnMe.y = mod(returnMe.y,1.0+modifiedPadding) - p/2.0;
	
	return returnMe;
}


void main()	{
	vec4		inputPixelColor = vec4(0.0);

	int			depth = int(repetitions);
	vec2		loc = isf_FragNormCoord;
	float		minZoomLevel = (1.0/RENDERSIZE.x);
	float		startZoomLevel = (startSize < minZoomLevel) ?  minZoomLevel : startSize;
	float		endZoomLevel = (endSize < minZoomLevel) ?  minZoomLevel : endSize;
	float		zoomIncrement = (depth < 2) ? 0.0 : (endZoomLevel - startZoomLevel)/float(depth-1);
	vec2		centerIncrement = (depth < 2) ? vec2(0.0) : (endCenter - startCenter)/float(depth-1);
	float		paddingIncrement = (depth < 2) ? 0.0 : (endPadding - startPadding)/float(depth-1);
	float		opacityIncrement = (depth < 2) ? 0.0 : (endOpacity - startOpacity)/float(depth-1);
	
	for (int i = 0;i < depth;++i)	{
		float	modZoom = startZoomLevel + zoomIncrement * float(i);
		vec2	modCenter = startCenter + centerIncrement * float(i);
		float	modPad = startPadding + paddingIncrement * float(i);
		float	modOpacity = startOpacity + opacityIncrement * float(i);
		modOpacity = clamp(modOpacity,0.0,1.0);
		loc = paddedZoomedPosition(isf_FragNormCoord,modZoom,modCenter,modPad);
		if ((loc.x < 0.0)||(loc.y < 0.0)||(loc.x > 1.0)||(loc.y > 1.0))	{
			//inputPixelColor = vec4(0.0);
		}
		else	{
			vec4	tmpColor = IMG_NORM_PIXEL(inputImage,loc);
			inputPixelColor.rgb = inputPixelColor.rgb + tmpColor.rgb * tmpColor.a * modOpacity;
			inputPixelColor.a += (tmpColor.a * modOpacity);
			if (inputPixelColor.a > 0.99)	{
				break;
			}
		}
	}
	
	gl_FragColor = inputPixelColor;
}
