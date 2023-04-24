Feature: Feature1

A short summary of the feature

@tag1
Scenario: Recordings add to schedule Events
	Given a schedule view model
	And a service that returns 2 appointments
	When you load the page as a teacher
	Then the schedule events should have a count of 2
