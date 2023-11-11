#define MAX_ITERS 1024
#define MAX_DIST 1000000

struct Ray
{
	float3 origin;
	float3 direction;
};

float lengthSqr(float3 vec)
{
	return vec.x * vec.x + vec.y * vec.y + vec.z * vec.z;
}

float sdfSphere(float3 pos, float3 center, float radius)
{
	return (lengthSqr(pos));
}

void Raymarch_float(float3 rayOrigin, float3 rayDirection, float3 objectPos, float radius, out float4 color)
{
	float dist = 0.01f;
	for (int i = 0; i < MAX_ITERS; i++)
	{
		float3 p = rayOrigin + rayDirection * dist;

		float m = sdfSphere(p, objectPos, radius);

		if (m < radius)
		{
			// hit the sphere
			color = float4(1, 1, 1, 1);
			return;
		}

		dist += m;
		if (dist >= MAX_DIST)
		{
			break;
		}
	}

	color = float4(0, 0, 0, 0);
}