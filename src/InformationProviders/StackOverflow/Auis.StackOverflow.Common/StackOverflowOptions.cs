﻿namespace Auis.StackOverflow.Common;

public class StackOverflowOptions
{
    public string BaseUrl { get; set; } = string.Empty;
    public bool IsNetworkShare { get; set; }
    public string NetworkShareBasePath { get; set; } = string.Empty;
}