﻿global using eShop.Services.Catalog.CatalogAPI;
global using Microsoft.OpenApi.Models;
global using Microsoft.AspNetCore.Mvc;
global using Serilog;
global using Serilog.Formatting.Compact;
global using eShop.BuildingBlocks.Logging.CommonLogging;
global using Microsoft.EntityFrameworkCore;
global using eShop.Services.Catalog.Infrastructure.EntityConfigurations;
global using Microsoft.EntityFrameworkCore.Design;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using eShop.Services.Catalog.Infrastructure;
global using System.Reflection;
global using Polly;
global using System.Data.SqlClient;
global using Polly.Contrib.WaitAndRetry;
global using eShop.BuildingBlocks.Host.CommonHost;
global using  MediatR;
global using eShop.Services.Catalog.Domain.SeedWork;
global using eShop.Services.Catalog.Catalog.Domain.Exceptions;
global using eShop.Services.Catalog.Domain.AggregatesModel.CatalogAggregate;
global using eShop.Services.Catalog.Domain.AggregatesModel.CatalogTypeAggregate;
global using eShop.Services.Catalog.Domain.AggregatesModel.SupplierAggregate;
global using eShop.Services.Catalog.Infrastructure.Repositories;
global using eShop.Services.Catalog.Domain.Event;
global using eShop.Services.Catalog.CatalogAPI.Applicatioin.Features.Catalog.CreateCatalog.Command;
global using eShop.Services.Catalog.CatalogAPI.Applicatioin.Features.Catalog.Command.UpdatePrice;
global using eShop.Services.Catalog.CatalogAPI.Applicatioin.Exceptions;
global using eShop.Services.Catalog.CatalogAPI.Applicatioin.Features.Catalog.Commands.UpdateStock;
global using Microsoft.Extensions.Options;
global using eShop.Services.Catalog.CatalogAPI.Applicatioin.Features.Catalog.Queries.GetCatalogList;
global using eShop.Services.CatalogAPI.Applicatioin.Behaviors;
global using eShop.Services.Catalog.CatalogAPI.Applicatioin.Features.Catalog.Queries.GetCatalog;
global using Dapper;
global using eShop.Services.Catalog.CatalogAPI.Extensions;
global using Microsoft.EntityFrameworkCore.Storage;
global  using Serilog.Context;
global using eShop.Services.Catalog.Domain;
global using eShop.BuildingBlocks.Common.ErrorHandler;
global using Microsoft.AspNetCore.Mvc.Filters;
global using eShop.Services.Catalog.Catalog.API.Infrastructure.Filters;
global using System.Net;
global using eShop.Services.CatalogAPI.Catalog.API.Applicatioin.Validation;
global using FluentValidation;
global using FluentValidation.AspNetCore;
global using System.Text.Json;
global using System.Text.Json.Serialization;