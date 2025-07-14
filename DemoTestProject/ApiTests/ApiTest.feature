Feature: ApiTest

Feaure to test GET and POST API calls

Scenario: Get Request
	When the Test Application Server is pinged
	Then the api call is successful
	And the system info is returned 

Scenario: Post Request
	Given we have data to submit
	When submit the data
	Then the api call is successful
	And the data is returned 