using FluentAssertions;
using NUnit.Framework;
using TacomaTrivia.Domain;

[TestFixture]
public class TacomaVenue_Update_Tests
{
    private static TacomaVenue Valid() =>
        TacomaVenue.Create("Old", "p", "a", allowsPets: true, rounds: 2);

    [Test]
    public void Update_Should_Throw_When_Name_Missing()
    {
        var v = Valid();
        var act = () => v.Update("", "p", "a", true, 2);
        act.Should().Throw<ArgumentException>().WithMessage("*Name Required*");
    }

    [Test]
    public void Update_Should_Throw_When_Rounds_Negative()
    {
        var v = Valid();
        var act = () => v.Update("OK", "p", "a", true, -1);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Test]
    public void Update_Should_Trim_Name_And_Preserve_Id()
    {
        var v = Valid();
        var idBefore = v.Id;

        v.Update("  New Name  ", "2", "B", false, 6);

        v.Id.Should().Be(idBefore);
        v.Name.Should().Be("New Name");
        v.Phone.Should().Be("2");
        v.Address.Should().Be("B");
        v.AllowsPets.Should().BeFalse();
        v.Rounds.Should().Be(6);
    }

    [TestCase("")]
    [TestCase("   ")]
    public void Update_Should_Reject_EmptyOrWhitespace_Name(string bad)
    {
        var v = Valid();
        var act = () => v.Update(bad, "p", "a", true, 1);
        act.Should().Throw<ArgumentException>().WithMessage("*Name Required*");
    }
}
