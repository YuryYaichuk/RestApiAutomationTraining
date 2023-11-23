using RestApiAutomationTraining.ApiActions;
using RestApiAutomationTraining.Helpers;
using RestApiAutomationTraining.Models;

namespace RestApiAutomationTraining.ApiTests;

[TestFixture]
public class Task20Tests : BaseApiTest
{
    [SetUp]
    protected void TestSetup()
    {
        ClearZipCodes();
    }

    [Test]
    public void GetZipCodes_AvailableZipCodesReturned_Valid()
    {
        #region Test pre-setup

        var expectedZipCodeList = new List<string>
        {
            StringHelper.GetRandomString(5),
            StringHelper.GetRandomNumericString(5)
        };
        var response = WriteApiActions.CreateZipCodes(expectedZipCodeList.ToArray());
        Assert.That((int)response.StatusCode, Is.EqualTo(201));

        #endregion

        var getZipCodesResponse = ReadApiActions.GetZipCodes();
        var zipCodes = JsonHelper.DeserializeObject<List<string>>(getZipCodesResponse);

        Assert.Multiple(() =>
        {
            Assert.That((int)response.StatusCode, Is.EqualTo(200));
            Assert.That(zipCodes, Has.Count.EqualTo(2));
        });

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
    public void ExpandZipCodes_NewZipCodesAdded_Valid()
    {
        var expectedZipCodeList = new List<string>
        {
            StringHelper.GetRandomString(5),
            StringHelper.GetRandomNumericString(5)
        };
        var response = WriteApiActions.CreateZipCodes(expectedZipCodeList.ToArray());
        var actualZipCodes = JsonHelper.DeserializeObject<List<string>>(ReadApiActions.GetZipCodes());

        Assert.Multiple(() =>
        {
            Assert.That((int)response.StatusCode, Is.EqualTo(201));
            Assert.That(actualZipCodes, Is.EquivalentTo(expectedZipCodeList));
        });
    }

    [Test]
    public void ExpandZipCodes_NoDuplicatesCreated_InputAlreadyExists_Valid()
    {
        #region Test pre-setup

        var duplicatedZipCode = StringHelper.GetRandomNumericString(5);
        var response = WriteApiActions.CreateZipCodes(duplicatedZipCode);
        Assert.That((int)response.StatusCode, Is.EqualTo(201));

        #endregion

        var expectedZipCodeList = new List<string>
        {
            duplicatedZipCode,
            StringHelper.GetRandomNumericString(5)
        };

        var createZipCodesResponse = WriteApiActions.CreateZipCodes(expectedZipCodeList.ToArray());
        var actualZipCodes = JsonHelper.DeserializeObject<List<string>>(ReadApiActions.GetZipCodes());

        Assert.Multiple(() =>
        {
            Assert.That((int)createZipCodesResponse.StatusCode, Is.EqualTo(201));
            Assert.That(actualZipCodes, Is.EquivalentTo(expectedZipCodeList));
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
    public void ExpandZipCodes_NoDuplicatesCreatedForUsedZipCodes_Valid()
    {
        #region Test pre-setup

        var usedZipCode = StringHelper.GetRandomNumericString(5);
        var response = WriteApiActions.CreateZipCodes(usedZipCode);
        Assert.That((int)response.StatusCode, Is.EqualTo(201));

        var randomUser = UserModel.GenerateRandomUser(usedZipCode, Random.Next(1, 124));
        response = WriteApiActions.CreateUser(randomUser);
        Assert.That((int)response.StatusCode, Is.EqualTo(201));

        #endregion

        var expectedZipCodeList = new List<string>
        {
            StringHelper.GetRandomNumericString(5)
        };

        var createZipCodeResponse = WriteApiActions.CreateZipCodes(expectedZipCodeList.Single(), usedZipCode);
        var actualZipCodes = JsonHelper.DeserializeObject<List<string>>(ReadApiActions.GetZipCodes());

        Assert.Multiple(() =>
        {
            Assert.That((int)createZipCodeResponse.StatusCode, Is.EqualTo(201));
            Assert.That(actualZipCodes, Is.EquivalentTo(expectedZipCodeList));
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
