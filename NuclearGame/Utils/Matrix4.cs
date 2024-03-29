﻿using System.Numerics;
using Veldrid;

namespace NuclearGame.Utils;

public static class Matrix4
{
    public static Matrix4x4 CreatePerspective(float fov, float aspectRatio, float near, float far)
    {
        Matrix4x4 persp;
        if (Util.UseReverseDepth)
        {
            persp = CreatePerspectiveMatrix(fov, aspectRatio, far, near);
        }
        else
        {
            persp = CreatePerspectiveMatrix(fov, aspectRatio, near, far);
        }
        if (Util.IsClipSpaceYInverted)
        {
            persp *= new Matrix4x4(
                1, 0, 0, 0,
                0, -1, 0, 0,
                0, 0, 1, 0,
                0, 0, 0, 1);
        }

        return persp;
    }

    private static Matrix4x4 CreatePerspectiveMatrix(float fov, float aspectRatio, float near, float far)
    {
        if (fov <= 0.0f || fov >= MathF.PI)
            throw new ArgumentOutOfRangeException(nameof(fov));

        if (near <= 0.0f)
            throw new ArgumentOutOfRangeException(nameof(near));

        if (far <= 0.0f)
            throw new ArgumentOutOfRangeException(nameof(far));

        float yScale = 1.0f / MathF.Tan(fov * 0.5f);
        float xScale = yScale / aspectRatio;

        Matrix4x4 result;

        result.M11 = xScale;
        result.M12 = result.M13 = result.M14 = 0.0f;

        result.M22 = yScale;
        result.M21 = result.M23 = result.M24 = 0.0f;

        result.M31 = result.M32 = 0.0f;
        var negFarRange = float.IsPositiveInfinity(far) ? -1.0f : far / (near - far);
        result.M33 = negFarRange;
        result.M34 = -1.0f;

        result.M41 = result.M42 = result.M44 = 0.0f;
        result.M43 = near * negFarRange;

        return result;
    }
}