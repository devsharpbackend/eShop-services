﻿namespace eShop.Services.CatalogAPI.Domain.AggregatesModel;
public class CatalogType: Entity
{
    public CatalogType(string type)
    {
        _type = type;
    }
    public string _type;
    public string? Type => _type;
}