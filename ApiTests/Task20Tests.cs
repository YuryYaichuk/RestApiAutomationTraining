using NUnit.Allure.Attributes;
using RestApiAutomationTraining.ApiActions;
using RestApiAutomationTraining.Helpers;
using RestApiAutomationTraining.Models;
using System.Net;

namespace RestApiAutomationTraining.ApiTests;

[TestFixture]
public class Task20Tests : BaseApiTest
{
    //Scenario #1 
    //Given I am authorized user
    //When I send GET request to /zip-codes endpoint
    //Then I get 200 response code
    //And I get all available zip codes in the application for now

    [Test]
    [AllureName("Test Get Available ZIP Codes")]
    [AllureEpic("Task 20")]
    [AllureIssue("Wrong status code is returned for Get /zip-codes")]
    public void GetZipCodes_AvailableZipCodesReturned_Valid()
    {
        #region Test pre-setup

        var expectedZipCodeList = new List<string>
        {
            StringHelper.GetRandomString(5),
            StringHelper.GetRandomNumericString(6)
        };

        AddNewZipCodes(expectedZipCodeList.ToArray());

        #endregion

        var getZipCodesResponse = ReadApiActions.GetZipCodes();
        var zipCodes = getZipCodesResponse.ToModel<List<string>>();

        Assert.Multiple(() =>
        {
            Asserts.AssertStatusCode(getZipCodesResponse, HttpStatusCode.OK);
            Asserts.AssertContainsAll(zipCodes, expectedZipCodeList);
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

    //Scenario #2 
    //Given I am authorized user
    //When I send POST request to /zip-codes/expand endpoint
    //And Request body contains list of zip codes
    //Then I get 201 response code
    //And Zip codes from request body are added to available zip codes of application

    [Test]
    [AllureName("Test Add New ZIP Codes")]
    [AllureEpic("Task 20")]
    public void ExpandZipCodes_NewZipCodesAdded_Valid()
    {
        var expectedZipCodeList = new List<string>
        {
            StringHelper.GetRandomString(5),
            StringHelper.GetRandomNumericString(6)
        };
        var response = WriteApiActions.CreateZipCodes(expectedZipCodeList.ToArray());
        PayloadCollector.AddPayload(expectedZipCodeList);
        var actualZipCodes = GetZipCodesModel();

        Assert.Multiple(() =>
        {
            Asserts.AssertStatusCode(response, HttpStatusCode.Created);
            Asserts.AssertContainsAll(actualZipCodes, expectedZipCodeList);
        });
    }

    //Scenario #3 
    //Given I am authorized user
    //When I send POST request to /zip-codes/expand endpoint
    //And Request body contains list of zip codes
    //And List of zip codes has duplications for available zip codes
    //Then I get 201 response code
    //And Zip codes from request body are added to available zip codes of application
    //And There are no duplications in available zip codes

    [Test]
    [AllureName("Test Add New ZIP Codes - No Duplicates Are Created for Not Used ZIP Codes")]
    [AllureEpic("Task 20")]
    [AllureIssue("The system creates duplicates for zip codes")]
    public void ExpandZipCodes_NoDuplicatesCreated_InputAlreadyExists_Valid()
    {
        #region Test pre-setup

        var duplicatedZipCode = StringHelper.GetRandomNumericString(5);
        AddNewZipCodes(duplicatedZipCode);

        #endregion

        var createZipCodesResponse = WriteApiActions.CreateZipCodes(duplicatedZipCode);
        PayloadCollector.AddPayload(duplicatedZipCode);

        Assert.Multiple(() =>
        {
            Asserts.AssertStatusCode(createZipCodesResponse, HttpStatusCode.Created);
            Asserts.AssertCount(1, GetZipCodesModel(), duplicatedZipCode);
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

    //Scenario #4 
    //Given I am authorized user
    //When I send POST request to /zip-codes/expand endpoint
    //And Request body contains list of zip codes
    //And List of zip codes has duplications for already used zip codes
    //Then I get 201 response code
    //And Zip codes from request body are added to available zip codes of application
    //And There are no duplications between available zip codes and already used zip codes

    [Test]
    [AllureName("Test Add New ZIP Codes - No Duplicates Are Created for Used ZIP Codes")]
    [AllureEpic("Task 20")]
    [AllureIssue("The system creates zip codes that ulready used")]
    public void ExpandZipCodes_NoDuplicatesCreatedForUsedZipCodes_Valid()
    {
        #region Test pre-setup

        var usedZipCode = StringHelper.GetRandomNumericString(5);
        AddNewZipCodes(usedZipCode);

        var randomUser = UserModel.GenerateRandomUser(usedZipCode, Random.Next(1, 124));
        AddNewUser(randomUser);

        #endregion

        var createZipCodeResponse = WriteApiActions.CreateZipCodes(usedZipCode);
        PayloadCollector.AddPayload(usedZipCode);

        Assert.Multiple(() =>
        {
            Asserts.AssertStatusCode(createZipCodeResponse, HttpStatusCode.Created);
            Asserts.AssertCount(0, GetZipCodesModel(), usedZipCode);
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
