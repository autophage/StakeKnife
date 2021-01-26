using FluentAssertions;
using Microsoft.FSharp.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NodaMoney;
using StakeKnife.DomainActions;
using System;
using System.Linq;
using static StakeKnife.BackEnd.Entities;

namespace StakeKnife.UnitTests
{
    [TestClass]
    public class PaymentCalculatorTests
    {
        PaymentCalculator classUnderTest = new PaymentCalculator();

        [TestMethod]
        public void GivenSingleStakeholder_AllMoneyGoesToStakeholder()
        {
            // Arrange
            var stakeholder = new Stakeholder("test", new Address("123", "", null, null, null));

            var falloff = FSharpList<decimal>.Cons(1m, FSharpList<decimal>.Empty);

            var shares = FSharpList<Share>.Cons(new Share(1, "test", new DateTime(2020, 1, 1), new DateTime(2050, 1, 1), falloff, stakeholder), FSharpList<Share>.Empty);

            var buckets = FSharpList<Bucket>.Cons(new Bucket(1, shares), FSharpList<Bucket>.Empty);

            var project = new Project("Test", buckets);

            // Act
            var result = this.classUnderTest.CalculatePaymentsForProject(project, Money.USDollar(100m));

            // Assert
            result.Count.Should().Be(1);
            result.Single().Amount.Should().Be(100);
        }

        [TestMethod]
        public void GivenSingleStakeholder_WithTwoShares_AllMoneyGoesToStakeholder()
        {
            // Arrange
            var stakeholder = new Stakeholder("test", new Address("123", "", null, null, null));

            var falloff = FSharpList<decimal>.Cons(1m, FSharpList<decimal>.Empty);

            var shares = FSharpList<Share>
                .Cons(new Share(1, "test", new DateTime(2020, 1, 1), new DateTime(2050, 1, 1), falloff, stakeholder),
                    FSharpList<Share>
                        .Cons(new Share(1, "test", new DateTime(2020, 1, 1), new DateTime(2050, 1, 1), falloff, stakeholder), FSharpList<Share>.Empty));

            var buckets = FSharpList<Bucket>.Cons(new Bucket(1, shares), FSharpList<Bucket>.Empty);

            var project = new Project("Test", buckets);

            // Act
            var result = this.classUnderTest.CalculatePaymentsForProject(project, Money.USDollar(100m));

            // Assert
            result.Count.Should().Be(1);
            result.Single().Amount.Should().Be(100);
        }

        [TestMethod]
        public void GivenTwoStakeholders_WithOneShareEach_MoneyIsSplitEvenly()
        {
            // Arrange
            var stakeholderA = new Stakeholder("one", new Address("123", "", null, null, null));
            var stakeholderB = new Stakeholder("two", new Address("123", "", null, null, null));

            var falloff = FSharpList<decimal>.Cons(1m, FSharpList<decimal>.Empty);

            var shares = FSharpList<Share>
                .Cons(new Share(1, "test", new DateTime(2020, 1, 1), new DateTime(2050, 1, 1), falloff, stakeholderA),
                    FSharpList<Share>
                        .Cons(new Share(1, "test", new DateTime(2020, 1, 1), new DateTime(2050, 1, 1), falloff, stakeholderB), FSharpList<Share>.Empty));

            var buckets = FSharpList<Bucket>.Cons(new Bucket(1, shares), FSharpList<Bucket>.Empty);

            var project = new Project("Test", buckets);

            // Act
            var result = this.classUnderTest.CalculatePaymentsForProject(project, Money.USDollar(100m));

            // Assert
            result.Count.Should().Be(2);
            result.Single(p => p.Stakeholder == stakeholderA).Amount.Should().Be(50);
            result.Single(p => p.Stakeholder == stakeholderB).Amount.Should().Be(50);
        }

        [TestMethod]
        public void GivenTwoStakeholders_WithThreeToOneShare_MoneyIsSplitThreeToOne()
        {
            // Arrange
            var stakeholderA = new Stakeholder("one", new Address("123", "", null, null, null));
            var stakeholderB = new Stakeholder("two", new Address("123", "", null, null, null));

            var falloff = FSharpList<decimal>.Cons(1m, FSharpList<decimal>.Empty);

            var shares = FSharpList<Share>
                .Cons(new Share(3, "test", new DateTime(2020, 1, 1), new DateTime(2050, 1, 1), falloff, stakeholderA),
                    FSharpList<Share>
                        .Cons(new Share(1, "test", new DateTime(2020, 1, 1), new DateTime(2050, 1, 1), falloff, stakeholderB), FSharpList<Share>.Empty));

            var buckets = FSharpList<Bucket>.Cons(new Bucket(1, shares), FSharpList<Bucket>.Empty);

            var project = new Project("Test", buckets);

            // Act
            var result = this.classUnderTest.CalculatePaymentsForProject(project, Money.USDollar(100m));

            // Assert
            result.Count.Should().Be(2);
            result.Single(p => p.Stakeholder == stakeholderA).Amount.Should().Be(75);
            result.Single(p => p.Stakeholder == stakeholderB).Amount.Should().Be(25);
        }

        [TestMethod]
        public void GivenTwoStakeholders_InDifferentBucketsWithThreeToOneRatio_MoneyIsSplitThreeToOne()
        {
            // Arrange
            var stakeholderA = new Stakeholder("one", new Address("123", "", null, null, null));
            var stakeholderB = new Stakeholder("two", new Address("123", "", null, null, null));

            var falloff = FSharpList<decimal>.Cons(1m, FSharpList<decimal>.Empty);

            var sharesA = FSharpList<Share>.Cons(new Share(1, "test", new DateTime(2020, 1, 1), new DateTime(2050, 1, 1), falloff, stakeholderA), FSharpList<Share>.Empty);
            var sharesB = FSharpList<Share>.Cons(new Share(1, "test", new DateTime(2020, 1, 1), new DateTime(2050, 1, 1), falloff, stakeholderB), FSharpList<Share>.Empty);

            var buckets = FSharpList<Bucket>.Cons(new Bucket(.75m, sharesA),
                FSharpList<Bucket>.Cons(new Bucket(.25m, sharesB), FSharpList<Bucket>.Empty));

            var project = new Project("Test", buckets);

            // Act
            var result = this.classUnderTest.CalculatePaymentsForProject(project, Money.USDollar(100m));

            // Assert
            result.Count.Should().Be(2);
            result.Single(p => p.Stakeholder == stakeholderA).Amount.Should().Be(75);
            result.Single(p => p.Stakeholder == stakeholderB).Amount.Should().Be(25);
        }
    }
}
