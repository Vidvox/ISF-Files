varying vec2 translated_coord;

const float pi = 3.14159265359;

void main()	{
	isf_vertShaderInit();
	
	//	'loc' is the location in pixels of this vertex.  we're going to convert this to polar coordinates (radius/angle)
	//vec2		loc = IMG_SIZE(inputImage) * vec2(isf_FragNormCoord[0],isf_FragNormCoord[1]);
	vec2		loc = _inputImage_imgRect.zw * vec2(isf_FragNormCoord[0],isf_FragNormCoord[1]);
	//	'r' is the radius- the distance in pixels from 'loc' to the center of the rendering space
	//float		r = distance(IMG_SIZE(inputImage)/2.0, loc);
	float		r = distance(_inputImage_imgRect.zw/2.0, loc);
	//	'a' is the angle of the line segment from the center to loc is rotated
	//float		a = atan ((loc.y-IMG_SIZE(inputImage).y/2.0),(loc.x-IMG_SIZE(inputImage).x/2.0));
	float		a = atan ((loc.y-_inputImage_imgRect.w/2.0),(loc.x-_inputImage_imgRect.z/2.0));
	
	//	now modify 'a', and convert the modified polar coords (radius/angle) back to cartesian coords (x/y pixels)
	loc.x = r * cos(a + 2.0 * pi * angle);
	loc.y = r * sin(a + 2.0 * pi * angle);
	
	translated_coord = loc / _inputImage_imgRect.zw + vec2(0.5);
}