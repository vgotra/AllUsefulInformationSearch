﻿namespace AllUsefulInformationSearch.Common;

public interface IEntity<T> where T : struct
{
    T Id { get; set; }
}