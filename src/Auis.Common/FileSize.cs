namespace Auis.Common;

public static class FileSize
{
    public static int Kb => 1024;
    public static int Mb => Kb * Kb;
    public static int Gb => Mb * Kb;
    public static int Tb => Gb * Kb;
}