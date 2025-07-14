Feature: UI

A short summary of the feature

Scenario: Update Single Asset Availability for Within Day
	Given we open UI
	And we navigate to the Asset Availabilities page
	And we update the Asset Availability for a single row
	When we save the changes
	Then a success message will be displayed
	And we navigate to the Within Day page
	And Refresh the data
	And the Within Day Availability for a single row will be updated