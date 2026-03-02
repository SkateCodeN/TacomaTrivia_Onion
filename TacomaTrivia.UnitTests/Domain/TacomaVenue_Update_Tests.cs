using FluentAssertions;
using NUnit.Framework;
using TacomaTrivia.Domain;

[TestFixture]
public class TacomaVenue_Update_Tests
{
    //Valid mock of the TacomaVenue
    private static TacomaVenue Valid() =>
        TacomaVenue.Create(
            "name",
            "phone",
            "address",
            allowsPets: true,
            rounds: 2,
            2,
            new TimeOnly(21, 0),
            "www",
            allowsKids: true
        );

    [Test]
    public void Update_Should_Throw_When_Name_Missing()
    {
        var v = Valid();
        var act = () => v.Update(
            "",
            "p",
            "a",
            true,
            2,
            2,
            new TimeOnly(12, 0),
            "new website",
            true
        );
        act.Should().Throw<ArgumentException>().WithMessage("*Name Required*");
    }

    [Test]
    public void Update_Should_Throw_When_Rounds_Negative()
    {
        var v = Valid();
        var act = () => v.Update(
            "OK",
            "p",
            "a",
            true,
            -1,
            2,
            new TimeOnly(12, 0),
            "new website",
            true
        );
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Test]
    public void Update_Should_Trim_Name_And_Preserve_Id()
    {
        var v = Valid();
        var idBefore = v.Id;

        v.Update(
            "  New Name  ",
            "2",
            "B",
            false,
            6,
            4,
            new TimeOnly(12, 0),
            "new website",
            true
        );

        v.Id.Should().Be(idBefore);
        v.Name.Should().Be("New Name");
        v.Phone.Should().Be("2");
        v.Address.Should().Be("B");
        v.AllowsPets.Should().BeFalse();
        v.Rounds.Should().Be(6);
        v.TriviaDay.Should().Be(4);
        v.TriviaStart.Should().Be(new TimeOnly(12, 0));
        v.Website.Should().Be("new website");
        v.AllowsKids.Should().BeTrue();
    }

    [TestCase("")]
    [TestCase("   ")]
    public void Update_Should_Reject_EmptyOrWhitespace_Name(string bad)
    {
        var v = Valid();
        var act = () => v.Update(
            bad,
            "p",
            "a",
            true,
            1,
            2,
            new TimeOnly(12, 0),
            "new website",
            true
        );
        act.Should().Throw<ArgumentException>().WithMessage("*Name Required*");
    }
}
