# GitHub-Intermediary-Api
An intermediary Api that is used to query GitHub's public API to retrieve a user. Please use the "Auth/GenerateToken" api endpoint to generate an authentication token, and include the token in the header, i.e. "Authorization: Bearer {token}"
The "Auth/GenerateToken" api endpoint generates an authentication token with 5 min lifespan - this is a stand-in for those that want to implement their own forms of authentication.

This Api is built with swagger for easier usage.
