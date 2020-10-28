#ifndef CUSTOM_LIGHTING_INCLUDED
#define CUSTOM_LIGHTING_INCLUDED

float GetLighting(Surface surface)
{
    return surface.color * surface.normal.y;
}

#endif