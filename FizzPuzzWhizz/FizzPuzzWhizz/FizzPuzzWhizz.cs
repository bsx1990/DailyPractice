using System;
using System.Linq;

namespace FizzPuzzWhizz
{
    public static class NumberIdentification
    {
        public static (bool IsPassed, string Result) ValidatedBy(this int input, Rule rule)
        {
            return rule.IsPassed(input) ? (true, rule.Result) : (false, string.Empty);
        }
    }

    public class FizzPuzzWhizz
    {
        private const string FizzDisplay = "Fizz";
        private const string BuzzDisplay = "Buzz";
        private const string WhizzDisplay = "Whizz";

        public string Translate(int input)
        {
            var rule = GetRule();
            var (isPassed, result) = input.ValidatedBy(rule);
            return isPassed ? result : input.ToString();
        }

        private static Rule GetRule()
        {
            var containsThreeRule = new Rule(new ContainsCondition(3), FizzDisplay);
            var devidedByThreeRule = new Rule(new DevidedCondition(3), FizzDisplay);
            var devidedByFiveRule = new Rule(new DevidedCondition(5), BuzzDisplay);
            var devidedBySevenRule = new Rule(new DevidedCondition(7), WhizzDisplay);

            var devidecByThreeAndFiveAndSevenRule = new AndRule(devidedByThreeRule, devidedByFiveRule, devidedBySevenRule);
            var devidedByThreeAndFiveRule = new AndRule(devidedByThreeRule, devidedByFiveRule);
            var devidedByThreeAndSevenRule = new AndRule(devidedByThreeRule, devidedBySevenRule);
            var devidedByFiveAndSevenRule = new AndRule(devidedByFiveRule, devidedBySevenRule);

            return new OrRule(containsThreeRule,
                              devidecByThreeAndFiveAndSevenRule,
                              devidedByThreeAndFiveRule,
                              devidedByThreeAndSevenRule,
                              devidedByFiveAndSevenRule,
                              devidedByThreeRule,
                              devidedByFiveRule,
                              devidedBySevenRule);
        }
    }

    public class AndRule : Rule
    {
        public AndRule(params Rule[] rules)
        {
            Condition = new Condition { Expression = input => { return rules.All(rule => rule.IsPassed(input)); } };
            Result = rules.Select(rule => rule.Result).Aggregate((a, b) => $"{a}{b}");
        }
    }

    public class OrRule : Rule
    {
        public OrRule(params Rule[] rules)
        {
            Condition = new Condition
                        {
                            Expression = input =>
                                         {
                                             foreach (var rule in rules)
                                             {
                                                 if (!rule.IsPassed(input)) { continue; }
                                                 Result = rule.Result;
                                                 return true;
                                             }
                                             return false;
                                         }
                        };
        }
    }

    public class Rule
    {
        public Rule() { }

        public Rule(Condition condition, string result)
        {
            Condition = condition;
            Result = result;
        }

        public Condition Condition { get; set; }
        public string Result { get; set; }

        public bool IsPassed(int input)
        {
            return Condition.Expression.Invoke(input);
        }
    }

    public class Condition
    {
        public Func<int,bool> Expression { get; set; }
    }

    public class ContainsCondition : Condition
    {
        public ContainsCondition(int containedNumber)
        {
            Expression = input => input.ToString().Contains(containedNumber.ToString());
        }
    }

    public class DevidedCondition : Condition
    {
        public DevidedCondition(int devidedNumber)
        {
            Expression = input => input % devidedNumber == 0;
        }
    }
    
}
