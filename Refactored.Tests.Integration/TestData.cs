using System;

namespace Refactored.Tests.Integration
{
    // normally I would create test data in each test ('arrange'),
    // but I didn't want to overcomplicate the refactoring
    public static class TestData
    {
        public static readonly Guid GalaxyProductId = Guid.Parse("8f2e9176-35ee-4f0a-ae55-83023d2db1a3");
        // ReSharper disable once InconsistentNaming
        public static readonly Guid IPhoneProductId = Guid.Parse("de1287c0-4b15-4a7b-9d8a-dd21b3cafec3");

        public static readonly Guid GalaxyWhiteOptionId = Guid.Parse("0643ccf0-ab00-4862-b3c5-40e2731abcc9");
    }
}
