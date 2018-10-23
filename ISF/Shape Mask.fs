/*{
	"CREDIT": "by VIDVOX",
	"ISFVSN": "2",
	"CATEGORIES": [
		"Masking"
	],
	"INPUTS": [
		{
			"NAME": "inputImage",
			"TYPE": "image"
		},
		{
			"NAME": "maskShapeMode",
			"LABEL": "Mask Shape Mode",
			"TYPE": "long",
			"VALUES": [
				0,
				1,
				2,
				3
			],
			"LABELS": [
				"Rectangle",
				"Triangle",
				"Circle",
				"Diamond"
			],
			"DEFAULT": 1
		},
		{
			"NAME": "shapeWidth",
			"LABEL": "Shape Width",
			"TYPE": "float",
			"MIN": 0.0,
			"MAX": 2.0,
			"DEFAULT": 0.5
		},
		{
			"NAME": "shapeHeight",
			"LABEL": "Shape Height",
			"TYPE": "float",
			"MIN": 0.0,
			"MAX": 2.0,
			"DEFAULT": 0.5
		},
		{
			"NAME": "center",
			"TYPE": "point2D",
			"DEFAULT": [
				0.5,
				0.5
			]
		},
		{
			"NAME": "invertMask",
			"LABEL": "Invert Mask",
			"TYPE": "bool",
			"DEFAULT": false
		},
		{
			"NAME": "horizontalRepeat",
			"LABEL": "Horizontal Repeat",
			"TYPE": "long",
			"VALUES": [
				1,
				2,
				3,
				4,
				5,
				6,
				7,
				8,
				9
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
				"9"
			],
			"DEFAULT": 1
		},
		{
			"NAME": "verticalRepeat",
			"LABEL": "Vertical Repeat",
			"TYPE": "long",
			"VALUES": [
				1,
				2,
				3,
				4,
				5,
				6,
				7,
				8,
				9
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
				"9"
			],
			"DEFAULT": 1
		},
		{
			"NAME": "maskApplyMode",
			"LABEL": "Apply Mask",
			"TYPE": "long",
			"VALUES": [
				0,
				1,
				2
			],
			"LABELS": [
				"Apply Mask",
				"Set Alpha",
				"Show Mask"
			],
			"DEFAULT": 0
		}
	]
}*/



const float pi = 3.14159265359;


vec2 rotatePoint(vec2 pt, float angle, vec2 center)
{
	vec2 returnMe;
	float s = sin(angle * pi);
	float c = cos(angle * pi);

	returnMe = pt;

	// translate point back to origin:
	returnMe.x -= center.x;
	returnMe.y -= center.y;

	// rotate point
	float xnew = returnMe.x * c - returnMe.y * s;
	float ynew = returnMe.x * s + returnMe.y * c;

	// translate point back:
	returnMe.x = xnew + center.x;
	returnMe.y = ynew + center.y;
	return returnMe;
}

float sign(vec2 p1, vec2 p2, vec2 p3)
{
	return (p1.x - p3.x) * (p2.y - p3.y) - (p2.x - p3.x) * (p1.y - p3.y);
}

bool PointInTriangle(vec2 pt, vec2 v1, vec2 v2, vec2 v3)
{
	bool b1, b2, b3;

	b1 = sign(pt, v1, v2) < 0.0;
	b2 = sign(pt, v2, v3) < 0.0;
	b3 = sign(pt, v3, v1) < 0.0;

	return ((b1 == b2) && (b2 == b3));
}

bool RotatedPointInTriangle(vec2 pt, vec2 v1, vec2 v2, vec2 v3, vec2 center)
{
	bool b1, b2, b3;
	
	vec2 v1r = v1;
	vec2 v2r = v2;
	vec2 v3r = v3;

	b1 = sign(pt, v1r, v2r) < 0.0;
	b2 = sign(pt, v2r, v3r) < 0.0;
	b3 = sign(pt, v3r, v1r) < 0.0;

	return ((b1 == b2) && (b2 == b3));
}


float isPointInShape(vec2 pt, int shape, vec4 shapeCoordinates)	{
	float returnMe = 0.0;
	
	//	rectangle
	if (shape == 0)	{
		if (RotatedPointInTriangle(pt, shapeCoordinates.xy, shapeCoordinates.xy + vec2(0.0, shapeCoordinates.w), shapeCoordinates.xy + vec2(shapeCoordinates.z, 0.0), shapeCoordinates.xy + shapeCoordinates.zw / 2.0))	{
			returnMe = 1.0;
			// soft edge if needed
			if ((pt.x > shapeCoordinates.x) && (pt.x < shapeCoordinates.x)) {
				returnMe = clamp(((pt.x - shapeCoordinates.x) / RENDERSIZE.x), 0.0, 1.0);
				returnMe = pow(returnMe, 0.5);
			}
			else if ((pt.x > shapeCoordinates.x + shapeCoordinates.z) && (pt.x < shapeCoordinates.x + shapeCoordinates.z)) {
				returnMe = clamp(((shapeCoordinates.x + shapeCoordinates.z - pt.x) / RENDERSIZE.x), 0.0, 1.0);
				returnMe = pow(returnMe, 0.5);
			}
		}
		else if (RotatedPointInTriangle(pt, shapeCoordinates.xy + shapeCoordinates.zw, shapeCoordinates.xy + vec2(0.0, shapeCoordinates.w), shapeCoordinates.xy + vec2(shapeCoordinates.z, 0.0), shapeCoordinates.xy + shapeCoordinates.zw / 2.0))	{
			returnMe = 1.0;
			// soft edge if needed
			if ((pt.x > shapeCoordinates.x) && (pt.x < shapeCoordinates.x)) {
				returnMe = clamp(((pt.x - shapeCoordinates.x) / RENDERSIZE.x), 0.0, 1.0);
				returnMe = pow(returnMe, 0.5);
			}
			else if ((pt.x > shapeCoordinates.x + shapeCoordinates.z) && (pt.x < shapeCoordinates.x + shapeCoordinates.z)) {
				returnMe = clamp(((shapeCoordinates.x + shapeCoordinates.z - pt.x) / RENDERSIZE.x), 0.0, 1.0);
				returnMe = pow(returnMe, 0.5);
			}
		}
	}
	//	triangle
	else if (shape == 1)	{
		if (RotatedPointInTriangle(pt, shapeCoordinates.xy, shapeCoordinates.xy + vec2(shapeCoordinates.z / 2.0, shapeCoordinates.w), shapeCoordinates.xy + vec2(shapeCoordinates.z, 0.0), shapeCoordinates.xy + shapeCoordinates.zw / 2.0))	{
			returnMe = 1.0;
		}
	}
	//	oval
	else if (shape == 2)	{
		returnMe = distance(pt, vec2(shapeCoordinates.xy + shapeCoordinates.zw / 2.0));
		if (returnMe < min(shapeCoordinates.z,shapeCoordinates.w) / 2.0)	{
			returnMe = 1.0;
		}
		else	{
			returnMe = 0.0;
		}
	}
	//	diamond
	else if (shape == 3)	{
		if (RotatedPointInTriangle(pt, shapeCoordinates.xy + vec2(0.0, shapeCoordinates.w / 2.0), shapeCoordinates.xy + vec2(shapeCoordinates.z / 2.0, shapeCoordinates.w), shapeCoordinates.xy + vec2(shapeCoordinates.z, shapeCoordinates.w / 2.0), shapeCoordinates.xy + shapeCoordinates.zw / 2.0))	{
			returnMe = 1.0;
		}
		else if (RotatedPointInTriangle(pt, shapeCoordinates.xy + vec2(0.0, shapeCoordinates.w / 2.0), shapeCoordinates.xy + vec2(shapeCoordinates.z / 2.0, 0.0), shapeCoordinates.xy + vec2(shapeCoordinates.z, shapeCoordinates.w / 2.0), shapeCoordinates.xy + shapeCoordinates.zw / 2.0))	{
			returnMe = 1.0;
		}
	}

	return returnMe;	
}



void main() {
	vec4		srcPixel = IMG_THIS_PIXEL(inputImage);
	vec2		centerPt = center;
	vec2		tmpVec = RENDERSIZE * vec2(shapeWidth,shapeHeight) / 2.0;
	vec4		patternRect = vec4(vec2(centerPt - tmpVec),tmpVec * 2.0);
	vec2		thisPoint = RENDERSIZE * isf_FragNormCoord;
	
	if ((thisPoint.x >= patternRect.x) && (thisPoint.x <= patternRect.x + abs(patternRect.z)))	{
		patternRect.z = patternRect.z / float(horizontalRepeat);
		patternRect.x = patternRect.x + abs(patternRect.z) * floor((thisPoint.x - patternRect.x) / abs(patternRect.z));
	}
	else	{
		patternRect.z = patternRect.z / float(horizontalRepeat);	
	}
	
	if ((thisPoint.y >= patternRect.y) && (thisPoint.y <= patternRect.y + abs(patternRect.w)))	{
		patternRect.w = patternRect.w / float(verticalRepeat);
		patternRect.y = patternRect.y + abs(patternRect.w) * floor((thisPoint.y - patternRect.y) / abs(patternRect.w));
	}
	else	{
		patternRect.w = patternRect.w / float(verticalRepeat);	
	}
	
	float		luminance = isPointInShape(thisPoint.xy, maskShapeMode, patternRect);
	
	if (invertMask)
		luminance = 1.0 - luminance;
	
	if (maskApplyMode == 0)	{
		srcPixel = srcPixel * luminance;
	}
	else if (maskApplyMode == 1)	{
		srcPixel.a = srcPixel.a * luminance;
	}
	else if (maskApplyMode == 2)	{
		srcPixel.rgb = vec3(luminance);
	}
	
	gl_FragColor = srcPixel;
}

