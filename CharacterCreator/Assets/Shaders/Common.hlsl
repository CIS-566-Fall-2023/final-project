#ifndef COMMONSHADERINCLUDE
#define COMMONSHADERINCLUDE

#define MAX_SDF_OBJECTS 128

// Uniforms set using MaterialPropertyBlock
float SDFType[MAX_SDF_OBJECTS];
float4 SDFData[MAX_SDF_OBJECTS];
float SDFBlendFactor[MAX_SDF_OBJECTS];
float SDFBlendOperation[MAX_SDF_OBJECTS];
float4x4 SDFTransformMatrices[MAX_SDF_OBJECTS];
float4 SDFPrimaryColors[MAX_SDF_OBJECTS];
float4 SDFSecondaryColors[MAX_SDF_OBJECTS];
float SDFTextureType[MAX_SDF_OBJECTS];
float4 SDFTextureData[MAX_SDF_OBJECTS];				// .x is texture scale
													// .y
													// .z
													// .w is normal blending strength (for triplanar)
float4 SDFEmissionColors[MAX_SDF_OBJECTS];
float SDFSmoothness[MAX_SDF_OBJECTS];
float SDFMetallic[MAX_SDF_OBJECTS];
float SDFCountTotal;								// in this number each compound object counts itself but also its children		
float SDFCountCompounded;							// in this number compound objects = 1
float OffsetsToNextSdf[MAX_SDF_OBJECTS];

#endif