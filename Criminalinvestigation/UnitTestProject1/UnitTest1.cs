using System;
using System.Collections.Generic;
using System.Linq;
using Criminalinvestigation;
using NUnit.Framework;

namespace UnitTestProject1
{
    [TestFixture]
    public class UnitTest1
    {
        private static readonly OptionValue[] Answers = new OptionValue[10];

        #region useless

        [Test]
        public void LeastOptionValueShouldBeA_WhenGivenABBBCCDD()
        {
            OptionValue[] given = { OptionValue.B, OptionValue.B, OptionValue.A, OptionValue.C, OptionValue.C, OptionValue.B,OptionValue.D,OptionValue.D };
            Assert.AreEqual(OptionValue.A,GetLeastOptionValue(given));
        }

        private static IList<Tuple<OptionValue, int>> CountAnswers(OptionValue[] answers)
        {
            var options = new List<OptionValue>{OptionValue.A, OptionValue.B, OptionValue.C, OptionValue.D};
            return options
                   .Select(option => new Tuple<OptionValue, int>(option, answers.Count(answer => answer == option)))
                   .ToList();
        }

        public static OptionValue GetLeastOptionValue(OptionValue[] answers)
        {
            var countedOptions = CountAnswers(answers);
            var leastCount = countedOptions.Select(option => option.Item2).Min();
            return countedOptions.First(option => option.Item2 == leastCount).Item1;
        }

        public static int GetAbsBetweenLeastAndMostOptionValues(OptionValue[] answers)
        {
            var countedOptions = CountAnswers(answers);
            var leastCount = countedOptions.Select(option => option.Item2).Min();
            var mostCount = countedOptions.Select(option => option.Item2).Max();
            return mostCount - leastCount;
        }

        [Test]
        public void AShouldNearB()
        {
            Assert.IsTrue(IsNearBy(OptionValue.A,OptionValue.B));
        }

        [Test]
        public void AShouldNotNearC()
        {
            Assert.IsFalse(IsNearBy(OptionValue.A,OptionValue.C));
        }

        public static bool IsNearBy(OptionValue left, OptionValue right)
        {
            return Math.Abs(left - right) == 1;
        }

        #endregion

        private static readonly Question Q1 =
            new Question(() => Answers[0] == OptionValue.A,
                         () => Answers[0] == OptionValue.B,
                         () => Answers[0] == OptionValue.C,
                         () => Answers[0] == OptionValue.D);
        private static readonly Question Q2 =
            new Question(() => Answers[4] == OptionValue.C,
                         () => Answers[4] == OptionValue.D,
                         () => Answers[4] == OptionValue.A,
                         () => Answers[4] == OptionValue.B);
        private static readonly Question Q3 =
            new Question(() => Answers[1] == Answers[3] && Answers[3] == Answers[5] && Answers[1] != Answers[2],
                         () => Answers[1] == Answers[2] && Answers[2] == Answers[3] && Answers[1] != Answers[5],
                         () => Answers[2] == Answers[3] && Answers[3] == Answers[5] && Answers[2] != Answers[1],
                         () => Answers[1] == Answers[2] && Answers[2] == Answers[5] && Answers[1] != Answers[3]);
        private static readonly Question Q4 =
            new Question(() => Answers[0] == Answers[4],
                         () => Answers[1] == Answers[6],
                         () => Answers[0] == Answers[8],
                         () => Answers[5] == Answers[9]);
        private static readonly Question Q5 =
            new Question(() => Answers[4] == Answers[7],
                         () => Answers[4] == Answers[3],
                         () => Answers[4] == Answers[8],
                         () => Answers[4] == Answers[6]);
        private static readonly Question Q6 =
            new Question(() => Answers[7] == Answers[1] && Answers[7] == Answers[3],
                         () => Answers[7] == Answers[0] && Answers[7] == Answers[5],
                         () => Answers[7] == Answers[2] && Answers[7] == Answers[9],
                         () => Answers[7] == Answers[4] && Answers[7] == Answers[8]);
        private static readonly Question Q7 =
            new Question(() => GetLeastOptionValue(Answers) == OptionValue.C,
                         () => GetLeastOptionValue(Answers) == OptionValue.B,
                         () => GetLeastOptionValue(Answers) == OptionValue.A,
                         () => GetLeastOptionValue(Answers) == OptionValue.D);
        private static readonly Question Q8 =
            new Question(() => !IsNearBy(Answers[0] ,Answers[6]),
                         () => !IsNearBy(Answers[0] ,Answers[4]),
                         () => !IsNearBy(Answers[0] ,Answers[1]),
                         () => !IsNearBy(Answers[0] ,Answers[9]));
        private static readonly Question Q9 =
            new Question(() => (Answers[0] == Answers[5]) ^ (Answers[4] == Answers[5]),
                         () => (Answers[0] == Answers[5]) ^ (Answers[4] == Answers[9]),
                         () => (Answers[0] == Answers[5]) ^ (Answers[4] == Answers[1]),
                         () => (Answers[0] == Answers[5]) ^ (Answers[4] == Answers[8]));
        private static readonly Question Q10 =
            new Question(() => GetAbsBetweenLeastAndMostOptionValues(Answers) == 3,
                         () => GetAbsBetweenLeastAndMostOptionValues(Answers) == 2,
                         () => GetAbsBetweenLeastAndMostOptionValues(Answers) == 4,
                         () => GetAbsBetweenLeastAndMostOptionValues(Answers) == 1);

        private static readonly Question[] Questions = {Q1, Q2, Q3, Q4, Q5, Q6, Q7, Q8, Q9, Q10 };

        [Test, Combinatorial]
        public void CheckAllAnswers(
            [Values(OptionValue.A, OptionValue.B, OptionValue.C)] OptionValue answer1,
            [Values(OptionValue.A, OptionValue.B, OptionValue.C)] OptionValue answer2,
            [Values(OptionValue.A, OptionValue.B, OptionValue.C)] OptionValue answer3,
            [Values(OptionValue.A, OptionValue.B, OptionValue.C)] OptionValue answer4,
            [Values(OptionValue.A, OptionValue.B, OptionValue.C)] OptionValue answer5,
            [Values(OptionValue.A, OptionValue.B, OptionValue.C)] OptionValue answer6,
            [Values(OptionValue.A, OptionValue.B, OptionValue.C)] OptionValue answer7,
            [Values(OptionValue.A, OptionValue.B, OptionValue.C)] OptionValue answer8,
            [Values(OptionValue.A, OptionValue.B, OptionValue.C)] OptionValue answer9,
            [Values(OptionValue.A, OptionValue.B, OptionValue.C)] OptionValue answer10)
        {
            Answers[0] = answer1;
            Answers[1] = answer2;
            Answers[2] = answer3;
            Answers[3] = answer4;
            Answers[4] = answer5;
            Answers[5] = answer6;
            Answers[6] = answer7;
            Answers[7] = answer8;
            Answers[8] = answer9;
            Answers[9] = answer10;

            var result = true;

            for (var index = 0; index < 10; index++)
            {
                var question = Questions[index];
                var answer = Answers[index];
                if (question.IsRightAnswer(answer)) { continue; }
                result = false;
                break;
            }

            Assert.IsTrue(result);
        }

        //[Test]
        //public void IsPassedAllQuestion()
        //{
        //    Answers[0] = OptionValue.B;
        //    Answers[1] = OptionValue.C;
        //    Answers[2] = OptionValue.A;
        //    Answers[3] = OptionValue.C;
        //    Answers[4] = OptionValue.A;
        //    Answers[5] = OptionValue.C;
        //    Answers[6] = OptionValue.D;
        //    Answers[7] = OptionValue.A;
        //    Answers[8] = OptionValue.B;
        //    Answers[9] = OptionValue.A;

        //    var result = true;
        //    for (var index = 0; index < 10; index++)
        //    {
        //        var question = Questions[index];
        //        var answer = Answers[index];
        //        if (!question.IsRightAnswer(answer))
        //        {
        //            result = false;
        //        }
        //    }

        //    Assert.IsTrue(result);
        //}
    }
}
