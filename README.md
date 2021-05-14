# Summary

This package provides a basic forward pipe operator. This is a commonly requested language feature in C#, but appears to not currently be on the schedule.

Here are currently open tickets requesting the feature:
- [C# ticket #74](https://github.com/dotnet/csharplang/discussions/74)
- [C# ticket #96](https://github.com/dotnet/csharplang/discussions/96).

To my knowledge, we can't override an operator for every object type, so I've opted instead to use extension methods.

# Install

[NuGet page](https://www.nuget.org/packages/WinstonPuckett.PipeExtensions/1.0.0#)

# What is a forward pipe operator?

A forward pipe operator is a way to visualize the call of functions from top to bottom instead of inside to outside. A typical call structure in an imperative language might look like:

```csharp
var model = Query(input);
Validate(model);
var outputModel = Transform(model);
Submit(outputModel);
```

Or even worse:

```csharp
Submit(Transform(Validate(Query(input);
```

In a functional language (example in F#) there is a way to "pipe" the results of one function to another:

```fsharp
input
  |> Query
  |> Validate
  |> Transform
  |> Submit
```

The above syntax means, "Take input, pass it to Query, then pass the result to Validate, then pass the result to Transform, then pass the result to Submit."

# What does a forward pipe operator look like in C#?

As far as I know, you can't overload an operator for the base object or even overload two operators squished together (|>). So, barring this being added to C# natively, we can only simulate a forward pipe operator with extension methods.

## Implementation

The code for this is dead simple:

```csharp
public static U Pipe<T, U>(this T input, Func<T, U> @operator)
{
    return @operator(input);
}
```

## Using the pipe operator

To use the Pipe operator, you would just call .Pipe on whatever object you're using and pass in the function you're hoping to run.

```csharp
input
  .Pipe(Query)
  .Pipe(Validate)
  .Pipe(Transform)
  .Pipe(Submit);
```

## Asynchronous processing

You can also use async methods. Because of how C# conceptualizes tasks, you **must** use all .PipeAsync after your first async method regardless of whether your subsequent method is async. PipeAsync refers to the return type of the method, which will always be asynchronous after the first asynchronous request. Under the covers, PipeAsync awaits the result of Task<T> and passes T to the non-async method.

```csharp
await input
    .Pipe(Query)
    .Pipe(Validate)
    .PipeAsync(TransformAsync)
    // Notice this is PipeAsync even though Submit is synchronous.
    .PipeAsync(Submit);
```

# Why do we need a forward pipe operator?

Code readability is important. While it's the standard to create temporary variables to make code more readable, temporary variables polute the readability. The way that F# has solved the problem is with the forward pipe operator. We should attempt to solve the problem for C# as well. One way to do this is by borrowing the concept of a forward pipe operator.

# Example user flow

```csharp
public class UserFlow
{
  public async Task<ActionResult> Pipe_UserFlow(Input input)
  {
    try 
    {
      await input
        .Pipe(Query)
        .Pipe(Validate)
        .Pipe(Transform)
        .PipeAsync(SubmitAsync);
        
      return Ok();
    }
    catch(ValidationException validationException)
    {
      return BadRequest(validationException.Message);
    }
  }
  
  private Model Query(Input input)
  {
    // Your query to a database to add extra information.
    // I've called the new model "Model", but it's a terrible name.
    
    return new Model(input);
  }
  
  private Model Validate(Model model)
  {
    // Your validation which throws an exception when it's not right.
    
    return model;
  }
  
  private Output Transform(Model model)
  {
    // Often you need to provide a different model to the universe than 
    // the model you use to complete the current operation. You could
    // Do this as part of the submit if you have lots of submit operations,
    // But I've elected to do it here.
    
    return new Output(model);
  }
  
  private async Task SubmitAsync(Output output)
  {
    // This could be any operation. In this case, we'll just wait for two seconds
    // to simulate saving something to a database.
    
    await Task.Delay(2000);
  }
}
```
