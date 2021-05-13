# Summary

This package provides a basic forward pipe operator like would be common in a functional language. This is a commonly requested language feature in C#, but appears to not currently be on the schedule [C# ticket #74](https://github.com/dotnet/csharplang/discussions/74), or my prefered syntax, [C# ticket #96](https://github.com/dotnet/csharplang/discussions/96).

To my knowledge, we can't override an operator for every object type, so I've opted instead to use extension methods.

# Usage

- Install this package from NuGet (tbd).
- On any object, call myObject.Pipe(FunctionWithMyObjectParameter).

# Example
```csharp
public class UserFlow
{
  public ActionResult Pipe_UserFlow(Input input)
  {
    // Utilize the pipe for extreme readability.
    
    try 
    {
      input
        .Pipe(Query)
        .Pipe(Validate)
        .Pipe(Transform)
        .Pipe(Submit);
        
      return Ok();
    }
    catch(ValidationException validationException)
    {
      return BadRequest(validationException.Message);
    }
  }
  
  public void WithoutPipe_UserFlow(Input input)
  {
    try 
    {
      var model = Query(input);
      Validate(model);
      var outputModel = Transform(model);
      Submit(outputModel);
    }
    catch(ValidationException validationException)
    {
      return BadRequest(validationException.Message);
    }
  }
  
  public void WorstExample_WithoutPipe_UserFlow(Input input)
  {
    try 
    {
      Submit(Transform(Validate(Query(input);
    }
    catch(ValidationException validationException)
    {
      return BadRequest(validationException.Message);
    }
  }
  
  #region - Private method explanations
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
  
  private void Submit(Output output)
  {
    // This could be any operation. In this case, it's adding it to an entity
    // framework database. You could submit to a API or servicebus or anything else.
  
    _context.Add(output);
    _context.SaveChanges();
  }
  #endregion - Private method explanations
}
```
