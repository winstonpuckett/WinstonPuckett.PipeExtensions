# Summary

This package provides a basic forward pipe operator. This is a commonly requested language feature in C#, but appears to not currently be on the schedule.

Here are currently open tickets requesting the feature:
- [C# ticket #74](https://github.com/dotnet/csharplang/discussions/74)
- [C# ticket #96](https://github.com/dotnet/csharplang/discussions/96)

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

Barring it being added to C# natively, the best way to simulate a forward pipe operator is with extension methods.

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

### Cancellation Tokens
  
Cancellation Tokens are available as of version 1.1.0. To use them, pass in the token after passing in the function to operate on. For this to compile, the function must accept a cancellation token (Func<T, CancellationToken, TResult>).
  
```csharp
await input
    .Pipe(Query)
    .Pipe(Validate)
    // Note the cancellation token.
    .PipeAsync(TransformAsync, cancellationToken)
    .PipeAsync(Submit);
```
  
## Passing multiple arguments.
  
This package has opted to retain a consistent "Take what's on the left and pass it to the right" syntax, which means that we can only operate on one variable at a time. Even with this limitation, there are many ways to pass extra variables to functions. The most readable is a Tuple.
  
```csharp
bool Validate((int id, string name))
  => id > 0 && name != "invalid";

var isValid = (0, "Charlie").Pipe(Validate);
```

# Why do we need a forward pipe operator?

Code readability is important. While temporary variables are a step towards readability, they consistently create noise. F# and other functional languages have solved this problem. We should attempt to solve the problem for C# as well. One way to do this is by borrowing the concept of a forward pipe operator.

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

# Other forward pipe operator projects

After I released this package, I found out there is another repository out there with a similar aim. While I didn't draw inspiration from the repo, it was first and deserves mention. [TomyDurazno's PipeExtensions](https://github.com/TomyDurazno/PipeExtensions)
