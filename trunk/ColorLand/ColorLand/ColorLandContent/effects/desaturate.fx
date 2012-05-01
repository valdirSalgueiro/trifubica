// Effect dynamically changes color saturation.

sampler TextureSampler : register(s0);

// A timer to animate our shader
float fTimer;

// the amount of distortion
float fNoiseAmount;

// just a random starting number
int iSeed;

bool bHeat;
bool bShip;

float4 main(float4 color : COLOR0, float2 texCoord : TEXCOORD0) : COLOR0
{
    float NoiseX = iSeed * fTimer * sin(texCoord.x * texCoord.y+fTimer);
	NoiseX=fmod(NoiseX,8) * fmod(NoiseX,4);	

	// Use our distortion factor to compute how much it will affect each
	// texture coordinate
	float DistortX = fmod(NoiseX,fNoiseAmount);
	float DistortY = fmod(NoiseX,fNoiseAmount+0.002);
	
	// Create our new texture coordinate based on our distortion factor
	float2 DistortTex = float2(DistortX,DistortY);

	// Look up the texture color.
    float4 tex;
    
	if(bHeat){
		tex = tex2D(TextureSampler, texCoord+DistortTex);
    }else if(bShip){
		tex = tex2D(TextureSampler, float2(texCoord.x,texCoord.y+sin(fTimer)*0.02f));
    }else{
		tex = tex2D(TextureSampler, texCoord);
	}
    // Convert it to greyscale. The constants 0.3, 0.59, and 0.11 are because
    // the human eye is more sensitive to green light, and less to blue.
    float greyscale = dot(tex.rgb, float3(0.3, 0.59, 0.11));
    
    // The input color alpha controls saturation level.
    tex.rgb = lerp(greyscale, tex.rgb, color.a * 4);
    
    return tex;
}


technique Desaturate
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 main();
    }
}
