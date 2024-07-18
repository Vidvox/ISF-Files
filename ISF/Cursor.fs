/*{
    "CATEGORIES": [
        "Utility"
    ],
    "CREDIT": "VIDVOX",
    "DESCRIPTION": "Draws a cursor.",
    "INPUTS": [
		{
		  "NAME" : "cursor_scale",
		  "TYPE" : "float",
		  "MAX" : 2.0,
		  "DEFAULT" : 1.0,
		  "MIN" : 0.0,
		  "LABEL" : "Cursor Size"
		},
        {
            "DEFAULT": [
                0.5,
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
            "NAME": "cursor_position",
            "TYPE": "point2D",
            "LABEL" : "Cursor Pos"
        },
		{
		  "NAME" : "bgColor",
		  "LABEL" : "BG Color",
		  "TYPE" : "color",
		  "DEFAULT" : [
			0,
			0,
			0,
			0
		  ]
		}
    ],
	"IMPORTED": {
		"cursorImage": {
			"PATH": "cursor.png"
		}
	},
    "ISFVSN": "2"
}
*/


bool pointInRect(vec2 pt, vec4 r)
{
	bool	returnMe = false;
	if ((pt.x >= r.x)&&(pt.y >= r.y)&&(pt.x <= r.x + r.z)&&(pt.y <= r.y + r.w))
		returnMe = true;
	return returnMe;
}



void main() {	
	//	default to showing the bgColor
	vec4	this_pixel = bgColor;
	vec4	out_color = this_pixel;
	
	//	only do this if our scale is greater than 0.0 (otherwise we'd get a divide by 0)
	if (cursor_scale > 0.0)	{
		//	get the image size and multiply it by our scale
		vec2		img_size = IMG_SIZE(cursorImage) * cursor_scale;
		//	get the pixel location of this pixel
		vec2		loc = gl_FragCoord.xy;
		//	make a vec4 that describes the rectangle that we need to do our extra drawing in
		vec2		cursor_origin = (RENDERSIZE.xy - img_size) * cursor_position.xy;
		vec4		cursor_bounds;
		cursor_bounds.xy = cursor_origin;
		cursor_bounds.zw = img_size;
		
		//	determine if we are in that rectangle
		bool	isInShape = pointInRect(loc,cursor_bounds);
		
		//	if we are, draw the cursor over our background
		if (isInShape)	{
			//	figure out the coordinate from cursor image we should read from
			vec2	read_loc = loc - cursor_origin;
			out_color = IMG_PIXEL(cursorImage, read_loc / cursor_scale);
			//	if the alpha of that pixel is less than one, blend
			if (out_color.a < 1.0)	{
				out_color += this_pixel * (1.0 - out_color.a);
			}
		}
	}
	
	gl_FragColor = out_color;
}