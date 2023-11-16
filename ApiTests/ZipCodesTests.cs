using RestApiAutomationTraining.Helpers;
using RestApiAutomationTraining.Models;

namespace RestApiAutomationTraining.ApiTests
{
    [TestFixture]
    public class ZipCodesTests : BaseApiTest
    {

        [Test]
        public void GetZipCodes_StatusCode200Returned_Valid()
        {
            var response = ApiActions.GetZipCodes();

            Assert.That((int)response.StatusCode, Is.EqualTo(200));

            /*
             * Bug: Wrong status code is returned for Get /zip-codes
             * 
             * Preconditions:
             * - the user is authorized
             * 
             * Steps:
             * 1. Send GET request to /zip-codes endpoint
             * 
             * Expected result: response code 200 is returned
             * Actual result: response code 201 is returned
             */
        }

        [Test]
        public void GetZipCodes_AvailableZipCodesReturned_Valid()
        {
            var response = ApiActions.GetZipCodes();
            var zipCodes = DeserializeResponse<string[]>(response);

            Assert.That(zipCodes, Has.Length.GreaterThanOrEqualTo(0));
        }

        [Test]
        public void ExpandZipCodes_NewZipCodesAdded_Valid()
        {
            var newZipCodeAsLetterString = StringHelper.GetRandomString(5);
            var newZipCodeAsNumericString = StringHelper.GetRandomNumericString(5);
            var response = ApiActions.CreateZipCodes(newZipCodeAsLetterString, newZipCodeAsNumericString);

            var actualZipCodes = DeserializeResponse<List<string>>(ApiActions.GetZipCodes());

            Assert.Multiple(() =>
            {
                Assert.That((int)response.StatusCode, Is.EqualTo(201));
                Assert.That(actualZipCodes, Contains.Item(newZipCodeAsLetterString));
                Assert.That(actualZipCodes, Contains.Item(newZipCodeAsNumericString));
            });
        }

        [Test]
        public void ExpandZipCodes_NoDuplicatesCreated_InputAlreadyExists_Valid()
        {
            #region Test pre-setup

            var duplicatedZipCode = StringHelper.GetRandomNumericString(5);
            ApiActions.CreateZipCodes(duplicatedZipCode);

            #endregion

            var uniqueZipCode = StringHelper.GetRandomNumericString(5);
            var response = ApiActions.CreateZipCodes(duplicatedZipCode, uniqueZipCode);
            var actualZipCodes = DeserializeResponse<List<string>>(ApiActions.GetZipCodes());

            Assert.Multiple(() =>
            {
                Assert.That((int)response.StatusCode, Is.EqualTo(201));
                Assert.That(actualZipCodes, Contains.Item(uniqueZipCode));
                Assert.That(actualZipCodes.Count(_ => _ == duplicatedZipCode), Is.EqualTo(1),
                    $"Duplicates found for zip code [{duplicatedZipCode}]");
            });

            /*
             * Bug: The system creates duplicates for zip codes
             * 
             * Preconditions:
             * - the user is authorized
             * - Get or create a new zip code that will be duplicated
             * 
             * Steps:
             * 1. Send POST request to /zip-codes/expand endpoint with body:
             *  [ "unique zip code", "duplicated zip code" ]
             * 2. Send GET request to /zip-codes endpoint
             * 
             * Expected result: only unique zip code was added, no duplicates
             * Actual result: the response result shows a duplicated for the zip code taken from Preconditions
             */
        }

        [Test]
        public void ExpandZipCodes_NoDuplicatesCreated_InputContainsDuplicates_Valid()
        {
            #region Test pre-setup

            var usedZipCode = StringHelper.GetRandomNumericString(5);
            ApiActions.CreateZipCodes(usedZipCode);

            var randomUser = UserModel.GenerateRandomUser(usedZipCode, new Random().Next(1, 124));
            ApiActions.CreateUser(randomUser);

            #endregion

            var uniqueZipCode = StringHelper.GetRandomNumericString(5);
            var response = ApiActions.CreateZipCodes(usedZipCode, uniqueZipCode);
            var actualZipCodes = DeserializeResponse<List<string>>(ApiActions.GetZipCodes());

            Assert.Multiple(() =>
            {
                Assert.That((int)response.StatusCode, Is.EqualTo(201));
                Assert.That(actualZipCodes, Contains.Item(uniqueZipCode));
                Assert.That(actualZipCodes.Count(_ => _ == usedZipCode), Is.EqualTo(0),
                    $"Already used zip code found: [{usedZipCode}]");
            });

            /*
             * Bug: The system creates zip codes that ulready used
             * 
             * Preconditions:
             * - the user is authorized
             * - Get or create a new user with a valid zip code
             * 
             * Steps:
             * 1. Send POST request to /zip-codes/expand endpoint with body:
             *  [ "unique zip code", "already used zip code" ]
             * 2. Send GET request to /zip-codes endpoint
             * 
             * Expected result: only unique zip code is added, already used zip code is not added
             * Actual result: both unique and used zip codes were added
             */
        }
    }
}
