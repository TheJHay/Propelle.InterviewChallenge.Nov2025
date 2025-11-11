# Propelle.InterviewChallenge

## Background

At Propelle, we're expecting customers to make deposits with us so we can make investments on their behalf.
In this sample application, a customer can make a deposit through an API request and it is then automatically submitted to a fictional investment partner, SmartInvest.
Although this sounds like a simple problem, it's not always that easy. There are 2 technical issues plaguing the application:

1. The SmartInvest API is unreliable and can often throw transient exceptions
1. The Database can go down for maintenance at any time

Although this application doesn't use a real database or consume a real API, the transient issues caused by these 2 services
are simulated to have a 20% chance of failing whenever they are invoked. So when large volumes of deposits are coming in, we need to be sure that we can
recover from these failures and ensure that the deposits are processed.

## Your Challenge

There is a test class (MakeDepositTests) that simulates 100 deposits being made to the API. These 2 tests check that that
100 deposits have been stored and submitted to SmartInvest respectively. One of these tests is failing - for some reason there are more deposits being
stored than are being sent to SmartInvest.

Your challenge is to get all the tests passing, but there are a few rules:

- You can't change any code in the tests
- You can't change the PointOfFailure class
- You can't remove or change the invocations of the PointOfFailure.SimulatePotentialFailure() method (e.g. passing 0 as a parameter)

Don't spend too long on this challenge (as a guideline: no more than 1 hour), but please include any thoughts about your solution and the changes you make.

To show us your changes, fork this repository, and send a link back to us (make sure it's a publicly available repository)

Have fun :)
