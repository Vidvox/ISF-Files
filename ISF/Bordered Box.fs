/*{
    "CATEGORIES": [
        "Geometry"
    ],
    "CREDIT": "VIDVOX",
    "DESCRIPTION": "Draws a simple box with a border over a transparent background.",
    "INPUTS": [
		{
		  "NAME" : "box_width",
		  "TYPE" : "float",
		  "MAX" : 1.0,
		  "DEFAULT" : 0.2,
		  "MIN" : 0.0,
		  "LABEL" : "Box Width"
		},
		{
		  "NAME" : "box_height",
		  "TYPE" : "float",
		  "MAX" : 1.0,
		  "DEFAULT" : 0.2,
		  "MIN" : 0.0,
		  "LABEL" : "Box Height"
		},
		{
		  "NAME" : "border_thickness",
		  "TYPE" : "float",
		  "MAX" : 1.0,
		  "DEFAULT" : 0.5,
		  "MIN" : 0.0,
		  "LABEL" : "Border Size"
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
            "NAME": "box_position",
            "TYPE": "point2D",
            "LABEL" : "Box Center"
        },
		{
			"NAME" : "box_color",
			"LABEL" : "Fill Color",
			"TYPE" : "color",
			"DEFAULT" : [
				1,
				1,
				0,
				0.25]
		},
		{
			"NAME" : "border_color",
			"LABEL" : "Border Color",
			"TYPE" : "color",
			"DEFAULT" : [
				1,
				1,
				0,
				0.5]
		}
    ],
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

vec4 insetRect(vec2 inset, vec4 r)
{
	vec4	returnMe = r;
	vec2	aspectInset = inset;
	aspectInset.x *= RENDERSIZE.y / RENDERSIZE.x;
	
	returnMe.xy += aspectInset.xy / 2.0;
	returnMe.zw -= aspectInset.xy;
	
	return returnMe;
}



void main() {	
	//	default to showing the pixel being passed in
	vec4	this_pixel = vec4(0.0);
	vec4	out_color = this_pixel;
	
	//	only do this if our scale is greater than 0.0 (otherwise we'd get a divide by 0)
	if (box_width > 0.0 && box_height > 0)	{
	
		//	get the image size and multiply it by our scale
		vec2		img_size = vec2(box_width, box_height);
		//	get the normalized pixel location of this pixel
		vec2		loc = isf_FragNormCoord.xy;
		//	make a vec4 that describes the rectangle that we need to do our extra drawing in
		vec2		box_origin = box_position.xy - img_size / 2.0;
		vec4		box_bounds;
		box_bounds.xy = box_origin;
		box_bounds.zw = img_size;
		
		//	determine if we are in that rectangle
		bool	isInShape = pointInRect(loc,box_bounds);
		
		//	if we are, draw the cursor over our image
		if (isInShape)	{
			vec4	fill_bounds = insetRect(vec2(border_thickness / 100.0), box_bounds);
			bool	isInFillBounds = pointInRect(loc, fill_bounds);
			vec4	tmpColor = (isInFillBounds) ? box_color : border_color;
			//	figure out the coordinate from cursor image we should read from
			out_color.rgb = mix(this_pixel.rgb, tmpColor.rgb, tmpColor.a);
		}
	}
	
	gl_FragColor = out_color;
}