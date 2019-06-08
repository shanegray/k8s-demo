using KellermanSoftware.CompareNetObjects;
using Xunit.Sdk;

namespace DriverService.Tests
{
    public class DeepAssert
    {
        public static void Equal(object expected, object actual, params string[] membersToIgnore)
        {
            var compareLogic = new CompareLogic();
            foreach (var memberToIgnore in membersToIgnore)
            {
                compareLogic.Config.MembersToIgnore.Add(memberToIgnore);
            }

            var result = compareLogic.Compare(actual, expected);
            if (result.AreEqual == false)
            {
                throw new DeepAssertException(expected, actual, result.DifferencesString);
            }
        }
    }

    public class DeepAssertException : AssertActualExpectedException
    {
        public DeepAssertException(object expected, object actual, string message) :
            base(expected, actual, "DeepAssert.Equal() Failure")
        {
            Message = message;
        }

        public override string Message { get; }
    }
}
