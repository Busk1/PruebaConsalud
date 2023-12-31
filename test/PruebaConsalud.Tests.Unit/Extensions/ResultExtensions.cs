﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PruebaConsalud.Tests.Unit.Extensions;

public static class ResultExtensions
{
    public static T? GetOkObjectResultValue<T>(this IResult result)
    {
        return (T?)Type.GetType("Microsoft.AspNetCore.Http.Result.OkObjectResult, Microsoft.AspNetCore.Http.Results")?
            .GetProperty("Value",
            System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public)?
            .GetValue(result);
    }

    public static int? GetOkObjectResultStatusCode(this IResult result)
    {
        return (int?)Type.GetType("Microsoft.AspNetCore.Http.Result.OkObjectResult, Microsoft.AspNetCore.Http.Results")?
            .GetProperty("StatusCode",
            System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public)?
            .GetValue(result);
    }

    public static T? GetObjectResultValue<T>(this IResult result)
    {
        return (T?)Type.GetType("Microsoft.AspNetCore.Http.Result.ObjectResult, Microsoft.AspNetCore.Http.Results")?
            .GetProperty("Value",
            System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public)?
            .GetValue(result);
    }

    public static int? GetObjectResultStatusCode(this IResult result)
    {
        return (int?)Type.GetType("Microsoft.AspNetCore.Http.Result.ObjectResult, Microsoft.AspNetCore.Http.Results")?
            .GetProperty("StatusCode",
            System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public)?
            .GetValue(result);
    }
}
