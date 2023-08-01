using System;

public class MathHelper
{
    public static int Round(double value)
    {
        return (int)Math.Round(value, MidpointRounding.AwayFromZero);
    }

}