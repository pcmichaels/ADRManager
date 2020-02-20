# Use Rules Analyser Service

## Context and Problem Statement

There may be a number of rules in the project that require 
conditional logic around the name and type of the project files

## Considered Options

* Add the logic inline each time
* Use a strategy pattern to try to implement these rules
* Use a single service to encapsulate the rules in a single place

## Decision Outcome

Chosen option: Use a single service to encapsulate the rules in a single place

* There is currently only a single rule
* It will be easier to track and test the rule(s) in a single location
