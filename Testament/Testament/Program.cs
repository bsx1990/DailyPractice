using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Testament
{
    internal class Program
    {
        private static readonly Human L = new Human("L");
        private static readonly Human Y = new Human("Y");
        private static readonly Human P = new Human("P");
        private static readonly IList<Human> Humen = new List<Human>{L,Y,P};
        private static readonly TestamentRule R1 = new TestamentRule(()=>Humen.Any(human => human.WearGreenTie) && Humen.Where(human=>human.WearGreenTie).Min(h=>h.Sequence) > L.Sequence);
        private static readonly TestamentRule R2 = new TestamentRule(() =>!Humen.First(human => human.Sequence == 1).HadLentAnUmbrellaToMe);
        private static readonly TestamentRule R3 = new TestamentRule(()=>Humen.Any(human=> human.IsTheFirstOneWhoFallInLove) && Humen.First(human=>human.IsTheFirstOneWhoFallInLove).Sequence-1==P.Sequence);
        private static readonly TestamentRule R4 = new TestamentRule(()=>Humen.Count(human => human.IsTheFirstOneWhoFallInLove) == 1);
        private static readonly TestamentRule R5 = new TestamentRule(()=>Y.Sequence == 2 || P.Sequence == 2);
        private static readonly IList<TestamentRule> Rules = new List<TestamentRule>{R1,R2,R3,R4,R5};
        
        private static readonly StringBuilder StringBuilder = new StringBuilder();

        public static void Main()
        {
            StringBuilder.AppendLine("Y left London in 1920");
            SetSequenceForHumen();
            var result = StringBuilder.ToString();
            Console.WriteLine(result);
        }

        private static void SetSequenceForHumen()
        {
            var sequences = IteratingAllSequence();
            foreach (var sequence in sequences)
            {
                SetSequence(sequence);
                IteratingPropertiesForHuman(1);
            }
        }

        private static void IteratingPropertiesForHuman(int index)
        {
            if (index > 3)
            {
                if (IsPassedAllRules()) { PrintHumenInfo(); }
                return;
            }
            var human = Humen.First(h => h.Sequence == index);
            IteratingProperty(wearGreenTieValue =>
                              {
                                  SetWearGreenTie(human, wearGreenTieValue);
                                  IteratingProperty(lentUmbrellaValue =>
                                                    {
                                                        SetHadLentAnUmbrellaToMe(human, lentUmbrellaValue);
                                                        IteratingProperty(firstFallInLoveValue =>
                                                                          {
                                                                              SetFirstFallInLove(human, firstFallInLoveValue);
                                                                              IteratingPropertiesForHuman(index + 1);
                                                                          });
                                                    });
                              });
        }

        private static void PrintHumenInfo()
        {
            StringBuilder.AppendLine("Found One Solution:");
            foreach (var human in Humen)
            {
                StringBuilder.AppendLine(human.ToString());
            }
            StringBuilder.AppendLine("");
        }

        private static bool IsPassedAllRules()
        {
            return Rules.All(rule => rule.IsPassed());
        }

        private static void IteratingProperty(Action<PropertyValues> action)
        {
            foreach (PropertyValues propertyValue in Enum.GetValues(typeof(PropertyValues)))
            {
                action(propertyValue);
            }
        }

        private static void SetFirstFallInLove(Human human, PropertyValues propertyValue)
        {
            human.IsTheFirstOneWhoFallInLove = propertyValue == PropertyValues.TrueValue;
        }

        private static void SetHadLentAnUmbrellaToMe(Human human, PropertyValues propertyValue)
        {
            human.HadLentAnUmbrellaToMe = propertyValue == PropertyValues.TrueValue;
        }

        private static void SetWearGreenTie(Human human, PropertyValues propertyValue)
        {
            human.WearGreenTie = propertyValue == PropertyValues.TrueValue;
        }

        private static void SetSequence(IList<int> sequence)
        {
            for (var index = 0; index < 3; index++)
            {
                Humen[index].Sequence = sequence[index];
            }
        }

        private static IEnumerable<List<int>> IteratingAllSequence()
        {
            var sequenceOptions = Enumerable.Range(1, 3).ToList();
            var result = new List<List<int>>();
            return IteratingSequences(sequenceOptions, result);
        }

        private static IEnumerable<List<int>> IteratingSequences(IReadOnlyCollection<int> sequenceOptions, ICollection<List<int>> result)
        {
            foreach (var sequence in sequenceOptions)
            {
                var currentSequences = new List<int> {sequence};

                var leftSequences = sequenceOptions.Where(option => option != sequence).ToList();
                if (leftSequences.Count == 0) { break; }
                
                IteratingSequences(leftSequences, result, currentSequences.ToList());
            }

            return result;
        }

        private static void IteratingSequences(IReadOnlyCollection<int> sequenceOptions, ICollection<List<int>> result, IReadOnlyCollection<int> parenetSequence)
        {
            foreach (var sequence in sequenceOptions)
            {
                var tempSequence = parenetSequence.ToList();
                tempSequence.Add(sequence);
                var leftSequences = sequenceOptions.Where(option => option != sequence).ToList();
                if (leftSequences.Count == 0)
                {
                    result.Add(tempSequence);
                    break;
                }

                IteratingSequences(leftSequences, result, tempSequence.ToList());
            }
        }
    }

    public class TestamentRule
    {
        private readonly Func<bool> _rule;

        public TestamentRule(Func<bool> rule)
        {
            _rule = rule;
        }

        public bool IsPassed()
        {
            return _rule.Invoke();
        }
    }

    public class Human
    {
        public string Name { get; set; }
        public int Sequence { get; set; }
        public bool WearGreenTie { get; set; }
        public bool HadLentAnUmbrellaToMe { get; set; }
        public bool IsTheFirstOneWhoFallInLove { get; set; }

        public Human(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return
                $"[{Name}] Sequence:{Sequence} WearGreenTie:{WearGreenTie} HadLentAnUmbrellaToMe:{HadLentAnUmbrellaToMe} IsTheFirstOneWhoFallInLove:{IsTheFirstOneWhoFallInLove}";
        }
    }

    public enum PropertyValues
    {
        TrueValue,
        FalseValue
    }
}
