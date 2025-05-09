/*{
    "CATEGORIES": [
        "Geometry"
    ],
    "CREDIT": "VIDVOX",
    "DESCRIPTION": "Draws a series of lines at different angles along a line.",
    "INPUTS": [
        {
            "DEFAULT": 0.125,
            "MAX": 0.25,
            "MIN": 0,
            "NAME": "startLineLength",
            "TYPE": "float"
        },
        {
            "DEFAULT": 0.125,
            "MAX": 0.25,
            "MIN": 0,
            "NAME": "endLineLength",
            "TYPE": "float"
        },
        {
            "DEFAULT": 250,
            "MAX": 720,
            "MIN": 0,
            "NAME": "startAngle",
            "TYPE": "float"
        },
        {
            "DEFAULT": 315,
            "MAX": 720,
            "MIN": 0,
            "NAME": "endAngle",
            "TYPE": "float"
        },
        {
            "DEFAULT": 1,
            "MAX": 2,
            "MIN": 0,
            "NAME": "lineThickness",
            "TYPE": "float"
        },
        {
            "DEFAULT": 81,
            "MAX": 360,
            "MIN": 0,
            "NAME": "lineCount",
            "TYPE": "float"
        },
        {
            "DEFAULT": 0.5,
            "MAX": 1,
            "MIN": 0,
            "NAME": "dithering",
            "TYPE": "float"
        },
        {
            "DEFAULT": [
                0,
                1,
                1,
                1
            ],
            "NAME": "startLineColor",
            "TYPE": "color"
        },
        {
            "DEFAULT": [
                1,
                1,
                0,
                1
            ],
            "NAME": "endLineColor",
            "TYPE": "color"
        },
        {
            "DEFAULT": [
                0.1,
                0.5
            ],
            "MAX": [
                1,
                1
            ],
            "MIN": [
                0,
                0
            ],
            "NAME": "startPt",
            "TYPE": "point2D"
        },
        {
            "DEFAULT": [
                0.9,
                0.5
            ],
            "MAX": [
                1,
                1
            ],
            "MIN": [
                0,
                0
            ],
            "NAME": "endPt",
            "TYPE": "point2D"
        }
    ],
    "ISFVSN": "2"
}
*/



const float pi = 3.14159265359;


float drawLine (vec2 p1, vec2 p2, vec2 uv, float a)	{
    float r = 0.;
    float one_px = 1. / min(RENDERSIZE.x, RENDERSIZE.y); //not really one px
    
    // get dist between points
    float d = distance(p1, p2);
    
    // get dist between current pixel and p1
    float duv = distance(p1, uv);

    //if point is on line, according to dist, it should match current uv 
    r = 1.-floor(1.-(a*one_px)+distance (mix(p1, p2, clamp(duv/d, 0., 1.)),  uv));
        
    return r;
}

float drawCircle(vec2 p, float d, vec2 uv)	{
    return (distance(p, uv) <= d) ? 1. : 0.;
}

float curveFunction(float x)	{
	//	test val, 0.5
	float	leftX = min(startPt.x, endPt.x);
	float	rightX = max(startPt.x, endPt.x);
	
	float	leftY = (startPt.x <= endPt.x) ? startPt.y : endPt.y;
	float	rightY = (startPt.x > endPt.x) ? startPt.y : endPt.y;
	
	float	progress = x / RENDERSIZE.x;
	progress = (startPt.x == endPt.x) ? 0.5 : (progress - leftX) / (rightX - leftX);
	
	return mix(leftY, rightY, progress) * RENDERSIZE.y;
}

//	if the startPt.x and endPt.x are nearly the same, this function gets used instead
float curveFunctionForY(float yIn)	{
	//	test val, 0.5
	float	y = 0.5 * RENDERSIZE.y;
	float	botY = min(startPt.y, endPt.y);
	float	topY = max(startPt.y, endPt.y);
	
	float	progress = yIn / RENDERSIZE.y;
	progress = (startPt.y == endPt.y) ? startPt.y : (progress - botY) / (topY - botY);
	
	return mix(botY, topY, progress) * RENDERSIZE.y;
}


float nearestNodeIndex(float x)	{

	//	the offset, in pixels
	float	leftX = min(startPt.x, endPt.x) * RENDERSIZE.x;
	float	rightX = max(startPt.x, endPt.x) * RENDERSIZE.x;
	
	if (x < leftX)
		return 0;
	else if (x > rightX)
		return lineCount - 1;
	
	//	if there are two nodes, the distance between them is the total width
	//	if there are three nodes, the distance between them is half the total width
	//	etc, etc
	float	totalNormLength = abs(startPt.x - endPt.x);
	
	float	distanceBetweenNodes = totalNormLength * RENDERSIZE.x / (lineCount - 1);
	
	float	nearestNode = (x - leftX) / distanceBetweenNodes;
	
	nearestNode = (fract(nearestNode) < 0.5) ? floor(nearestNode) : ceil(nearestNode);
	
	return nearestNode;
}



float nearestXForNodeIndex(int i)	{

	
	//	the offset, in pixels
	//	a node offset of 0.0 puts the first node at x = 0
	//float	nodeOffset = 0.0;
	
	//	if there are two nodes, the distance between them is the total width
	//	if there are three nodes, the distance between them is half the total width
	//	etc, etc
	float	totalNormLength = abs(startPt.x - endPt.x);
	
	float	distanceBetweenNodes = totalNormLength * RENDERSIZE.x / (lineCount - 1);
	
	float	nearestNode = float(i);
	
	nearestNode = (fract(nearestNode) < 0.5) ? floor(nearestNode) : ceil(nearestNode);
	nearestNode *= distanceBetweenNodes;
	nearestNode += min(startPt.x, endPt.x) * RENDERSIZE.x;
	
	return nearestNode;
}

float nearestYForNodeIndex(int i)	{

	
	//	the offset, in pixels
	//	a node offset of 0.0 puts the first node at x = 0
	//float	nodeOffset = 0.0;
	
	//	if there are two nodes, the distance between them is the total width
	//	if there are three nodes, the distance between them is half the total width
	//	etc, etc
	float	totalNormLength = abs(startPt.y - endPt.y);
	
	float	distanceBetweenNodes = totalNormLength * RENDERSIZE.y / (lineCount - 1);
	
	float	nearestNode = float(i);
	
	nearestNode = (fract(nearestNode) < 0.5) ? floor(nearestNode) : ceil(nearestNode);
	nearestNode *= distanceBetweenNodes;
	nearestNode += min(startPt.y, endPt.y) * RENDERSIZE.y;
	
	return nearestNode;
}


//	based on https://github.com/hughsk/glsl-dither

float luma(vec3 color)	{
	return (color.r + color.g + color.b) / 3.0;	
}

float luma(vec4 color)	{
	return color.a * (color.r + color.g + color.b) / 3.0;	
}

float dither8x8(vec2 position, float brightness) {
  int x = int(mod(position.x, 8.0));
  int y = int(mod(position.y, 8.0));
  int index = x + y * 8;
  float limit = 0.0;

  if (x < 8) {
    if (index == 0) limit = 0.015625;
    if (index == 1) limit = 0.515625;
    if (index == 2) limit = 0.140625;
    if (index == 3) limit = 0.640625;
    if (index == 4) limit = 0.046875;
    if (index == 5) limit = 0.546875;
    if (index == 6) limit = 0.171875;
    if (index == 7) limit = 0.671875;
    if (index == 8) limit = 0.765625;
    if (index == 9) limit = 0.265625;
    if (index == 10) limit = 0.890625;
    if (index == 11) limit = 0.390625;
    if (index == 12) limit = 0.796875;
    if (index == 13) limit = 0.296875;
    if (index == 14) limit = 0.921875;
    if (index == 15) limit = 0.421875;
    if (index == 16) limit = 0.203125;
    if (index == 17) limit = 0.703125;
    if (index == 18) limit = 0.078125;
    if (index == 19) limit = 0.578125;
    if (index == 20) limit = 0.234375;
    if (index == 21) limit = 0.734375;
    if (index == 22) limit = 0.109375;
    if (index == 23) limit = 0.609375;
    if (index == 24) limit = 0.953125;
    if (index == 25) limit = 0.453125;
    if (index == 26) limit = 0.828125;
    if (index == 27) limit = 0.328125;
    if (index == 28) limit = 0.984375;
    if (index == 29) limit = 0.484375;
    if (index == 30) limit = 0.859375;
    if (index == 31) limit = 0.359375;
    if (index == 32) limit = 0.0625;
    if (index == 33) limit = 0.5625;
    if (index == 34) limit = 0.1875;
    if (index == 35) limit = 0.6875;
    if (index == 36) limit = 0.03125;
    if (index == 37) limit = 0.53125;
    if (index == 38) limit = 0.15625;
    if (index == 39) limit = 0.65625;
    if (index == 40) limit = 0.8125;
    if (index == 41) limit = 0.3125;
    if (index == 42) limit = 0.9375;
    if (index == 43) limit = 0.4375;
    if (index == 44) limit = 0.78125;
    if (index == 45) limit = 0.28125;
    if (index == 46) limit = 0.90625;
    if (index == 47) limit = 0.40625;
    if (index == 48) limit = 0.25;
    if (index == 49) limit = 0.75;
    if (index == 50) limit = 0.125;
    if (index == 51) limit = 0.625;
    if (index == 52) limit = 0.21875;
    if (index == 53) limit = 0.71875;
    if (index == 54) limit = 0.09375;
    if (index == 55) limit = 0.59375;
    if (index == 56) limit = 1.0;
    if (index == 57) limit = 0.5;
    if (index == 58) limit = 0.875;
    if (index == 59) limit = 0.375;
    if (index == 60) limit = 0.96875;
    if (index == 61) limit = 0.46875;
    if (index == 62) limit = 0.84375;
    if (index == 63) limit = 0.34375;
  }

  return brightness < limit ? 0.0 : 1.0;
}

vec3 dither8x8(vec2 position, vec3 color) {
  return color * dither8x8(position, luma(color));
}

vec4 dither8x8(vec2 position, vec4 color) {
  return vec4(color.rgb * dither8x8(position, luma(color)), 1.0);
}


void main() {
	
	vec2	loc = gl_FragCoord.xy;
	vec4	out_color = vec4(0.0);
	vec4	lineColor = vec4(0.0);
	
	//	if the order is reversed...
	bool	reverseOrder = (endPt.x > startPt.x) ? true : false;
	//	if the line is a perfect vertical (the x values are the same) we need to do a special edge case
	bool	doYCurve = (abs(endPt.x - startPt.x) <= 2.0 / RENDERSIZE.x) ? true : false;
	
	//	figure out the range of nodes we have to iterate over (from this point plus / minus the max possible radius)
	float	radius = max(startLineLength, endLineLength) * RENDERSIZE.x;
	int		lNode = int(nearestNodeIndex(loc.x - radius));
	lNode = (lNode < 0) ? 0 : lNode;
	int		rNode = int(nearestNodeIndex(loc.x + radius));
	rNode = (rNode >= int(lineCount)) ? int(lineCount) - 1 : rNode;
	
	for (int i = lNode; i <= rNode; ++i)	{
		//	determine the overall progress of the lines being drawn
		float	progress = (reverseOrder) ? float(i) / (lineCount - 1.0) : 1.0 - float(i) / (lineCount - 1.0);
		
		//	figure out the nearest 'node'
		float	tmpX = (doYCurve) ? nearestYForNodeIndex(i) :nearestXForNodeIndex(i);
		float	tmpY = (doYCurve) ? curveFunctionForY(tmpX) : curveFunction(tmpX);
		vec2	nodePt = (doYCurve) ? vec2(RENDERSIZE.x * mix(startPt.x, endPt.x, progress), tmpY) : vec2(tmpX, tmpY);
		
		//	determine the length for the line at this node
		float	lineLength = mix(startLineLength, endLineLength, progress) * RENDERSIZE.x;

		//	if the length is greater than 0
		if (lineLength > 0.0)	{
			//	compute the angle for the line at this node
			float	lineAngle = mix(startAngle, endAngle, progress)  * pi / 180.0;
		
			//	figure out the two points we'll be drawing a line between
			vec2	linePt1 = nodePt + lineLength * vec2(cos(lineAngle), sin(lineAngle));
			vec2	linePt2 = nodePt - lineLength * vec2(cos(lineAngle), sin(lineAngle));
		
			//	check to see if we're on that line
			if (drawLine(linePt1 / RENDERSIZE, linePt2 / RENDERSIZE, isf_FragNormCoord, lineThickness) > 0.0)	{

				//	figure out what color to draw the line
				lineColor = mix(startLineColor, endLineColor, progress);
				
				//	if we're on a line
				if (lineColor.a > 0.0)	{
					out_color.rgb = lineColor.rgb;
					out_color.a += lineColor.a;
					
					//	if we're doing dithering...
					if (dithering > 0.0)	{
						float d = dither8x8(loc, luma(out_color));
						vec4 dithered = out_color * vec4(d,d,d,1.0);
						out_color = mix(out_color, dithered, dithering);
					}
					
					//	if we're already on a line, we can break the for loop
					//	note: change this if we are doing to handle multiple lines with different alphas for a single pixel
					break;
				}
			}
		}
	}
	

	
	gl_FragColor = out_color;
}