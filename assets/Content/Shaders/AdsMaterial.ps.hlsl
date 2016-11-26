/*-------------------------------------
 * STRUCTS
 *-----------------------------------*/

struct PS_INPUT {
    float4 pos      : POSITION0;
    float4 screenPos: SV_POSITION;
    float2 texCoord : TEXCOORD0;
};

struct PS_OUTPUT {
    float4 color: SV_TARGET;
};

/*-------------------------------------
 * CONSTANTS
 *-----------------------------------*/

SamplerState TextureSampler {
    AddressU = Clamp;
    AddressV = Clamp;
    Filter   = MIN_MAG_MIP_LINEAR;
};

/*-------------------------------------
 * GLOBALS
 *-----------------------------------*/

cbuffer cbConstants: register(b1) {
    float4 Ambient;
    float4 Diffuse;
    float4 Specular;
    bool   UseTexture;
};

tbuffer tbTextures {
    Texture2D Textures[1];
}

/*-------------------------------------
 * FUNCTIONS
 *-----------------------------------*/

void main(in PS_INPUT psIn, out PS_OUTPUT psOut) {
    float4 color = 1.0;

    if (UseTexture) {
        color = Textures[0].Sample(TextureSampler, psIn.texCoord);
    }

    float d = 1.0;
    float s = 1.0;

    psOut.color = color*Ambient + d*color*Diffuse + s*color*Specular;
}
