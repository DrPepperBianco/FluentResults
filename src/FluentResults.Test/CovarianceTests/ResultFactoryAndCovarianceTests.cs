using FluentResults;
using FluentResults.Results.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FluentResults.Test.CovarianceTests;
interface ICar;
sealed class Porsche : ICar;
sealed class BMW : ICar;
struct Trabant : ICar;


public class ResultFactoryAndCovarianceTests
{
    [Fact]
    public void ValueTypesAreNotCovariantWithObject()
    {
        IResult<object> result = ResultFactory.CreateEmptyResult<object>(1d);

        Assert.False(result is IResult<int>);
        Assert.False(result is IResult<double>);
    }

    [Fact]
    public void FailedResultWithoutCovariance()
    {
        IResult<ICar> result = Result.Fail<ICar>("My Error");

        Assert.True(result is IResult<ICar>);
        Assert.False(result is IResult<Porsche>);
        Assert.False(result is IResult<BMW>);
        Assert.False(result is IResult<Trabant>);
    }
    [Fact]
    public void FailedResultWithCovariance()
    {
        IResult<ICar> result = Result.Fail<Porsche>("My Error");

        Assert.True(result is IResult<ICar>);
        Assert.True(result is IResult<Porsche>);
        Assert.False(result is IResult<BMW>);
        Assert.False(result is IResult<Trabant>);
    }
    [Fact]
    public void PorscheIsACar()
    {
        IResult<ICar> result = Result.Ok<ICar>(new Porsche());

        Assert.True(result is IResult<ICar>);
        Assert.True(result is IResult<Porsche>);
        Assert.False(result is IResult<BMW>);
        Assert.False(result is IResult<Trabant>);
    }
    [Fact]
    public void BMWIsACar()
    {
        IResult<ICar> result = Result.Ok<ICar>(new BMW());

        Assert.True(result is IResult<ICar>);
        Assert.False(result is IResult<Porsche>,
            "Result of BMW cannot be cast into Result of Porsche");
        Assert.True(result is IResult<BMW>,
            "Result of ICar is Result of BMW and can be cast into Result of BMW");
        Assert.False(result is IResult<Trabant>,
            "Result of BMW cannot be cast into Result of Trabant");
    }
    [Fact]
    public void TrabantIsACar()
    {
        IResult<ICar> result = Result.Ok<ICar>(new Trabant());

        Assert.True(result is IResult<ICar>);
        Assert.False(result is IResult<Porsche>,
            "Result of Trabant cannot be cast into Result of Porsche");
        Assert.False(result is IResult<BMW>,
            "Result of Trabant cannot be cast into Result of Porsche");

        Assert.False(result is IResult<Trabant>,
            "Result of ICar contains a Trabant, but cannot be cast into Result of Trabant, " +
            "because the value is boxed.");
    }
    [Fact(Skip = "Shows something, that doesn’t compile")]
    public void StructDoesntSupportCovariance()
    {
        // ↓ doesn’t work, because IResult<Trabant> cannot be cast to IResult<ICar>,
        //   because Trabant is a struct and had to be boxed to be cast to ICar.
        // IResult<ICar> result = ResultFactory.CreateEmptyResult<Trabant>(new Trabant());
    }

    [Fact]
    public void Test42()
    {
        var data = Enumerable.Range(0, 42).Select(i => Result.Ok(i));
        var actual = data.Merge2();

        Assert.True(actual.IsSuccess);
        Assert.True(actual.Value is IEnumerable<int> ints && ints.ToArray() is { Length: 42 });
    }


}
