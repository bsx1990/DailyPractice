using System;

namespace Criminalinvestigation
{
    public class Question
    {
        private readonly Func<bool> _optionA;
        private readonly Func<bool> _optionB;
        private readonly Func<bool> _optionC;
        private readonly Func<bool> _optionD;

        public Question(Func<bool> optionA, Func<bool> optionB, Func<bool> optionC, Func<bool> optionD)
        {
            _optionA = optionA;
            _optionB = optionB;
            _optionC = optionC;
            _optionD = optionD;
        }

        public bool IsRightAnswer(OptionValue optionValue)
        {
            switch (optionValue)
            {
                case OptionValue.A:
                    return _optionA.Invoke();
                case OptionValue.B:
                    return _optionB.Invoke();
                case OptionValue.C:
                    return _optionC.Invoke();
                case OptionValue.D:
                    return _optionD.Invoke();
                default:
                    throw new ArgumentOutOfRangeException("optionValue", optionValue, null);
            }
        }
    }
}