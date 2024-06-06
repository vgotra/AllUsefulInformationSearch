namespace Auis.Common;

public static class FileSize
{
    public static long Kb => 1024;
    public static long Mb => Kb * Kb;
    public static long Gb => Mb * Kb;
    public static long Tb => Gb * Kb;
}