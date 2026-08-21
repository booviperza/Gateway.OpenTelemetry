using OpenIddict.Server;

var openIddictType =
    typeof(OpenIddictServerHandlers.Authentication);

var handlerType =
    openIddictType
        .GetNestedType(
            "ApplyAuthorizationResponse`1",
            System.Reflection.BindingFlags.Public |
            System.Reflection.BindingFlags.NonPublic);

Console.WriteLine(
    $"Handler: {handlerType}");

if (handlerType is null)
{
    return;
}

var closedType =
    handlerType.MakeGenericType(
        typeof(OpenIddictServerEvents.ApplyAuthorizationResponseContext));

Console.WriteLine(
    $"Closed: {closedType}");

var descriptor =
    closedType.GetProperty(
        "Descriptor",
        System.Reflection.BindingFlags.Public |
        System.Reflection.BindingFlags.Static);

Console.WriteLine(
    $"Descriptor property: {descriptor}");

if (descriptor?.GetValue(null)
    is OpenIddictServerHandlerDescriptor value)
{
    Console.WriteLine(
        $"Order: {value.Order}");

    Console.WriteLine(
        $"Type: {value.Type}");

    Console.WriteLine(
        $"Context: {value.ContextType}");
}