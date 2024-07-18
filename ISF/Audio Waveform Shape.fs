/*
{
  "CATEGORIES" : [
    "Audio Visualizer"
  ],
  "DESCRIPTION" : "Wraps an audio waveform around a shape and does video feedback.",
  "ISFVSN" : "2",
  "INPUTS" : [
    {
      "NAME" : "audioWave",
      "TYPE" : "audio"
    },
    {
      "NAME" : "audioGain",
      "TYPE" : "float",
      "MAX" : 2,
      "DEFAULT" : 1.0,
      "MIN" : 0
    },
    {
      "NAME" : "lineWidth",
      "TYPE" : "float",
      "MAX" : 0.1,
      "DEFAULT" : 0.01,
      "MIN" : 0
    },
    {
      "NAME" : "shapeRadius",
      "TYPE" : "float",
      "MAX" : 1,
      "DEFAULT" : 0.25,
      "MIN" : 0
    },
    {
      "NAME" : "feedbackLevel",
      "TYPE" : "float",
      "MAX" : 1,
      "DEFAULT" : 0.25,
      "MIN" : 0
    },
    {
      "NAME" : "zoomLevel",
      "TYPE" : "float",
      "MAX" : 1.1,
      "DEFAULT" : 1.01,
      "MIN" : 0.9,
      "LABEL" : "Feedback Zoom"
    },
    {
      "NAME" : "mixPoint",
      "TYPE" : "float",
      "MAX" : 1,
      "DEFAULT" : 0.0,
      "MIN" : 0
    },
    {
      "NAME" : "waveColor1",
      "TYPE" : "color",
      "DEFAULT" : [
        0.95969659090042114,
        0.80407930507175662,
        0.44830461173716851,
        1
      ]
    },
    {
      "NAME" : "waveColor2",
      "TYPE" : "color",
      "DEFAULT" : [
        0.055977690787502407,
        0.71078224912611609,
        0.93459457159042358,
        1
      ]
    },
    {
      "LABELS" : [
        "Circle",
        "Triangle",
        "Rect",
        "Pentagram",
        "Hexagon",
        "Star1",
        "Star2",
        "Heart",
        "Rays"
      ],
      "NAME" : "shape1",
      "TYPE" : "long",
      "DEFAULT" : 0,
      "VALUES" : [
        0,
        1,
        2,
        3,
        4,
        5,
        6,
        7,
        8
      ]
    },
    {
      "LABELS" : [
        "Circle",
        "Triangle",
        "Rect",
        "Pentagram",
        "Hexagon",
        "Star1",
        "Star2",
        "Heart",
        "Rays"
      ],
      "NAME" : "shape2",
      "TYPE" : "long",
      "DEFAULT" : 1,
      "VALUES" : [
        0,
        1,
        2,
        3,
        4,
        5,
        6,
        7,
        8
      ]
    },
    {
      "NAME" : "shapeCenter",
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
      "NAME" : "zoomCenter",
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
    }
  ],
  "PASSES" : [
    {
      "TARGET" : "feedbackBuffer",
      "PERSISTENT" : true
    }
  ],
  "CREDIT" : ""
}
*/

const float pi = 3.1415926535897932384626433832795;
const float tau =  6.2831853071795864769252867665590;

//	borrowed from pixel spirit deck!
//	https://github.com/patriciogonzalezvivo/PixelSpiritDeck/tree/master/lib

float triSDF(vec2 st) {
    st = (st*2.-1.)*2.;
    return max(abs(st.x) * 0.866025 + st.y * 0.5, -st.y * 0.5);
}
float circleSDF(vec2 st) {
    return length(st-.5)*2.;
}
float polySDF(vec2 st, int V) {
    st = st*2.-1.;
    float a = atan(st.x,st.y)+pi;
    float r = length(st);
    float v = tau/float(V);
    return cos(floor(.5+a/v)*v-a)*r;
}
float pentSDF(vec2 st)	{
	vec2 pt = st;
	pt.y /= 0.89217;
	return polySDF(pt, 5);
}
float hexSDF(vec2 st) {
    st = abs(st*2.-1.);
    return max(abs(st.y), st.x * 0.866025 + st.y * 0.5);
}
float flowerSDF(vec2 st, int N) {
    st = st*2.-1.;
    float r = length(st)*2.;
    float a = atan(st.y,st.x);
    float v = float(N)*.5;
    return 1.-(abs(cos(a*v))*.5+.5)/r;
}
float heartSDF(vec2 st) {
    st -= vec2(.5,.8);
    float r = length(st)*5.5;
    st = normalize(st);
    return r - 
         ((st.y*pow(abs(st.x),0.67))/ 
         (st.y+1.5)-(2.)*st.y+1.26);
}
float starSDF(vec2 st, int V, float s) {
    st = st*4.-2.;
    float a = atan(st.y, st.x)/tau;
    float seg = a * float(V);
    a = ((floor(seg) + 0.5)/float(V) + 
        mix(s,-s,step(.5,fract(seg)))) 
        * tau;
    return abs(dot(vec2(cos(a),sin(a)),
                   st));
}
float raysSDF(vec2 st, int N) {
    st -= .5;
    return fract(atan(st.y,st.x)/tau*float(N));
}

float shapeForType(vec2 st, int shape)	{
	if (shape == 0)
		return circleSDF(st);
	else if (shape == 1)	
		return triSDF(st);
	else if (shape == 2)
		return polySDF(st,4);
	else if (shape == 3)
		return pentSDF(st);
	else if (shape == 4)
		return hexSDF(st);
	else if (shape == 5)
		return starSDF(st,5,0.07);
	else if (shape == 6)
		return starSDF(st,12,0.12);
	else if (shape == 7)
		return heartSDF(st);
	else if (shape == 8)
		return raysSDF(st,6);
}


void main()	{
	vec4		inputPixelColor = vec4(0.0);
	vec2		loc = isf_FragNormCoord.xy;
	loc -= (shapeCenter - vec2(0.5));
	loc = mix(vec2((loc.x*RENDERSIZE.x/RENDERSIZE.y)-(RENDERSIZE.x*.5-RENDERSIZE.y*.5)/RENDERSIZE.y,loc.y), 
				vec2(loc.x,loc.y*(RENDERSIZE.y/RENDERSIZE.x)-(RENDERSIZE.y*.5-RENDERSIZE.x*.5)/RENDERSIZE.x), 
				step(RENDERSIZE.x,RENDERSIZE.y));
	
	float		val1 = shapeForType(loc,shape1);
	float		val2 = shapeForType(loc,shape2);
	vec2		locCenter = shapeCenter * RENDERSIZE;
	loc = gl_FragCoord.xy;
	float		val = mix(val1,val2,mixPoint);
	float		a1 = (atan(locCenter.y-loc.y,locCenter.x-loc.x) + pi) / (tau);
	vec4		audioVal = IMG_NORM_PIXEL(audioWave,vec2(a1,0.0));
	
	//val += (audioGain == 0.0) ? 0.0 : audioGain * ((sin(TIME+10.0*tau*(a1)))/13.0 + (sin(-TIME*2.1+17.0*tau*(a1)))/17.0 + (sin(19.0*tau*(a1)))/19.0);
	val += ((audioVal.r - 0.5)*2.0) * audioGain;
	
	float		scaledRadius = shapeRadius * min(RENDERSIZE.x,RENDERSIZE.y);
	float		dist = val * min(RENDERSIZE.x,RENDERSIZE.y);
	bool		invertMask = false;
	float		lineWidthPix = lineWidth * min(RENDERSIZE.x,RENDERSIZE.y);
	
	//	if within the shape, just do the shape
	if ((dist>=scaledRadius-lineWidthPix)&&(dist<=scaledRadius+lineWidthPix))	{
		//inputPixelColor = IMG_PIXEL(inputImage,loc);
		inputPixelColor = mix(waveColor1,waveColor2,mixPoint);
	}
	
	vec2		feedbackLoc = (zoomCenter) + ((isf_FragNormCoord - zoomCenter) / zoomLevel);
	vec4		feedbackColor = IMG_NORM_PIXEL(feedbackBuffer,feedbackLoc);
	feedbackColor.a *= pow(feedbackLevel, 0.1);
	inputPixelColor = mix(inputPixelColor,feedbackColor,1.0-inputPixelColor.a);
	
	gl_FragColor = inputPixelColor;
}
