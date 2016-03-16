# RepoOrchestrator

A simple WebAPI service that listens for [GitHub Web Hooks](https://developer.github.com/webhooks/) "Push" requests 
to {rootUrl}/CommitPushed. When invoked, RepoOrchestrator will inspect the commit that was pushed, and if it
matches any event subscriptions in the "EventRegistrationTable", it kicks off the appropriate action.

## GitHub Web Hooks

Currently, this web service is hooked up to https://github.com/eerhardt/versions, which holds current build version
information for various .NET repos. When build information changes in the versions repo, this web service is invoked.

## EventRegistrationTable

This is implemented as an Azure Storage Table that stores all the "subscriptions" of which actions to execute when 
upstream repo current build information is updated.

## Available actions

For now, VSO builds can be triggered by the RepoOrchestrator. The VSO builds can do anything it wants. There may be more
"action" types in the future.
