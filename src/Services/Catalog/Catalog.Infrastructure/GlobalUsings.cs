

global using Microsoft.EntityFrameworkCore;
global using eShop.Services.Catalog.Infrastructure.EntityConfigurations;
global using Microsoft.EntityFrameworkCore.Design;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;

global using  MediatR;
global using eShop.Services.Catalog.Domain.SeedWork;
global using eShop.Services.Catalog.Catalog.Domain.Exceptions;
global using eShop.Services.Catalog.Domain.AggregatesModel.CatalogAggregate;
global using eShop.Services.Catalog.Domain.AggregatesModel.CatalogTypeAggregate;
global using eShop.Services.Catalog.Domain.AggregatesModel.SupplierAggregate;

global using Microsoft.EntityFrameworkCore.Storage;

global using Microsoft.AspNetCore.Hosting;
global using Microsoft.Extensions.Logging;
global using System.IO.Compression;
global using eShop.BuildingBlocks.Event.IntegrationEventLogEF;

global using eShop.Services.Catalog.Infrastructure;
global using eShop.Services.Catalog.Infrastructure.Repositories;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;